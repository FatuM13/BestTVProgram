using BestTVProgram.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.Models
{
    public class AppUser : IdentityUser
    {
        public TvProgramViewFormatsEnum TvProgramViewFormat { get; set; }
    }
}
