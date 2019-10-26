using Microsoft.Extensions.Configuration;
using PaymentGateway.Enums;
using PaymentGateway.ExternalServices;
using PaymentGateway.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace PaymentGateway.Test.Integration
{
    public class EndToEndTests
    {
        private readonly IConfigurationRoot _config;

        public EndToEndTests()
        {
            // Read configuration from the appsettings.json file
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            _config = configurationBuilder.Build();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async void Submit_a_valid_payment_and_check_it_exists_in_the_database()
        {
            using var client = new HttpClient { BaseAddress = new Uri(_config["HostUrl"]) };
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
