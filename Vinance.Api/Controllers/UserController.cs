using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Vinance.Api.Controllers
{
    using Contracts.Models.Identity;
    using Identity;
    using Viewmodels;

    [Route("api/users")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<VinanceUser> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public UserController(UserManager<VinanceUser> userManager, IMapper mapper, IIdentityService identityService)
        {
            _userManager = userManager;
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
            var result = await _identityService.GetToken(model);
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
            if (result)
            {
                return Ok("password changed");
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody]string email)
        {
            var token = await _identityService.ResetPassword(email);
            return Ok(token);
        }

        public async Task<IActionResult> ResetPassword(string token, string pass)
        {
            var user = await _userManager.FindByEmailAsync(pass);
            var result = await _userManager.ResetPasswordAsync(user, token, pass);
            if (result.Succeeded)
            {
                return Ok("password changed");
            }
            return BadRequest("invalid data");
        }

        [HttpPost]
        [Route("email")]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            return Ok(token);
        }

        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            return Ok(user);
        }
    }
}