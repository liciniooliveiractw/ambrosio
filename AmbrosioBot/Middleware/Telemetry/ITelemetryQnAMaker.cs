namespace AmbrosioBot.Middleware.Telemetry
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.QnA;
    using System.Threading.Tasks;

    public interface ITelemetryQnAMaker
    {
        bool LogPersonalInformation { get; }

        Task<QueryResult[]> GetAnswersAsync(ITurnContext context, QnAMakerOptions options = null);
    }
}