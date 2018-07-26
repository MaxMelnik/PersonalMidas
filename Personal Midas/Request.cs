using System;
using System.Diagnostics;
using System.Net;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using xNet;
using System.IO;

namespace Midas
{
    class Request
    {
        public string publicKey = "YOUR PUBLIC KEY";
        public string privateKey = "YOUR PRIVATE KEY";
        public string apiSign = "";

        public static Random rnd = new Random();

        public int out_order_id = 2;
        public int nonce = rnd.Next(1, 10000);

        public Request() { }

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

        public string setApiSign(string out_order_id, string amount, string nonce)
        {
            string api_sign = "";
            api_sign = "out_order_id=" + out_order_id + "&amount=" + amount + "&nonce=" + nonce + privateKey;
            api_sign = hash(api_sign);
            return api_sign;
        }

        public string setApiSign(string count, string nonce, string price, string out_order_id, string currency1, string currency)
        {
            string api_sign = "";
            api_sign = "count=" + count + "&nonce=" + nonce + "&price=" + price + "&out_order_id=" + out_order_id + "&currency1=" + currency1 + "&currency=" + currency + privateKey;
            api_sign = hash(api_sign);
            return api_sign;
        }

        public List<Deal> deal() //Выгрузка сделок за последние сутки. Работает корректно
        {
            string urlDeals = "https://btc-trade.com.ua/api/deals/btc_uah";  // Адрес, по которому хранятся сделки

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(urlDeals); //Выгружаем
                List<Deal> deals = JsonConvert.DeserializeObject<List<Deal>>(json); //Десериализируем  
                return deals;
            }
        }

        public string authorization(HttpRequest request) //Авторизация. Работает корректно
        {
            string url = "https://btc-trade.com.ua/api/auth";
            nonce++;

            // Создаём коллекцию параметров
            var pars = new RequestParams();

            // Добавляем необходимые параметры в виде пар ключ, значение
            pars["out_order_id"] = out_order_id.ToString();
            pars["nonce"] = nonce.ToString();

            string apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

            request.AddHeader("public-key", publicKey);
            request.AddHeader("api-sign", apiSign);

            string res = request.Post(url, pars).ToString();
            return res;

        }

