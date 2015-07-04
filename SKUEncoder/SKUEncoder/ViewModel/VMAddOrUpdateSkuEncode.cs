using Microsoft.Practices.Prism.Commands;
using SKUEncoder.BLL;
using SKUEncoder.Entities;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SKUEncoder.ViewModel
{
    public class VMAddOrUpdateSkuEncode : BaseViewModel
    {
        #region  Private Fields

        private SKUEncodeModel _model;
        private BLLSKUEncodeManagement _bll;
        private bool _isAdd;

        #endregion

        #region Constructors

        public VMAddOrUpdateSkuEncode(SKUEncodeDetail detail)
        {
            _isAdd = true;
            _model = new SKUEncodeModel()
            {
                Code = detail.Code,
                Name = detail.Name,
                Att3ID = detail.ATT3.ID,
                Att4ID = detail.ATT4.ID,
                Att5ID = detail.ATT5.ID,
                Att6ID = detail.ATT6.ID,
                Att7ID = detail.ATT7.ID,
            };
            this.CmdSave = new DelegateCommand(this.CmdSaveExecute);
            _bll = new BLLSKUEncodeManagement();
        }

        public VMAddOrUpdateSkuEncode(SKUEncodeModel model)
        {
            _isAdd = false;
            _model = new SKUEncodeModel(model.Entity);
            this.CmdSave = new DelegateCommand(this.CmdSaveExecute);
            _bll = new BLLSKUEncodeManagement();
        }

        #endregion

        #region Public Properties


        public bool IsComplete
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Code)
                    && !string.IsNullOrWhiteSpace(this.Name);
            }
        }

        public override bool CanSubmit
        {
            get
            {
                return this.IsComplete
                    && base.HasChanges
                    && !base.HasErrors;
            }
        }

        /// <summary>
        /// 一级编码
        /// </summary>
        public string Code
        {
            get
            {
                return _model.Code;
            }
            set
            {
                if (value != _model.Code)
                {
                    base.HasChanges = true;
                }
                _model.Code = value;
                base.OnPropertyChanged("Code");
                base.RemoveError("Code");
            }
        }

        /// <summary>
        /// 一级名称
        /// </summary>
        public string Name
        {
            get
            {
                return _model.Name;
            }
            set
            {
                if (value != _model.Name)
                {
                    base.HasChanges = true;
                }
                _model.Name = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    base.AddError("Name", "属性名称不能为空");
                    return;
                }
                base.OnPropertyChanged("Name");
                base.RemoveError("Name");
            }
        }

        #endregion

        #region Commands/Events

        public ICommand CmdSave { get; private set; }

        /// <summary>
        /// 添加或删除一级目录完毕事件
        /// </summary>
        public event EventHandler<EntityEventArgs> HandleCompleted;

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void CmdSaveExecute()
        {
            if (_isAdd)
            {
                try
                {
                    if (_bll.AddSKUEncode(_model.Entity))
                    {
                        if (this.HandleCompleted != null)
                        {
                            this.HandleCompleted(this, new EntityEventArgs(_model, true));
                        }
                        MessageBox.Show("添加SKU编码成功");
                    }
                    else
                    {
                        MessageBox.Show("添加SKU编码失败");
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("添加SKU编码失败!");
                }
            }
            else
            {
                try
                {
                    if (_bll.UpdateSKUEncode(_model.Entity))
                    {
                        if (this.HandleCompleted != null)
                        {
                            this.HandleCompleted(this, new EntityEventArgs(_model, false));
                            MessageBox.Show("更新SKU编码成功");
                        }
                        else
                        {
                            MessageBox.Show("更新SKU编码失败");
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("更新SKU编码失败!");
                }
            }
        }

        #endregion
    }
}
