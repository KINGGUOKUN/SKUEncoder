using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKUEncoder.Entity
{
    public class EntityEventArgs : EventArgs
    {
        public EntityEventArgs(object entity, bool isAdd)
        {
            this.Entity = entity;
            this.IsAdd = isAdd;
        }

        public object Entity { get; set; }
        public bool IsAdd { get; set; }
    }
}
