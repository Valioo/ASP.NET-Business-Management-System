using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aspnet_core_mvc_crud.Data;
using aspnet_core_mvc_crud.Models;
using Microsoft.AspNetCore.Authorization;
using aspnet_core_mvc_crud.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace aspnet_core_mvc_crud.Views.Tasks
{
    public class ServiceRequestsController : Controller
    {
        private readonly RequestDbContext _context;
        private readonly RequestService _requestService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServiceRequestsController(RequestDbContext context, RequestService requestService, UserManager<ApplicationUser> umanager)
        {
            _userManager = umanager;
            _requestService = requestService;
            _context = context;
        }

        // GET: ServiceRequests
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;
            var user = await GetUserFromUsername(username);
            if (User.IsInRole("Admin"))
                return View(await _context.ServiceRequests.ToListAsync());
            if (User.IsInRole("Tech"))
            {
                return View(await _requestService.GetAllRequestsForTech(user.UserId));
            }
            if (User.IsInRole("Customer"))
            {
                return View(await _requestService.GetUserTasks(user.UserId));
            }
            else
            {
                return NotFound();
            }
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ServiceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Address,Picture,Status,DateOfVisit,AssignedTechnicianId,CreatorId")] ServiceRequest serviceRequest)
        {

            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var user = await GetUserFromUsername(username);
                serviceRequest.CreatorId = user.UserId;
                _context.Add(serviceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        [Authorize(Roles = "Admin, Tech")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return NotFound();
            }
            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Address,Picture,Status,DateOfVisit,AssignedTechnicianId,CreatorId")] ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceRequestExists(serviceRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _context.ServiceRequests
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.Id == id);
        }

        private async Task<UserRolesViewModel> GetUserFromUsername(string uname)
        {
            try
            {
                UserRolesViewModel userRolesViewModel = new UserRolesViewModel();
                var user = await _userManager.Users.FirstOrDefaultAsync(a => a.UserName == uname);
                userRolesViewModel.UserName = user.UserName;
                userRolesViewModel.UserId = user.Id;
                userRolesViewModel.FirstName = user.FirstName;
                userRolesViewModel.LastName = user.LastName;
                userRolesViewModel.Email = user.Email;
                return userRolesViewModel;
            }
            catch (Exception)
            {
                return new UserRolesViewModel();
            }
        }

        public async Task<string> GetNameFromId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user.FirstName;
        }
    }
}
