using System;
using System.Collections.Generic;
using System.IO;

namespace ShortestPathReplyCodeChallenge2019
{
    static class Output
    {
        static public void OutputFile(List<Route> routes, String filename)
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                foreach (var r in routes)
                {
                    foreach (var p in r.Paths)
                    {
                        sw.WriteLine("{0} {1} {2}", r.Office.X, r.Office.Y, p.GetDirection());
                    }
                }
            }
        }
    }
}
