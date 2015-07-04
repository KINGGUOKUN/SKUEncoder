using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.Prism.PubSubEvents;
using SKUEncoder.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SKUEncoder
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.InitLog();
            this.InitDatabase();
            ApplicationContext.EventAggregator = new EventAggregator();
            ResourceDictionary defaultDictionary = new ResourceDictionary()
            {
                Source = new Uri("/Themes;component/Themes/Default.xaml", UriKind.RelativeOrAbsolute),
            };
            this.Resources.MergedDictionaries.Add(defaultDictionary);
            MainWindow window = new MainWindow();
            window.Show();
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        private void InitLog()
        {
            string dir = string.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Log");
            string logFile = string.Format(@"{0}\{1}", dir, "Log" + ".txt");
            FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write);
            TextWriterTraceListener listener = new TextWriterTraceListener(fs);
            listener.TraceOutputOptions = TraceOptions.None;
            Trace.Listeners.Add(listener);
            Trace.AutoFlush = true;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        private void InitDatabase()
        {
            IConfigurationSource configurationSource =
                new FileConfigurationSource(string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Database.config"));
            DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(configurationSource));
        }
    }
}
