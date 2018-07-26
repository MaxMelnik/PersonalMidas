using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midas
{
    class OrderInfo
    {
        public string status { get; set; }        //Статус заявки
        public string sum2_history { get; set; }  //Получаю
        public string currency1 { get; set; }     //Валюта, которую отдаю
        public float sum2 { get; set; }           //Осталось получить
        public float sum1 { get; set; }           //Осталось отдать
        public string currency2 { get; set; }     //Валюта, которую получаю
        public string sum1_history { get; set; }  //Отдаю
        public string pub_date { get; set; }      //Время выставления заявки
        public string id { get; set; }            //Идентификатор заявки

        public override string ToString()
        {
            return status + " " + sum2_history + " " + currency1 + " " + sum2 + " " + sum1 + " " + currency2 + " " + sum1_history + " " + " " + pub_date + " " + id;
        }
    }
}
