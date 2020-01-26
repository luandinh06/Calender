using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Calender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //StartUP();
            Apptime = 0;
            tmNotify.Start();
            LoadMatrix();
        }

        #region Function

        void LoadMatrix()
        {
            Matrix = new List<List<Button>>();

            Button oldBtn = new Button() { Width = 0, Height = 0, Location = new Point(-Cons.buttonMargin, 0) };
            for (int i = 0; i < Cons.DayOfColumn; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.DayOfWeek; j++)
                {
                    Button btn = new Button() { Width = Cons.buttonWidth, Height = Cons.buttonHeight };
                    btn.Location = new Point(oldBtn.Location.X + oldBtn.Width + Cons.buttonMargin, oldBtn.Location.Y);
                    btn.Click += Btn_Click;

                    pnMatrix.Controls.Add(btn);
                    Matrix[i].Add(btn);

                    oldBtn = btn;
                }
                oldBtn = new Button() { Width = 0, Height = 0, Location = new Point(-Cons.buttonMargin, oldBtn.Location.Y + Cons.buttonHeight + Cons.buttonMargin)};
            }

            AddNumberIntoMatrixByDate(dtpDate.Value);  
        }

        int DayOfMonth(DateTime date)
        {
            switch(date.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    return 31;
                case 2:
                    if ((date.Year % 4 == 0 && date.Year % 100 != 0) || date.Year % 400 == 0)
                        return 29;
                    else
                        return 28;
                default:
                    return 30;
            }
        }

        void AddNumberIntoMatrixByDate(DateTime date)
        {
            Clear_Matrix();

            DateTime usedate = new DateTime(date.Year, date.Month, 1);
            int column = dateOFWeek.IndexOf(usedate.DayOfWeek.ToString());
            int row = 0;
            for (int i = 1; i <= DayOfMonth(date); i++)
            {
                Button btn = Matrix[row][column];
                btn.Text = i.ToString();
                column++;

                if (column > 6)
                {
                    column = 0;
                    row++;
                }

                if (isEqualDate(DateTime.Now, usedate) && isEqualDate(date, usedate)) btn.BackColor = Color.LawnGreen;
                else if (isEqualDate(DateTime.Now, usedate)) btn.BackColor = Color.LawnGreen;
                else if (isEqualDate(date, usedate)) btn.BackColor = Color.RoyalBlue;          

                usedate = usedate.AddDays(1);
            }
        }

        void Clear_Matrix()
        {
            for(int i = 0; i < Matrix.Count; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++)
                {
                    Button btn = Matrix[i][j];
                    btn.Text = "";
                    btn.BackColor = Color.Empty;
                }
            }
        }

        bool isEqualDate(DateTime dateA, DateTime dateB)
        {
            return dateA.Year == dateB.Year && dateA.Month == dateB.Month && dateA.Day == dateB.Day;
        }

        #endregion

        #region Serialize

        private void SerializeToXML(object data, string FilePath)
        {
            //StreamWriter fs = new StreamWriter(new FileStream(FilePath, FileMode.Create));           
            XmlSerializer sr = new XmlSerializer(typeof(PlanData));
            FileStream fs = new FileStream(FilePath, FileMode.Create,FileAccess.Write);

            sr.Serialize(fs,data);
            fs.Close();
        }
        
        private object DeserializeFromXML(string FilePath)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
            try
            {
                XmlSerializer sr = new XmlSerializer(typeof(PlanData));
                object result = sr.Deserialize(fs);
                fs.Close();
                return result;
            }
            catch (Exception e)
            {
                fs.Close();
                throw new NotImplementedException();
            }

        }

        void SetDefaultJob()
        {
            Job = new PlanData();
            Job.Job = new List<PlanItem>();
            //PlanItem pi = new PlanItem();
            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                Notify = false,
                Job = "Test",
                FromTime = new Point(0, 0),
                ToTime = new Point(1, 1),
                Status = Cons.ListStatus[(int)EPlanItem.DONE]
            });
            Job.Job.Add(new PlanItem()
            {
                Date = DateTime.Now,
                Notify = false,
                Job = "Test",
                FromTime = new Point(0, 0),
                ToTime = new Point(2, 2),
                Status = Cons.ListStatus[(int)EPlanItem.MISSED]
            });
        }

        //void StartUP()
        //{
        //    RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\Calender");
        //    RegistryKey regstart = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
        //    String keyvalue = "1";
        //    try
        //    {
        //        regkey.SetValue("Index", keyvalue);
        //        regstart.SetValue("Calender", Application.StartupPath + "\\Calender.exe");
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        #endregion

        #region Peoperties
        private List<List<Button>> matrix;

        public List<List<Button>> Matrix { get => matrix; set => matrix = value; }     

        private PlanData job;

        public PlanData Job { get => job; set => job = value; }

        private List<string> dateOFWeek = new List<string>{"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};

        private string filePath = "data.xml";

        public string FilePath { get => filePath; set => filePath = value; }

        private int apptime;
        public int Apptime { get => apptime; set => apptime = value; }

        #endregion

        #region Event
        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            AddNumberIntoMatrixByDate(dtpDate.Value);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            dtpDate.Value = dtpDate.Value.AddMonths(-1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dtpDate.Value =  dtpDate.Value.AddMonths(1);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Now;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Job = DeserializeFromXML(FilePath) as PlanData;
            }
            catch
            {
                SetDefaultJob();
                SerializeToXML(job, FilePath);
                MessageBox.Show("chưa có file data");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerializeToXML(Job, FilePath);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((sender as Button).Text)) return;
            DailyPlan daily = new DailyPlan(new DateTime(dtpDate.Value.Year, dtpDate.Value.Month, Convert.ToInt32((sender as Button).Text)), job);
            daily.ShowDialog();
        }

        private void tmNotify_Tick(object sender, EventArgs e)
        {
            if (!cbNotify.Checked)
                return;
            Apptime++;
            if (Apptime < Cons.notifyTime)
                return;
            if (Job == null || Job.Job == null)
                return;
            DateTime currentdate = DateTime.Now;
            List<PlanItem> todayjobs = Job.Job.Where(l => l.Date.Year == currentdate.Year && l.Date.Month == currentdate.Month
            && l.Date.Day == currentdate.Day).ToList();
            Notify.ShowBalloonTip(Cons.notifyTimeOut, "Lịch Làm Việc", string.Format("Bạn có {0} việc trong ngày hôm nay", todayjobs.Count), ToolTipIcon.Info);
            Apptime = 0;
        }

        private void nmNotify_ValueChanged(object sender, EventArgs e)
        {
            Cons.notifyTime = (int)nmNotify.Value;
        }

        private void cbNotify_CheckedChanged(object sender, EventArgs e)
        {
            nmNotify.Enabled = cbNotify.Checked;
        }
        #endregion


    }
}
