using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Linq;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class CompanyDataProcessor : ICompanyDataProcessor
    {
        private ThermoDataContext _thermoDataContext;
        public CompanyDataProcessor(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public CompanyDataStore GetCompanySettings(int companyId)
        {
            return _thermoDataContext.Company.Where(x => x.Nid == companyId).ToList().FirstOrDefault(); 
        }
    }

    public interface ICompanyDataProcessor
    {
        CompanyDataStore GetCompanySettings(int companyId);
    }
}
