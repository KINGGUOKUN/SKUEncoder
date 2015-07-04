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
    public class VMAddOrUpdateATT : BaseViewModel
    {
        #region  Private Fields

        private SKUATTModel _model;
        private BLLAttManagement _bll;
        private bool _isAdd;

        #endregion

        #region Constructors

        public VMAddOrUpdateATT(SKUATTModel model,short attTypeID, Guid skucid)
        {
            _isAdd = model == null;
            if (_isAdd)
            {
                _model = new SKUATTModel()
                {
                    SKUCID = skucid,
                    ATTType = attTypeID,
                };
            }
            else
            {
                _model = new SKUATTModel(model.Entity);
            }
            this.CmdSave = new DelegateCommand(this.CmdSaveExecute);
            _bll = new BLLAttManagement();
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    base.AddError("Code", "属性编码不能为空");
                    return;
                }
                if (_bll.IsATTCodeExits(_model.ATTType, _model.SKUCID, Code))
                {
                    base.AddError("Code", "属性编码已经存在");
                    return;
                }
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
                    if (_bll.AddATT(_model.Entity))
                    {
                        if (this.HandleCompleted != null)
                        {
                            this.HandleCompleted(this, new EntityEventArgs(_model, true));
                        }
                        MessageBox.Show("添加属性成功");
                    }
                    else
                    {
                        MessageBox.Show("添加属性失败");
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0}{1},{2}", "VMAddOrUpdateATT.CmdSaveExecute()", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("添加属性失败!");
                }
            }
            else
            {
                try
                {
                    if (_bll.UpdateATT(_model.Entity))
                    {
                        if (this.HandleCompleted != null)
                        {
                            this.HandleCompleted(this, new EntityEventArgs(_model, false));
                            MessageBox.Show("更新属性成功");
                        }
                        else
                        {
                            MessageBox.Show("更新属性失败");
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("更新属性失败!");
                }
            }
        }

        #endregion
    }
}
