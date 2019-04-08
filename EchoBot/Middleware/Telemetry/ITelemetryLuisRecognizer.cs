namespace AmbrosioBot.Middleware.Telemetry
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITelemetryLuisRecognizer : IRecognizer
    {
        bool LogPersonalInformation { get; }

        Task<T> RecognizeAsync<T>(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new();

        new Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new();
    }
}