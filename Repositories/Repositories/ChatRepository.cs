using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.DAL;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public class ChatRepository:IChatRepository
    {
        readonly IAutoMapper autoMapper;
        private readonly IDbContextEmar dbContext;
        public ChatRepository(IAutoMapper autoMapper, IDbContextEmar dbContext)
        {
            this.autoMapper = autoMapper;
            this.dbContext = dbContext;
        }
        public List<ChatUsersEntity> GetChatUsersList(int userId)
        {
            //var users = this.dbContext.Users1.Where(cu => cu.Is_Active == 1 && cu.User_Id!=userId).Select(cu => cu.User_Id).ToList();
            var users = this.dbContext.Users1.Where(cu => cu.Is_Active == 1).Select(cu => cu.User_Id).ToList();
            List<ChatUsersEntity> chatusers = new List<ChatUsersEntity>();
            for(int i=0;i<users.Count;i++)
            {
                ChatUsersEntity obj = new ChatUsersEntity();
                int usersId = Convert.ToInt16(users[i]);
                var user = (from us in this.dbContext.Users
                             join cu in this.dbContext.Users1 on us.User_Id equals cu.User_Id
                             where cu.User_Id == usersId
                            select new ChatParticipantEntity {
                                 id=cu.User_Id,
                                 displayName=us.User_DisplayName,
                                 avatar=null,
                                 status=0,
                                 participantType=0
                             }).FirstOrDefault();
                obj.participant = user;
                obj.metadata = 1;
                chatusers.Add(obj);
            }
            return chatusers;
        }
        public List<ChatMessageEntity> GetMessageHistory(string fromId, string toId)
        {
            int conversationId = 0;
            List<ChatMessageEntity> messages = new List<ChatMessageEntity>();
            var fromUserId = this.dbContext.Users1.Where(us => us.socket_Id == fromId).Select(s => s.User_Id).FirstOrDefault();
            var toUserId = this.dbContext.Users1.Where(us => us.socket_Id == toId).Select(s => s.User_Id).FirstOrDefault();
            var conversaion1 = this.dbContext.Conversations.Where(con => con.Creator_Id == fromUserId && con.Receiver_Id == toUserId).FirstOrDefault();
            var conversaion2 = this.dbContext.Conversations.Where(con => con.Creator_Id == toUserId && con.Receiver_Id == fromUserId).FirstOrDefault();
            if (conversaion1==null && conversaion2==null)
            {
                Conversation conObj = new Conversation();
                conObj.Title = "Personal Chat";
                conObj.Creator_Id = fromUserId;
                conObj.Receiver_Id = toUserId;
                conObj.Created_Date = DateTime.Now;
                this.dbContext.Conversations.Add(conObj);
                this.dbContext.SaveChanges();
                conversationId = Convert.ToInt16(this.dbContext.Conversations.Where(con => con.Creator_Id == fromUserId && con.Receiver_Id == toUserId).Select(cn=>cn.Conversation_Id).FirstOrDefault());
            }
            else
            {
                if(conversaion1 != null)
                conversationId = Convert.ToInt16(conversaion1.Conversation_Id);
                else
                    conversationId = Convert.ToInt16(conversaion2.Conversation_Id);
            }
            if(conversationId!=0)
            {
                messages = (from ms in this.dbContext.Messages
                            join us in this.dbContext.Users1 on ms.Sender_Id equals us.User_Id
                            where ms.Conversation_Id == conversationId
                            select new ChatMessageEntity {
                                type=ms.Message_Type,
                                fromId= us.socket_Id,
                                toId= fromId,
                                message=ms.Message1,
                                dateSent=ms.Created_Date,
                                dateSeen=ms.Created_Date
                            }).ToList();
                return messages;
            }
            else
            {
                return null;
            }
        }
        public int SendMessage(ChatMessageEntity message)
        {
            var fromUserId = this.dbContext.Users1.Where(us => us.socket_Id == message.fromId).Select(s => s.User_Id).FirstOrDefault();
            var toUserId = this.dbContext.Users1.Where(us => us.socket_Id == message.toId).Select(s => s.User_Id).FirstOrDefault();
            var conversationId = this.dbContext.Conversations.Where(cn => cn.Creator_Id == fromUserId && cn.Receiver_Id == toUserId).Select(cn=>cn.Conversation_Id).FirstOrDefault();
            if(conversationId==0)
                conversationId = this.dbContext.Conversations.Where(cn => cn.Creator_Id == toUserId && cn.Receiver_Id == fromUserId).Select(cn => cn.Conversation_Id).FirstOrDefault();
            Message msg = new Message();
            msg.Message_Type = message.type;
            msg.Sender_Id = fromUserId;
            msg.Conversation_Id = conversationId;
            msg.Message1 = message.message;
            msg.Created_Date = DateTime.Now;
            this.dbContext.Messages.Add(msg);
            this.dbContext.SaveChanges();
            return 1;
        }
        public int UpdateSocketId(string socketId,int userId)
        {
            var record = this.dbContext.Users1.Where(us => us.User_Id == userId).FirstOrDefault();
            if(record!=null)
            {
                record.socket_Id = socketId;
                this.dbContext.SaveChanges();
            }
            return 1;
        }
    }
}
