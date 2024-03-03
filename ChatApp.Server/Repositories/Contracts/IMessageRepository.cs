using ChatApp.Server.DTOs.Messages;
using ChatApp.Server.Models;

namespace ChatApp.Server.Repositories.Contracts
{
    public interface IMessageRepository
    {
        Task<Message> GetMessage(int id);
        IEnumerable<Message> GetMessages();
        IEnumerable<Message> GetMessages(string userId);

        Task<Message> AddMessage(AddMessageDto message);
        Task<Message> DeleteMessage(int id);
        Task<Message> UpdateMessage(int id,UpdateMessageDto message);
    }
}
