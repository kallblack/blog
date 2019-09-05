using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore_consul_example.Models
{
    public class ConsulConfig
    {
        public string ClientName { get; set; }
        public string ClientIP { get; set; }
        public string Server { get; set; }
        public string DataCenter { get; set; }
        public Uri HostingUri { get; set; }
    }
}
