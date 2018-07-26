using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas
{
    class Order
    {
        public string amnt_base { get; set; }   // сумма сделки в базовой валюте
        public string pub_date { get; set; }    // дата сделки 
        public string price { get; set; }       // цена
        public float sum2 { get; set; }        // прибавляю к balanceUAH, если type == buy
        public float sum1 { get; set; }        // прибавляю к balanceBTC, если type == sell
        public string amnt_trade { get; set; }  // сумма сделки в валюте торга
        public string type { get; set; }        // тип операции sell/buy 
        public string id { get; set; }          // id заявки


        public override string ToString()
        {
            return amnt_base + " " + pub_date + " " + price + sum2 + " " + sum1 + " " + amnt_trade + " " + type + " " + id;
        }
    }
}
//{"amnt_base": "1538.2358440000", "pub_date": "Oct. 18, 2014, 7:40 p.m.", "price": "5400.0000000000", "sum2": "1538.2358440000", "sum1": "0.2848584896", "amnt_trade": "0.2848584896", "type": "sell", "id": 4624}