using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPathReplyCodeChallenge2019
{
    class Cell
    {
        public Coordinate Coo { get; }
        public int Cost { get; set; }
        public Coordinate PrevCoo { get; set; }
        public bool Checked { get; set; }

        public Cell(Coordinate coo)
        {
            Coo = coo;
            Cost = int.MaxValue;
            Checked = false;
        }
    }
}
