using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calender
{
    public partial class DailyPlan : Form
    {
        public DailyPlan(DateTime date, PlanData job)
        {
            InitializeComponent();
            this.Date = date;
            this.Job = job;
        }

        #region Peoperties
        private DateTime date;
        private PlanData job;

        public DateTime Date { get => date; set => date = value; }
        public PlanData Job { get => job; set => job = value; }

        FlowLayoutPanel fpn = new FlowLayoutPanel();

        List<PlanItem> GetJobByDate(DateTime date)
        {
            return Job.Job.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month && p.Date.Day == date.Day).ToList();
        }

        #endregion

        #region Funtion

        void ShowJobByDate(DateTime date)
        {

            fpn.Width = panel3.Width;
            fpn.Height = panel3.Height;
            fpn.AutoScroll = true;
            panel3.Controls.Add(fpn);
            fpn.Controls.Clear();

            if (Job != null && Job.Job != null)
            {
                List<PlanItem> ChoosedJob = GetJobByDate(date);
                for (int i = 0; i < ChoosedJob.Count; i++)
                {
                    AJob aj = new AJob(ChoosedJob[i]);

                    aj.Edited += Aj_Edited;
                    aj.Deleted += Aj_Deleted;

                    fpn.Controls.Add(aj);
                }
            }
        }

        #endregion

        #region Event

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            ShowJobByDate((sender as DateTimePicker).Value);
        }

        private void btnTomorrow_Click(object sender, EventArgs e)
        {
            dtp.Value = dtp.Value.AddDays(1);      
        }

        private void btnYesterday_Click(object sender, EventArgs e)
        {
            dtp.Value = dtp.Value.AddDays(-1);
        }

        private void DailyPlan_Load(object sender, EventArgs e)
        {
            dtp.Value = date;
        }

        private void mnsiAddJob_Click(object sender, EventArgs e)
        {
            PlanItem pi = new PlanItem() { Date = dtp.Value };
            Job.Job.Add(pi);
            AJob aj = new AJob(pi);

            aj.Edited += Aj_Edited;
            aj.Deleted += Aj_Deleted;

            fpn.Controls.Add(aj);


        }

        private void mnsiToday_Click(object sender, EventArgs e)
        {
            dtp.Value = DateTime.Now;
        }

        private void Aj_Deleted(object sender, EventArgs e)
        {
            AJob uc = sender as AJob;
            PlanItem job = uc.Job;
            fpn.Controls.Remove(uc);
            Job.Job.Remove(job);
        }

        private void Aj_Edited(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
