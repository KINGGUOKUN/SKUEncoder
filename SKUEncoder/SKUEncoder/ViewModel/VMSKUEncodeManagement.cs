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
    public class VMSKUEncodeManagement : BindableBase
    {
        #region Private Fields

        private BLLSKUEncodeManagement _bll;
        private BLLAttManagement _bllAtt;
        private TaskFactory<List<SKUCGY>> _factorySKUCGY = new TaskFactory<List<SKUCGY>>();
        private TaskFactory<List<SKUATT>> _factorySKUATT = new TaskFactory<List<SKUATT>>();
        private TaskFactory<List<SKUEncode>> _factorySKUEncode = new TaskFactory<List<SKUEncode>>();
        private TaskScheduler _schedule = TaskScheduler.FromCurrentSynchronizationContext();

        #endregion

        #region Constructors

        public VMSKUEncodeManagement()
        {
            _bll = new BLLSKUEncodeManagement();
            _bllAtt = new BLLAttManagement();
            this.ItemsSource = new ObservableCollection<SKUEncodeModel>();
            this.ItemsSource.CollectionChanged += (s, e) =>
                {
                    (this.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
                    (this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
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
        /// 删除命令
        /// </summary>
        public ICommand CmdDel { get; private set; }

        public ICommand CmdExport { get; private set; }

        /// <summary>
        /// 刷新命令
        /// </summary>
        public ICommand CmdRefresh { get; private set; }

        public ICommand CmdSearch { get; private set; }

        public event EventHandler TreeViewRefreshed;

        #endregion

        #region Public Properties

        public ObservableCollection<SKUEncodeModel> ItemsSource
        {
            get;
            private set;
        }

        private SKUEncodeModel _selectedItem;
        public SKUEncodeModel SelectedItem
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
                this.ClearAtts();
                if (value != null && value.LevelIndex == 2)
                {
                    this.LoadAttsBySKUID();
                }
            }
        }

        private ObservableCollection<SKUATT> _colATT3 = new ObservableCollection<SKUATT>();
        public ObservableCollection<SKUATT> ColATT3
        {
            get
            {
                return _colATT3;
            }
            set
            {
                base.SetProperty(ref _colATT3, value);
            }
        }

        private SKUATT _selectedATT3;
        public SKUATT SeletedATT3
        {
            get
            {
                return _selectedATT3;
            }
            set
            {
                base.SetProperty(ref _selectedATT3, value);
                //(this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SKUATT> _colATT4 = new ObservableCollection<SKUATT>();
        public ObservableCollection<SKUATT> ColATT4
        {
            get
            {
                return _colATT4;
            }
            set
            {
                base.SetProperty(ref _colATT4, value);
            }
        }

        private SKUATT _selectedATT4;
        public SKUATT SeletedATT4
        {
            get
            {
                return _selectedATT4;
            }
            set
            {
                base.SetProperty(ref _selectedATT4, value);
                //(this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SKUATT> _colATT5 = new ObservableCollection<SKUATT>();
        public ObservableCollection<SKUATT> ColATT5
        {
            get
            {
                return _colATT5;
            }
            set
            {
                base.SetProperty(ref _colATT5, value);
            }
        }

        private SKUATT _selectedATT5;
        public SKUATT SeletedATT5
        {
            get
            {
                return _selectedATT5;
            }
            set
            {
                base.SetProperty(ref _selectedATT5, value);
                //(this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SKUATT> _colATT6 = new ObservableCollection<SKUATT>();
        public ObservableCollection<SKUATT> ColATT6
        {
            get
            {
                return _colATT6;
            }
            set
            {
                base.SetProperty(ref _colATT6, value);
            }
        }

        private SKUATT _selectedATT6;
        public SKUATT SeletedATT6
        {
            get
            {
                return _selectedATT6;
            }
            set
            {
                base.SetProperty(ref _selectedATT6, value);
                //(this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<SKUATT> _colATT7 = new ObservableCollection<SKUATT>();
        public ObservableCollection<SKUATT> ColATT7
        {
            get
            {
                return _colATT7;
            }
            set
            {
                base.SetProperty(ref _colATT7, value);
            }
        }

        private SKUATT _selectedATT7;
        public SKUATT SeletedATT7
        {
            get
            {
                return _selectedATT7;
            }
            set
            {
                base.SetProperty(ref _selectedATT7, value);
                //(this.CmdAdd as DelegateCommand).RaiseCanExecuteChanged();
            }
        }

        private string _SKUKeyword;
        public string SKUKeyword
        {
            get
            {
                return _SKUKeyword;
            }
            set
            {
                base.SetProperty(ref _SKUKeyword, value);
            }
        }

        private bool _isKeywordChecked;
        public bool IsKeywordChecked
        {
            get
            {
                return _isKeywordChecked;
            }
            set
            {
                base.SetProperty(ref _isKeywordChecked, value);
                if (value)
                {
                    this.KeywordVisibility = Visibility.Visible;
                }
                else
                {
                    this.KeywordVisibility = Visibility.Collapsed;
                    this.SKUKeyword = null;
                }
            }
        }

        private Visibility _keywordVisibility = Visibility.Collapsed;
        public Visibility KeywordVisibility
        {
            get
            {
                return _keywordVisibility;
            }
            set
            {
                base.SetProperty(ref _keywordVisibility, value);
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
            this.CmdDel = new DelegateCommand(this.CmdDelExecute, this.CanCmdDelExecute);
            this.CmdExport = new DelegateCommand(this.CmdExportExecute, this.CanCmdExportExecute);
            this.CmdRefresh = new DelegateCommand(this.CmdRefreshExecute, this.CanCmdRefreshExecute);
            this.CmdSearch = new DelegateCommand(this.CmdSearchExecute, this.CanCmdSearchExecute);
        }

        private void InitData()
        {
            this.InitSKUCGYTree();
        }

        private void InitSKUCGYTree()
        {
            this.TreeSKUCGY.Clear();
            _factorySKUCGY.StartNew(GetAllSKUCGY).ContinueWith(GetAllSKUCGYComplete, _schedule);
        }

        private void LoadAttsBySKUID()
        {
            _factorySKUATT.StartNew(GetSKUATTByCondition, this.SelectedSKUCGY.ID).ContinueWith(GetSKUATTByConditionComplete, _schedule);
        }

        private void ClearAtts()
        {
            this.ColATT3.Clear();
            this.ColATT4.Clear();
            this.ColATT5.Clear();
            this.ColATT6.Clear();
            this.ColATT7.Clear();
            this.ColATT3.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            this.ColATT4.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            this.ColATT5.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            this.ColATT6.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            this.ColATT7.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            this.SeletedATT3 = this.ColATT3.FirstOrDefault();
            this.SeletedATT4 = this.ColATT4.FirstOrDefault();
            this.SeletedATT5 = this.ColATT5.FirstOrDefault();
            this.SeletedATT6 = this.ColATT6.FirstOrDefault();
            this.SeletedATT7 = this.ColATT7.FirstOrDefault();
            this.CmdSearchExecute();
        }

        #region Command Executes

        private void CmdAddExecute()
        {
            SKUEncodeDetail detail = new SKUEncodeDetail() 
            {
                One = this.SelectedSKUCGY.Parent,
                Two = this.SelectedSKUCGY,
                ATT3 = this.SeletedATT3,
                ATT4 = this.SeletedATT4,
                ATT5 = this.SeletedATT5,
                ATT6 = this.SeletedATT6,
                ATT7 = this.SeletedATT7
            };
            AddOrUpdateSkuEncode winAdd = new AddOrUpdateSkuEncode(detail);
            winAdd.Owner = Application.Current.MainWindow;
            winAdd.HandleCompleted += AddOrUpdate_HandleCompleted;
            winAdd.Show();
        }

        private bool CanCmdAddExecute()
        {
            return this.SelectedSKUCGY != null && this.SelectedSKUCGY.LevelIndex == 2
                && this.SeletedATT3 != null && this.SeletedATT3.ID != Guid.Empty
                && this.SeletedATT4 != null && this.SeletedATT4.ID != Guid.Empty
                && this.SeletedATT5 != null && this.SeletedATT5.ID != Guid.Empty
                && this.SeletedATT6 != null && this.SeletedATT6.ID != Guid.Empty
                && this.SeletedATT7 != null && this.SeletedATT7.ID != Guid.Empty
                && this.ItemsSource.Count < 1;
        }

        private void CmdUpdateExecute()
        {
            AddOrUpdateSkuEncode winUpdate = new AddOrUpdateSkuEncode(this.SelectedItem);
            winUpdate.Owner = Application.Current.MainWindow;
            winUpdate.HandleCompleted += AddOrUpdate_HandleCompleted;
            winUpdate.ShowDialog();
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
                _bll.DeleteSKUEncodes(IDs);
                this.CmdSearch.Execute(null);
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

        private void CmdExportExecute()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Files(*.xlsx,*.xls)|*.xlsx;*.xls";
            bool? result = sfd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                using (ExcelExporter exporter = new ExcelExporter())
                {
                    var items = this.ItemsSource.Where(x => x.IsChecked).ToList();
                    SKUEncodeModel model;
                    for (int i = 0; i < items.Count; i++)
                    {
                        model = items[i];
                        exporter.SetCellValue(i + 1, 1, model.Code);
                        exporter.SetCellValue(i + 1, 2, model.Name);
                    }
                    exporter.ColumnAutoFit(1, 2);
                    exporter.SaveAs(sfd.FileName);
                }
            }
        }

        private bool CanCmdExportExecute()
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

        private void CmdSearchExecute()
        {
            if(this.RefreshingVisibility == Visibility.Visible)
            {
                return;
            }
            this.ItemsSource.Clear();
            this.RefreshingVisibility = Visibility.Visible;
            SKUSearchParams param = new SKUSearchParams();
            //param.IDOne = this.SelectedSKUCGY.PID.HasValue ? this.SelectedSKUCGY.PID.Value : Guid.Empty;
            //param.IDTwo = this.SelectedSKUCGY.ID;
            param.SelectedSKUCID = this.SelectedSKUCGY == null ? Guid.Empty : this.SelectedSKUCGY.ID;
            param.Att3 = this.SeletedATT3.ID;
            param.Att4 = this.SeletedATT4.ID;
            param.Att5 = this.SeletedATT5.ID;
            param.Att6 = this.SeletedATT6.ID;
            param.Att7 = this.SeletedATT7.ID;
            param.Keyword = this.SKUKeyword;
            _factorySKUEncode.StartNew(this.GetSKUEncodeByCondition, param).ContinueWith(this.GetSKUEncodeByConditionComplete, _schedule);
        }

        private bool CanCmdSearchExecute()
        {
            return this.RefreshingVisibility == Visibility.Collapsed;
        }

        private void AddOrUpdate_HandleCompleted(object sender, EntityEventArgs e)
        {
            SKUEncodeModel model = e.Entity as SKUEncodeModel;
            if (e.IsAdd)
            {
                this.ItemsSource.Add(model);
            }
            else
            {
                SKUEncodeModel item = this.ItemsSource.Where(x => x.ID == model.ID).First();
                int index = this.ItemsSource.IndexOf(item);
                this.ItemsSource[index] = model;
            }
            this.SelectedItem = model;
        }

        #endregion

        private void Refresh()
        {
            //if(this.SelectedSKUCGY != null && this.SelectedSKUCGY.LevelIndex == 2 && this.SelectedAttType != null)
            //{
            //    this.ItemsSource.Clear();
            //    this.RefreshingVisibility = Visibility.Visible;
            //}
        }

        #region 生成一级二级树相关

        private List<SKUCGY> GetAllSKUCGY()
        {
            return _bllAtt.GetAllSKUCGY();
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
                    node.Parent = temp;
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

        #endregion

        #region 生成5级属性菜单

        private List<SKUATT> GetSKUATTByCondition(object obj)
        {
            Guid skucid = (Guid)obj;
            return _bllAtt.GetSKUATTByCondition(skucid);
        }

        private void GetSKUATTByConditionComplete(Task<List<SKUATT>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取属性信息出错!");
                return;
            }
            List<SKUATT> list = task.Result;
            this.GenerateAttLists(list);
        }

        private void GenerateAttLists(List<SKUATT> atts)
        {
            if (atts != null && atts.Count(x => x.ATTType == 3) > 0)
            {
                this.ColATT3 = new ObservableCollection<SKUATT>(atts.Where(x => x.ATTType == 3));    
            }
            if(this.ColATT3.Count(x => x.ID == Guid.Empty) == 0)
            {
                this.ColATT3.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            }
            this.SeletedATT3 = this.ColATT3.FirstOrDefault();
            if (atts != null && atts.Count(x => x.ATTType == 4) > 0)
            {
                this.ColATT4 = new ObservableCollection<SKUATT>(atts.Where(x => x.ATTType == 4));
            }
            if (this.ColATT4.Count(x => x.ID == Guid.Empty) == 0)
            {
                this.ColATT4.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            }
            this.SeletedATT4 = this.ColATT4.FirstOrDefault();
            if (atts != null && atts.Count(x => x.ATTType == 5) > 0)
            {
                this.ColATT5 = new ObservableCollection<SKUATT>(atts.Where(x => x.ATTType == 5));
            }
            if (this.ColATT5.Count(x => x.ID == Guid.Empty) == 0)
            {
                this.ColATT5.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            }
            this.SeletedATT5 = this.ColATT5.FirstOrDefault();
            if (atts != null && atts.Count(x => x.ATTType == 6) > 0)
            {
                this.ColATT6 = new ObservableCollection<SKUATT>(atts.Where(x => x.ATTType == 6));
            }
            if (this.ColATT6.Count(x => x.ID == Guid.Empty) == 0)
            {
                this.ColATT6.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            }
            this.SeletedATT6 = this.ColATT6.FirstOrDefault();
            if (atts != null && atts.Count(x => x.ATTType == 7) > 0)
            {
                this.ColATT7 = new ObservableCollection<SKUATT>(atts.Where(x => x.ATTType == 7));
            }
            if (this.ColATT7.Count(x => x.ID == Guid.Empty) == 0)
            {
                this.ColATT7.Insert(0, new SKUATT(Guid.Empty) { Name = "全部" });
            }
            this.SeletedATT7 = this.ColATT7.FirstOrDefault();
            this.CmdSearch.Execute(null);
        }

        #endregion

        #region 获取SKU编码列表

        private List<SKUEncode> GetSKUEncodeByCondition(object obj)
        {
            SKUSearchParams param = obj as SKUSearchParams;
            return _bll.GetSKUEncodeByCondition(param);
        }

        private void GetSKUEncodeByConditionComplete(Task<List<SKUEncode>> task)
        {
            if (task.Exception != null)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                Trace.TraceError(string.Format(@"{0},{1}", DateTime.Now.ToString(), task.Exception.Message));
                MessageBox.Show("获取SKU编码信息出错!");
                return;
            }
            List<SKUEncode> list = task.Result;
            if (list == null || list.Count == 0)
            {
                this.RefreshingVisibility = Visibility.Collapsed;
                return;
            }
            SKUEncodeModel model;
            foreach (SKUEncode encode in list)
            {
                model = new SKUEncodeModel(encode);
                this.ItemsSource.Add(model);
            }
            this.RefreshingVisibility = Visibility.Collapsed;
        }

        #endregion

        #endregion
    }
}
