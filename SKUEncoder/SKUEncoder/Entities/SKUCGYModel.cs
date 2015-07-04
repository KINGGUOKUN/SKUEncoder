using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using SKUEncoder.Entity;

namespace SKUEncoder.Entities
{
    /// <summary>
    /// 一级目录界面绑定类
    /// </summary>
    public class SKUCGYModel : BindableBase
    {
        #region Constructors

        public SKUCGYModel()
        {
            this.ID = Guid.NewGuid();
        }

        public SKUCGYModel(SKUCGY cgy)
        {
            this.ID = cgy.ID;
            this.Code = cgy.Code;
            this.Name = cgy.Name;
            this.PID = cgy.PID;
            this.LevelIndex = cgy.LevelIndex;
        }

        #endregion

        #region Public Properties

        public SKUCGY Entity
        {
            get
            {
                SKUCGY entity = new SKUCGY(this.ID)
                {
                    Code = this.Code,
                    Name = this.Name,
                    PID = this.PID,
                    LevelIndex = this.LevelIndex
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

        private Guid? _PID;
        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? PID
        {
            get
            {
                return _PID;
            }
            set
            {
                base.SetProperty(ref _PID, value);
            }
        }

        private short _levelIndex;
        /// <summary>
        /// 层次，主要指是几级目录
        /// </summary>
        public short LevelIndex
        {
            get
            {
                return _levelIndex;
            }
            set
            {
                base.SetProperty(ref _levelIndex, value);
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
