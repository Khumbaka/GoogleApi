using GoogleApi.Common;
using GoogleApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authenticate()
        {
            string email = Request.Form["email"].FirstOrDefault();
            return RedirectPermanent(GoogleApiHelper.GetOauthUri(email));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> OauthCallback(string code, string error, string state)
        {
            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    var token = await GoogleApiHelper.GetTokenByCode(code);
                    if (token != null)
                    {
                        ViewBag.AccessToken = token.access_token;
                        ViewBag.RefreshToken = token.refresh_token;
                    }
                }
                if (!string.IsNullOrEmpty(error))
                {
                    ViewBag.Message = "Error " + error;
                }
                if (!string.IsNullOrEmpty(state))
                {
                    ViewBag.MailAddress = state;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
