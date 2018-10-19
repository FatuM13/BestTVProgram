using BestTVProgram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.Components
{
    public class CountViewComponent : ViewComponent
    {
        private UserManager<AppUser> userManager;

        private AppIdentityDbContext _context;
        public CountViewComponent(UserManager<AppUser> userMgr, AppIdentityDbContext appIdentityDbContext)
        {
            userManager = userMgr;
            _context = appIdentityDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToList();
                    return View(ceList.Count);
                }
            }
            return View(0);

        }
    }

}
