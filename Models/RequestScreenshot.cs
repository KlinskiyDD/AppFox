using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFox.Models
{
    [JsonObject]
    public class RequestScreenshot
    {
        [JsonProperty("startDate")]
        public string startDate { get; set; }

        [JsonProperty("endDate")]
        public string endDate { get; set; }
    }
}
