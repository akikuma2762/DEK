using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Support;

namespace Support.DashBoard
{
    public partial class ucChart : ucVisObj
    {
        public int update_rate_ms = 5000;
        private int update_time_left = 0;
        public DateTime datetime_start;
        public TimeSpan timespan_interval;
        //------------------------------------
        public List<string> mach_id_list = new List<string>();
        public string what;
        public int max_points = 10;
        //-------------------------------
        public ucChart()
        {
            InitializeComponent();
            _ucChild = this;
            ChartUtils.ChartInit(chart_main);
            
        }

        //public Chart _ucChart { get { return chart_main; } set { chart_main = value; } }

        public void AddSeries(string name, string chart_type)
        {
            ChartUtils.AddSeries(chart_main, null, name, chart_type);
                     
        }

        public Chart getChart
        {
            get { return chart_main; }
        }

        public string Title
        {
            get { return chart_main.Titles[0].Text; }
            set { chart_main.Titles[0].Text = value; }
        }

        public void MoveTo(int left,int top,int width=0,int height=0)
        {
            Left = left;
            Top = top;
            if (width>0) Width = width;
            if (height>0) Height = height;
        }

        public void UpdateRefresh(int time_ms)
        {
            if (update_time_left >= 0)
            {
                update_time_left -= time_ms;
                if (update_time_left <= 0)
                {
                    Refresh();
                    update_time_left = update_rate_ms;
                }
            }
        }
        public override void Refresh()
        {
            // string[] TargetValue, string what_, string where_, bool isNew
            DateTime time_end = DateTime.Now;
            DateTime time_start = time_end.Subtract(timespan_interval);
            string[] TargetValue = MachData.GetValue(mach_id_list, what, time_start, time_end);

            string str_time_range = time_start.ToString() + " ~ " + time_end.ToString();
            chart_main.ChartAreas[0].AxisX.Title = str_time_range;

            //chart data get 取得資料
            Series series;
            string time_str = time_end.ToString("HH:mm:ss");
            double highest = chart_main.ChartAreas[0].AxisY.Maximum;
            for (int i = 0; i < TargetValue.Length; i++)
            {
                string[] str_tab = TargetValue[i].Split(',');
                double value = Convert.ToDouble(str_tab[1]);
                series = chart_main.Series[i];
                series.LegendText = string.Format("{0}:  {1}%", series.Name, value);

                if (series.ChartType == SeriesChartType.FastLine ||
                    series.ChartType == SeriesChartType.Column)
                {
                    while (series.Points.Count >= max_points)
                        series.Points.RemoveAt(0);
                    series.Points.AddXY(time_str, value);
                }
                else
                {
                    if (series.Points.Count <= 0)
                        series.Points.AddXY(series.Name, value);
                    else series.Points[0].SetValueY(value);
                }
                value *= 1.1;
                if (value > highest) highest = value;
            }
            if (highest < 10) highest = 10;
            chart_main.ChartAreas[0].AxisY.Maximum = highest;
            base.Refresh();
        }


    }
}
