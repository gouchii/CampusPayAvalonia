namespace ClientApp.Contexts;

public enum TransactionMode
{
    None,
    ReceiveRfid,
    ReceiveQr,
    SendRfid,
    SendQr,
    SendUsername
}