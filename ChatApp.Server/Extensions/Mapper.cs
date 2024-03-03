using ChatApp.Server.DTOs.Messages;
using ChatApp.Server.Models;

namespace ChatApp.Server.Extensions
{
    public static class Mapper
    {
        public static Message ToModel(this AddMessageDto messageDto)
        {

            return new Message
            {
                Body = messageDto.Body,
                DateTime = DateTime.UtcNow,
                ReceiverId = messageDto.ReceiverId,
                SenderId = messageDto.SenderId,

            };
        }
        

    }
}
