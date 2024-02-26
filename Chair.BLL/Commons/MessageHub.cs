using Chair.BLL.Dto.Message;
using Microsoft.AspNetCore.SignalR;

namespace Chair.BLL.Commons;

public class MessageHub : Hub
{
    
    /*public async Task SendMessage(MessageDto message)
    {
        //await Clients.User(message.RecipientId).SendAsync("ReceiveMessage", message);
    }*/
}