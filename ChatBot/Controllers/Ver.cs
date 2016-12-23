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
    public class Ver
    {
        public static string Response(JsonObject response)
        {

            string resposta = string.Empty;
            foreach (var item in response.entities)
                {
                    switch (item.type)
                    {
                        case "Fatura::Vencimento":
                        resposta = "Sua fatura vence na data 22/12!";
                        break;

                        case "Fatura::Valor":
                        resposta = "O valor da sua fatura é de R$250,00!";
                        break;
                    }
                }
            
            return resposta;
        }

    
    }
}
       