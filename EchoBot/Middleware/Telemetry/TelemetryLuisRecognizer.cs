namespace AmbrosioBot.Middleware.Telemetry
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.Luis;
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class TelemetryLuisRecognizer : LuisRecognizer, ITelemetryLuisRecognizer
    {
        private LuisApplication _luisApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryLuisRecognizer"/> class.
        /// </summary>
        /// <param name="application">The LUIS application to use to recognize text.</param>
        /// <param name="predictionOptions">The LUIS prediction options to use.</param>
        /// <param name="includeApiResults">TRUE to include raw LUIS API response.</param>
        /// <param name="logPersonalInformation">TRUE to include personally indentifiable information.</param>
        public TelemetryLuisRecognizer(LuisApplication application, LuisPredictionOptions predictionOptions = null, bool includeApiResults = false, bool logPersonalInformation = false)
            : base(application, predictionOptions, includeApiResults)
        {
            _luisApplication = application;

            LogPersonalInformation = logPersonalInformation;
        }

        /// <summary>
        /// Gets a value indicating whether determines whether to log the Activity message text that came from the user.
        /// </summary>
        /// <value>If true, will log the Activity Message text into the AppInsight Custom Event for Luis intents.</value>
        public bool LogPersonalInformation { get; }

        public async Task<T> RecognizeAsync<T>(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            var result = new T();
            result.Convert(await RecognizeAsync(dialogContext, cancellationToken).ConfigureAwait(false));
            return result;
        }

        public new async Task<T> RecognizeAsync<T>(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
            where T : IRecognizerConvert, new()
        {
            var result = new T();
            result.Convert(await RecognizeAsync(turnContext, cancellationToken).ConfigureAwait(false));
            return result;
        }

        /// <summary>
        /// Return results of the analysis (Suggested actions and intents), passing the dialog id from dialog context to the TelemetryClient.
        /// </summary>
        /// <param name="dialogContext">Dialog context object containing information for the dialog being executed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The LUIS results of the analysis of the current message text in the current turn's context activity.</returns>
        public async Task<RecognizerResult> RecognizeAsync(DialogContext dialogContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (dialogContext == null)
            {
                throw new ArgumentNullException(nameof(dialogContext));
            }

            return await RecognizeInternalAsync(dialogContext.Context, dialogContext.ActiveDialog?.Id, cancellationToken);
        }

        /// <summary>
        /// Return results of the analysis (Suggested actions and intents), using the turn context. This is missing a dialog id used for telemetry..
        /// </summary>
        /// <param name="context">Context object containing information for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The LUIS results of the analysis of the current message text in the current turn's context activity.</returns>
        public async new Task<RecognizerResult> RecognizeAsync(ITurnContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await RecognizeInternalAsync(context, null, cancellationToken);
        }

        /// <summary>
        /// Analyze the current message text and return results of the analysis (Suggested actions and intents).
        /// </summary>
        /// <param name="context">Context object containing information for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The LUIS results of the analysis of the current message text in the current turn's context activity.</returns>
        private async Task<RecognizerResult> RecognizeInternalAsync(ITurnContext context, string dialogId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Call Luis Recognizer
            var recognizerResult = await base.RecognizeAsync(context, cancellationToken);
            return recognizerResult;
        }
    }
}