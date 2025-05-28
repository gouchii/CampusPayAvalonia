using ClientApp.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClientApp.Messages;

public class UserLoadedMessage : ValueChangedMessage<UserModel>
{
    public UserLoadedMessage(UserModel userModel) : base((userModel))
    {
    }
}