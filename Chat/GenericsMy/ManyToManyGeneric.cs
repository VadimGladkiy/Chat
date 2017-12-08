using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.GenericsMy
{
    public abstract class ManyToManyGeneric: Object
    {
        public virtual IEnumerable<Int32> ParamsAsArray()
        {
            Int32[] array = new Int32[2];
            array[0] = this.Column_1;
            array[1] = this.Column_2;
            return array;
        }
        public abstract Int32 Column_1 { set; get; }
        public abstract Int32 Column_2 { set; get; }
    }
}