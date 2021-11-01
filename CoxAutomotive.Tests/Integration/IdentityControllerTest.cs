
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace CoxAutomotive.Tests.Integration
{
    public class IdentityControllerTest: IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public IdentityControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }
        
        [Fact]
        public async Task Identity_Register_GetOk()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.Complete);
            var response = await client.PostAsync("/Identity/Register", content);
            //Assert

            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }


        [Fact]
        public async Task Identity_Register_GetBadRequestWithoutUserName()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.WithoutUserName);
            var response = await client.PostAsync("/Identity/Register", content);
            //Assert

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Identity_Register_GetBadRequestWithoutEmail()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.WithoutEmail);
            var response = await client.PostAsync("/Identity/Register", content);
            //Assert

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Identity_Register_GetBadRequestWithoutPassword()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.WithoutPassword);
            var response = await client.PostAsync("/Identity/Register", content);
            //Assert

            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Identity_Login_GetOk()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.Complete);
            var registrationResponse = await client.PostAsync("/Identity/Register", content);
            var loginResponse = await client.PostAsync("/Identity/Login", content);

            //Assert
            Assert.True(registrationResponse.StatusCode == HttpStatusCode.OK);
            Assert.True(loginResponse.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task Identity_Login_GetUnauthorized()
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var content = await NewUser(UserRequestModelEnum.Complete);            
            var loginResponse = await client.PostAsync("/Identity/Login", content);

            //Assert
            Assert.True(loginResponse.StatusCode == HttpStatusCode.Unauthorized);
        }

        private async Task<StringContent> NewUser(UserRequestModelEnum type)
        {
            var user="";
            switch (type)
            {
                case UserRequestModelEnum.Complete:
                    user = JsonConvert.SerializeObject(new { UserName = await this.generateUserName(), Email = "test@gmail.com", password = "test12" });
                    break;
                case UserRequestModelEnum.WithoutEmail:
                    user = JsonConvert.SerializeObject(new { UserName = await this.generateUserName(), password = "test12" });
                    break;
                case UserRequestModelEnum.WithoutPassword:
                    user = JsonConvert.SerializeObject(new { UserName = await this.generateUserName(), Email = "test@gmail.com"});
                    break;
                case UserRequestModelEnum.WithoutUserName:
                    user = JsonConvert.SerializeObject(new { Email = "test@gmail.com", password = "test12" });
                    break;
                default:
                    break;
            }
            
            return new StringContent(user, UnicodeEncoding.UTF8, "application/json");
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
