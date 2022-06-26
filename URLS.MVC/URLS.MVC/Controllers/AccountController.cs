using Extensions.DeviceDetector;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using URLS.Models;
using URLS.MVC.Infrastructure.Services.Interfaces;
using URLS.MVC.Infrastructure.Settings;
using URLS.MVC.Models;

namespace URLS.MVC.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authClient;
        private readonly IDetector _detector;
        public AccountController(IAuthService authClient, IDetector detector)
        {
            _authClient = authClient;
            _detector = detector;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCreateModel createModel)
        {
            try
            {
                var result = await _authClient.LoginAsync(createModel);

                if (!result.Ok)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(createModel);
                }

                await Authenticate(result.Data);

                return LocalRedirect("~/");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(createModel);
            }
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authClient.LogoutAsync();
            if (result.Ok)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Bunners = new List<Banner>
            {
                new Banner
                {
                    IsError = true,
                    Text = result.Message
                }
            };
            return LocalRedirect("~/");
        }

        [HttpGet("sessions")]
        public async Task<IActionResult> AllSessions(int q = 0)
        {
            try
            {
                var sessions = await _authClient.GetSessionsAsync(q, 0, 10);
                return View(sessions.Data);
            }
            catch(Exception)
            {
                return View();
            }
        }

        [HttpPost("sessions/{id}")]
        public async Task<IActionResult> CloseSession(Guid id)
        {
            try
            {
                var res = await _authClient.CloseSessionByIdAsync(id);

                var d = res.Data;

                return LocalRedirect("~/account/sessions");
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [NonAction]
        private async Task Authenticate(JwtToken jwtToken)
        {
            var claims = jwtToken.Claims.Select(s => new Claim(s.Type, s.Value)).ToList();
            claims.Add(new Claim(Constants.JwtCookieKey, jwtToken.Token));

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
