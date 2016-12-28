using ChatBot.Controllers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Dialogs
{
    [LuisModel("e0357190-4455-4506-8764-055b7e04a674", "a91e3e2044be4be99c291c54a153f3a6")]
    [Serializable]
    public class SimpleLUISDialog : LuisDialog<object>
    {


        [LuisIntent("Pagar")]
        //sending the payment by email
        public async Task Payment(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            // logic to retrieve the current payment info..
            var email = "test@email.com";

            PromptDialog.Confirm(context, AfterEmailConfirmation, $"Is it {email} your current email?");
        }

        private async Task AfterEmailConfirmation(IDialogContext context, IAwaitable<bool> result)
        {
            try
            {
                var response = await result;

                // if the way to store the payment email is the same as the one used to store the email when going through the ChangeInfo intent, then you can use the same After... method; otherwise create a new one
                PromptDialog.Text(context, AfterEmailProvided, "What's your current email?");
            }
            catch
            {
                // here handle your errors in case the user doesn't not provide an email
            }

            context.Wait(this.MessageReceived);
        }

        private async Task AfterEmailProvided(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                var email = await result;

                // logic to store your email...
            }
            catch
            {
                // here handle your errors in case the user doesn't not provide an email
            }

            context.Wait(this.MessageReceived);
        }


        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("sorry, I dont understad");
            context.Wait(MessageReceived);
        }


    }
}