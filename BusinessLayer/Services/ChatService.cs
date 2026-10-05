using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LTCPro.Entities;
using LTCPro.Repositories;

namespace LTCPro.ServiceLayer
{
    public class ChatService:IChatService
    {
        private readonly IAutoMapper _autoMapper;
        private readonly IChatRepository _chatRepository;
        private readonly ILogger _log;
        public ChatService(IAutoMapper autoMapper, IChatRepository chatRepository, ILogger log)
        {
            this._autoMapper = autoMapper;
            this._chatRepository = chatRepository;
            this._log = log;
        }
        public async Task<List<ChatUsersEntity>> GetChatUsersList(int userId)
        {
            this._log.Debug("---Executing GetChatUsersList() in ChatService----");
            return await Task.FromResult<List<ChatUsersEntity>>(this._chatRepository.GetChatUsersList(userId));
        }
        public async Task<List<ChatMessageEntity>> GetMessageHistory(string fromId, string toId)
        {
            this._log.Debug("---Executing getMessageHistory() in ChatService----");
            return await Task.FromResult<List<ChatMessageEntity>>(this._chatRepository.GetMessageHistory(fromId, toId));
        }
        public async Task<int> SendMessage(ChatMessageEntity message)
        {
            this._log.Debug("---Executing SendMessage() in ChatService----");
            return await Task.FromResult<int>(this._chatRepository.SendMessage(message));
        }
        public async Task<int> UpdateSocketId(string socketId, int userId)
        {
            this._log.Debug("---Executing UpdateSocketId() in ChatService----");
            return await Task.FromResult<int>(this._chatRepository.UpdateSocketId(socketId, userId));
        }
    }
}
