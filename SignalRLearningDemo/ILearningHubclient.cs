namespace SignalRLearningDemo
{
    public interface ILearningHubclient
    {
        Task ReceiveMessage(string message);
        Task ConnectionIDRecived(string ID);

        Task ReceiveGroupMessage(string message);
    }
}
