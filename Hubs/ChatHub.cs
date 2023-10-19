using Microsoft.AspNetCore.SignalR;

namespace ChatServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageToGroup(int roomId, string message, string userMail)
        {
            await Clients.Group($"{roomId}_text").SendAsync("ReceiveMessage", roomId, $"{userMail} send '{message}'");
        }


        public async Task JoinRoom(int roomId, string userMail)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{roomId}_text");
            var clientsInGroup = Clients.Group($"{roomId}_text");
            await clientsInGroup.SendAsync("ReceiveMessage", roomId, $"User with mail {userMail} was joined");
        }

        public async Task LeaveRoom(int roomId, string userMail)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{roomId}_text");
            await Clients.Group($"{roomId}_text").SendAsync("ReceiveMessage", roomId, $"User with mail {userMail} was removed");
        }


        // Used in rtc.razor/webrtcservice.cs
        public async Task Join(string channel)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{channel}_audio_video");
            var clientsOthersInGroup = Clients.OthersInGroup($"{channel}_audio_video");

            //TODO: зачем этот метод ниже, если я нигде не обрабатываю этот метод из хаба на клиенте?
            await clientsOthersInGroup.SendAsync("Join", Context.ConnectionId);
        }

        // пользователь удаляется из группы и уведомляет об этом других участников
        public async Task Leave(string channel)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{channel}_audio_video");
            await Clients.OthersInGroup(channel).SendAsync("Leave", Context.ConnectionId);
            //TODO: а если все пользователи вішли из группы, что делать с этой группой? Может стоит удалять? Или группу на 100% нужно удалить, когда игра заканчивается? Как удалить группу?
        }
        public async Task SignalWebRtc(string channel, string type, string payload)
        {
            await Clients.OthersInGroup($"{channel}_audio_video").SendAsync("SignalWebRtc", channel, type, payload);
        }
    }
}
