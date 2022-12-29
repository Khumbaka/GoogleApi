using GoogleApi.Data;
using GoogleApi.Models;
using GoogleApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleApi.Controllers
{
    public class EmailController : Controller
    {
        private readonly GoogleDbContext _googleDbContext;

        public EmailController(GoogleDbContext googleDbContext)
        {
            _googleDbContext = googleDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var googleEmails = await _googleDbContext.GoogleEmails.OrderByDescending(m => m.Date).ToListAsync();
            return View(googleEmails);
        }

        public async Task<IActionResult> ImportEmail()
        {
            List<GoogleEmail> googleEmails;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("RefreshToken")))
            {
                googleEmails = await GoogleEmailService.GetEmailService(HttpContext.Session.GetString("RefreshToken"));
                if (googleEmails != null && googleEmails.Count > 0)
                {
                    var messageIds = await _googleDbContext.GoogleEmails.Select(m => m.MessageId).ToListAsync();
                    await _googleDbContext.GoogleEmails.AddRangeAsync(googleEmails.Where(m => !messageIds.Contains(m.MessageId)));
                    await _googleDbContext.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> BodyEmail(int id)
        {
            var googleEmail = await _googleDbContext.GoogleEmails.FindAsync(id);
            return View(googleEmail);
        }
    }
}
