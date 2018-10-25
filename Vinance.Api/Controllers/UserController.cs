﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vinance.Api.Viewmodels.Identity;

namespace Vinance.Api.Controllers
{
    using Contracts.Models.Identity;
    using Identity;

    [Route("api/users")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public UserController(IIdentityService identityService, IMapper mapper)
        {
            _mapper = mapper;
            _identityService = identityService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewmodel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
            }

            var user = _mapper.Map<VinanceUser>(model);
            var result = await _identityService.Register(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Success");
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Login(LoginViewmodel loginModel)
        {
            var model = _mapper.Map<LoginModel>(loginModel);
            var result = await _identityService.GetAccessToken(model);
            if (result.Succeeded)
            {
                return Ok(result.Token);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewmodel changeViewmodel)
        {
            var model = _mapper.Map<PasswordChangeModel>(changeViewmodel);
            var result = await _identityService.ChangePassword(model);
            if (result.Succeeded)
            {
                return Ok("password changed");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]string email)
        {
            var token = await _identityService.GetPasswordResetToken(email);
            return Ok(token);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetViewmodel resetModel)
        {
            var model = _mapper.Map<PasswordResetModel>(resetModel);
            var result = await _identityService.ResetPassword(model);
            if (result.Succeeded)
            {
                return Ok("password changed");
            }
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("email/{newEmail}")]
        public async Task<IActionResult> ChangeEmailToken(string newEmail)
        {
            var token = await _identityService.GetEmailChangeToken(newEmail);
            return Ok(token);
        }

        [HttpGet]
        [Route("email/{newEmail}")]
        public async Task<IActionResult> ChangeEmail(EmailChangeViewmodel emailChangeViewmodel)
        {
            var model = _mapper.Map<EmailChangeModel>(emailChangeViewmodel);
            var result = await _identityService.ChangeEmail(model);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> Details()
        {
            var user = await _identityService.GetCurrentUser();
            return Ok(user);
        }
    }
}