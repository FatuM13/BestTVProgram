using BestTVProgram.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestTVProgram.Models
{
    public class Favourites
    {
        private List<Channel> lineCollection = new List<Channel>();

        public virtual void AddItem(Channel channel) //, int quantity
        {
            Channel line = lineCollection
                .Where(p => p.Id == channel.Id)
                .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(channel);
            }
        }

        public virtual void RemoveLine(Channel channel) =>
            lineCollection.RemoveAll(l => l.Id == channel.Id);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<Channel> FavouriteLines => lineCollection;

    }
}
