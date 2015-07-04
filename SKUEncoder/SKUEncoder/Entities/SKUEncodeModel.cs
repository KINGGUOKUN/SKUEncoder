using Microsoft.Practices.Prism.Mvvm;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entities
{
    public class SKUEncodeModel : BindableBase
    {
        #region Constructors

        public SKUEncodeModel()
        {
            this.ID = Guid.NewGuid();
        }

        public SKUEncodeModel(SKUEncode skuEncode)
        {
            this.ID = skuEncode.ID;
            this.Code = skuEncode.Code;
            this.Name = skuEncode.Name;
            this.Att3ID = skuEncode.Att3ID;
            this.Att4ID = skuEncode.Att4ID;
            this.Att5ID = skuEncode.Att5ID;
            this.Att6ID = skuEncode.Att6ID;
            this.Att7ID = skuEncode.Att7ID;
        }

        #endregion

        #region Public Properties

        public SKUEncode Entity
        {
            get
            {
                SKUEncode entity = new SKUEncode(this.ID)
                {
                    Code = this.Code,
                    Name = this.Name,
                    Att3ID = this.Att3ID,
                    Att4ID = this.Att4ID,
                    Att5ID = this.Att5ID,
                    Att6ID = this.Att6ID,
                    Att7ID = this.Att7ID
                };
                return entity;
            }
        }

        private Guid _ID;
        /// <summary>
        /// ID键值
        /// </summary>
        public Guid ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                _ID = value;
            }
        }

        private string _code;
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                base.SetProperty(ref _code, value);
            }
        }

        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                base.SetProperty(ref _name, value);
            }
        }

        private Guid _att3ID;
        public Guid Att3ID
        {
            get
            {
                return _att3ID;
            }
            set
            {
                base.SetProperty(ref _att3ID, value);
            }
        }

        private Guid _att4ID;
        public Guid Att4ID
        {
            get
            {
                return _att4ID;
            }
            set
            {
                base.SetProperty(ref _att4ID, value);
            }
        }

        private Guid _att5ID;
        public Guid Att5ID
        {
            get
            {
                return _att5ID;
            }
            set
            {
                base.SetProperty(ref _att5ID, value);
            }
        }

        private Guid _att6ID;
        public Guid Att6ID
        {
            get
            {
                return _att6ID;
            }
            set
            {
                base.SetProperty(ref _att6ID, value);
            }
        }

        private Guid _att7ID;
        public Guid Att7ID
        {
            get
            {
                return _att7ID;
            }
            set
            {
                base.SetProperty(ref _att7ID, value);
            }
        }

        private bool _isChecked = false;
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                base.SetProperty(ref _isChecked, value);
            }
        }

        #endregion   
    }
}
