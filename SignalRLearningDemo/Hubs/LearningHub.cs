using Microsoft.AspNetCore.SignalR;

namespace SignalRLearningDemo.Hubs
{
    public class LearningHub : Hub<ILearningHubclient>
    {
        public async Task BrodcastMessage(string message)
        {
            await Clients.All.ReceiveMessage(GetMessageToSend(message));
        }

        public async Task SendToOther(string message)
        {
            await Clients.Others.ReceiveMessage(GetMessageToSend(message));
        }

        public async Task SendToCaller(string message)
        {
            await Clients.Caller.ReceiveMessage(GetMessageToSend(message));
        }

        public async Task SendToIndividual(string connectionID, string message)
        {
            await Clients.Client(connectionID).ReceiveMessage(GetMessageToSend(message));
        }

        public async Task GetConnectionID()
        {
            await Clients.Caller.ConnectionIDRecived(Context.ConnectionId);
        }

        public async Task SendToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessage(GetMessageToSend(message));
        }

        public async Task AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"Current user added to {groupName} group.");
            await Clients.Others.ReceiveMessage($"User {Context.ConnectionId} added to {groupName} group.");
        }

        public async Task RemoveUserFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Caller.ReceiveMessage($"Current user removed from {groupName} group.");
            await Clients.Others.ReceiveMessage($"User {Context.ConnectionId} remove from {groupName} group.");
        }

        private string GetMessageToSend(string originalMessage)
        {
            return $"User Connection ID: {Context.ConnectionId}. Message: {originalMessage}";
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "HubUsers");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
