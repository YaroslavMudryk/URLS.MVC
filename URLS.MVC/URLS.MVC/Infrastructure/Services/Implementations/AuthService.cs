using Extensions.DeviceDetector;
using System.Net.Http.Headers;
using URLS.Models;
using URLS.MVC.Infrastructure.Extensions;
using URLS.MVC.Infrastructure.Services.Interfaces;
using URLS.MVC.Models;

namespace URLS.MVC.Infrastructure.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IDetector _detector;
        public AuthService(HttpClient httpClient, IDetector detector, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _detector = detector;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext.GetBearerToken());
        }

        public HttpClient HttpClient => _httpClient;

        public async Task<Result<bool>> CloseSessionByIdAsync(Guid id)
        {
            return await _httpClient.DeleteFromJsonAsync<Result<bool>>($"api/v1/account/sessions/{id}");
        }

        public async Task<Result<bool>> CloseSessionsAsync(bool withCurrent = false)
        {
            return await _httpClient.DeleteFromJsonAsync<Result<bool>>($"api/v1/account/sessions?withCurrent={withCurrent}");
        }

        public async Task<Result<UserViewModel>> ConfigUserAsync(BlockUserModel model)
        {
            return await _httpClient.PostFromJsonAsync<Result<UserViewModel>>("api/v1/account/config", model);
        }

        public async Task<Result<bool>> DisableMFAAsync(string code)
        {
            return await _httpClient.DeleteFromJsonAsync<Result<bool>>($"api/v1/account/mfa/{code}");
        }

        public async Task<Result<MFAViewModel>> EnableMFAAsync(string code = null)
        {
            var url = "api/v1/account/mfa";
            if (!string.IsNullOrEmpty(code))
                url += $"?code={code}";
            return await _httpClient.PostFromJsonAsync<Result<MFAViewModel>>(url, null);
        }

        public async Task<List<ClaimViewModel>> GetClaimsAsync(string token = null)
        {
            if(!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<List<ClaimViewModel>>("api/v1/account/claims");
        }

        public async Task<Result<UserFullViewModel>> GetMeAsync()
        {
            return await _httpClient.GetFromJsonAsync<Result<UserFullViewModel>>("api/v1/account/me");
        }

        public async Task<Result<SessionViewModel>> GetSessionByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<Result<SessionViewModel>>($"api/v1/account/sessions/{id}");
        }

        public async Task<Result<List<SessionViewModel>>> GetSessionsAsync(int q, int offset = 0, int limit = 10)
        {
            return await _httpClient.GetFromJsonAsync<Result<List<SessionViewModel>>>($"api/v1/account/sessions?q={q}&offset={offset}&limit={limit}");
        }

        public async Task<Result<List<SocialViewModel>>> GetUserSocialsAsync()
        {
            return await _httpClient.GetFromJsonAsync<Result<List<SocialViewModel>>>($"api/v1/account/socials");
        }

        public async Task<Result<JwtToken>> LoginAsync(LoginCreateModel model)
        {
            model.App = new AppLoginCreateModel
            {
                Version = "0.1",
                Id = "x3Vw-QFdk-Pt0B-7jjd",
                Secret = "DlST6311QjEfEoUX0JJAsjQGmHrBlUl8qjhXMCaJFJrlTiv0Fn0PcWqOPCEfsrEZuSfvMj"
            };
            model.Lang = "uk";
            model.Client = _detector.GetClientInfo();
            return await _httpClient.PostFromJsonAsync<Result<JwtToken>>("api/v1/account/login", model);
        }

        public async Task<Result<JwtToken>> LoginByMFAAsync(LoginMFAModel model)
        {
            return await _httpClient.PostFromJsonAsync<Result<JwtToken>>("api/v1/account/login/mfa", model);
        }

        public async Task<Result<bool>> LogoutAsync()
        {
            return await _httpClient.PostFromJsonAsync<Result<bool>>("api/v1/account/logout", null);
        }

        public async Task<Result<bool>> RegistrationAsync(RegisterViewModel registerViewModel)
        {
            return await _httpClient.PostFromJsonAsync<Result<bool>>("api/v1/account/registration", registerViewModel);
        }

        public async Task<Result<bool>> RemoveSocialAsync(int id)
        {
            return await _httpClient.DeleteFromJsonAsync<Result<bool>>($"api/v1/account/socials/{id}");
        }
    }
}