namespace AmbrosioBot.Dialogs.Enei
{
    using Microsoft.Bot.Builder.Dialogs;
    using System.Threading;
    using System.Threading.Tasks;

    public class EneiDialog : ComponentDialog
    {
        private EneiResponses _responder = new EneiResponses();

        public EneiDialog(BotServices services)
        : base(nameof(EneiDialog))
        {
            var dialogId = nameof(EneiDialog);

            var stepsDialogs = new WaterfallStep[]
            {
                WelcomeToEnei,
            };

            AddDialog(new WaterfallDialog(dialogId, stepsDialogs));
        }

        private async Task<DialogTurnResult> WelcomeToEnei(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            await _responder.ReplyWith(sc.Context, EneiResponses.ResponseIds.WelcomeEneiMessage);
            return await sc.EndDialogAsync();
        }
    }
}