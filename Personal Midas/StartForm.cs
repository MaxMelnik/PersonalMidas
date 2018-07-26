using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Midas
{
    public partial class StartForm : Form
    {
        Request req = new Request();
        float balancePriceUAH; //Не покупаю дороже этого значения. Выставляю заявку на покупку с максимальной ценой и не поднимаю выше этого значения
        float balancePriceBTC; //Не продаю дешевле этого значения. Выставляю заявку на продажу с минимальной ценой и не опускаю ниже этого значения
        float expectableBalancePriceUAH;
        float expectableBalancePriceBTC;
        int indexBPU;
        int indexBPB;
        float mySellPrice = 0;
        float myBuyPrice = 0;


        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            StreamReader balancePriceFile = new StreamReader("pyScripts/data/balanceprice.dat");
            float.TryParse(balancePriceFile.ReadLine(), out balancePriceUAH);
            float.TryParse(balancePriceFile.ReadLine(), out balancePriceBTC);
            balancePriceFile.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Test form = new Test();
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)//Запуск бота
        {
            button2.Enabled = false;                                                //Нельзя запустить когда запущено
            tick = true;                                                            //Идет работа
            button4.Enabled = true;                                                 //Можно остановить
            timer1.Enabled = true;                                                  //Запускаеться первый таймер
            Second.Enabled = true;                                                  //Запускаеться таймер времени
            richTextBox1.AppendText(second + " >> Started!" + Environment.NewLine); //Информация о запуске
        }

        int second = 0;
        //int count = 0;

        private void timer1_Tick(object sender, EventArgs e)//Один проход алгоритма
        {
            try
            {
                //      //++++++++++++++++++++++++++++++1
                //      bool buyDeal = false;
                //      bool sellDeal = false;
                //      // richTextBox1.AppendText(count + ">>" + req.sell("0.001", "1000000", "UAH", "BTC"));
                //      string json = req.balance();                                             //Получение баланса
                //      balance balance = JsonConvert.DeserializeObject<balance>(json);          //Десериализируем 
                //      //++++++++++++++++++++++++++++++2
                //      List<Order> myOrders = req.myOrders().your_open_orders;                  //Получения всех моих заявок
                //      List<Order> buyOrders = myOrders.Where(x => x.type == "buy").ToList();   //Получение моих заявок на покупку
                //      List<Order> sellOrders = myOrders.Where(x => x.type == "sell").ToList(); //Получение моих заявок на продажу
                //      float balanceUAHDiff = 0;
                //      float balanceBTCDiff = 0;
                //      foreach (Order order in buyOrders)
                //      {
                //          balanceUAHDiff += order.sum2;                                        //Подсчет гривен на заявках
                //      }
                //      foreach (Order order in sellOrders)
                //      {
                //          balanceBTCDiff += order.sum1;                                        //Подсчет биткоинов на заявках
                //      }
                //
                //      float balanceUAH = balance.accounts[0].balance + balanceUAHDiff;         //Подсчет общего баланса гривен
                //      float balanceBTC = balance.accounts[1].balance + balanceBTCDiff;         //Подсчет общего баланса биткоинов
                //      richTextBox1.AppendText(second + " seconds >> UAH: " + balanceUAH.ToString() + " BTC: " + balanceBTC.ToString() + " " + Environment.NewLine);
                //      StreamReader balancePriceFile = new StreamReader("pyScripts/data/balanceprice.dat"); //Получение цен остатков
                //      float.TryParse(balancePriceFile.ReadLine(), out balancePriceUAH);
                //      float.TryParse(balancePriceFile.ReadLine(), out balancePriceBTC);
                //      int.TryParse(balancePriceFile.ReadLine(), out indexBPU);
                //      int.TryParse(balancePriceFile.ReadLine(), out indexBPB);
                //      balancePriceFile.Close();
                //      label1.Text = "balancePriceUAH: " + balancePriceUAH.ToString();
                //      label2.Text = "balancePriceBTC: " + balancePriceBTC.ToString();
                //      label3.Text = "UAH: " + balance.accounts[0].balance.ToString();
                //      label4.Text = "BTC: " + balance.accounts[1].balance.ToString();
                //
                //      float balanceRatio = balancePriceUAH / (balancePriceUAH + balancePriceBTC);
                //
                //      if (balanceRatio < 0.3) { sellDeal = true; }                               //
                //      else if (balanceRatio < 0.7) { sellDeal = true; buyDeal = true; }          //Проверка необходимых сделок
                //      else { buyDeal = true; }                                                   //
                //      //
                //      //++++++++++++++++++++++++++++++3
                //      if (!sellDeal)                                              //
                //      {                                                           //
                //          foreach (Order order in sellOrders)                     //
                //          {                                                       //
                //              richTextBox1.AppendText(second + " seconds >> Удаление заяки на продажу, потому что не нужно продавать: " + req.remove(order.id) + Environment.NewLine);// Удаляю сделки на продажу, если не нужно продавать       
                //          }                                                       //
                //      }                                                           //
                //      //++++++++++++++++++++++++++++++4
                //
                //
                //      if (!buyDeal)                                               //
                //      {                                                           //
                //          foreach (Order order in buyOrders)                      //
                //          {                                                       //
                //              richTextBox1.AppendText(second + " seconds >> Удаление заявки на покупку, потому что не нужно покупать: " + req.remove(order.id) + Environment.NewLine);//  Удаляю сделки на покупку, если не нужно покупать
                //          }                                                       //
                //      }                                                           //
                //
                //      //++++++++++++++++++++++++++++++5
                //      float sellPrice = 0;
                //      if (sellDeal) { sellPrice = req.sellMinPrice(); }
                //      //++++++++++++++++++++++++++++++6
                //      if (sellDeal)//Если надо продавать
                //      {
                //          bool reOrder = false;                     //
                //          if (sellPrice != mySellPrice) { sellPrice -= 0.0001F; reOrder = true; } //Проверяем, если моя сделка не первая в очереди, то перебиваем первую сделку
                //          if (sellPrice > balancePriceBTC * 1.0015 && reOrder)
                //          {
                //              foreach (Order order in sellOrders)                     //
                //              {                                                       //
                //                  richTextBox1.AppendText(second + " seconds >> Удалена старая сделка на продажу: " + req.remove(order.id) + " ");// Удаляю старые сделки на продажу
                //              }                                                       //
                //              richTextBox1.AppendText(second + " seconds >> Создана заявка на продажу: " + req.sell("0.001", sellPrice.ToString(), "UAH", "BTC") + " " + sellPrice + Environment.NewLine);      //
                //              int i = indexBPU;
                //              expectableBalancePriceUAH = (balancePriceUAH * i + sellPrice) / (indexBPU + 1);
                //              indexBPU++;
                //              indexBPB--;
                //              mySellPrice = sellPrice;                                    //
                //              reOrder = false;
                //          }                                                               //
                //
                //      }
                //      //++++++++++++++++++++++++++++++7
                //      float buyPrice = 0;
                //      if (buyDeal) { buyPrice = req.buyMaxPrice(); }
                //      //++++++++++++++++++++++++++++++8
                //      if (buyDeal)//Если надо покупать
                //      {
                //          bool reOrder = false;
                //          if (buyPrice != myBuyPrice) { buyPrice += 0.0001F; reOrder = true; }   //Проверяем, если моя сделка не первая в очереди, то перебиваем первую сделку   
                //          if (buyPrice < balancePriceUAH * 0.9985 && reOrder)
                //          {
                //              foreach (Order order in buyOrders)                      //
                //              {                                                       //
                //                  richTextBox1.AppendText(second + " seconds >> Удалена старая сделка на покупку: " + req.remove(order.id) + " ");//  Удаляю старые сделки на покупку
                //              }                                                       //
                //              richTextBox1.AppendText(second + " seconds >> Создана заявка на покупку: " + req.buy("0.001", buyPrice.ToString(), "UAH", "BTC") + " " + buyPrice + Environment.NewLine);
                //              int i = indexBPB;
                //              expectableBalancePriceBTC = (balancePriceBTC * i + buyPrice) / (indexBPB + 1);
                //              indexBPU--;
                //              indexBPB++;
                //              myBuyPrice = buyPrice;
                //              reOrder = false;
                //          }
                //      }
                //
                //      StreamWriter balancePriceFileWriter = new StreamWriter("pyScripts/data/balanceprice.dat", false); //Получение цен остатков
                //      balancePriceFileWriter.WriteLine(balancePriceUAH);
                //      balancePriceFileWriter.WriteLine(balancePriceBTC);
                //      balancePriceFileWriter.WriteLine(indexBPU);
                //      balancePriceFileWriter.WriteLine(indexBPB);
                //      balancePriceFileWriter.Close();
                //
                //      count++;
                //      richTextBox1.AppendText(Environment.NewLine);
            }
            catch { richTextBox1.AppendText(second + " seconds >> Something wrong..." + Environment.NewLine); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = "log/" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".log";
            richTextBox1.SaveFile(fileName); //Логирование 
            Close();
        }

        bool tick = false;

        private void button4_Click(object sender, EventArgs e)
        {
            tick = false;
            button2.Enabled = false;
            button4.Enabled = false;
        }

        private void Second_Tick(object sender, EventArgs e)
        {
            second += 1;
        }

        bool buyDeal = false;
        bool sellDeal = false;
        balance balance;

        private void timer1_Tick_1(object sender, EventArgs e) //Получение баланса аккаунта (Без открытых заявок)
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 1" + Environment.NewLine);
                //++++++++++++++++++++++++++++++1
                buyDeal = false;
                sellDeal = false;
                // richTextBox1.AppendText(count + ">>" + req.sell("0.001", "1000000", "UAH", "BTC"));
                string json = req.balance();                                             //Получение баланса
                balance = JsonConvert.DeserializeObject<balance>(json);          //Десериализируем 
                timer2.Enabled = true;
                timer1.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 1 Smt wrong" + Environment.NewLine); }
        }

        List<Order> myOrders;
        List<Order> buyOrders;
        List<Order> sellOrders;

        float balanceUAH;
        float balanceBTC;

        private void timer2_Tick(object sender, EventArgs e)//Получение моих заявок. Подсчет общего баланса. Получение цен остатков. Проверка необходисмости сделок
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 2" + Environment.NewLine);
                //++++++++++++++++++++++++++++++2
                myOrders = req.myOrders().your_open_orders;                  //Получения всех моих заявок
                buyOrders = myOrders.Where(x => x.type == "buy").ToList();   //Получение моих заявок на покупку
                sellOrders = myOrders.Where(x => x.type == "sell").ToList(); //Получение моих заявок на продажу
                float balanceUAHDiff = 0;
                float balanceBTCDiff = 0;
                foreach (Order order in buyOrders)
                {
                    balanceUAHDiff += order.sum2;                                        //Подсчет гривен на заявках
                }
                foreach (Order order in sellOrders)
                {
                    balanceBTCDiff += order.sum1;                                        //Подсчет биткоинов на заявках
                }

                balanceUAH = balance.accounts[0].balance + balanceUAHDiff;         //Подсчет общего баланса гривен
                balanceBTC = balance.accounts[1].balance + balanceBTCDiff;         //Подсчет общего баланса биткоинов
                richTextBox1.AppendText(second + " seconds >> UAH: " + balanceUAH.ToString() + " BTC: " + balanceBTC.ToString() + " " + Environment.NewLine);
                StreamReader balancePriceFile = new StreamReader("pyScripts/data/balanceprice.dat"); //Получение цен остатков
                float.TryParse(balancePriceFile.ReadLine(), out balancePriceUAH);  //Цена остатка гривен
                float.TryParse(balancePriceFile.ReadLine(), out balancePriceBTC);  //Цена остатка биткоинов
                int.TryParse(balancePriceFile.ReadLine(), out indexBPU);           //Соотнощение гривен
                int.TryParse(balancePriceFile.ReadLine(), out indexBPB);           //          и биткоинов
                balancePriceFile.Close();
                label1.Text = "balancePriceUAH: " + balancePriceUAH.ToString();
                label2.Text = "balancePriceBTC: " + balancePriceBTC.ToString();
                // label3.Text = "UAH: " + balance.accounts[0].balance.ToString();
                // label4.Text = "BTC: " + balance.accounts[1].balance.ToString();
                label3.Text = "UAH: " + balanceUAH.ToString() + " " + balance.accounts[0].balance.ToString();
                label4.Text = "BTC: " + balanceBTC.ToString() + " " + balance.accounts[1].balance.ToString();

                float balanceRatio = balanceUAH / (balanceUAH + (balanceBTC*balancePriceBTC));

                if (balanceRatio < 0.3) { sellDeal = true; }                               //
                else if (balanceRatio < 0.7) { sellDeal = true; buyDeal = true; }          //Проверка необходимых сделок
                else { buyDeal = true; }                                                   //
                                                                                           //

                timer3.Enabled = true;
                timer2.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 2 Smt wrong" + Environment.NewLine); }
        }

        private void timer3_Tick(object sender, EventArgs e)//Удаление ненужных заявок на продажу 
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 3" + Environment.NewLine);
                //++++++++++++++++++++++++++++++3
                if (!sellDeal)                                              //
                {                                                           //
                    foreach (Order order in sellOrders)                     //
                    {                                                       //
                        richTextBox1.AppendText(second + " seconds >> Удаление заяки на продажу, потому что не нужно продавать: " + req.remove(order.id) + " ");// Удаляю сделки на продажу, если не нужно продавать       
                    }                                                       //
                }                                                           //

                timer4.Enabled = true;
                timer3.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 3 Smt wrong" + Environment.NewLine); }
        }

        private void timer4_Tick(object sender, EventArgs e)//Удаление ненужных заявок на покупку
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 4" + Environment.NewLine);
                //++++++++++++++++++++++++++++++4


                if (!buyDeal)                                               //
                {                                                           //
                    foreach (Order order in buyOrders)                      //
                    {                                                       //
                        richTextBox1.AppendText(second + " seconds >> Удаление заявки на покупку, потому что не нужно покупать: " + req.remove(order.id) + " ");//  Удаляю сделки на покупку, если не нужно покупать
                    }                                                       //
                }                                                           //

                timer5.Enabled = true;
                timer4.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 4 Smt wrong" + Environment.NewLine); }
        }

        float sellPrice;

        private void timer5_Tick(object sender, EventArgs e)//Получение самой дешовой заявки на продажу
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 5" + Environment.NewLine);
                //++++++++++++++++++++++++++++++5
                sellPrice = 0;
                if (sellDeal) { sellPrice = req.sellMinPrice(); }

                timer6.Enabled = true;
                timer5.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 5 Smt wrong" + Environment.NewLine); }
        }

        private void timer6_Tick(object sender, EventArgs e)//Продажа
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 6" + Environment.NewLine);
                //++++++++++++++++++++++++++++++6
                if (sellDeal)//Если надо продавать
                {
                    bool reOrder = false;                     //
                    if (sellPrice != mySellPrice) { sellPrice -= 0.0001F; reOrder = true; } //Проверяем, если моя сделка не первая в очереди, то перебиваем первую сделку
                    if (sellPrice > balancePriceBTC * 1.0015 && reOrder)
                    {
                        foreach (Order order in sellOrders)                     //
                        {                                                       //
                            if (order.id != "0")
                            {
                                richTextBox1.AppendText(second + " seconds >> Удалена старая сделка на продажу: " + req.remove(order.id) + " " + Environment.NewLine);// Удаляю старые сделки на продажу
                                order.id = "0";
                            }
                        }                                                       //
                        float count = 0.001F;
                        if (balanceBTC / indexBPB > count) { count = balanceBTC / indexBPB; }
                        richTextBox1.AppendText(second + " seconds >> Создана заявка на продажу: " + req.sell(count.ToString(), (sellPrice - 0.000001F).ToString(), "UAH", "BTC") + " " + (sellPrice - 0.000001F).ToString() + Environment.NewLine);      //
                        int i = indexBPU;
                        expectableBalancePriceUAH = (balancePriceUAH * i + sellPrice) / (indexBPU + 1);
                        indexBPU++;
                        indexBPB--;
                        mySellPrice = sellPrice;                                    //
                        reOrder = false;
                    }

                }

                timer7.Enabled = true;
                timer6.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 6 Smt wrong" + Environment.NewLine); }
        }

        float buyPrice;

        private void timer7_Tick(object sender, EventArgs e)//Получение самой дорогой заявки на покупку
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 7" + Environment.NewLine);
                //++++++++++++++++++++++++++++++7
                buyPrice = 0;
                if (buyDeal) { buyPrice = req.buyMaxPrice(); }

                timer8.Enabled = true;
                timer7.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 7 Smt wrong" + Environment.NewLine); }
        }

        private void timer8_Tick(object sender, EventArgs e)//Покупка
        {
            try
            {
                richTextBox1.AppendText(second + " >> Timer 8" + Environment.NewLine);
                //++++++++++++++++++++++++++++++8
                if (buyDeal)//Если надо покупать
                {
                    bool reOrder = false;
                    if (buyPrice != myBuyPrice) { buyPrice += 0.0001F; reOrder = true; }   //Проверяем, если моя сделка не первая в очереди, то перебиваем первую сделку   
                    if (buyPrice < balancePriceUAH * 0.9985 && reOrder)
                    {
                        foreach (Order order in buyOrders)                      //
                        {
                            if (order.id != "0")
                            {
                                richTextBox1.AppendText(second + " seconds >> Удалена старая сделка на покупку: " + req.remove(order.id) + " ");//  Удаляю старые сделки на покупку
                                order.id = "0";
                            }
                        }
                        float count = 0.001F;
                        if (balanceUAH / indexBPU > count) { count = balanceUAH / indexBPU; }//
                        richTextBox1.AppendText(second + " seconds >> Создана заявка на покупку: " + req.buy(count.ToString(), (buyPrice + 0.000001F).ToString(), "UAH", "BTC") + " " + (buyPrice + 0.000001F).ToString() + Environment.NewLine);
                        int i = indexBPB;
                        expectableBalancePriceBTC = (balancePriceBTC * i + buyPrice) / (indexBPB + 1);
                        indexBPU--;
                        indexBPB++;
                        myBuyPrice = buyPrice;
                        reOrder = false;
                    }
                }
                timer9.Enabled = true;
                timer8.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 8 Smt wrong" + Environment.NewLine); }
        }

        //Проверить заявки
        string sellOrderId;
        string buyOrderId;
        //++++++++++++++++++++++++++++++9
        private void timer9_Tick(object sender, EventArgs e)
        {
           try
           {
                if (sellDeal)//Сделка на продажу, которая будет проверена на выполненость
                {
                    StreamReader sellOrderIdFile = new StreamReader("pyScripts/tmp/sellOrderId.dat");
                    sellOrderId = sellOrderIdFile.ReadLine();
                    sellOrderIdFile.Close();
                }

                OrderInfo order = req.orderCheck(sellOrderId);
                if (order.status == "processed") { balancePriceUAH = expectableBalancePriceUAH; }

                richTextBox1.AppendText(second + " >> Timer 9" + Environment.NewLine);

                timer10.Enabled = true;
                timer9.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 9 Smt wrong" + Environment.NewLine); }
        }

        //++++++++++++++++++++++++++++++++++10
        private void timer10_Tick(object sender, EventArgs e)
        {
            try
            {
                if (buyDeal)//Сделка на покупку, которая будет проверена на выполненость
                {
                    StreamReader buyOrderIdFile = new StreamReader("pyScripts/tmp/buyOrderId.dat");
                    buyOrderId = buyOrderIdFile.ReadLine();
                    buyOrderIdFile.Close();
                }

                OrderInfo order = req.orderCheck(buyOrderId);
                if (order.status == "processed") { balancePriceBTC = expectableBalancePriceBTC; }

                StreamWriter balancePriceFileWriter = new StreamWriter("pyScripts/data/balanceprice.dat", false); //Сохранение цен остатков
                balancePriceFileWriter.WriteLine(balancePriceUAH);
                balancePriceFileWriter.WriteLine(balancePriceBTC);
                balancePriceFileWriter.WriteLine(indexBPU);
                balancePriceFileWriter.WriteLine(indexBPB);
                balancePriceFileWriter.Close();

                richTextBox1.AppendText(second + " >> Timer 10" + Environment.NewLine);

                if (tick)
                {
                    timer1.Enabled = true;
                }
                else { button2.Enabled = true; }
                timer10.Enabled = false;
            }
            catch { richTextBox1.AppendText(second + " >> Step 10 Smt wrong" + Environment.NewLine); }
        }
    }
}

