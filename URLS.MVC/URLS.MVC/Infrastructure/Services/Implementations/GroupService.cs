using System.Net.Http.Headers;
using URLS.Models;
using URLS.MVC.Infrastructure.Extensions;
using URLS.MVC.Infrastructure.Services.Interfaces;
using URLS.MVC.Models;

namespace URLS.MVC.Infrastructure.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly HttpClient _httpClient;
        public GroupService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", httpContextAccessor.HttpContext.GetBearerToken());
        }

        public async Task<Result<GroupViewModel>> CreateGroupAsync(GroupCreateModel model)
        {
            return await _httpClient.PostFromJsonAsync<Result<GroupViewModel>>($"api/v1/groups", model);
        }

        public async Task<Result<List<SpecialtyViewModel>>> GetAllSpecialtiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<Result<List<SpecialtyViewModel>>>("api/v1/specialties");
        }

        public async Task<Result<GroupViewModel>> GetGroupsByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Result<GroupViewModel>>($"api/v1/groups/{id}");
        }

        public async Task<Result<List<GroupViewModel>>> SearchGroupsAsync(string name, int offset, int count)
        {
            return await _httpClient.GetFromJsonAsync<Result<List<GroupViewModel>>>($"api/v1/groups/search?offset={offset}&count={count}");
        }
    }
}