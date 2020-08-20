namespace AzCloudApp.MessageProcessor.Core.EmailSummary.Model
{
    public class CompanyTotalScanResult
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }

        public int TotalAbnormalScan { get; set; }
    }
}
