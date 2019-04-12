using AmbrosioBot.Dialogs.Enei;
using AmbrosioBot.Dialogs.TellTime;
using AmbrosioBot.Dialogs.WantCandy;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace AmbrosioBot
{
    using System;
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
        /// <summary>
        /// The dialogs.
        /// </summary>
        private DialogSet dialogs;

        private readonly StateAccessors accessors;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="botServices">The bot service</param>
        /// <param name="accessors">The accessors.</param>
        /// <param name="conversationState">The managed conversation state.</param>
        /// <param name="loggerFactory">A <see cref="ILoggerFactory" /> that is hooked to the Azure App Service provider.</param>
        /// <exception cref="ArgumentNullException">
        /// botServices
        /// or
        /// conversationState
        /// or
        /// loggerFactory
        /// </exception>
        /// <exception cref="System.ArgumentNullException">accessors</exception>
        /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-2.1#windows-eventlog-provider" />
        public AmbrosioBot(
            BotServices botServices,
            ConversationState conversationState,
            StateAccessors accessors,
            ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (conversationState == null)
            {
                throw new ArgumentNullException(nameof(conversationState));
            }

            this.accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
            var services = botServices ?? throw new ArgumentNullException(nameof(botServices));

            var property = conversationState.
                CreateProperty<DialogState>(nameof(DialogState));

            dialogs = new DialogSet(property);
            dialogs.Add(new MainDialog(services, loggerFactory));

            var logger = loggerFactory.CreateLogger<AmbrosioBot>();
            logger.LogTrace("Turn start.");
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
            if (turnContext.Activity.Code == EndOfConversationCodes.BotTimedOut)
            {
                var responseMessage = $"Timeout in {turnContext.Activity.ChannelId} channel: Bot took too long to respond.";
                await turnContext.SendActivityAsync(responseMessage);
                return;
            }

            var dc = await dialogs.CreateContextAsync(turnContext);
            if (dc.ActiveDialog != null)
            {
                await dc.ContinueDialogAsync();
            }
            else
            {
                await dc.BeginDialogAsync(nameof(MainDialog));
            }

            await accessors.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await accessors.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
    }
}