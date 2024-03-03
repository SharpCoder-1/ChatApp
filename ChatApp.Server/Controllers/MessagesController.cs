using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatApp.Server.Data;
using ChatApp.Server.Models;
using ChatApp.Server.Repositories.Contracts;
using ChatApp.Server.DTOs.Messages;
using Microsoft.AspNetCore.SignalR;
using ChatApp.Server.Hubs;

namespace ChatApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepos;

        public MessagesController(DataContext context,IMessageRepository messageRepos)
        {
            _messageRepos = messageRepos;
        }


        // GET: api/Messages
        [HttpGet]
        public ActionResult<IEnumerable<Message>> GetMessages()
        {
            return Ok(_messageRepos.GetMessages());
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _messageRepos.GetMessage(id);

            if (message == null)
            {
                return NotFound("Message is not found");
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id,UpdateMessageDto messageDto)
        {
            var message = await _messageRepos.UpdateMessage(id,messageDto);

            return NoContent();
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(AddMessageDto messageDto)
        {
            var message = await _messageRepos.AddMessage(messageDto);
            if (message != null)
            {

                return CreatedAtAction("GetMessage", new { id = message.Id }, message);
            }
            return BadRequest("Invalid message");
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageRepos.DeleteMessage(id);
            if (message == null)
            {
                return NotFound("Message is not found");
            }

            

            return NoContent();
        }

        
    }
}
