using AmbrosioBot.Dialogs.WantCandy.Resources;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace AmbrosioBot.Dialogs.WantCandy
{
    public class WantCandyDialog : ComponentDialog
    {
        private static List<Choice> candyOptions = new List<Choice>
        {
            new Choice(CandyTypes.Chocolate),
            new Choice(CandyTypes.Cake),
            new Choice(CandyTypes.CandyCane),
            new Choice(CandyTypes.JellyBeans),
        };

        public WantCandyDialog(BotServices services)
            : base(nameof(WantCandyDialog))
        {
            var steps = new WaterfallStep[]
            {
                WhatKindAsync,
                GetCandyAsync,
            };

            AddDialog(new WaterfallDialog(DialogIds.WantCandy, steps));
            AddDialog(new ChoicePrompt(DialogIds.SelectCandy));

            InitialDialogId = DialogIds.WantCandy;
        }

        private static async Task<DialogTurnResult> GetCandyAsync(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            dynamic result = sc.Result;
            var response = new Activity();

            switch ((string)result.Value)
            {
                case CandyTypes.Chocolate:

                    response = sc.Context.Activity.CreateReply(WantCandyStrings.ChocolateResponse);
                    await sc.Context.SendActivityAsync(response, cancellationToken);

                    break;
                case CandyTypes.Cake:
                    response = sc.Context.Activity.CreateReply(WantCandyStrings.CakeRespponse);
                    await sc.Context.SendActivityAsync(response, cancellationToken);

                    break;
                case CandyTypes.CandyCane:

                    response= sc.Context.Activity.CreateReply(WantCandyStrings.CandyCaneResponse);
                    await sc.Context.SendActivityAsync(response, cancellationToken);

                    break;
                case CandyTypes.JellyBeans:

                    response = sc.Context.Activity.CreateReply(WantCandyStrings.JellyBeansResponse);
                    await sc.Context.SendActivityAsync(response, cancellationToken);

                    break;
            }

            return await sc.EndDialogAsync();
        }

        private static  async Task<DialogTurnResult> WhatKindAsync(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            var prompt = sc.Context.
                Activity.
                CreateReply(WantCandyStrings.IWantCandy);

            return await sc.PromptAsync(
                DialogIds.SelectCandy,
                new PromptOptions
                {
                    Prompt = prompt,
                    Choices = candyOptions
                    
                },
                cancellationToken);
        }

        private static class DialogIds
        {
            public const string SelectCandy = nameof(SelectCandy);
            public const string WantCandy = nameof(WantCandy);
        }
        private static class CandyTypes
        {
            public const string Chocolate = "Chocolate";
            public const string Cake = "Cake";
            public const string JellyBeans = "Jelly beans";
            public const string CandyCane = "Candy cane";

        }
    }
}