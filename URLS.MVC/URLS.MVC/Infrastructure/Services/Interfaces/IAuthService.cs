using URLS.Models;
using URLS.MVC.Models;

namespace URLS.MVC.Infrastructure.Services.Interfaces
{
    public interface IAuthService
    {
        Task<List<ClaimViewModel>> GetClaimsAsync(string token = null);
        Task<Result<UserFullViewModel>> GetMeAsync();
        Task<Result<List<SocialViewModel>>> GetUserSocialsAsync();
        Task<Result<bool>> RemoveSocialAsync(int id);
        Task<Result<MFAViewModel>> EnableMFAAsync(string code = null);
        Task<Result<bool>> DisableMFAAsync(string code);
        Task<Result<JwtToken>> LoginByMFAAsync(LoginMFAModel model);
        Task<Result<JwtToken>> LoginAsync(LoginCreateModel model);
        Task<Result<bool>> RegistrationAsync(RegisterViewModel registerViewModel);
        Task<Result<bool>> LogoutAsync();
        Task<Result<UserViewModel>> ConfigUserAsync(BlockUserModel model);
        Task<Result<List<SessionViewModel>>> GetSessionsAsync(int q, int offset = 0, int limit = 10);
        Task<Result<SessionViewModel>> GetSessionByIdAsync(Guid id);
        Task<Result<bool>> CloseSessionsAsync(bool withCurrent = false);
        Task<Result<bool>> CloseSessionByIdAsync(Guid id);
        HttpClient HttpClient { get; }
    }
}