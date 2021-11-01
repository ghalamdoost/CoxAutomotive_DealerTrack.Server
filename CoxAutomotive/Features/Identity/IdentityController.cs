using CoxAutomotive.Data.Models;
using CoxAutomotive.Features.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace CoxAutomotive.Features.Identity
{
   
    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identityService;
        private readonly AppSettings appSettings;
        private readonly ILogger<IdentityController> logger;

        public IdentityController(UserManager<User> userManager, IIdentityService identityService, IOptions<AppSettings> appSettings, ILogger<IdentityController> logger) 
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            try
            {                
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName
                };

                var result = await this.userManager.CreateAsync(user, model.Password);                

                if (result.Succeeded)
                {
                    this.logger.LogInformation("new user has been registered, UserId:" + user.Id);
                    return Ok();
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex}");
            }            
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            try
            {
                var user = await this.userManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    return Unauthorized();
                }

                var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);

                if (!passwordValid)
                {
                    return Unauthorized();
                }


                var token = await this.identityService.GenerateJwtToken(user.Id, user.UserName, this.appSettings.Secret);
                this.logger.LogInformation($"A token has been generated for UserId:{user.Id}");
                this.logger.LogInformation($"UserId:{user.Id} loged in");

                return new LoginResponseModel
                {
                    Token = token
                };
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Internal server error: {ex}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
            
        }
    }
}
