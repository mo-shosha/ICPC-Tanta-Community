using Core.Entities;
using Core.IRepositories;
using Core.IServices;

namespace ICPC_Tanta_Web.Services
{
    public class ChatService: IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _chatRepository.SaveMessageAsync(message);
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string senderId, string receiverId)
        {
            return await _chatRepository.GetChatHistoryAsync(senderId, receiverId);
        }
    }
}
