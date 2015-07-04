using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    public class SKUEncodeDetail
    {
        public SKUCGY One { get; set; }

        public SKUCGY Two { get; set; }

        public string Code
        {
            get
            {
                return string.Format("{0}{1}-{2}{3}-{4}{5}{6}", One.Code, Two.Code, ATT3.Code, ATT4.Code, ATT5.Code, ATT6.Code, ATT7.Code);
            }
        }

        public string Name { get; set; }

        public SKUATT ATT3 { get; set; }

        public SKUATT ATT4 { get; set; }

        public SKUATT ATT5 { get; set; }

        public SKUATT ATT6 { get; set; }

        public SKUATT ATT7 { get; set; }
    }
}
