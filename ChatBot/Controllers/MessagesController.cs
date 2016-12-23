using ChatBot.Serialization;
using ChatBot.Services;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using System.Linq;

namespace ChatBot.Controllers
{
    public class MessagesController : ApiController
    {

        string intencaoHistoria = string.Empty;
        public HttpResponseMessage Post([FromBody]Activity activity)
        {
            StateClient stateClient = activity.GetStateClient();

            // BotData userData = stateClient.BotState.GetUserData(activity.ChannelId, activity.From.Id);
            BotData privateConversationData = stateClient.BotState.GetPrivateConversationData(activity.ChannelId, activity.Conversation.Id, activity.From.Id);
            var actid = activity.Id;
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            Activity reply;

            var responseLuis = Luis.GetResponse(activity.Text);
            string strBot = activity.Text;

            if (activity.Type == ActivityTypes.Message)
            {

                var intent = new Intent();
                var entity = new Serialization.Entity();
                string botResposta = string.Empty;
                string intencao = string.Empty;
                if (responseLuis.intents.Count() > 0)
                {
                    intencao = responseLuis.topScoringIntent.intent;
                    double topScore = responseLuis.topScoringIntent.score;

                    if (intencao != null && topScore >= 0.8)
                    {
                        privateConversationData = stateClient.BotState.GetPrivateConversationData(activity.ChannelId, activity.Conversation.Id, activity.From.Id);

                        switch (intencao)
                        {
                            case "Pagar":
                                reply = activity.CreateReply($"O boleto da fatura será enviado para o seu e-mail. O e-mail fulano@hotmail.com é o seu e-mail atual?");
                                connector.Conversations.ReplyToActivity(reply);
                                privateConversationData.SetProperty<string>("intencaoAnterior", "Pagar");
                                stateClient.BotState.SetPrivateConversationData(activity.ChannelId, activity.Conversation.Id, activity.From.Id, privateConversationData);
                               
                                break;

                            case "Ver":
                                
                                botResposta = Ver.Response(responseLuis);
                                reply = activity.CreateReply(botResposta);
                                connector.Conversations.ReplyToActivity(reply);
                                break;

                            case "FinalizarConversa":
                                reply = activity.CreateReply($"Posso te ajudar com mais alguma coisa?");
                                connector.Conversations.ReplyToActivity(reply);
                                break;

                            case "Cancelar":
                                reply = activity.CreateReply($"Para cancelar, é necessário quitar os eventuais débitos em atraso. Por favor, ligue para 10611 para maiores informações.");
                                connector.Conversations.ReplyToActivity(reply);
                                break;

                            case "NaoDevo":
                                reply = activity.CreateReply($"Você tem certeza que não está devendo? No nosso sistema tem uma fatura da data 20/12 que ainda não foi paga!");
                                connector.Conversations.ReplyToActivity(reply);
                                break;

                            case "NaoConhecoDevedor":
                                reply = activity.CreateReply($"Desculpe pelo transtorno, atualizaremos nosso sistema!");
                                connector.Conversations.ReplyToActivity(reply);
                                break;

                            case "None":
                                botResposta = None.Response(responseLuis, privateConversationData);
                                reply = activity.CreateReply(botResposta);
                                connector.Conversations.ReplyToActivity(reply);
                                break;
                        }

                    }
                
           
                    else if(topScore < 0.8 && intencao == "None")
                    {
                        reply = activity.CreateReply($"Não entendi, me diga com outras palavras!");
                        connector.Conversations.ReplyToActivity(reply);
                    }
                }
            }
            else
            {
                reply = HandleSystemMessage(activity, privateConversationData, stateClient);
                connector.Conversations.ReplyToActivity(reply);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;

        }

        private Activity HandleSystemMessage(Activity activity, BotData userData, StateClient stateClient)
        {

            if (activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                Activity reply = activity.CreateReply("Olá {{nome}}. Pague seu débito com 20% de desconto. Acesse: https://sky.negocie.online/2YWLJFD e retire seu boleto.");
                //  userData = stateClient.BotState.getUserData(activity.ChannelId, activity.From.Id);
                // userData2 = stateClient.BotState.DeleteStateForUser(activity.ChannelId, activity.From.Id);
                //  userData.SetProperty<bool>("Já viu o vencimento", false);
                //  stateClient.BotState.SetUserData(activity.ChannelId, activity.From.Id, userData);
                return reply;
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (activity.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (activity.Type == ActivityTypes.Ping)
            {
                Activity reply = activity.CreateReply();
                reply.Type = "Ping";
                return reply;
            }

            return null;
        }

    }
}


