using ChatApp.Server.DTOs.Messages;
using ChatApp.Server.Models;
using ChatApp.Server.Repositories.Contracts;
using ChatApp.Server.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Server.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly JWTService _jwtService;

        public override async Task OnConnectedAsync() {
            var messages = _messageRepository.GetMessages(GetId()).ToList();
            await Clients.Caller.SendAsync("ReceiveMessages",
                messages);
        }   
        
        
        private string GetId()
        {
            var token = Context.GetHttpContext().Request.Headers["Authorization"];
            
            return _jwtService.GetClaim(token, "nameid");
        }
        public ChatHub(IMessageRepository messageRepository,JWTService jwtService)
        {
            _messageRepository = messageRepository;
            _jwtService = jwtService;
        }
        public async Task SendMessage(AddMessageDto messageDto)
        {
            var message = await _messageRepository.AddMessage(messageDto);
            await Clients.User(message.ReceiverId).SendAsync("ReceiveMessage",message);
            
        }
        
        
    }
}
