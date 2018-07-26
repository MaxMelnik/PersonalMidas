using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas
{
    public class Deal
    {
        public string amnt_base { get; set; }   // сумма сделки в базовой валюте,  
        public string amnt_trade { get; set; }  // сумма сделки в валюте торга
        public string price { get; set; }       // цена
        public string pub_date { get; set; }    // дата сделки
        public string user { get; set; }        // участник сделки 
        public string type { get; set; }        // тип операции sell/buy 

        public string ToString()
        {
          return  this.amnt_base + " UAH " + this.amnt_trade + " BTC " + this.price + " UAH/BTC " + this.pub_date + " " + this.user + " " + this.type;
        }
    }
}
