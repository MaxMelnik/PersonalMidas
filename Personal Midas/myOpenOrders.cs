using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas
{
    class myOpenOrders
    {
        public List<Order> your_open_orders { get; set; }   //Список заявок 
        public float balance_buy { get; set; }//
        public float balance_sell {get; set; }//


        public override string ToString()
        {
            return your_open_orders.ToString() + " balance_buy: " + balance_buy + " balance_sell: " + balance_sell;
        }
    }

}
