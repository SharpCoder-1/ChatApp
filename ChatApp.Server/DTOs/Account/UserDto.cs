using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace ChatApp.Server.DTOs.Account
{
    public record UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonPropertyName("JWT")]
        public string JWT { get; set; }
    }
}
