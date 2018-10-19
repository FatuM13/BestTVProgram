using BestTVProgram.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.Models.ViewModels
{
    public class ChannelsWithProgramAndTvProgramViewFormatViewModel
    {
        public List<ChannelWithProgram> ChannelWithProgramList { get; set; }
        public TvProgramViewFormatsEnum TvProgramViewFormat { get; set; }
    }
}
