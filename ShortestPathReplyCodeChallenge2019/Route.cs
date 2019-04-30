using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Route
    {
        public Coordinate Office { get; }
        public List<Path> Paths { get; }
        public int Reward { get; }

        public Route (Coordinate coo, List<Path> paths)
        {
            Office = coo;
            Paths = paths;
            Reward = 0;

            foreach (var p in paths)
            {
                Reward += p.Reward;
            }
        }
    }
}
