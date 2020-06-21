using System;
using System.Threading;
using System.Threading.Tasks;
using Service.MessageBusServiceProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoDataModel.Requests;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System.Collections.Generic;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public class ThermoDataLogic : IThermoDataLogic
    {
        private const string ServiceBusConfigurationKey = "ServiceBusConfiguration";
        private const string ThermoRestApiConfigurationKey = "ThermoRestConfiguration";
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ThermoRestConfiguration _restConfiguration;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;
        string targetBaseUrl;
        private IThermoDataRequester thermoDataRequester;
        private CancellationToken _stoppingToken;
        private IQueueMessageSender _messageSender;

        public ThermoDataLogic(ILogger logger, IConfiguration configuration, CancellationToken token)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();
            _restConfiguration = configuration.GetSection(ThermoRestApiConfigurationKey).Get<ThermoRestConfiguration>();
            _stoppingToken = token;
        }

        public void Setup()
        {
            targetBaseUrl = _restConfiguration.HostName;
            thermoDataRequester = RequestFactory.CreateRestService(targetBaseUrl, _stoppingToken, _logger);
            _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, _logger);
        }

        public async Task ExecuteAsync()
        {
            var targetRequest = RequestFactory.CreatePersonRequest(
                _restConfiguration.AttendanceUrl, new AttendanceRequest
            {
                StartId = 1,
                ReqCount = 20, 
                NeedImg = false
            });

            _logger.LogInformation($"Executing ThermoDataLogic {DateTimeOffset.Now}");
            // Get data 
            var result = await thermoDataRequester.GetAttendanceRecordAsync<IEnumerable<AttendRecord>>(targetRequest);
            // send message 
            //_logger.LogInformation($"{result.Data.Deviceid}");
            //_logger.LogInformation($"{result.Data.}");
            //_logger.LogInformation($"Sending message to service bus {DateTimeOffset.Now} : {MessageConverter.Serialize(result)} ");
            //await _messageSender.SendMessagesAsync(MessageConverter.Serialize(result));
            await _messageSender.SendMessagesAsync("mydatadatadat");
        }
    }
}