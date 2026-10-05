using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;

namespace LTCPro.ServiceLayer
{
    public interface IChatService
    {
        Task<List<ChatUsersEntity>> GetChatUsersList(int userId);
        Task<List<ChatMessageEntity>> GetMessageHistory(string fromId, string toId);
        Task<int> SendMessage(ChatMessageEntity message);
        Task<int> UpdateSocketId(string socketId, int userId);
    }
}
