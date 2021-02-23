using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTB
{
    class Team
    {
        public List<SummonerData> Summoners { get; set; } = new List<SummonerData>();
        public int Avg { get; set; }
    }
}
