using BestTVProgram.Core.Models;
using BestTVProgram.Services.Interfaces;
using System;
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
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(10*60), Program="Передача №1" }, //"10:00"
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(11*60), Program="Передача №2" }  //"11:00"
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 1" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(10*60), Program="Передача №1" }, //"10:00"
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(11*60), Program="Передача №2" }  //"11:00"
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 2" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(10*60), Program="Передача №1" }, //"10:00"
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(11*60), Program="Передача №2" }  //"11:00"
                    }
                }
                ,
                new ChannelWithProgram
                {
                    Channel = new Channel {Name = "Беларусь 3" },
                    TVPrograms = new List<TVProgram>()
                    {
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(10*60), Program="Передача №1" }, //"10:00"
                        new TVProgram { DateTime=DateTime.Today.AddMinutes(11*60), Program="Передача №2" }  //"11:00"
                    }
                }
            };
        }

    }
}
