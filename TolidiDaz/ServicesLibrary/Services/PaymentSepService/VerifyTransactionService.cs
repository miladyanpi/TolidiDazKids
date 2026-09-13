using Dto.Models.DtpPayment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary.Services.PaymentSepService
{
    public class VerifyTransactionService(
        IHttpClientFactory _httpClientFactory
        )
    {
        public async Task<PaymentSepResult> VerifyTransaction(PaymentResponse paymentResponse)
        {
            var client = _httpClientFactory.CreateClient();

            var url =ApiLink.VerifyTransaction;
            var requestBody = new
            {
                RefNum = paymentResponse.RefNum,
                TerminalNumber = long.Parse(paymentResponse.TerminalId),
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PaymentSepResult>(responseText);

        }
    }
}
