using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Path
    {
        public List<char> Direction { get; }
        public List<Coordinate> Way { get; }
        public Customer Customer { get; }
        public int Cost { get; }
        public int Reward { get; }

        public Path(List<Coordinate> way, Customer cus, int cost)
        {
            Direction = BuildDirection(way);
            Way = way;
            Customer = cus;
            Cost = cost;
            Reward = cus.Reward - cost;
        }

        //Build directions R L U D
        private List<char> BuildDirection(List<Coordinate> way)
        {
            int x, y;
            List<char> dir = new List<char>();

            for (int i = 1; i < way.Count; i++)
            {
                x = way[i].X - way[i - 1].X;
                y = way[i].Y - way[i - 1].Y;

                if (x != 0)
                    if (x == 1)
                        dir.Add('R');
                    else
                        dir.Add('L');
                if (y != 0)
                    if (y == 1)
                        dir.Add('D');
                    else
                        dir.Add('U');

            }

            return dir;
        }

        //Get direction
        public String GetDirection()
        {
            return string.Concat(Direction);
        }

    }
}
