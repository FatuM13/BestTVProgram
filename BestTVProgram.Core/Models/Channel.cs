using System;
using System.Collections.Generic;
using System.Text;

namespace BestTVProgram.Core.Models
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderNumber { get; set; }
        public bool Checked { get; set; }
    }
}
