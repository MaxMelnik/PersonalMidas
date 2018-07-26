using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace Midas
{
    //public class Deal
    //{
    //    public string amnt_base { get; set; }   // сумма сделки в базовой валюте,  
    //    public string amnt_trade { get; set; }  // сумма сделки в валюте торга
    //    public string price { get; set; }       // цена
    //    public string pub_date { get; set; }    // дата сделки
    //    public string user { get; set; }        // участник сделки 
    //    public string type { get; set; }        // тип операции sell/buy 
    //}
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            Request req = new Request();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Deal> deals = new List<Deal>();
            deals = req.deal();
            label1.Text = deals[0].ToString();
        }

        Request req = new Request();
        
        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = req.balance() + " nonce=" + req.nonce;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = req.sell("0.001", "1000000", "UAH", "BTC");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label1.Text = req.buy("0.001", "100", "UAH", "BTC");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Text = req.ask("1");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            label1.Text = req.remove("5420028");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Process.Start(@"pyScripts\pyTest.py");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            label1.Text = req.sellMinPrice().ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label1.Text = req.buyMaxPrice().ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label1.Text = req.priceBTC().ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label1.Text = req.myOrders().your_open_orders[0].ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            label1.Text = req.orderCheck("5066726").ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (var request = new HttpRequest())
            {
               label1.Text = req.authorization(request);
            }
        }
    }
}
