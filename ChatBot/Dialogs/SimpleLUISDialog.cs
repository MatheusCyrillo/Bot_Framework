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
        public async Task AlterarDados(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            EntityRecommendation entidade;
            if (result.TryFindEntity("E-mail", out entidade))
            {
                email = "EmailDoBanco@email.com"; // linha para teste, correto é fazer uma busca no banco;
                PromptDialog.Confirm(context, TrocarEmail, $"Seu email atual é: {email}, você gostaria de trocá-lo?");
            }
            else
            {
                string [] informacoes = { "Email", "telefone", "endereco" };
                PromptOptions<string> options = new PromptOptions<string>("Qual das seguintes informacoes você quer alterar?",
                    "desculpe, selecione uma das opcoes validas", "desculpe, nao podemos atualizar essa informacao por aqui, por favbor ligue para 08005263", informacoes, 2);
                //PromptDialog.Choice<string>(context, AlterarInfo, options);
            }
        }



        private async Task TrocarEmail(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                PromptDialog.Text(context, AtualizandoEmail, "Qual é o seu e-mail?");
            }
            else
            {
                await context.PostAsync($"Ok, o boleto foi enviado para o e-mail: {email}.");
            }  
        }


        private async Task AtualizandoEmail(IDialogContext context, IAwaitable<string> result)
        {
            IMessageActivity Activity = context.Activity.AsMessageActivity();
            email = Activity.Text;

            if (Ultil.IsValidEmail(email))
            {
                PromptDialog.Confirm(context, ConfirmarEmail, $"Ok, você confirma que esse e-mail {email} está correto?");
            }
            else
            {
                PromptDialog.Text(context, AtualizandoEmail, $"Esse é um e-mail inválido, por favor entre com novo email valido.");
            }
            
        }

        private async Task ConfirmarEmail(IDialogContext context, IAwaitable<bool> result)
        {
            if (await result)
            {
                await context.PostAsync($"Ok, enviaremos seu boleto para o e-mail: {email}.");
            }
            else
            {
                PromptDialog.Text(context, AtualizandoEmail, "Qual é o seu e-mail?");
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
        [LuisIntent("Cancelar")]
        public async Task Cancelar(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Para cancelar, é necessário quitar os eventuais débitos em atraso. Por favor, ligue para 10611 para maiores informações.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("NaoDevo")]
        public async Task naoDevo(IDialogContext context, LuisResult result)
        {
            int mes = 12 ;
            double valor = 50.00;

            PromptDialog.Text(context, ConfimaPagamento, $"Consta em aberto a parcela do mês" + mes+" no valor de R$ "+valor+". Qual mês e ano você efetuou o pagamento?");
            context.Wait(MessageReceived);
        }

        private Task ConfimaPagamento(IDialogContext context, IAwaitable<string> result)
        {
            throw new NotImplementedException();
        }

        [LuisIntent("NaoConhecoDevedor")]
        public async Task NaoConhecoDevedor(IDialogContext context, LuisResult result)
        {
            
            await context.PostAsync("Desculpe pelo incomodo, esse telefone será retirado de nossos bancos de dados.");
            //retira o telefone do banco
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