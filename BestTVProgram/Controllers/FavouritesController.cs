using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestTVProgram.Core.Models;
using BestTVProgram.Infrastructure;
using BestTVProgram.Models;
using BestTVProgram.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BestTVProgram.Controllers
{
    public class FavouritesController : Controller
    {
        private IProgramService repository;

        private UserManager<AppUser> userManager;

        private AppIdentityDbContext _context;
        private Task<AppUser> CurrentUser => userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        public FavouritesController(IProgramService repo, UserManager<AppUser> userMgr, AppIdentityDbContext appIdentityDbContext)
        {
            repository = repo;
            userManager = userMgr;
            _context = appIdentityDbContext;
        }

        [Authorize]
        public async Task<ViewResult> Index(string returnUrl)
        {
            Favourites favouritesFromBD = new Favourites();
            //if (User.Identity.IsAuthenticated)
            //{
            AppUser user = await CurrentUser;
            if (user != null)
            {
                var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToList();
                if (ceList.Count() > 0)
                {

                    var result = from channel in ceList
                                 orderby channel.OrderNumber
                                 select channel;
                    int tempId = -1;
                    foreach (var item in result)
                    {
                        if (tempId != item.Id)
                        {
                            favouritesFromBD.AddItem(new Channel { Id = item.ChannelId, Name = item.Name, OrderNumber = item.OrderNumber });
                        }

                        tempId = item.Id;
                    }
                }
            }
            //}

            return View(new Models.ViewModels.FavouritesIndexViewModel
            {
                Favourites = favouritesFromBD,
                ReturnUrl = returnUrl
            });
        }

        [Authorize]
        public async Task<RedirectToActionResult> AddManyToFavourites(IList<Channel> channels, int Id, string returnUrl)
        {
            AppUser user = await CurrentUser;
            bool isChanged = false;
            var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToArray();
            //applicationDbContext.ChannelElements.Remove(ceList.Where(c => c.ChannelId == id).FirstOrDefault());
            //await applicationDbContext.SaveChangesAsync();

            foreach (var item in channels)
            {

                if (item.Checked)
                {

                    ChannelWithProgram channel = repository.ChannelsWithProgram
                        .FirstOrDefault(c => c.Channel.Id == item.Id);

                    if ((channel != null) && (user != null))
                    {

                        if (!ceList.Select(c => c.ChannelId).Contains(item.Id))
                        {
                            isChanged = true;

                            _context.ChannelElements.Add(new ChannelElement
                            {
                                ChannelId = item.Id,
                                Name = channel.Channel.Name,
                                OrderNumber = channel.Channel.OrderNumber,
                                UserId = user.Id
                            });
                        }
                    }
                }
            }
            if (isChanged)
            {
                await _context.SaveChangesAsync();
            }


            return RedirectToAction("Index", new { returnUrl });
        }

        [Authorize]
        public async Task<RedirectToActionResult> RemoveFromFavourites(int id,
                string returnUrl)
        {
            AppUser user = await CurrentUser;

            if (user != null)
            {
                var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToArray();
                var deletedChannels = ceList.Where(c => c.ChannelId == id);
                if (deletedChannels.Count() > 0)
                {
                    var deletedChannel = deletedChannels.FirstOrDefault();
                    if (deletedChannel != null)
                    {
                        _context.ChannelElements.Remove(deletedChannel);
                        await _context.SaveChangesAsync();

                    }
                }
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [Authorize]
        public async Task<RedirectToActionResult> ClearAllFavourites(string returnUrl)
        {
            AppUser user = await CurrentUser;

            if (user != null)
            {
                var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToArray();
                _context.ChannelElements.RemoveRange(ceList);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}