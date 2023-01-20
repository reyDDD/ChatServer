using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace ChatServer.Hubs
{
	public class ChatHub : Hub
	{
		public async Task SendMessageToGroup(int roomId, string message, string userMail)
		{
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", roomId, $"{userMail} send '{message}'");
		}


		public async Task JoinRoom(int roomId, string userMail)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", roomId, $"User with mail {userMail} was joined");
		}

		public async Task LeaveRoom(int roomId, string userMail)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", roomId, $"User with mail {userMail} was removed");
		}

		// Used in rtc.razor/webrtcservice.cs
		public async Task SignalWebRtc(string channel, string type, string payload)
		{
			await Clients.OthersInGroup(channel).SendAsync("SignalWebRtc", channel, type, payload);
		}
	}
}
