using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Algo
    {
        public string Action { get; set; }
        public string XPath { get; set; }
        public int Timeout { get; set; }
    }

    public class Work
    {
        public int id { get; set; }
        public string URL { get; set; }
        public string Referrer { get; set; }
        public List<Algo> Algo { get; set; }
    }

    public class Root
    {
        public List<Work> Works { get; set; }
    }
}
