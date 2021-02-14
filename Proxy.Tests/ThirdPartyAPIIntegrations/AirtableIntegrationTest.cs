using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Tests.ThirdPartyIntegrations
{
    public class AirtableIntegrationTest
    {
        private HttpClient _client;

        [SetUp]
        public void OneTimeSetUp()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://api.airtable.com/v0/appD1b1YjWoXkUJwR/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "key46INqjpp7lMzjd");         
        }

        [TestCase("1", "Grid+view")]
        public async Task GetMessages_FromAirtableAPI_IntegrationTest(string maxRecords, string view)
        {
            //Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"messages?maxRecords={maxRecords}&view={view}");
            //Act
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.NotNull(result);
            Assert.AreEqual("OK", response.StatusCode.ToString());
        }
    }
}
