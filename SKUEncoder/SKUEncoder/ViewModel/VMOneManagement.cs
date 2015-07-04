using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using SKUEncoder.BLL;
using SKUEncoder.Entities;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SKUEncoder.View;
using Microsoft.Win32;
using ExcelHelper;
using System.Data;

namespace SKUEncoder.ViewModel
{
    /// <summary>
    /// 一级目录管理ViewModel
    /// </summary>
    public class VMOneManagement : BindableBase
    {
        #region Private Fields

        private BLLOneManagement _oneMangement;
        private TaskFactory<List<SKUCGY>> _factory = new TaskFactory<List<SKUCGY>>();
        private TaskScheduler _schedule = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion

        #region Constructors

        public VMOneManagement()
        {
            _oneMangement = new BLLOneManagement();
            this.ItemsSource = new ObservableCollection<SKUCGYModel>();
            this.ItemsSource.CollectionChanged += (s, e) =>
            {
                (this.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
                (this.CmdExport as DelegateCommand).RaiseCanExecuteChanged();
            };
            this.InitCommands();
            this.Refresh();
        }

        #endregion

        #region Commands/Events

        /// <summary>
        /// 添加一级
        /// </summary>
        public ICommand CmdAdd { get; private set; }

        /// <summary>
        /// 修改一级
        /// </summary>
        public ICommand CmdUpdate { get; private set; }

        /// <summary>
        /// 导入命令
        /// </summary>
        public ICommand CmdImport { get; private set; }

        public ICommand CmdExport { get; private set; }

        /// <summary>
        /// 删除命令
        /// </summary>
        public ICommand CmdDel { get; private set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand CmdRefresh { get; private set; }

        /// <summary>
        /// 测试命令
        /// </summary>
        public ICommand CmdTest { get; private set; }

        #endregion

        #region Public Properties

        public ObservableCollection<SKUCGYModel> ItemsSource
        {
            get;
            private set;
        }

        private SKUCGYModel _selectedItem;
        public SKUCGYModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                base.SetProperty(ref _selectedItem, value);
            }
        }

        private Visibility _refreshingVisibility = Visibility.Collapsed;
        public Visibility RefreshingVisibility
        {
            get
            {
                return _refreshingVisibility;
            }
            private set
            {
                base.SetProperty(ref _refreshingVisibility, value);
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void InitCommands()
        {
            this.CmdAdd = new DelegateCommand(this.CmdAddExecute);
            this.CmdUpdate = new DelegateCommand(this.CmdUpdateExecute);
            this.CmdImport = new DelegateCommand(this.CmdImportExecute);
            this.CmdExport = new DelegateCommand(this.CmdExportExecute, this.CanCmdExportExecute);
            this.CmdDel = new DelegateCommand(this.CmdDelExecute, this.CanCmdDelExecute);
            this.CmdRefresh = new DelegateCommand(this.CmdRefreshExecute);
            this.CmdTest = new DelegateCommand(this.CmdTestExecute);
        }

        /// <summary>
        /// 添加一级目录
        /// </summary>
        private void CmdAddExecute()
        {
            AddOrUpdateOne winOne = new AddOrUpdateOne(null);
            winOne.HandleCompleted += AddOrUpdate_HandleCompleted;
            winOne.Show();
        }


        private void CmdUpdateExecute()
        {
            AddOrUpdateOne winUpdate = new AddOrUpdateOne(this.SelectedItem);
            winUpdate.Owner = Application.Current.MainWindow;
            winUpdate.HandleCompleted += AddOrUpdate_HandleCompleted;
            winUpdate.ShowDialog();
        }

        /// <summary>
        /// 修改完毕事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrUpdate_HandleCompleted(object sender, EntityEventArgs e)
        {
            SKUCGYModel model = e.Entity as SKUCGYModel;
            if (e.IsAdd)
            {
                this.ItemsSource.Add(model);
            }
            else
            {
                SKUCGYModel item = this.ItemsSource.Where(x => x.ID == model.ID).First();
                int index = this.ItemsSource.IndexOf(item);
                this.ItemsSource[index] = model;
            }
            this.SelectedItem = model;
            ApplicationContext.EventAggregator.GetEvent<OneChangedEvent>().Publish(model.Entity);
        }

        private void CmdImportExecute()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if(result.HasValue && result.Value)
            {
                #region Step1:读取Excel表格
                string file = ofd.FileName;
                ExcelImporter importer = new ExcelImporter();
                DataTable dt = null;
                try
                {
                    dt = importer.Import(ofd.FileName, "Sheet1");
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("读取Excel出错!");
                    return;
                }
                #endregion

                #region Step2:解析Excel表格中数据
                List<SKUCGY> cgys;
                try
                {
                    cgys = this.GenerateSKUCGYList(dt);
                }
                catch (Exception e)
                {
                    MessageBox.Show("解析Excel数据失败!");
                    return;
                }
                #endregion

                if (cgys != null && cgys.Count > 0)
                {
                    #region Step3:判断表格数据中编码字段是否与数据库中已有编码冲突
                    #endregion

                    #region Step4:将解析出来的一级数据保存至服务器
                    try
                    {
                        _oneMangement.ImportOnes(cgys);
                        ApplicationContext.EventAggregator.GetEvent<OneChangedEvent>().Publish(null);
                        this.Refresh();
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                        MessageBox.Show("导入一级数据出错!");
                    }
                    #endregion
                }
            }
        }

        private void CmdExportExecute()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            bool? result = sfd.ShowDialog();
            if(result.HasValue && result.Value)
            {
                using (ExcelExporter exporter = new ExcelExporter())
                {
                    var items = this.ItemsSource.Where(x => x.IsChecked).ToList();
                    SKUCGYModel model;
                    for (int i = 0; i < items.Count; i++ )
                    {
                        model = items[i];
                        exporter.SetCellValue(i + 1, 1, model.Code);
                        exporter.SetCellValue(i + 1, 2, model.Name);
                    }
                    exporter.SaveAs(sfd.FileName);
                }
            }
        }

