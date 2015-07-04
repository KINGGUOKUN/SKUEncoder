using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    /// <summary>
    /// SKU编码
    /// </summary>
    public class SKUEncode
    {
        public SKUEncode()
        {
            this.ID = Guid.NewGuid();
        }

        public SKUEncode(string ID)
        {
            Guid guid = Guid.NewGuid();
            Guid.TryParse(ID, out guid);
            this.ID = guid;
        }

        public SKUEncode(Guid ID)
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

        public Guid Att3ID { get; set; }

        public Guid Att4ID { get; set; }

        public Guid Att5ID { get; set; }

        public Guid Att6ID { get; set; }

        public Guid Att7ID { get; set; }
    }
}
