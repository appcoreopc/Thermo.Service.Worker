using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public interface ISummaryMailContentParser
    {
        MailContentData CreateSummaryEmailAlertMessage(ParseEmailParam param, ILogger logger);
    }
}
