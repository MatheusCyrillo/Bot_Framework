using ChatBot.Controllers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatBot.Dialogs
{
    [LuisModel("91eaac2c-7694-4cf2-be62-d45a29f5418d", "82e8297cfa944fa7b40e1cde76ca8d1a")]
    [Serializable]
    public class SimpleLUISDialog : LuisDialog<object>
    {


        [LuisIntent("AlterarDados")]
        //sending the payment by email
        public async Task Alterar(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            // logic to retrieve the current payment info..
            var email = "example@email.com";

            

          //  PromptDialog.Confirm(context, AfterEmailConfirmation, $"Enviaremos o boleto para este e-mail: {email}, você gostaria de trocá-lo?");
        }

        //private async Task AfterEmailConfirmation(IDialogContext context, IAwaitable<bool> result)
        //{
        //    try
        //    {
        //        var response = await result;

        //        // if the way to store the payment email is the same as the one used to store the email when going through the ChangeInfo intent, then you can use the same After... method; otherwise create a new one
        //        PromptDialog.Text(context, AfterEmailProvided, "What's your current email?");
        //    }
        //    catch
        //    {
        //        // here handle your errors in case the user doesn't not provide an email
        //    }

        //    context.Wait(this.MessageReceived);
        //}

        //private async Task AfterEmailProvided(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        var email = await result;

        //        // logic to store your email...
        //    }
        //    catch
        //    {
        //        // here handle your errors in case the user doesn't not provide an email
        //    }

        //    context.Wait(this.MessageReceived);
        //}

        [LuisIntent("Pagar")]
        //sending the payment by email
        public async Task Pagar(IDialogContext context, LuisResult result)
        {
            // logic to retrieve the current payment info..
            var valor = "200,00";
            var linhaDigitavel = "592030592-3239";
            var data = "05/01/2016";
            var nome = "Michele";  
              
            await context.PostAsync($"Prezado {nome}, sua fatura está no valor de R${valor}, com a data de vencimento em {data}. Linha digitável: {linhaDigitavel} ");
            context.Wait(MessageReceived);
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