namespace AmbrosioBot.Middleware.Telemetry
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.AI.QnA;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TelemetryQnAMaker : QnAMaker, ITelemetryQnAMaker
    {
        public const string QnaMsgEvent = "QnaMessage";

        private QnAMakerEndpoint _endpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryQnAMaker"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint of the knowledge base to query.</param>
        /// <param name="options">The options for the QnA Maker knowledge base.</param>
        /// <param name="logPersonalInformation">TRUE to include personally indentifiable information.</param>
        /// <param name="httpClient">An alternate client with which to talk to QnAMaker.
        /// If null, a default client is used for this instance.</param>
        public TelemetryQnAMaker(QnAMakerEndpoint endpoint, QnAMakerOptions options = null, bool logPersonalInformation = false, HttpClient httpClient = null)
            : base(endpoint, options, httpClient)
        {
            LogPersonalInformation = logPersonalInformation;

            _endpoint = endpoint;
        }

        public bool LogPersonalInformation { get; }

        public new async Task<QueryResult[]> GetAnswersAsync(ITurnContext context, QnAMakerOptions options = null)
        {
            // Call QnA Maker
            var queryResults = await base.GetAnswersAsync(context, options);
            return queryResults;
        }
    }
}