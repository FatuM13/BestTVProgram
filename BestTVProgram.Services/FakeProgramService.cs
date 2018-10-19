using BestTVProgram.Core.Models;
using BestTVProgram.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace BestTVProgram.Services
{
    public class FakeProgramService : IProgramService
    {
        private static List<ChannelWithProgram> channelsWithProgram;
        public List<ChannelWithProgram> ChannelsWithProgram => channelsWithProgram;
        public ChannelWithProgram this[string name] => ChannelsWithProgram.Where(c => c.Channel.Name==name).FirstOrDefault<ChannelWithProgram>();

        public FakeProgramService()
        {
            channelsWithProgram =  new List<ChannelWithProgram>()
            {
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "ОНТ" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { Time="10:00", Program="Передача №1" },
                        new TVProgram { Time="11:00", Program="Передача №2" }
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 1" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { Time="10:00", Program="Передача №1" },
                        new TVProgram { Time="11:00", Program="Передача №2" }
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 2" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { Time="10:00", Program="Передача №1" },
                        new TVProgram { Time="11:00", Program="Передача №2" }
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 3" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { Time="10:00", Program="Передача №1" },
                        new TVProgram { Time="11:00", Program="Передача №2" }
                    }
                }
            };
        }

    }
}
