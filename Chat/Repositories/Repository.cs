using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Configuration;
using Chat.JsonStruct;
using Chat.Models;

using Chat.GenericsMy;

namespace Chat.Repositories
{
    public class Repository
    {
        //fields
        private static readonly String connectionString =
            ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private Int32 user_identity;
        private DataContext data_entities;
        private List<Models.Chat> chats;
        private List<Models.Message> msgs;
        private List<Models.Friend> friends;
        // constructors
        public Repository(Int32 userId)
        {
            user_identity = userId;
            data_entities = new DataContext(connectionString);
            ChatsInitialize(userId);
        }
        private void FriendsInitializing<T>(int user) where T: ManyToManyGeneric
        {
            IEnumerable<T> _elems =
                data_entities.GetTable<T>()
                .Where(y => y.Column_2 == user ||
                y.Column_1 == user ).ToList();

            var _Ids = _elems
                .SelectMany(y =>  y.ParamsAsArray())
                .Except(new Int32[] { user });

            _Ids.Count();
            var TempCusts = data_entities.GetTable<Customer>()
                .Where(x => _Ids.Contains(x.Customer_id))
                .ToList();

            if (TempCusts.Count() == 0) return;
            if (friends == null)
                friends = new List<Friend>();
            Friend _fr;
            foreach (var cust in TempCusts)
            {
                _fr = new Friend()
                {
                    FriendId = cust.Customer_id,
                    FullName = cust.FullName,
                    Email = cust.Email,
                    Login = cust.Login,
                };
                friends.Add(_fr);
            }
        }
        internal void SetBidsList(int userId)
        {
            FriendsInitializing<BidToPerson>(userId);
        }

        internal void SetFriendList(int userId)
        {
            FriendsInitializing<SomeCustXHisFriend>(userId);
        }
        /*
public void FriendsInitialize(int userId)
{

   IEnumerable<SomeCustXHisFriend> _friends = 
       data_entities.GetTable<SomeCustXHisFriend>()
       .Where(y => y.SomeCustomer == userId ||
       y.HisFriend == userId).ToList();

   var _Ids = _friends 
   .SelectMany(y => y.ParamsAsArray()).Except(new Int32[]{ userId });
   _Ids.Count();

   var TempCusts = data_entities.GetTable<Customer>()
       .Where(x => _Ids.Contains(x.Customer_id))
       .ToList();

   if (TempCusts.Count() == 0) return;
   if (friends == null)
   friends = new List<Friend>();
   Friend _friend = new Friend();
   foreach (var cust in TempCusts)
   {
       _friend.FriendId = cust.Customer_id;
       _friend.FullName = cust.FullName;
       _friend.Email = cust.Email;
       _friend.Login = cust.Login;

       friends.Add(_friend);
   }
}
*/
        /*
        public void BidsInitialize(int userId)
        {
            IEnumerable<BidToPerson> _bids =
                data_entities.GetTable<BidToPerson>()
                .Where(y => y.OutBid == userId).ToList();

            var _Ids = _bids
            .SelectMany(y => new int[] { y.InBid, y.OutBid })
            .Except(new Int32[] { userId });

            _Ids.Count();

            var TempCusts = data_entities.GetTable<Customer>()
                .Where(x => _Ids.Contains(x.Customer_id))
                .ToList();

            if (TempCusts.Count() == 0) return;
            if (friends == null)
                friends = new List<Friend>();
            Friend _friend = new Friend();
            foreach (var cust in TempCusts)
            {
                _friend.FriendId = cust.Customer_id;
                _friend.FullName = cust.FullName;
                _friend.Email = cust.Email;
                _friend.Login = cust.Login;

                friends.Add(_friend);
            }
            throw new NotImplementedException();
        }
        */
        public void BidToPerson(Int32 fromWho, Int32 bidTo)
        {
            Customer askedPers = data_entities.GetTable<Customer>()
                .FirstOrDefault(x => x.Customer_id == bidTo);
            if (askedPers != null)
            {
                // create a new bid  
                var Bid = new BidToPerson()
                {
                    Column_1 = fromWho,
                    Column_2 = bidTo 
                };
                data_entities.GetTable<BidToPerson>()
                .InsertOnSubmit(Bid);
                // save
                data_entities.SubmitChanges();
            }
        }

