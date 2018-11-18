using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Models.Identity;
    using Identity.Interfaces;
    using Viewmodels.Identity;

    [Route("users")]
    [ApiController]
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
            var user = _mapper.Map<RegisterModel>(model);
            var result = await _identityService.Register(user, model.Password);
            var res = result.ToString();
            if (!result.Succeeded)
            {
                return BadRequest(res);
            }

            var token = await _identityService.GetEmailConfirmationToken(user.Email);
            if (token.Succeeded)
            {
                return Ok(token);
            }
            return BadRequest();
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
                return Ok(result);
            }
            return Forbid();
        }

        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewmodel changeViewmodel)
        {
            var model = _mapper.Map<PasswordChangeModel>(changeViewmodel);
            var result = await _identityService.ChangePassword(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var user = await _identityService.GetCurrentUser();
            var viewmodel = _mapper.Map<VinanceUserViewmodel>(user);
            return Ok(viewmodel);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password-token")]
        public async Task<IActionResult> ResetPassword([FromBody]string email)
        {
            var result = await _identityService.GetPasswordResetToken(email);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest();
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
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewModel viewmodel)
        {
            var model = _mapper.Map<EmailConfirmationModel>(viewmodel);
            var result = await _identityService.ConfirmEmail(model);
            if (result)
            {
                return NoContent();
            }

            return BadRequest("There was an error confirming the email");
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
            var viewmodel = _mapper.Map<VinanceUserViewmodel>(user);
            return Ok(viewmodel);
        }
    }
}