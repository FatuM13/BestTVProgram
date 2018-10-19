using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BestTVProgram.Models;
using BestTVProgram.Services;
using BestTVProgram.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using BestTVProgram.Core.Models;
using BestTVProgram.Models.ViewModels;

namespace BestTVProgram.Controllers
{
    public class HomeController : Controller
    {
        private IProgramService programService;

        private UserManager<AppUser> userManager;

        private AppIdentityDbContext _context;

        public HomeController(IProgramService programService, UserManager<AppUser> userMgr, AppIdentityDbContext appIdentityDbContext)
        {
            this.programService = programService;
            userManager = userMgr;
            _context = appIdentityDbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await CurrentUser;
                if (user != null)
                {
                    var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToList();
                   
                    if (ceList.Count() > 0)
                    {
                        var result = from channel in ceList
                                     orderby channel.OrderNumber
                                     select channel;

                        List<ChannelWithProgram> resultCollection = new List<ChannelWithProgram>();
                        foreach (var item in result)
                        {
                            var channels = programService.ChannelsWithProgram.Where(channel => channel.Channel.Id == item.ChannelId);
                            if (channels.Count() > 0)
                            {
                                var addingChannelWithProgram = channels.FirstOrDefault();
                                addingChannelWithProgram.Channel.OrderNumber = item.OrderNumber;
                                resultCollection.Add(addingChannelWithProgram);
                            }
                        }

                        var tvProgramViewFormat = TvProgramViewFormatsEnum.ForAllDay;
                        if (Enum.IsDefined(typeof(TvProgramViewFormatsEnum), user.TvProgramViewFormat))
                        {
                            tvProgramViewFormat = user.TvProgramViewFormat;
                        }


                        return View(new ChannelsWithProgramAndTvProgramViewFormatViewModel { ChannelWithProgramList = resultCollection, TvProgramViewFormat = tvProgramViewFormat });
                    }
                }
            }

            return View(new ChannelsWithProgramAndTvProgramViewFormatViewModel { ChannelWithProgramList = programService.ChannelsWithProgram, TvProgramViewFormat = TvProgramViewFormatsEnum.ForAllDay });
        }

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            return View(await CurrentUser);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(List<string> tvProgramViewFormats)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;

                foreach (var item in tvProgramViewFormats)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        user.TvProgramViewFormat = (TvProgramViewFormatsEnum)Enum.Parse(typeof(TvProgramViewFormatsEnum), item);
                    }
                }

                await userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
        }

        private Task<AppUser> CurrentUser =>
            userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        public IActionResult GetChannelList()
        { 
            var homeData = programService.ChannelsWithProgram.Select(c => (c.Channel.Id+"|"+c.Channel.Name));
            return View(homeData);
        }

        [Authorize]
        public async Task<IActionResult> ChooseFavourites()
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await CurrentUser;
                if (user != null)
                {
                    var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToList();
                    if (ceList.Count() > 0)
                    {
                        var result = from channel in ceList
                                     orderby channel.OrderNumber
                                     select channel;

                        List<Channel> resultCollection = new List<Channel>();
                        foreach (var item in result)
                        {
                            resultCollection.Add(new Channel
                            {
                                Id = item.ChannelId,
                                Name = item.Name,
                                OrderNumber = item.OrderNumber,
                                Checked = true
                            });
                        }

                        var channelIds = ceList.Select(c => c.ChannelId);
                        var notChoosen = programService.ChannelsWithProgram.Where(channel => !channelIds.Contains(channel.Channel.Id)).Select(cwp => cwp.Channel);
                        foreach (var item in notChoosen)
                        {
                            resultCollection.Add(item);
                        }
                        return View(resultCollection);

                    }
                }
            }
            return View(programService.ChannelsWithProgram.Select(c => c.Channel).ToList());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChooseFavourites(List<string> ids, List<string> names, List<string> orderNumbers)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await CurrentUser;

                var ceList = _context.ChannelElements.Where(c => c.UserId == user.Id).ToArray();
                _context.ChannelElements.RemoveRange(ceList);
                await _context.SaveChangesAsync();


                for (int i = 0; i < ids.Count; i++)
                {
                    //Update
                    _context.ChannelElements.Add(new ChannelElement
                    {
                        ChannelId = Convert.ToInt32(ids[i]),
                        Name = names[i],
                        OrderNumber = Convert.ToInt32(orderNumbers[i]),
                        UserId = user.Id
                    });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(await CurrentUser);
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
