using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Customer
    {
        public int Id { get; }
        public Coordinate Coo {get;}
        public int Reward { get; }
        public int RealReward { get; set; }

        public Customer(int id, Coordinate coo, int reward)
        {
            Id = id;
            Coo = coo;
            Reward = reward;
        }

    }
}
