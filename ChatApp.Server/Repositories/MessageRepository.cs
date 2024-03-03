using ChatApp.Server.Data;
using ChatApp.Server.DTOs.Messages;
using ChatApp.Server.Extensions;
using ChatApp.Server.Models;
using ChatApp.Server.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Server.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Message> AddMessage(AddMessageDto messageDto)
        {
            var message = messageDto.ToModel();
            await _context.AddAsync(message);
            
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<Message> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return default;
            _context.Remove(message);
            await _context.SaveChangesAsync();
            
            return message;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public IEnumerable<Message> GetMessages()
        {
            return _context.Messages;
        }
        public IEnumerable<Message> GetMessages(string userId)
        {
            return _context.Messages.Where(m=>m.SenderId==userId);
        }

        public async Task<Message> UpdateMessage(int id,UpdateMessageDto messageDto)
        {
            var message = await _context.Messages.FindAsync(id);
            message.IsEdited = true;
            message.Body = messageDto.Body;
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
