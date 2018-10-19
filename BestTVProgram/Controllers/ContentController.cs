using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestTVProgram.Core.Models;
using BestTVProgram.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BestTVProgram.Controllers
{
    [Produces("application/json")]
    [Route("api/Content")]
    public class ContentController : Controller
    {
        private IProgramService repository;

        public ContentController(IProgramService repo) => repository = repo;

        [HttpGet]
        public List<ChannelWithProgram> Get() => repository.ChannelsWithProgram;

        [HttpGet("{id}")]
        public ChannelWithProgram Get(string channelName) => repository[channelName];
        
    }
}