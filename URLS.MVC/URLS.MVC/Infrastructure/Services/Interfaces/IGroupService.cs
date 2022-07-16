using URLS.Models;
using URLS.MVC.Models;

namespace URLS.MVC.Infrastructure.Services.Interfaces
{
    public interface IGroupService
    {
        Task<Result<List<GroupViewModel>>> SearchGroupsAsync(string name, int offset, int count);
        Task<Result<GroupViewModel>> GetGroupsByIdAsync(int id);
    }
}