        private bool CanCmdExportExecute()
        {
            return this.ItemsSource != null && this.ItemsSource.Count(x => x.IsChecked) > 0; 
        }

        private void CmdDelExecute()
        {
            if(this.ItemsSource.Count(x => x.IsChecked) < 1)
            {
                MessageBox.Show("未选中任何记录");
                return;
            }
            List<Guid> IDs = new List<Guid>();
            IDs.AddRange(this.ItemsSource.Where(x => x.IsChecked).Select(x => x.ID));
            try
            {
                _oneMangement.DeleteOnes(IDs);
                ApplicationContext.EventAggregator.GetEvent<OneChangedEvent>().Publish(null);
                this.Refresh();
                MessageBox.Show("删除成功！");
            }
            catch (Exception e)
            {
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                MessageBox.Show("删除一级出错!");
            }
        }

        private bool CanCmdDelExecute()
        {
            return this.ItemsSource != null && this.ItemsSource.Count(x => x.IsChecked) > 0;
        }

        private void CmdRefreshExecute()
        {
            this.Refresh();
        }

        private void CmdTestExecute()
        {
            int count = this.ItemsSource.Count(item => item.IsChecked);
        }

        private void Refresh()
        {
            this.ItemsSource.Clear();
            this.RefreshingVisibility = Visibility.Visible;
            _factory.StartNew(GetOneList).ContinueWith(GetOneListComplete, _schedule);
        }

        private List<SKUCGY> GetOneList()
        {
            return _oneMangement.GetOneList();
        }

        private void GetOneListComplete(Task<List<SKUCGY>> task)
        {
            if(task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取一级列表出错!");
                return;
            }
            List<SKUCGY> list = task.Result;
            if(list == null || list.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            SKUCGYModel model;
            foreach (SKUCGY cgy in list)
            {
                model = new SKUCGYModel(cgy);
                this.ItemsSource.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        private List<SKUCGY> GenerateSKUCGYList(DataTable dt)
        {
            List<SKUCGY> result = null;
            if(dt != null && dt.Rows.Count > 0)
            {
                result = new List<SKUCGY>();
                SKUCGY cgy = null;
                foreach(DataRow dr in dt.Rows)
                {
                    cgy = new SKUCGY()
                    {
                        Code = dr[0].ToString(),
                        Name = dr[1].ToString(),
                        PID = Guid.Empty,
                        LevelIndex = 1,
                    };
                    result.Add(cgy);
                }
            }

            return result;
        }

        #endregion
    }
}
