using ClientApp.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClientApp.Messages;

public class TransactionReceivedMessage : ValueChangedMessage<TransactionModel>
{
    public TransactionReceivedMessage(TransactionModel value) : base(value) { }
}
