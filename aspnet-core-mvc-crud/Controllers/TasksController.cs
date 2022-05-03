using aspnet_core_mvc_crud.Data;
using aspnet_core_mvc_crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace aspnet_core_mvc_crud.Controllers
{
    public class TasksController : Controller
    {
        private readonly RequestService _requestService;
        public TasksController(RequestService requestService)
        {
            _requestService = requestService;
        }
        public IActionResult Index()
        {
            return View(_requestService.GetAllServiceRequests().Result);
        }
    }
}
