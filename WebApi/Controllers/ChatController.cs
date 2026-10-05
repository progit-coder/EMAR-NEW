using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LTCPro.ServiceLayer;
using LTCPro.Entities;
using WebApi.Filters;
using System.Web.Http.Results;

namespace LTCPro.WebApi.Controllers
{
    [CustomApiExceptionFilter]
    [RoutePrefix("Chat")]
    [CustomAuthorizationFilter]
    public class ChatController : ApiController
    {
        private readonly IChatService _chatService;
        private readonly ILogger _log;
        public ChatController(IChatService chatService, ILogger log)
        {
            this._chatService = chatService;
            this._log = log;
        }
        [Route("GetChatUsersList/{userId}")]
        [HttpGet]
        public JsonResult<List<ChatUsersEntity>> GetChatUsersList(int userId)
        {
            this._log.Debug("---Executing GetOrderDetails() in ChatController----");
            var usersData = this._chatService.GetChatUsersList(userId).Result;
            this._log.Debug("---Executed Successfully GetOrderDetails() in ChatController----");
            return Json<List<ChatUsersEntity>>(usersData);
        }
        [Route("GetMessageHistory/{fromId}/{toId}")]
        [HttpGet]
        public JsonResult<List<ChatMessageEntity>> GetMessageHistory(string fromId, string toId)
        {
            this._log.Debug("---Executing GetMessageHistory() in ChatController----");
            var messageData = this._chatService.GetMessageHistory(fromId, toId).Result;
            this._log.Debug("---Executed Successfully GetMessageHistory() in ChatController----");
            return Json<List<ChatMessageEntity>>(messageData);
        }
        [Route("SendMessage")]
        [HttpPost]
        public int SendMessage(ChatMessageEntity message)
        {
            this._log.Debug("---Executing SendMessage() in ChatController----");
            var result = this._chatService.SendMessage(message).Result;
            this._log.Debug("---Executed Successfully SendMessage() in ChatController----");
            return result;
        }
        [Route("UpdateSocketId/{socketId}/{userId}")]
        [HttpGet]
        public int UpdateSocketId(string socketId, int userId)
        {
            this._log.Debug("---Executing UpdateSocketId() in ChatController----");
            var result = this._chatService.UpdateSocketId(socketId, userId).Result;
            this._log.Debug("---Executed Successfully UpdateSocketId() in ChatController----");
            return result;
        }
    }
}