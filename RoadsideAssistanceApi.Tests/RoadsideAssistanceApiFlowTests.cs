using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RoadsideAssistanceApi.Dto;
using RoadsideAssistanceBL.Models;
using System.Net;
using System.Text;

namespace RoadsideAssistanceApi.Tests
{
    public class RoadsideAssistanceApiFlowTests
    {
        private readonly WebApplicationFactory<Program> _webAppFactory;
        private readonly HttpClient _httpClient;

        public RoadsideAssistanceApiFlowTests()
        {
            _webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = _webAppFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task MainFlowTests()
        {
            /*
                * Main flow
                * Step 1: Check if any available assistants near by where an assistance needed
                * Step 2: Customer creates a roadside assitance service request
                * Step 3: resserve an assitant
                * Step 4: Once the job done, release the assitant
                * Step 5: assistant update his location
            */

            //Step 1: Check if any available assistants near by where an assistance needed
            var location = new Geolocation(2, 1);
            var request = JsonConvert.SerializeObject(location);
            var httpContent = new StringContent(request, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/roadsideAssistance/findNearestAssistants/3", httpContent);
            string content = await response.Content.ReadAsStringAsync();
            var assitants = JsonConvert.DeserializeObject<List<Assistant>>(content);


            //Step 2: Customer creates a roadside assitance service request
            var customer = new Customer()
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
            var reserveAssistantDto = new ReserveAssistantDto();
            reserveAssistantDto.Customer = customer;
            reserveAssistantDto.Geolocation = new Geolocation(2, 1);

            //Step 3: resserve an assitant
            request = JsonConvert.SerializeObject(reserveAssistantDto);
            httpContent = new StringContent(request, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("/api/roadsideAssistance/reserveAssistant", httpContent);
            content = await response.Content.ReadAsStringAsync();
            var assitant = JsonConvert.DeserializeObject<Assistant>(content);
            Assert.NotNull(assitant);

            if(assitant != null)
            {
                //Step 4: Once the job done, release the assitant
                var releaseAssistantDto = new ReleaseAssistantDto();
                releaseAssistantDto.Customer = customer;
                releaseAssistantDto.Assistant = assitant;
                request = JsonConvert.SerializeObject(releaseAssistantDto);
                httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                response = await _httpClient.PutAsync("/api/roadsideAssistance/releaseAssistant", httpContent);
                Assert.True(response.StatusCode == HttpStatusCode.NoContent);

                //Step 5: assistant update his location
                var updateAssistantLocationDto = new UpdateAssistantLocationDto();
                updateAssistantLocationDto.Geolocation = new Geolocation(2, 1);
                updateAssistantLocationDto.Assistant = assitant;
                request = JsonConvert.SerializeObject(updateAssistantLocationDto);
                httpContent = new StringContent(request, Encoding.UTF8, "application/json");
                response = await _httpClient.PutAsync("/api/roadsideAssistance/updateAssistantLocation", httpContent);
                Assert.True(response.StatusCode == HttpStatusCode.NoContent);
            }
        }
    }
}
