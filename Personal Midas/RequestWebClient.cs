using System;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace Midas
{
    public class RequestWebClient
    {
        public string publicKey = "YOUR PUBLIC KEY";
        public string privateKey = "YOUR PRIVATE KEY";
        public string apiSign = "";

        public int out_order_id = 2;
        public int nonce = 10;

        public RequestWebClient() { }

        public string hash(string text)
        {
            byte[] data = Encoding.Default.GetBytes(text);
            var result = new SHA256Managed().ComputeHash(data);
            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public string setApiSign(string out_order_id, string nonce)
        {
            string api_sign = "";
            api_sign = "out_order_id=" + out_order_id + "&nonce=" + nonce + privateKey;
            api_sign = hash(api_sign);
            return api_sign;
        }

        public List<Deal> deal() //Выгрузка сделок за последние сутки
        {
            // Адрес, по которому хранятся сделки
            string urlDeals = "https://btc-trade.com.ua/api/deals/btc_uah";

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(urlDeals); //Выгружаем
                List<Deal> deals = JsonConvert.DeserializeObject<List<Deal>>(json); //Десериализируем  
                return deals;
            }
        }

        public string auth()
        {
            string url = "https://btc-trade.com.ua/api/auth";

            out_order_id++;
            nonce++;

            using (var webClient = new WebClient())
            {
                // Создаём коллекцию параметров
                var pars = new NameValueCollection();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars.Add("out_order_id", out_order_id.ToString());
                pars.Add("nonce", nonce.ToString());

                string apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                var response = new WebClient();
                response.Headers.Add("public-key: " + publicKey);
                response.Headers.Add("api-sign: " + apiSign);
                response.UploadValues(url, pars);  // Посылаем параметры на сервер
                byte[] responsebytes = response.UploadValues(url, pars);
                string res = Encoding.UTF8.GetString(responsebytes);
                return res;
            }
        }

        public string balance()
        {
            string url = "https://btc-trade.com.ua/api/balance";

            out_order_id++;
            nonce++;

            using (var webClient = new WebClient())
            {
                // Создаём коллекцию параметров
                var pars = new NameValueCollection();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars.Add("out_order_id", out_order_id.ToString());
                pars.Add("nonce", nonce.ToString());

                string apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                var response = new WebClient();
                response.Headers.Add("public-key: " + publicKey);
                response.Headers.Add("api-sign: " + apiSign);
                response.UploadValues(url, pars);  // Посылаем параметры на сервер
                byte[] responsebytes = response.UploadValues(url, pars);
                string res = Encoding.UTF8.GetString(responsebytes);
                return res;
            }
        }
    }
}
