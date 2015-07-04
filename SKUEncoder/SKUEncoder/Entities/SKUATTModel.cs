using Microsoft.Practices.Prism.Mvvm;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entities
{
    public class SKUATTModel : BindableBase
    {
        #region Constructors

        public SKUATTModel()
        {
            this.ID = Guid.NewGuid();
        }

        public SKUATTModel(SKUATT att)
        {
            this.ID = att.ID;
            this.Code = att.Code;
            this.Name = att.Name;
            this.ATTType = att.ATTType;
            this.SKUCID = att.SKUID;
        }

        #endregion

        #region Public Properties

        public SKUATT Entity
        {
            get
            {
                SKUATT entity = new SKUATT(this.ID)
                {
                    Code = this.Code,
                    Name = this.Name,
                    ATTType = this.ATTType,
                    SKUID = this.SKUCID,
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

        private short _attType;
        /// <summary>
        /// 属性种类
        /// </summary>
        public short ATTType
        {
            get
            {
                return _attType;
            }
            set
            {
                base.SetProperty(ref _attType, value);
            }
        }

        private Guid _SKUCID;
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid SKUCID
        {
            get
            {
                return _SKUCID;
            }
            set
            {
                base.SetProperty(ref _SKUCID, value);
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
