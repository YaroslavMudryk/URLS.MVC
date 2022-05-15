using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using URLS.Api;
using URLS.Api.Models;
using URLS.MVC.Models;

namespace URLS.MVC.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUrlsClient _urlsClient;
        public AccountController(IUrlsClient urlsClient)
        {
            _urlsClient = urlsClient;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCreateModel createModel)
        {
            var result = await _urlsClient.LoginAsync(createModel);

            if (!result.Ok)
            {
                ModelState.AddModelError("", result.Error);
            }

            await Authenticate(result.Data);

            return LocalRedirect("~/");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _urlsClient.LogoutAsync();
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
                    Text = result.Error
                }
            };
            return LocalRedirect("~/");
        }

        [NonAction]
        private async Task Authenticate(JwtToken jwtToken)
        {
            var claims = jwtToken.Claims.Select(s => new Claim(s.Type, s.Value));

            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
