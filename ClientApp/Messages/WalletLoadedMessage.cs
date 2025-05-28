using ClientApp.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClientApp.Messages;

public class WalletLoadedMessage : ValueChangedMessage<WalletModel>
{
    public WalletLoadedMessage(WalletModel walletModel) : base((walletModel))
    {
    }
}