using System;
using System.Collections.Generic;
using System.Text;
using BestTVProgram.Core.Models;

namespace BestTVProgram.Services.Interfaces
{
    public interface IProgramService
    {
        List<ChannelWithProgram> ChannelsWithProgram { get; }
        ChannelWithProgram this[string name] { get; }

        //List<ChannelWithProgram> GetChannelsWithProgram();
    }
}