        public string ask(string amount)//Получение стоимости. Работает корректно
        {

            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/ask/btc_uah";
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["amount"] = amount;
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), amount, nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);

                string res = request.Post(url, pars).ToString(); //Получение баланса
                nonce = rnd.Next(1, 10000);
                return res;
            }
        }

        public string balance()//Получение баланса. Работает корректно
        {
            {
                using (var request = new HttpRequest())
                {
                    authorization(request);

                    string url = "https://btc-trade.com.ua/api/balance";
                    nonce++;

                    // Создаём коллекцию параметров
                    var pars = new RequestParams();

                    // Добавляем необходимые параметры в виде пар ключ, значение
                    pars["out_order_id"] = out_order_id.ToString();
                    pars["nonce"] = nonce.ToString();

                    apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                    request.AddHeader("public-key", publicKey);
                    request.AddHeader("api-sign", apiSign);

                    string res = request.Post(url, pars).ToString(); //Получение баланса
                    nonce = rnd.Next(1, 10000);
                    return res;
                }
            }
        }

        public float balanceUAH()//Получение баланса. Работает корректно
        {

            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/balance";
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);

                float res;
                var json = request.Post(url, pars).ToString(); //Получение баланса
                balance balance = JsonConvert.DeserializeObject<balance>(json); //Десериализируем  
                res = balance.accounts[0].balance;
                nonce = rnd.Next(1, 10000);
                return res;
            }
        }

        public float balanceBTC()//Получение баланса. Работает корректно
        {

            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/balance";
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);

                float res;
                var json = request.Post(url, pars).ToString(); //Получение баланса
                balance balance = JsonConvert.DeserializeObject<balance>(json); //Десериализируем  
                res = balance.accounts[1].balance;
                nonce = rnd.Next(1, 10000);
                return res;
            }
        }

        public float priceBTC() //Получение текущего курса BTC
        {
            string urlDeals = "https://api.coinmarketcap.com/v1/ticker/";  // Адрес, по которому хранится курс

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(urlDeals); //Выгружаем
                List<priceBTC> price = JsonConvert.DeserializeObject<List<priceBTC>>(json); //Десериализируем  
                return price[0].price_usd;
            }
        }

        public string sell(string count, string price, string currency1, string currency)//Создание заявки на продажу. Работает через Пайтон
        {
            StreamWriter countFile = new StreamWriter("pyScripts/tmp/count.dat", false);
            StreamWriter priceFile = new StreamWriter("pyScripts/tmp/price.dat", false);
            countFile.WriteLine(count);
            priceFile.WriteLine(price);
            countFile.Close();
            priceFile.Close();
            int oi = 0;
            Process sellP = Process.Start(@"pyScripts\sell.pyw");
            //  while (sellP.Responding) {oi++;}
            StreamReader orderIdFile = new StreamReader("pyScripts/tmp/sellOrderId.dat");
            string orderId = orderIdFile.ReadLine();
            orderIdFile.Close();
            //orderId = oi.ToString();
            return orderId;

            // using (var request = new HttpRequest())
            // {
            //     authorization(request);
            //     string url = "https://btc-trade.com.ua/api/sell/btc_uah";
            //     nonce++;
            //
            //     // Создаём коллекцию параметров
            //     var pars = new RequestParams();
            //
            //     // Добавляем необходимые параметры в виде пар ключ, значение
            //     pars["count"] = count;
            //     pars["nonce"] = nonce.ToString();
            //     pars["price"] = price;
            //     pars["out_order_id"] = out_order_id.ToString();
            //     pars["currency1"] = currency1;
            //     pars["currency"] = currency;
            //
            //     apiSign = setApiSign(count, nonce.ToString(), price, out_order_id.ToString(), currency1, currency);
            //
            //     request.AddHeader("public-key", publicKey);
            //     request.AddHeader("api-sign", apiSign);
            //
            //     string res = request.Post(url, pars).ToString(); //Cоздание заявки на продажу
            //     //  string res = count.ToString() + "|" + price.ToString() + "|" + out_order_id.ToString() + "|" + currency1 + "|" + currency + "|" + nonce.ToString();
            //     nonce = rnd.Next(1, 100);
            //     return res;
            // }
        }

        public string buy(string count, string price, string currency1, string currency)//Создание заявки на покупку. Работает через пайтон
        {

            using (var request = new HttpRequest())
            {
                StreamWriter countFile = new StreamWriter("pyScripts/tmp/count.dat", false);
                StreamWriter priceFile = new StreamWriter("pyScripts/tmp/price.dat", false);
                countFile.WriteLine(count);
                priceFile.WriteLine(price);
                countFile.Close();
                priceFile.Close();
                int oi = 0;
                Process sellP = Process.Start(@"pyScripts\buy.pyw");
                //  while (sellP.Responding) {oi++;}
                StreamReader orderIdFile = new StreamReader("pyScripts/tmp/buyOrderId.dat");
                string orderId = orderIdFile.ReadLine();
                orderIdFile.Close();
                //orderId = oi.ToString();
                return orderId;
            }
        }

        public string remove(string id) //Удаления заявки. Работает корректно
        {

            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/remove/order/" + id;
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);

                string res = request.Post(url, pars).ToString(); //Получение баланса
                nonce = rnd.Next(1, 10000);
                return res;
            }
        }
        public float sellMinPrice() //Самая дешовая заявка на продажу, моя должна быть дешевле этого значения. Работает корректно
        {
            string urlDeals = "https://btc-trade.com.ua/api/trades/sell/btc_uah";  // Адрес, по которому хранятся сделки

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(urlDeals); //Выгружаем
                sellOrders orders = JsonConvert.DeserializeObject<sellOrders>(json); //Десериализируем  
                return orders.min_price;
            }
        }
        public float buyMaxPrice() //Самая дорогая заявка на покупкку, моя должна быть дороже этого значения. Работает корректно
        {
            string urlDeals = "https://btc-trade.com.ua/api/trades/buy/btc_uah";  // Адрес, по которому хранятся сделки

            using (var webClient = new WebClient())
            {
                var json = webClient.DownloadString(urlDeals); //Выгружаем
                sellOrders orders = JsonConvert.DeserializeObject<sellOrders>(json); //Десериализируем  
                return orders.max_price;
            }
        }

        public myOpenOrders myOrders()
        {
            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/my_orders/btc_uah";
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);
                
                var json = request.Post(url, pars).ToString(); //Получение баланса
                myOpenOrders orders = JsonConvert.DeserializeObject<myOpenOrders>(json); //Десериализируем  
                nonce = rnd.Next(1, 10000);
                return orders;
            }
        }

        public OrderInfo orderCheck(string orderId)
        {
            using (var request = new HttpRequest())
            {
                authorization(request);

                string url = "https://btc-trade.com.ua/api/order/status/"+orderId;
                nonce++;

                // Создаём коллекцию параметров
                var pars = new RequestParams();

                // Добавляем необходимые параметры в виде пар ключ, значение
                pars["out_order_id"] = out_order_id.ToString();
                pars["nonce"] = nonce.ToString();

                apiSign = setApiSign(out_order_id.ToString(), nonce.ToString());

                request.AddHeader("public-key", publicKey);
                request.AddHeader("api-sign", apiSign);

                var json = request.Post(url, pars).ToString(); //Получение баланса
                OrderInfo order = JsonConvert.DeserializeObject<OrderInfo>(json); //Десериализируем  
                nonce = rnd.Next(1, 10000);
                return order;
            }
        }
            
    }
}
