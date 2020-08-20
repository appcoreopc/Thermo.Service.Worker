using System.Collections.Generic;
using AzCloudApp.MessageProcessor.Core.EmailSummary.Model;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface ISummaryEmailProviderDataProcessor
    {
        IEnumerable<CompanyTotalScanResult> GetTotalScansByCompany(QueryTotalScanParam param);

        IEnumerable<string> GetRecipientsByCompanyId(int companyId);

        IEnumerable<AbnornormalScanResult> GetTotalAbnormalScanByCompany(
            QueryTotalScanParam param);
    }
}