        public Repository()
        {
            data_entities = new DataContext(connectionString);
        }
        // ops chat
        public void CreateChat(Int32 user, Int32 friend)
        {
            // create a new chat (first it is empty)
            var new_chat = new Models.Chat
            {
                Late_Update = DateTime.Now
            };
            data_entities.GetTable<Models.Chat>()
                .InsertOnSubmit(new_chat);
            data_entities.SubmitChanges();

            // bind customer with this chat
            var chat_user = new Conversation
            {
                Customer_Identifier = user,
                Chat_Identifier = new_chat.Chat_Id
            };
            var chat_friend = new Conversation
            {
                Customer_Identifier = friend,
                Chat_Identifier = new_chat.Chat_Id
            };
            
            data_entities.GetTable<Conversation>()
                .InsertAllOnSubmit( new Conversation[] { chat_user, chat_friend });
            data_entities.SubmitChanges();
        }
        public void ChatsInitialize(Int32 currUser)
        {
            try
            {
                // get all chats for User
                List<Int32> conversations = data_entities.GetTable<Conversation>()
                    .Where(x => x.Customer_Identifier == currUser)
                    .Select(x => x.Chat_Identifier).ToList();
                if (conversations.Count() == 0) return;
                // set chats into list
                List<Models.Chat> Query = data_entities.GetTable<Models.Chat>()
                .Where(x => conversations.Contains(x.Chat_Id)).ToList();
                if (Query.Count() != 0) chats = Query.ToList();
                // set participants for each chat
                foreach (var chat in chats)
                {
                    List<Int32> Ids = data_entities.GetTable<Conversation>()
                    .Where(x => x.Chat_Identifier == chat.Chat_Id)
                    .Select(x => x.Customer_Identifier).ToList();
                    chat.ChatParticipantsIds = Ids;
                    chat.ChatParticipantsNames = data_entities.GetTable<Customer>()
                    .Where(x => Ids.Contains(x.Customer_id))
                    .Select(x => x.FullName).ToList();
                }
            }
            catch (Exception)
            {

            }
        }
        public void ChoseChatInitialize(Int32 chat_id)
        {
            msgs = data_entities.GetTable<Message>()
            .Where(x => x.Chat_Iden == chat_id)
            .OrderBy(x => x.Time).ToList();
        }
        // functions with open access
        public void InsertCustomer(Customer user)
        {
            data_entities.GetTable<Customer>().InsertOnSubmit(user);
            data_entities.SubmitChanges();
        }
        public List<Models.Chat> GetCurrentChats()
        {
            return chats;
        }
        public List<Models.Message> GetMessages()
        {
            return msgs;
        }
        public List<Models.Friend> GetFriends()
        {
            return friends;
        }
        public List<Models.Friend> GetFoundPeople(String seekingName)
        {
            List<Customer> foundPeple = data_entities.GetTable<Customer>()
                .Where(x => x.FullName == seekingName).ToList();
            List<Friend> _friends = new List<Friend>();
            foreach (var fr in foundPeple)
            {
                _friends.Add(new Friend
                {
                    FriendId = fr.Customer_id,
                    Email = fr.Email,
                    FullName = fr.FullName,
                    Login = fr.Login
                });
            }
            return _friends;
        }
        public void AcceptBid(Int32 currUser, Int32 applicant_id) 
        {
            var bid = data_entities.GetTable<BidToPerson>()
                .Where(x => x.Column_1 == applicant_id &&
                x.Column_2 == currUser).FirstOrDefault();

            if(bid != null)
            {
                // add friend
                var communication = new SomeCustXHisFriend
                {
                    Column_1 = currUser,
                    Column_2 = applicant_id
                };
                data_entities.GetTable<SomeCustXHisFriend>()
                    .InsertOnSubmit(communication);

                // delete bid
                data_entities.GetTable<BidToPerson>().DeleteOnSubmit(bid);
                // data save
                data_entities.SubmitChanges();
            }
            
        }
        public void DenyBid(Int32 currUser, Int32 applicant_id)
        {
            // delete bid
            var bid = data_entities.GetTable<BidToPerson>()
                .Where(x => x.Column_1 == applicant_id &&
                x.Column_2 == currUser).FirstOrDefault();
            if (bid != null)
            {
                data_entities.GetTable<BidToPerson>().DeleteOnSubmit(bid);
                data_entities.SubmitChanges();
            }
        }
        public void RemoveFriend(Int32 user, Int32 comrad)
        {
            // remove comrad
            var friend = data_entities.GetTable<SomeCustXHisFriend>()
                .Where(x => (x.Column_1 == user || x.Column_1 == comrad )
                && (x.Column_2 == user || x.Column_2 == comrad ))
                .FirstOrDefault();
            if (friend != null)
            {
                data_entities.GetTable<SomeCustXHisFriend>()
                    .DeleteOnSubmit(friend);
                data_entities.SubmitChanges();
            }
        }
        public void AddMessage(Message _msg, Int32 chat_id)
        {
            var chat = data_entities.GetTable<Models.Chat>()
                .FirstOrDefault(x => x.Chat_Id == chat_id);
            if (chat != null)
            {
                var message = new Message
                {
                    Chat_Iden = _msg.Chat_Iden,
                    OwnerId = _msg.OwnerId,
                    OwnerName = _msg.OwnerName,
                    Text = _msg.Text,
                    Time = _msg.Time
                };


                // This will throw an exception if the order has order details.

                chat.Late_Update = DateTime.Now;
                
                data_entities.GetTable<Message>()
                   .InsertOnSubmit(message);
                try
                {
                    // ConflictMode is an optional parameter.
                    data_entities.SubmitChanges(ConflictMode.ContinueOnConflict);
                }
                catch (ChangeConflictException e)
                {
                    data_entities.ChangeConflicts.ResolveAll(RefreshMode.KeepChanges);
                    {
                        try
                        {
                            data_entities.SubmitChanges();
                        }
                        catch (ChangeConflictException) { }
                    }

                }
            }
        }
        public IEnumerable<Message> UpdatedMsgs(int chatId, int lastMsgId)
        {
            IQueryable<Message> allMessages = 
            data_entities.GetTable<Message>()
                .Where(x => x.Chat_Iden == chatId)
                .OrderBy(x => x.Time);
            Message Last = allMessages
                .FirstOrDefault(x => x.Message_Id == lastMsgId);
            IQueryable<Message> afterLastNote = allMessages
                .Where(x => x.Time > Last.Time);
            IEnumerable<Message> updatedMsgs = afterLastNote;
            return updatedMsgs;
        }
        public Boolean IfBids(Int32 userId)
        {
            Boolean IsNewBid =
            data_entities.GetTable<BidToPerson>()
                .Where(x => x.Column_2 == userId).Any();
            return IsNewBid;
        }
        public IList<Int32> IfChatsMustUpdate(Int32 user_id, List<JsonData> lastUpdates)
        {
            IList<Int32> ToReturn;
            ChatsInitialize(user_id);

            if (chats == null) return new List<Int32>();

            if (lastUpdates != null)
            {
                var upIds = lastUpdates.Select(x => x.Id);
                ToReturn = chats.Select(t => t.Chat_Id).Except(upIds)
                    .ToList();
                try
                {
                    foreach (var c in chats)
                    {
                        var p = lastUpdates.First(x => x.Id == c.Chat_Id);
                        DateTime pTime;
                        DateTime.TryParse(p.Time, out pTime);
                        int result = DateTime.Compare(pTime,c.Late_Update);
                        if (result == 1)
                            ToReturn.Add(p.Id);
                        else; //do nothing 
                    }
                }
                catch (Exception) { }
            }
            else
            {
                ToReturn = new List<int>();
            }
            return ToReturn;
        }
    }

}