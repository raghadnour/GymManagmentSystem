using GymManagmentBLL.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public ActionResult Index()
        {
            var Data = _analyticsService.GetAnalyticsData();    
            return View(Data);
        }
    }
}
