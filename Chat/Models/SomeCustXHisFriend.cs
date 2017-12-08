using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using Chat.GenericsMy;

namespace Chat.Models
{
    [Table(Name = "SomeCustXHisFriend")]
    public class SomeCustXHisFriend : ManyToManyGeneric
    {
        [Column(IsPrimaryKey = true, IsDbGenerated =true)]
        public Int32 CC_Id { set; get; }
        [Column(Name = "SomeCustomer")]
        public override Int32 Column_1 { get; set; }
        [Column(Name = "HisFriend")]
        public override Int32 Column_2 { get; set; }
        /*
        public override IEnumerable<Int32> ParamsAsArray()
        {
            Int32[] array = new Int32[2];
            array[0] = this.Column_1;
            array[1] = this.Column_2;
            return array;
        }
        */
    }
    
}