using System.Collections.Generic;
using AmbrosioBot.Utils;

namespace AmbrosioBot.Dialogs.Main
{
    using global::AmbrosioBot.Dialogs.Enei;
    using global::AmbrosioBot.Dialogs.TellTime;
    using Luis;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MainDialog : RouterDialog
    {
        private BotServices services;
        private ConversationState conversationState;
        private ILoggerFactory loggerFactory;
        private MainResponses responder = new MainResponses();

        public MainDialog(BotServices services, ConversationState conversationState, ILoggerFactory loggerFactory)
            : base(nameof(MainDialog))
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            this.conversationState = conversationState;
            this.loggerFactory = loggerFactory;

            AddDialog(new EneiDialog(this.services));
            AddDialog(new TellTimeDialog(this.services));

        }

        protected override async Task OnStartAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            var view = new MainResponses();
            await view.ReplyWith(dc.Context, MainResponses.ResponseIds.Intro);
        }

        protected override async Task RouteAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check dispatch result
            var dispatchResult = await services.DispatchRecognizer.RecognizeAsync<Dispatch>(innerDc, CancellationToken.None);
            var intent = dispatchResult.TopIntent().intent;

            switch (intent)
            {
                case Dispatch.Intent.l_general:
                    // If dispatch result is general luis model
                    services.LuisServices.TryGetValue("general", out var luisService);

                    if (luisService == null)
                    {
                        throw new Exception("The specified LUIS Model could not be found in your Bot Services configuration.");
                    }
                    else
                    {
                        var result = await luisService.RecognizeAsync<LuisGeneral>(innerDc, CancellationToken.None);

                        var generalIntent = result?.TopIntent().intent;

                        // switch on general intents
                        switch (generalIntent)
                        {
                            case LuisGeneral.Intent.Cancel:
                                {
                                    // send cancelled response
                                    await responder.ReplyWith(innerDc.Context, MainResponses.ResponseIds.Cancelled);

                                    // Cancel any active dialogs on the stack
                                    await innerDc.CancelAllDialogsAsync();
                                    break;
                                }
                            case LuisGeneral.Intent.Help:
                                {
                                    // send help response
                                    await responder.ReplyWith(innerDc.Context, MainResponses.ResponseIds.Help);
                                    break;
                                }

                            case LuisGeneral.Intent.Enei:
                                {
                                    // send Enei dialog
                                    await innerDc.BeginDialogAsync(nameof(EneiDialog));
                                    break;
                                }
                            case LuisGeneral.Intent.TellTime:
                                {
                                    await innerDc.BeginDialogAsync(nameof(TellTimeDialog));
                                    break;
                                }
                            case LuisGeneral.Intent.None:
                            default:
                                {
                                    // No intent was identified, send confused message
                                    await responder.ReplyWith(innerDc.Context, MainResponses.ResponseIds.Confused);
                                    break;
                                }
                        }
                    }
                    break;

                case Dispatch.Intent.q_chitchat:
                    services.QnAServices.TryGetValue("chitchat", out var qnaServiceChitChat);

                    if (qnaServiceChitChat == null)
                    {
                        throw new Exception("The specified QnA Maker Service could not be found in your Bot Services configuration.");
                    }
                    else
                    {
                        var answers = await qnaServiceChitChat.GetAnswersAsync(innerDc.Context);

                        if (answers != null && answers.Count() > 0)
                        {
                            await innerDc.Context.SendActivityAsync(answers[0].Answer);
                        }
                    }
                    break;

                default:
                    // If dispatch intent does not map to configured models, send "confused" response.
                    await responder.ReplyWith(innerDc.Context, MainResponses.ResponseIds.Confused);
                    break;
            }
        }

        protected override async Task OnEventAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if there was an action submitted from intro card
            if (dc.Context.Activity.Value != null)
            {
                dynamic value = dc.Context.Activity.Value;
                if (value.action == "startOnboarding")
                {
                    await dc.BeginDialogAsync(nameof(EneiDialog));
                    return;
                }
            }
        }
    }
}