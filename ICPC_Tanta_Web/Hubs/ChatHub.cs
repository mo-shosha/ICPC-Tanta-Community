using Core.Entities;
using Core.IServices;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ICPC_Tanta_Web.Hubs
{
    public class ChatHub: Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId)) return;

            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message
            };

            await _chatService.SaveMessageAsync(chatMessage);

            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
        }

        public async Task GetChatHistory(string receiverId)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(senderId)) return;

            var messages = await _chatService.GetChatHistoryAsync(senderId, receiverId);
            await Clients.Caller.SendAsync("ChatHistory", messages);
        }
    }
}
