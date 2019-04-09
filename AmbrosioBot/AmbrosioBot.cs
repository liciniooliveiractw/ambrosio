// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
namespace AmbrosioBot
{
    using global::AmbrosioBot.Dialogs.Main;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a bot that processes incoming activities.
    /// For each user interaction, an instance of this class is created and the OnTurnAsync method is called.
    /// This is a Transient lifetime service.  Transient lifetime botServices are created
    /// each time they're requested. For each Activity received, a new instance of this
    /// class is created. Objects that are expensive to construct, or have a lifetime
    /// beyond the single turn, should be carefully managed.
    /// For example, the <see cref="MemoryStorage"/> object and associated
    /// <see cref="IStatePropertyAccessor{T}"/> object are created with a singleton lifetime.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    public class AmbrosioBot : IBot
    {
        private readonly ILogger logger;
        private readonly BotServices services;
        private DialogSet dialogs;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="botServices">The bot service</param>
        /// <param name="conversationState">The managed conversation state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory"/> that is hooked to the Azure App Service provider.</param>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider"/>
        public AmbrosioBot(BotServices botServices, ConversationState conversationState, ILoggerFactory loggerFactory)
        {
            services = botServices ?? throw new System.ArgumentNullException(nameof(botServices));

            if (conversationState == null)
            {
                throw new System.ArgumentNullException(nameof(conversationState));
            }

            if (loggerFactory == null)
            {
                throw new System.ArgumentNullException(nameof(loggerFactory));
            }

            logger = loggerFactory.CreateLogger<AmbrosioBot>();
            logger.LogTrace("Turn start.");

            dialogs = new DialogSet(conversationState.CreateProperty<DialogState>(nameof(AmbrosioBot)));
            dialogs.Add(new MainDialog(services, conversationState, loggerFactory));
        }

        /// <summary>
        /// Every conversation turn for our Echo Bot will call this method.
        /// There are no dialogs used, since it's "single turn" processing, meaning a single
        /// request and response.
        /// </summary>
        /// <param name="turnContext">A <see cref="ITurnContext"/> containing all the data needed
        /// for processing this conversation turn. </param>
        /// <param name="cancellationToken">(Optional) A <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the work queued to execute.</returns>
        /// <seealso cref="BotStateSet"/>
        /// <seealso cref="ConversationState"/>
        /// <seealso cref="IMiddleware"/>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {

            //await BasicMessageActivity(turnContext);

            await AdvanceMessageActivity(turnContext);
        }

        /// <summary>
        /// Basics the massage activity.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <returns></returns>
        private static async Task BasicMessageActivity(ITurnContext turnContext)
        {
            // Handle Message activity type, which is the main activity type within a conversational interface
            // Message activities may contain text, speech, interactive cards, and binary or unknown attachments.
            // see https://aka.ms/about-bot-activity-message to learn more about the message and other activity types
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                // Echo back to the user whatever they typed.
                await turnContext.SendActivityAsync($"You sent '{turnContext.Activity.Text}'");
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }

        /// <summary>
        /// Advance Message Activity using qna maker and luis cognitive services.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <returns></returns>
        private async Task AdvanceMessageActivity(ITurnContext turnContext)
        {
            // Client notifying this bot took to long to respond (timed out)
            if (turnContext.Activity.Code == EndOfConversationCodes.BotTimedOut)
            {
                var responseMessage = $"Timeout in {turnContext.Activity.ChannelId} channel: Bot took too long to respond.";
                await turnContext.SendActivityAsync(responseMessage);
                return;
            }

            var dc = await dialogs.CreateContextAsync(turnContext);

            if (dc.ActiveDialog != null)
            {
                var result = await dc.ContinueDialogAsync();
            }
            else
            {
                await dc.BeginDialogAsync(nameof(MainDialog));
            }
        }
    }
}