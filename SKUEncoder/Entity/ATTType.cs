using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    /// <summary>
    /// 属性种类
    /// </summary>
    public class ATTType
    {
        public ATTType(short ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
        }

        /// <summary>
        /// 属性种类
        /// </summary>
        public short ID { get; private set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; private set; }
    }
}
