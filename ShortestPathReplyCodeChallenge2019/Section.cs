using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Section
    {
        public Coordinate A { get; }
        public Coordinate B { get; }
        public int Cost { get; }

        public Section(Coordinate a, Coordinate b, int cost)
        {
            A = a;
            B = b;
            Cost = cost;
        }
    }
}
