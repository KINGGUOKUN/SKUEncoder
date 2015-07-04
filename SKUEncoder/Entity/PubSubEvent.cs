using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    /// <summary>
    /// 一级变化发布事件
    /// </summary>
    public class OneChangedEvent : PubSubEvent<SKUCGY>
    {
    }

    /// <summary>
    /// 二级变化发布事件
    /// </summary>
    public class TwoChangedEvent : PubSubEvent<SKUCGY>
    {
    }

    /// <summary>
    /// 属性变化事件
    /// </summary>
    public class AttChangedEvent : PubSubEvent<SKUATT>
    {
    }
}
