using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RoadsideAssistanceApi.Dto;
using RoadsideAssistanceBL.Models;
using System.Net;
using System.Text;

namespace RoadsideAssistanceApi.Tests
{
    public class RoadsideAssistanceApiTests
    {
        private readonly WebApplicationFactory<Program> _webAppFactory;
        private readonly HttpClient _httpClient;
       
        public RoadsideAssistanceApiTests()
        {
            _webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = _webAppFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task TestFindNearestAssistants()
        {
            var location = new Geolocation(2, 1);
            var request = JsonConvert.SerializeObject(location);
            var httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/roadsideAssistance/findNearestAssistants/3", httpContent);
            string content = await response.Content.ReadAsStringAsync();
            var assitants = JsonConvert.DeserializeObject<List<Assistant>>(content);
            Assert.True(assitants.Count > 0, "Assistants returned as empty");
        }

        [Fact]
        public async Task TestReserveAssistant()
        {
            var reserveAssistantDto = new ReserveAssistantDto();
            reserveAssistantDto.Customer = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "xyz"
                    },
                    ProblemDescription = "flat tire"
                }
            };
            reserveAssistantDto.Geolocation = new Geolocation(2, 1);

            var request = JsonConvert.SerializeObject(reserveAssistantDto);
            var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/roadsideAssistance/reserveAssistant", httpContent);
            string content = await response.Content.ReadAsStringAsync();
            var assitant = JsonConvert.DeserializeObject<Assistant>(content);
            Assert.NotNull(assitant);
        }

        [Fact]
        public async Task TestReleaseAssistant()
        {
            var releaseAssistantDto = new ReleaseAssistantDto();
            releaseAssistantDto.Customer = new Customer()
            {
                Id = 1,
                Name = "Jack",
                serviceRequest = new ServiceRequest()
                {
                    Id = 1001,
                    Vehicle = new Vehicle()
                    {
                        LicensePlate = "xyz"
                    },
                    ProblemDescription = "flat tire"
                }
            };
            releaseAssistantDto.Assistant = new Assistant
            {
                Id = 1,
                Name = "Rajesh",
            };
            var request = JsonConvert.SerializeObject(releaseAssistantDto);
            var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/roadsideAssistance/releaseAssistant", httpContent);
            Assert.True(response.StatusCode == HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task TestUpdateAssistantLocation()
        {
            var updateAssistantLocationDto = new UpdateAssistantLocationDto();
            updateAssistantLocationDto.Assistant = new Assistant
            {
                Id = 1,
                Name = "Rajesh",
            };
            updateAssistantLocationDto.Geolocation = new Geolocation(2, 1);
            var request = JsonConvert.SerializeObject(updateAssistantLocationDto);
            var httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/roadsideAssistance/updateAssistantLocation", httpContent);
            Assert.True(response.StatusCode == HttpStatusCode.NoContent);
        }

    }
}