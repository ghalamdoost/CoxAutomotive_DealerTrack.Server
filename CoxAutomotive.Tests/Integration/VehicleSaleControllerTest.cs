using CoxAutomotive.Features.Identity.Models;
using CoxAutomotive.Features.VehicleSale.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoxAutomotive.Tests.Integration
{
    public class VehicleSaleControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public VehicleSaleControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task VehicleSale_UploadFile_GetOk()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var user = new StringContent(JsonConvert.SerializeObject(new { UserName = await this.generateUserName(), Email = "test@gmail.com", password = "test12" }), UnicodeEncoding.UTF8, "application/json");
            await client.PostAsync("/Identity/Register", user);
            var loginResponse = await client.PostAsync("/Identity/Login", user);
            var responseHeaderString = loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<LoginResponseModel>(responseHeaderString.Result);
            var content = await GetContent("Ok.csv");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            var response = await client.PostAsync("/VehicleSale/UploadFile", content);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ResponseModel<VehicleSaleResponseModel>>(responseString);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(model.IsSucceeded);
            Assert.NotNull(model.Result.List);
            Assert.True(model.Result.MostOftenSoldVehicle == "2018 Jeep Grand Cherokee Trackhawk");
            Assert.True(model.Result.List.First().Price == 429987M);
        }

        [Fact]
        public async Task VehicleSale_UploadFile_GetBadRequest()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var user = new StringContent(JsonConvert.SerializeObject(new { UserName = await this.generateUserName(), Email = "test@gmail.com", password = "test12" }), UnicodeEncoding.UTF8, "application/json");
            await client.PostAsync("/Identity/Register", user);
            var loginResponse = await client.PostAsync("/Identity/Login", user);
            var responseHeaderString = loginResponse.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<LoginResponseModel>(responseHeaderString.Result);
            var content = await GetContent("InvalidType.txt");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            var response = await client.PostAsync("/VehicleSale/UploadFile", content);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        private async Task<MultipartFormDataContent> GetContent(string fileName)
        {
            var filePath = Path.Combine("Files", fileName);
            var bytes = await File.ReadAllBytesAsync(filePath);
            var fileContent = new StreamContent(new MemoryStream(bytes));
            var form = new MultipartFormDataContent { { fileContent, "file", fileName } };
            return form;
        }

        private async Task<string> generateUserName()
        {
            int length = 7;

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
    }
}
