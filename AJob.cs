using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calender
{
    public partial class AJob : UserControl
    {
        private PlanItem job;
        public PlanItem Job { get => job; set => job = value; }

        public AJob(PlanItem pi)
        {
            InitializeComponent();
            this.Job = pi;
            cbStatus.DataSource = Cons.ListStatus;
            showdata(pi);
        }

        private void showdata(PlanItem j)
        {
            tbJob.Text = j.Job;
            nmFromHour.Value = j.FromTime.X;
            nmFromMinute.Value = j.FromTime.Y;
            nmToHour.Value = j.ToTime.X;
            nmToMinute.Value = j.ToTime.Y;
            cbStatus.SelectedIndex = Cons.ListStatus.IndexOf(j.Status);
            cbDone.Checked = Cons.ListStatus.IndexOf(j.Status) == Cons.ListStatus.IndexOf("DONE") ? true : false;
        }

        private event EventHandler edited;
        public event EventHandler Edited
        {
            add { edited += value; }
            remove { edited -= value; }
        }

        private event EventHandler deleted;
        public event EventHandler Deleted
        {
            add { deleted += value; }
            remove { deleted -= value; }

        }
        
        private void btEdit_Click(object sender, EventArgs e)
        {
            Job.Job = tbJob.Text;
            Job.FromTime = new Point((int)nmFromHour.Value, (int)nmFromMinute.Value);
            Job.ToTime = new Point((int)nmToHour.Value, (int)nmToMinute.Value);
            Job.Status = cbStatus.SelectedItem.ToString();
            if (edited != null) edited(this, new EventArgs());
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (deleted != null) deleted(this, new EventArgs());
        }

        private void cbDone_CheckedChanged(object sender, EventArgs e)
        {
            cbStatus.SelectedIndex = cbDone.Checked ? (int)EPlanItem.DONE : (int)EPlanItem.DOING;
        }
    }
}
