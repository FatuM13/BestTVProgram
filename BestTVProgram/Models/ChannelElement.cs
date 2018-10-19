using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.Models
{
    public class ChannelElement
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public string UserId { get; set; }
        //[ForeignKey("UserId")]
        //public AppUser User { get; set; }
    }
}
