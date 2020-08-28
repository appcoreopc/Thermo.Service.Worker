namespace AzCloudApp.MessageProcessor.Core.EmailSummary.Model
{
    public class EmailSummaryConfiguration
    {
        public string EmailTemplate { get; set; }

        public string Sender { get; set; }

        public string SenderName { get; set; }

        public string Subject { get; set; }

        public double MaxTemperature { get; set; }

        public string TargetDate { get; set; }

        public double MinTemperature { get; set; }

    }
}
