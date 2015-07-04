using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    public class ApplicationContext
    {
        public static IEventAggregator EventAggregator { get; set; }
    }
}
