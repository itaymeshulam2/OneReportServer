using OneReportServer.Contract.Response;

namespace OneReportServer.Client.Interface
{
    public interface IEmailClient
    {
        void SendMessage(string message, string ToAddress);
    }
}
