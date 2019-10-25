using PaymentGateway.Enums;
using PaymentGateway.ExternalServices;
using PaymentGateway.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace PaymentGateway.Test.Integration
{
    public class EndToEndTests
    {
        [Fact]
        public async void Submit_a_valid_payment_and_check_it_exists_in_the_database()
        {
            // Todo - needs url
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44355");

            var request = new PaymentPostDto
            {
                CardNumber = "5555555555554444",
                ExpiryDate = DateTime.Now.AddDays(10),
                Amount = 10,
                Currency = "GBP",
                CVV = "123"
            };

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("/api/payment", content);

            var responseString = await postResponse.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<BankResponse>(responseString, jsonOptions);


            var getResponse = await client.GetAsync($"/api/payment/paymentId/{responseData.PaymentId}");
            var getResponseString = await getResponse.Content.ReadAsStringAsync();
            var getResponseData = JsonSerializer.Deserialize<PaymentGateway.Test.Integration.PaymentGetDto>(getResponseString, jsonOptions);

            Assert.Equal(responseData.PaymentId, getResponseData.PaymentId);
            Assert.Equal(PaymentStatus.Success.ToString(), getResponseData.PaymentStatus);
            Assert.Equal("************4444", getResponseData.CardNumber);
            Assert.Equal(request.ExpiryDate.Date, getResponseData.ExpiryDate);
            Assert.Equal(10, getResponseData.Amount);
            Assert.Equal("GBP", getResponseData.Currency);
            Assert.Equal("123", getResponseData.CVV);
        }
    }
}
