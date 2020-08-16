﻿using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public class EmailSummaryServiceHandler : ISummaryServiceHandler
    {
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private readonly ISummaryEmailProviderDataProcessor _dataProcessor;
        private readonly ISummaryMailContentParser _summaryMailContentParser;
        private readonly EmailSummaryConfiguration _emailSummaryConfiguration;

        public EmailSummaryServiceHandler(IOptions<ServiceBusConfiguration> notificationServiceBusOption,
            IOptions<EmailSummaryConfiguration> emailSummaryOption,
            ISummaryEmailProviderDataProcessor dataProcessor,
            ISummaryMailContentParser parser)
        {
            _dataProcessor = dataProcessor;
            _notificationServiceBusConfiguration = notificationServiceBusOption.Value;
            _summaryMailContentParser = parser;
            _emailSummaryConfiguration = emailSummaryOption.Value;
        }

        public async Task ProcessMessage(ILogger logger)
        {
            logger.LogInformation($"NotificationSummaryProcessor : Getting summary email list of notifications." +
                $"Connecion:{_notificationServiceBusConfiguration.ServiceBusConnection} and queuename: {_notificationServiceBusConfiguration.QueueName}");

            var _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_notificationServiceBusConfiguration, logger);

            var param = CreateParam();

            IEnumerable<CompanyTotalScanResult> totalScan = GetTotalScanRecord(param);

            IEnumerable<CompanyTotalScanResult> totalAbnormalScan = GetAbnormalScanRecord(param);

            if (totalScan != null)
            {
                foreach (var item in totalScan)
                {
                    // Parse email info //
                    var mailParam = new ParseEmailParam(item.CompanyId, item.TotalScans);
                    mailParam.Recipients = _dataProcessor.GetRecipientsByCompanyId(item.CompanyId);

                    ////////////////////////////////////////////////////////////////////////
                    mailParam.TotalAbnormalDetected = item.TotalScans;

                    var mailData = _summaryMailContentParser.CreateSummaryEmailAlertMessage(
                        mailParam, logger);

                    if (mailData != null)
                    {
                        var messgeInstance = MessageConverter.Serialize(mailData);
                        await _messageSender.SendMessagesAsync(messgeInstance);

                        logger.LogInformation($"Summary {item.CompanyId} data to notification queue.");
                    }
                    else
                    {
                        logger.LogInformation($"No notifcation sent to queue. {DateTime.Now}");
                    }
                }
            }
            else
            {
                logger.LogInformation($"Unable to find any summary record matches in the database.{DateTime.Now}");
            }
        }

        private QueryTotalScanParam CreateParam()
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-1);

            return new QueryTotalScanParam
            {
                StartDate = startDate,
                EndDate = endDate,
                TemperatureMax = _emailSummaryConfiguration.MaxTemperature,
            };
        }

        private IEnumerable<CompanyTotalScanResult> GetTotalScanRecord(QueryTotalScanParam param)
        {
           return _dataProcessor.GetTotalScansByCompany(param);
        }

        private IEnumerable<CompanyTotalScanResult> GetAbnormalScanRecord(QueryTotalScanParam param)
        {
            return _dataProcessor.GetTotalScansByCompany(param);
        }
    }
}
