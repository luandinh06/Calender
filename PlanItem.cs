using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calender
{
    public class PlanItem
    {
        private DateTime date;
        private bool notify;
        private string job;
        private Point fromTime;
        private Point toTime;
        private string status;
        //public List<string> ListStatus = new List<string>() {"DONE","DOING","COMING","MISSED"};

        public DateTime Date { get => date; set => date = value; }
        public bool Notify { get => notify; set => notify = value; }
        public string Job { get => job; set => job = value; }
        public Point FromTime { get => fromTime; set => fromTime = value; }
        public Point ToTime { get => toTime; set => toTime = value; }
        public string Status { get => status; set => status = value; }       
    }
 
}
