using AirlineSendAgent.Dtos;
using System.Threading.Tasks;

namespace AirlineSendAgent.Client
{
    public interface IWebhookClient
    {
        Task SendWebhookNotificationAsync(FlightDetailChangePayloadDto flightDetailChangePayloadDto);
    }
}