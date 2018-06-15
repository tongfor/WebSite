using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminLoginLogService _loginLogService;
        private readonly IAdminRoleService _adminRoleService;

        public HomeController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminUserService adminUserService, IAdminRoleService adminRoleService,
            IAdminLoginLogService adminLoginLog, IAdminMenuService adminMenuService, ILogger<AccountController> logger, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
            _loginLogService = adminLoginLog;
            _logger = logger;            
        }

        public async Task<IActionResult> Index()
        {
            await CreateLeftMenuAsync();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            
        }       
    }
}
