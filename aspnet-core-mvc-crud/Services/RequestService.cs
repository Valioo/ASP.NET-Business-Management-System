using aspnet_core_mvc_crud.Data;
using aspnet_core_mvc_crud.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace aspnet_core_mvc_crud.Services
{
    public class RequestService
    {
        private readonly RequestDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        public RequestService(RequestDbContext context, UserManager<ApplicationUser> userManager/*, RoleManager<IdentityRole> roleManager*/)
        {
            //_roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<List<ServiceRequest>> GetAllServiceRequests()
        {
            return await _context.ServiceRequests.ToListAsync();
        }
        public async Task<IEnumerable<ServiceRequest>> GetAllRequestsForTech(string techId)
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return await _context.ServiceRequests.Where(a => a.AssignedTechnicianId == techId).ToListAsync();
        }
        public async Task<List<ServiceRequest>> GetUserTasks(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new List<ServiceRequest>();
            }
            return await _context.ServiceRequests.Where(a => a.CreatorId == user.Id).ToListAsync();
        }

        public async Task ChangeTaskInfo(ServiceRequest newReq, string reqId)
        {
            var request = await _context.ServiceRequests.FirstOrDefaultAsync(x => x.Id == int.Parse(reqId));
            _context.Entry(request).CurrentValues.SetValues(newReq);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}
