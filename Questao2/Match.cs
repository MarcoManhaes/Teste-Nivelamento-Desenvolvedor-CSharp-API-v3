using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao2
{
    public class Match
    {
        [JsonProperty("team1")]
        public string Team1 { get; set; }

        [JsonProperty("team2")]
        public string Team2 { get; set; }

        [JsonProperty("team1goals")]
        public string Team1Goals { get; set; }

        [JsonProperty("team2goals")]
        public string Team2Goals { get; set; }
    }
}
