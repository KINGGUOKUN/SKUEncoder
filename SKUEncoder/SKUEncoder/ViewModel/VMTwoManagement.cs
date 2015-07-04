using ExcelHelper;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Win32;
using SKUEncoder.BLL;
using SKUEncoder.Entities;
using SKUEncoder.Entity;
using SKUEncoder.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SKUEncoder.ViewModel
{
    /// <summary>
    /// 二级管理ViewModel
    /// </summary>
    public class VMTwoManagement : BindableBase
    {
        #region Private Fields

        private BLLOneManagement _oneManagement;
        private BLLTwoManagement _TwoMangement;
        private TaskFactory<List<SKUCGY>> _factory = new TaskFactory<List<SKUCGY>>();
        private TaskScheduler _schedule = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion

        #region Constructors

        public VMTwoManagement()
        {
            _oneManagement = new BLLOneManagement();
            _TwoMangement = new BLLTwoManagement();
            ApplicationContext.EventAggregator.GetEvent<OneChangedEvent>().Subscribe(this.OneChangedEventExecute, ThreadOption.UIThread);
            this.ItemsSource = new ObservableCollection<SKUCGYModel>();
            this.ItemsSource.CollectionChanged += (s, e) =>
            {
                (this.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
                (this.CmdExport as DelegateCommand).RaiseCanExecuteChanged();
            };
            this.InitCommands();
            this.InitData();
        }

        #endregion

        #region Commands/Events

        /// <summary>
        /// 添加二级
        /// </summary>
        public ICommand CmdAdd { get; private set; }

        /// <summary>
        /// 修改二级
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

        private ObservableCollection<SKUCGY> _colOne;
        /// <summary>
        /// 一级下拉框绑定数据源
        /// </summary>
        public ObservableCollection<SKUCGY> ColOne
        {
            get
            {
                return _colOne;
            }
            set
            {
                base.SetProperty(ref _colOne, value);
            }
        }

        private SKUCGY _selectedOne;
        public SKUCGY SelectedOne
        {
            get
            {
                return _selectedOne;
            }
            set
            {
                base.SetProperty(ref _selectedOne, value);
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
            AddOrUpdateTwo winAdd = new AddOrUpdateTwo(null,this.SelectedOne.ID);
            winAdd.Owner = Application.Current.MainWindow;
            winAdd.HandleCompleted += AddOrUpdate_HandleCompleted;
            winAdd.Show();
        }


        private void CmdUpdateExecute()
        {
            AddOrUpdateTwo winUpdate = new AddOrUpdateTwo(this.SelectedItem, this.SelectedOne.ID);
            winUpdate.Owner = Application.Current.MainWindow;
            winUpdate.HandleCompleted += AddOrUpdate_HandleCompleted;
            winUpdate.ShowDialog();
        }

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
            ApplicationContext.EventAggregator.GetEvent<TwoChangedEvent>().Publish(model.Entity);
        }

        private void CmdImportExecute()
        {
            if(this.SelectedOne == null)
            {
                MessageBox.Show("请选择一级目录再行导入!");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            ofd.Multiselect = false;
            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                #region Step1:读取Excel表格
                string file = ofd.FileName;
                ExcelImporter importer = new ExcelImporter();
                DataTable dt = null;
                try
                {
                    dt = importer.Import(ofd.FileName, "Sheet2");
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
                        _TwoMangement.ImportTwos(cgys);
                        ApplicationContext.EventAggregator.GetEvent<TwoChangedEvent>().Publish(null);
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
            if (result.HasValue && result.Value)
            {
                using (ExcelExporter exporter = new ExcelExporter())
                {
                    var items = this.ItemsSource.Where(x => x.IsChecked).ToList();
                    SKUCGYModel model;
                    for (int i = 0; i < items.Count; i++)
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
            if (this.ItemsSource.Count(x => x.IsChecked) < 1)
            {
                MessageBox.Show("未选中任何记录");
                return;
            }
            List<Guid> IDs = new List<Guid>();
            IDs.AddRange(this.ItemsSource.Where(x => x.IsChecked).Select(x => x.ID));
            try
            {
                _TwoMangement.DeleteTwos(IDs);
                ApplicationContext.EventAggregator.GetEvent<TwoChangedEvent>().Publish(null);
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

        public void OneChangedEventExecute(SKUCGY cgy)
        {
            this.InitData();
        }

        private void InitData()
        {
            _factory.StartNew(GetOneList).ContinueWith(GetOneListComplete, _schedule);
        }

        private List<SKUCGY> GetOneList()
        {
            return _oneManagement.GetOneList();
        }

        private void GetOneListComplete(Task<List<SKUCGY>> task)
        {
            if (task.Exception != null)
            {
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取一级列表出错!");
                return;
            }
            List<SKUCGY> result = task.Result;
            if(result != null && result.Count > 0)
            {
                this.ColOne = new ObservableCollection<SKUCGY>(task.Result);
            }
            else
            {
                this.ColOne = new ObservableCollection<SKUCGY>();
            }
            
            this.SelectedOne = ColOne.FirstOrDefault();
            //this.Refresh();
        }

        private void Refresh()
        {
            this.ItemsSource.Clear();
            if(this.SelectedOne == null)
            {
                //MessageBox.Show("请选中一级再查询二级!");
                return;
            }
            this.RefreshingVisibility = Visibility.Visible;
            _factory.StartNew(GetTwoList, this.SelectedOne.ID).ContinueWith(GetTwoListComplete, _schedule);
        }

        private List<SKUCGY> GenerateSKUCGYList(DataTable dt)
        {
            List<SKUCGY> result = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = new List<SKUCGY>();
                SKUCGY cgy = null;
                foreach (DataRow dr in dt.Rows)
                {
                    cgy = new SKUCGY()
                    {
                        Code = dr[0].ToString(),
                        Name = dr[1].ToString(),
                        PID = this.SelectedOne.ID,
                        LevelIndex = 2,
                    };
                    result.Add(cgy);
                }
            }

            return result;
        }

        private List<SKUCGY> GetTwoList(object obj)
        {
            Guid pid = Guid.Parse(obj.ToString());
            return _TwoMangement.GetTwoList(pid);
        }


        private void GetTwoListComplete(Task<List<SKUCGY>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取二级列表出错!");
                return;
            }
            List<SKUCGY> list = task.Result;
            if (list == null || list.Count == 0)
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

        #endregion
    }
}
