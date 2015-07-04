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
    public class VMAttManagement : BindableBase
    {
        #region Private Fields

        private BLLAttManagement _bll;
        private TaskFactory<List<SKUCGY>> _factorySKUCGY = new TaskFactory<List<SKUCGY>>();
        private TaskFactory<List<SKUATT>> _factorySKUATT = new TaskFactory<List<SKUATT>>();
        private TaskScheduler _schedule = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion

        #region Constructors

        public VMAttManagement()
        {
            _bll = new BLLAttManagement();
            this.ItemsSource = new ObservableCollection<SKUATTModel>();
            this.ItemsSource.CollectionChanged += (s, e) =>
                {
                    (this.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
                };
            ApplicationContext.EventAggregator.GetEvent<OneChangedEvent>().Subscribe(OneChangedEventExecute, ThreadOption.UIThread);
            ApplicationContext.EventAggregator.GetEvent<TwoChangedEvent>().Subscribe(TwoChangedEventExecute, ThreadOption.UIThread);
            this.InitCommands();
            this.InitData();
        }

        #endregion

        #region Commands/Events

        /// <summary>
        /// 添加命令
        /// </summary>
        public ICommand CmdAdd { get; private set; }

        /// <summary>
        /// 修改命令
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

        public event EventHandler TreeViewRefreshed;

        #endregion

        #region Public Properties

        public ObservableCollection<SKUATTModel> ItemsSource
        {
            get;
            private set;
        }

        private SKUATTModel _selectedItem;
        public SKUATTModel SelectedItem
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

        private ObservableCollection<SKUCGY> _treeSKUCGY = new ObservableCollection<SKUCGY>();
        /// <summary>
        /// 一级二级树
        /// </summary>
        public ObservableCollection<SKUCGY> TreeSKUCGY
        {
            get
            {
                return _treeSKUCGY;
            }
            set
            {
                base.SetProperty(ref _treeSKUCGY, value);
            }
        }

        private SKUCGY _selectedSKUCGY;
        /// <summary>
        /// 选中的一级或二级
        /// </summary>
        public SKUCGY SelectedSKUCGY
        {
            get
            {
                return _selectedSKUCGY;
            }
            set
            {
                base.SetProperty(ref _selectedSKUCGY, value);
                (this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
                (this.CmdImport as DelegateCommand).RaiseCanExecuteChanged();
                if(value != null && value.LevelIndex == 2)
                {
                    this.Refresh();
                }
            }
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public ObservableCollection<ATTType> ColAttType
        {
            get;
            private set;
        }

        private ATTType _selectedAttType;
        /// <summary>
        /// 选中的属性
        /// </summary>
        public ATTType SelectedAttType
        {
            get
            {
                return _selectedAttType;
            }
            set
            {
                base.SetProperty(ref _selectedAttType, value);
                (this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
                if(value != null)
                {
                    Refresh();
                }
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
                (this.CmdRefresh as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Public Methods

        public void OneChangedEventExecute(SKUCGY cgy)
        {
            this.InitSKUCGYTree();
        }

        public void TwoChangedEventExecute(SKUCGY cgy)
        {
            this.InitSKUCGYTree();
        }

        #endregion

        #region Private Methods

        private void InitCommands()
        {
            this.CmdAdd = new DelegateCommand(this.CmdAddExecute, this.CanCmdAddExecute);
            this.CmdUpdate = new DelegateCommand(this.CmdUpdateExecute);
            this.CmdImport = new DelegateCommand(this.CmdImportExecute, this.CanCmdImportExecute);
            this.CmdExport = new DelegateCommand(this.CmdExportExecute, this.CanCmdExportExecute);
            this.CmdDel = new DelegateCommand(this.CmdDelExecute, this.CanCmdDelExecute);
            this.CmdRefresh = new DelegateCommand(this.CmdRefreshExecute, this.CanCmdRefreshExecute);
        }

        private void InitData()
        {
            this.InitAttTypes();
            this.InitSKUCGYTree();
        }

        private void InitSKUCGYTree()
        {
            this.TreeSKUCGY.Clear();
            _factorySKUCGY.StartNew(GetAllSKUCGY).ContinueWith(GetAllSKUCGYComplete, _schedule);
        }

        private void InitAttTypes()
        {
            List<ATTType> types = _bll.GetAllAttType();
            this.ColAttType = new ObservableCollection<ATTType>(types);
            this.SelectedAttType = this.ColAttType.FirstOrDefault();
        }

        #region Command Executes

        private void CmdAddExecute()
        {
            AddOrUpdateATT winAdd = new AddOrUpdateATT(null, this.SelectedAttType.ID, this.SelectedSKUCGY.ID);
            winAdd.Owner = Application.Current.MainWindow;
            winAdd.HandleCompleted += AddOrUpdate_HandleCompleted;
            winAdd.Show();
        }

        private bool CanCmdAddExecute()
        {
            return this.SelectedSKUCGY != null 
                && this.SelectedSKUCGY.LevelIndex == 2
                && this.SelectedAttType != null;
        }

        private void CmdUpdateExecute()
        {
            AddOrUpdateATT winUpdate = new AddOrUpdateATT(this.SelectedItem, this.SelectedAttType.ID, this.SelectedSKUCGY.ID);
            winUpdate.Owner = Application.Current.MainWindow;
            winUpdate.HandleCompleted += AddOrUpdate_HandleCompleted;
            winUpdate.ShowDialog();
        }

        private void CmdImportExecute()
        {
            if (this.SelectedSKUCGY == null || this.SelectedSKUCGY.LevelIndex !=2 || this.SelectedAttType == null)
            {
                MessageBox.Show("请选择二级以及属性类别再行导入!");
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
                    dt = importer.Import(ofd.FileName, "SheetATT");
                }
                catch (Exception e)
                {
                    Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                    MessageBox.Show("读取Excel出错!");
                    return;
                }
                #endregion

                #region Step2:解析Excel表格中数据
                List<SKUATT> atts;
                try
                {
                    atts = this.GenerateSKUATTList(dt);
                }
                catch (Exception e)
                {
                    MessageBox.Show("解析Excel数据失败!");
                    return;
                }
                #endregion

                if (atts != null && atts.Count > 0)
                {
                    #region Step3:判断表格数据中编码字段是否与数据库中已有编码冲突
                    #endregion

                    #region Step4:将解析出来的一级数据保存至服务器
                    try
                    {
                        _bll.ImportATTs(atts);
                        this.Refresh();
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                        MessageBox.Show("导入属性数据出错!");
                    }
                    #endregion
                }
            }
        }

        private bool CanCmdImportExecute()
        {
            return this.SelectedSKUCGY != null 
                && this.SelectedSKUCGY.LevelIndex == 2
                && this.SelectedAttType != null;
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
                    SKUATTModel model;
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
                _bll.DeleteATTs(IDs);
                this.Refresh();
                MessageBox.Show("删除成功！");
            }
            catch (Exception e)
            {
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), e.Message));
                MessageBox.Show("删除属性出错!");
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

        private bool CanCmdRefreshExecute()
        {
            return this.RefreshingVisibility == Visibility.Collapsed;
        }

        private void AddOrUpdate_HandleCompleted(object sender, EntityEventArgs e)
        {
            SKUATTModel model = e.Entity as SKUATTModel;
            if (e.IsAdd)
            {
                this.ItemsSource.Add(model);
            }
            else
            {
                SKUATTModel item = this.ItemsSource.Where(x => x.ID == model.ID).First();
                int index = this.ItemsSource.IndexOf(item);
                this.ItemsSource[index] = model;
            }
            this.SelectedItem = model;
        }

        #endregion

        private void Refresh()
        {
            if(this.SelectedSKUCGY != null && this.SelectedSKUCGY.LevelIndex == 2 && this.SelectedAttType != null)
            {
                this.ItemsSource.Clear();
                this.RefreshingVisibility = Visibility.Visible;
                _factorySKUATT.StartNew(GetSKUATTByCondition, new object[]{this.SelectedSKUCGY.ID, this.SelectedAttType.ID}).ContinueWith(GetSKUATTBySKUIDComplete, _schedule);
            }
        }

        private List<SKUCGY> GetAllSKUCGY()
        {
            return _bll.GetAllSKUCGY();
        }

        private void GetAllSKUCGYComplete(Task<List<SKUCGY>> task)
        {
            if (task.Exception != null)
            {
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取一级二级分类出错!");
                return;
            }
            List<SKUCGY> list = task.Result;
            if (list == null || list.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            this.GenerateTree(list);
            if(this.TreeViewRefreshed != null && this.TreeSKUCGY != null && this.TreeSKUCGY.Count > 0)
            {
                this.TreeViewRefreshed(this, null);
            }
        }

        private List<SKUATT> GetSKUATTByCondition(object obj)
        {
            object[] parameters = obj as object[];
            if (parameters != null && parameters.Length == 2)
            {
                Guid skucid = (Guid)parameters[0];
                short attTypeID = (short)parameters[1];
                return _bll.GetSKUATTByCondition(skucid, attTypeID);
            }
            else
            {
                return null;
            }
        }

        private void GetSKUATTBySKUIDComplete(Task<List<SKUATT>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取属性信息出错!");
                return;
            }
            List<SKUATT> list = task.Result;
            if (list == null || list.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            SKUATTModel model;
            foreach (SKUATT att in list)
            {
                model = new SKUATTModel(att);
                this.ItemsSource.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 生成一级二级树
        /// </summary>
        /// <param name="list"></param>
        private void GenerateTree(List<SKUCGY> list)
        {
            foreach (var node in list)
            {
                SKUCGY temp = null;
                if (this.FindParentNode(list, node, out temp))
                {
                    temp.Children.Add(node);
                }
                else
                {
                    this.TreeSKUCGY.Add(node);
                }
            }
        }

        /// <summary>
        /// 在制定列表中查找某个节点的父节点并返回查找情况
        /// </summary>
        /// <param name="list"></param>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private bool FindParentNode(List<SKUCGY> list, SKUCGY child, out SKUCGY parent)
        {
            parent = null;
            var parents = list.Where(x => x.ID == child.PID);
            if (parents != null && parents.Count() > 0)
            {
                parent = parents.First();
                return true;
            }
            else
            {
                return false;
            }
        }


        private List<SKUATT> GenerateSKUATTList(DataTable dt)
        {
            List<SKUATT> result = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                result = new List<SKUATT>();
                SKUATT att = null;
                foreach (DataRow dr in dt.Rows)
                {
                    att = new SKUATT()
                    {
                        Code = dr[0].ToString(),
                        Name = dr[1].ToString(),
                        ATTType = this.SelectedAttType.ID,
                        SKUID = this.SelectedSKUCGY.ID,
                    };
                    result.Add(att);
                }
            }

            return result;
        }

        #endregion
    }
}
