using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    public class SKUATT
    {
        public SKUATT()
        {
            this.ID = Guid.NewGuid();
        }

        public SKUATT(string ID)
        {
            Guid guid = Guid.NewGuid();
            Guid.TryParse(ID, out guid);
            this.ID = guid;
        }

        public SKUATT(Guid ID)
        {
            this.ID = ID;
        }

        /// <summary>
        /// ID键值
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性种类
        /// </summary>
        public short ATTType { get; set; }

        /// <summary>
        /// 二级ID
        /// </summary>
        public Guid SKUID { get; set; }
    }
}
