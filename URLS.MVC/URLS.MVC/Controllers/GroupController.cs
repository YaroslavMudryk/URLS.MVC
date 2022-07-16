using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLS.Models;
using URLS.MVC.Infrastructure.Services.Interfaces;

namespace URLS.MVC.Controllers
{
    [Authorize]
    [Route("group")]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search(string name, int count = 20, int offset = 0)
        {
            if (string.IsNullOrEmpty(name))
                return View(Enumerable.Empty<GroupViewModel>());
            var res = await _groupService.SearchGroupsAsync(name, offset, count);
            return View(res.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            var groupResult = await _groupService.GetGroupsByIdAsync(id);
            return View(groupResult);
        }

    }
}
