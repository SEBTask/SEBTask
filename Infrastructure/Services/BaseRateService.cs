using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Domain.Contracts.Services;
using Domain.Entities;
using Domain.Exceptions;

namespace Infrastructure.Services
{
    public class BaseRateService : IBaseRateService
    {
        private readonly HttpClient httpClient;
        public BaseRateService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<decimal> GetBaseRate(BaseRateCodeEnum baseRateCode)
        {
            var endpoint = $"http://www.lb.lt/webservices/VilibidVilibor/VilibidVilibor.asmx/getLatestVilibRate?RateType={baseRateCode.ToString()}";
            var response = await this.httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new IntegrationFailedException("Failed to retrieve the latest Vilib rate from www.lb.lt");
            }

            var content = await response.Content.ReadAsStringAsync();
            XDocument xmlContent = XDocument.Parse(content);
            var baseRateAsString = (xmlContent.FirstNode as XElement).Value;
            var baseRate = decimal.Parse(baseRateAsString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return baseRate;
        }
    }
}
