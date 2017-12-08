using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Chat.Models
{
    [Table(Name = "Chats")]
    public class Chat
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public Int32 Chat_Id { get; set; }
        [Column(UpdateCheck = UpdateCheck.WhenChanged)]
        public DateTime Late_Update { set; get; }
        [Column(IsVersion = true)]
        public System.Data.Linq.Binary Version { set; get; }
        // another objects
        private List<Int32> _listCustmsIds;
        private List<String> _paricipants;
        public Chat()
        {
            _paricipants = new List<string>();
            _listCustmsIds = new List<Int32>();
        }
        public List<Int32> ChatParticipantsIds
        {
            set { _listCustmsIds = value; }
            get { return _listCustmsIds; }
        }
        public List<String> ChatParticipantsNames { set; get; }
        public void AddParticipant(Int32 partId)
        {
            _listCustmsIds.Add(partId);
        }
        public void DeleteParticipant(Int32 partId)
        {
            _listCustmsIds.Remove(partId);
        }
    }
}