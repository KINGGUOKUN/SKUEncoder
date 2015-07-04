using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKUEncoder.Entity
{
    public class SKUCGY
    {
        public SKUCGY()
        {
            this.ID = Guid.NewGuid();
            this.Children = new ObservableCollection<SKUCGY>();
        }

        public SKUCGY(string ID)
        {
            Guid guid = Guid.NewGuid();
            Guid.TryParse(ID, out guid);
            this.ID = guid;
            this.Children = new ObservableCollection<SKUCGY>();
        }

        public SKUCGY(Guid ID)
        {
            this.ID = ID;
            this.Children = new ObservableCollection<SKUCGY>();
        }

        /// <summary>
        /// ID键值
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? PID { get; set; }

        public SKUCGY Parent { get; set; }

        /// <summary>
        /// 层次，主要指是几级目录
        /// </summary>
        public short LevelIndex { get; set; }

        public ObservableCollection<SKUCGY> Children
        {
            get;
            private set;
        }
    }
}
