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
using ChatBot.Ultils;

namespace ChatBot.Dialogs
{
    [LuisModel("91eaac2c-7694-4cf2-be62-d45a29f5418d", "82e8297cfa944fa7b40e1cde76ca8d1a")]
    [Serializable]
    public class SimpleLUISDialog : LuisDialog<object>
    {

        string email;

        //Alterando dados
        [LuisIntent("AlterarDados")]
        public async Task Alterar(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation entidade;
            if (result.TryFindEntity("E-mail", out entidade))
            {

                email = "example@email.com"; // linha para teste, correto é fazer uma busca no banco;
                PromptDialog.Confirm(context, TrocarEmail, $"Enviaremos o boleto para este e-mail: {email}, você gostaria de trocá-lo?");
            }
            else
            {
                await context.PostAsync($"Não entendi o que você gostaria de alterar!");
            }
        }

        IAwaitable<bool> result2;
        private async Task TrocarEmail(IDialogContext context, IAwaitable<bool> result)
        {
            
            if (await result || await result2)
            {
                result2 = result;
                var response = await result;

                PromptDialog.Text(context, AtualizandoEmail, "Qual é o seu e-mail?");
                

            }
            else
            {
                await context.PostAsync($"Ok, o boleto foi enviado para o e-mail: {email}.");
            }
            
        }

        private async Task AtualizandoEmail(IDialogContext context, IAwaitable<string> result)
        {
            string confirmaEmail = result.ToString();
           
            if (Ultil.IsValidEmail(confirmaEmail))
            {
                PromptDialog.Confirm(context, ConfirmarEmail, $"Ok, você confirma que esse e-mail {confirmaEmail} é o correto?");
            }
            else
            {
                await context.PostAsync($"Esse é um e-mail inválido!");
                
                await TrocarEmail(context, result2);
            }
            
        }

        private async Task ConfirmarEmail(IDialogContext context, IAwaitable<bool> result)
        {
            string confirmaEmail = result.ToString();

            if (Ultil.IsValidEmail(confirmaEmail))
            {
                PromptDialog.Confirm(context, ConfirmarEmail, $"Ok, você confirma que esse e-mail {confirmaEmail} é o correto?");
            }
            else
            {

            }

        }

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
            await context.PostAsync("Desculpe, eu não te entendi.");
            context.Wait(MessageReceived);
        }


    }
}