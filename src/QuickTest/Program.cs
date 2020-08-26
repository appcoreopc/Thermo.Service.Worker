using Service.MessageBusServiceProvider.AzBlob;
using Service.MessageBusServiceProvider.Imaging;
using Service.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuickTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var tpath = @"c:\temp\test1\tes2\test3\test.tmp";
            var x = Path.GetDirectoryName(tpath);

            // "WwKIZ+piHv/HoZIxFzsZFUfJQapQh4XpdU0a1/8nUGe8nTsfDm246097HxBAb0dink3r2O9sPcL4QTREk1+V334y1nAW16t1EIGoKXrpADwnAlYEp9S63D/Zfp3VcOfjG+CsfhuRDFVCY9/fzJ6OfoJuwBKvriFD2+0HPH3EGISknlEXIC7eIKUN0oluMD1m2pNhek4dY9suMGYvz4J1QnmzHoOtUiAh5grtMPAL29xSCdv13oOSjyzSlf/vVhfy"


            // "p6REkQCloU30wvs1ZGc0yGRLZYCFP2ne0dZIjOhRbJyDCmVxvDFIFh4UkB5e1wTFxMAEZCKfgA3cH1o3KyG46ecxfMzEmbnIrxNd1B5ZyUxFM1NJQ316fsa2JNzVXKXYdMRSUO5MaMerBGgUd9gPs6mtPR4c0OTuRhA3riyvvc5i5qKl7MzEO99jv92G3l++pgCzlsRzZUMGayRjvCu1ZSg9sToge7R84GGEBgFnu23azYLxRXeWQtIJTRLV7qOGyWFdkVq+08U3jYUm08uQOm++pTI0CzNR9IQ9iNtbDyk="

            var a = Encrypt.EncryptString("Endpoint=sb://subcriptioncust.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=X3eRpxbGtrGhXbX6FNEPBTwQ3y1sCNYUTAgqhB7hPis=");


            // c = "Endpoint=sb://subcriptioncust.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=X3eRpxbGtrGhXbX6FNEPBTwQ3y1sCNYUTAgqhB7hPis="
            var b = Encrypt.EncryptString("DefaultEndpointsProtocol=https;AccountName=sasubcriptioncust;AccountKey=PBY7ce03RTZHYtzZxUcuYMlkyjq/Q4QFL2wemeElK5q8RP8uPj6ix/Ck5tyAub27TgKD7SSzRqy2NgVoVPuuPQ==;EndpointSuffix=core.windows.net");
            //b = "G8e3r14rZzNiLMbOPD6Ocbmib14S5hYWqjEb1d7377FcZVHivF2skpfW8qM7nFNZ7Q0VckxsvFiDWHcFPu9KyZgQ8zfah4uTG1bEc2Jl4Vd9LEyi3LRGDFeW2JYBKkEOTJv/u644/ETEcOTGBmriDZKaMrUd6q2g5IlSI+FfgGWmN5WHPbCX6mQgqYdUpYMvAh7DkfTTxXofV5GayeuFYOf2RU85pklmOIDcH2mdCydBIeMG9Wzh/ATKSV/D/BCG...

            var c = Encrypt.Decrypt(a);


            var d = Encrypt.Decrypt(b);

            if (!Directory.Exists(x))
            {
               var e =  Directory.CreateDirectory(x);
            }
            //StoreImageToAzureStorage();

        }

        private static async Task StoreImageToAzureStorage()
        {
            try
            {
                var blob = new BlobClientProvider(null, null);
                await blob.PushImageToStoreAsync("attendance", @"c:\temp\1.png");
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                throw;
            }
        }
    }
}
