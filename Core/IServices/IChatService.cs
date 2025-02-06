using Core.Entities;
using Core.IRepositories;
using Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IChatService
    {
        Task SaveMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetChatHistoryAsync(string senderId, string receiverId);
    }
}


