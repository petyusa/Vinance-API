using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Vinance.Api.Helpers;

namespace Vinance.Api.Controllers
{
    using Contracts.Models.Identity;
    using Contracts.Models.ServiceResults;
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

        /// <summary>
        /// Registers a new user, and sends the email confirmation token.
        /// </summary>
        /// <param name="registerModel">The user to be registered.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<TokenResult>))]
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewmodel registerModel)
        {
            var user = _mapper.Map<RegisterModel>(registerModel);
            var result = await _identityService.Register(user, registerModel.Password);
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

        /// <summary>
        /// Returns the token for the user.
        /// </summary>
        /// <param name="loginModel">The login model of the user.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<AuthToken>))]
        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> Login(LoginViewmodel loginModel)
        {
            var model = _mapper.Map<LoginModel>(loginModel);
            var result = await _identityService.GetToken(model);

            return Ok(result);
        }

        /// <summary>
        /// Returns a new auth-token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<AuthToken>))]
        [AllowAnonymous]
        [HttpPost]
        [Route("token/refresh")]
        public IActionResult RefreshToken(RefreshTokenViewModel refreshToken)
        {
            return Ok(_identityService.RefreshToken(refreshToken.Token));
        }

        /// <summary>
        /// Changes the password for the user..
        /// </summary>
        /// <param name="passwordChangeModel">The model for changing the password.</param>
        [SwaggerResponse(204)]
        [HttpPut]
        [Route("password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeViewmodel passwordChangeModel)
        {
            var model = _mapper.Map<PasswordChangeModel>(passwordChangeModel);
            var result = await _identityService.ChangePassword(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        /// <summary>
        /// Returns a password-change token.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<TokenResult>))]
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

        /// <summary>
        /// Changes the password of the user.
        /// </summary>
        /// <param name="passwordResetModel">The model for changing the password.</param>
        [SwaggerResponse(204)]
        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(PasswordResetViewmodel passwordResetModel)
        {
            var model = _mapper.Map<PasswordResetModel>(passwordResetModel);
            var result = await _identityService.ResetPassword(model);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Confirms the email of the user.
        /// </summary>
        /// <param name="emailConfirmationModel">The model for email confirmation.</param>
        [SwaggerResponse(204)]
        [HttpPost]
        [AllowAnonymous]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationViewmodel emailConfirmationModel)
        {
            var model = _mapper.Map<EmailConfirmationModel>(emailConfirmationModel);
            var result = await _identityService.ConfirmEmail(model);
            if (result)
            {
                return NoContent();
            }

            return BadRequest("There was an error confirming the email");
        }

        /// <summary>
        /// Returns the token required for changing the email address.
        /// </summary>
        /// <param name="newEmail">The new email address.</param>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<TokenResult>))]
        [HttpGet]
        [Route("email/{newEmail}")]
        public async Task<IActionResult> ChangeEmailToken(string newEmail)
        {
            var token = await _identityService.GetEmailChangeToken(newEmail);
            return Ok(token);
        }

        /// <summary>
        /// Changes the email address of the user.
        /// </summary>
        /// <param name="emailChangeModel">The model for changing the email address.</param>
        [SwaggerResponse(204)]
        [HttpPost]
        [Route("email/{newEmail}")]
        public async Task<IActionResult> ChangeEmail(EmailChangeViewmodel emailChangeModel)
        {
            var model = _mapper.Map<EmailChangeModel>(emailChangeModel);
            var result = await _identityService.ChangeEmail(model);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Returns user details.
        /// </summary>
        [SwaggerResponse(200, Type = typeof(VinanceApiResponseExample<VinanceUserViewmodel>))]
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