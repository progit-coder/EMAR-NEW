using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.Repositories
{
    public interface IChatRepository
    {
        List<ChatUsersEntity> GetChatUsersList(int userId);
        List<ChatMessageEntity> GetMessageHistory(string fromId, string toId);
        int SendMessage(ChatMessageEntity message);
        int UpdateSocketId(string socketId, int userId);
    }
}
