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
    public class None
    {
        public static string Response(JsonObject response, BotData privateConversationData)
        {

            string botResposta = string.Empty;

            foreach (var item in response.entities)
            {
                switch (item.type)
                {
                    case "Boolean::true":

                        if (privateConversationData.GetProperty<string>("intencaoAnterior") == "Pagar")
                        {
                            botResposta = "Qual é o seu e-mail?";

                        }
                        else
                        {
                            botResposta = "Desculpe, não entendi";
                        }

                        break;

                    case "Boolean::false":
                        if (privateConversationData.GetProperty<string>("intencaoAnterior") == "Pagar")
                        {
                            botResposta = "OK, enviando boleto para o email fulano@hotmail.com";

                        }
                        else
                        {
                            botResposta = "Desculpe, não entendi";
                        }

                        break;

                    case "E-mail":
                        if (privateConversationData.GetProperty<string>("intencaoAnterior") == "Pagar")
                        {
                            botResposta = "Você tem certeza que o e-mail";

                        }
                        else
                        {
                            botResposta = "Desculpe, não entendi";
                        }
                        break;

                }
                return botResposta;
            }

            return botResposta;
        }
    }
}