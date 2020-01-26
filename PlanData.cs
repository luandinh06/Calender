using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calender
{
    [Serializable]
    public class PlanData
    {
        private List<PlanItem> job;
        //public PlanItem a = new PlanItem();
        public List<PlanItem> Job { get => job; set => job = value; }
    }
}
