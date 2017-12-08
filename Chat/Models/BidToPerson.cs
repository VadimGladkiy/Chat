using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;

using Chat.GenericsMy;

namespace Chat.Models
{
    [Table(Name ="BidToPerson")]
    public class BidToPerson : ManyToManyGeneric
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public Int32 Bid_Id { set; get; }
        [Column(Name ="InBid")]
        public override Int32 Column_1 { set; get; }
        [Column(Name = "OutBid")]
        public override Int32 Column_2 { set; get; }
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