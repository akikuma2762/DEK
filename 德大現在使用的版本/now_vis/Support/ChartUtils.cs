using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Support
{
    public class ChartUtils
    {
        public static void ChartInit(Chart chart)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Titles.Add("Title1");
            chart.Titles["Title1"].Font = new Font("Microsoft Sans Serif", 20);
            //Chart style 
            chart.BorderlineColor = Color.Black;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BackSecondaryColor = Color.White;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 2;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            //Chart1 style ChartAreas
            AddArea(chart, "ChartArea1");

        }

        public static ChartArea AddArea(Chart chart, string name)
        {
            ChartArea area = new ChartArea(name);
            chart.ChartAreas.Add(area);
            //---------------------------
            area.AxisY.TextOrientation = TextOrientation.Stacked;
            area.AxisX.MajorGrid.LineColor =
                area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.MajorGrid.LineDashStyle =
                area.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            //chart.ChartAreas[name].AxisX.LabelStyle.Angle = 20; //斜度
            //chart.ChartAreas[name].BackColor = Color.FromArgb(64, 165, 191, 228);
            //chart.ChartAreas[name].BackGradientStyle = GradientStyle.TopBottom;
            //chart.ChartAreas[name].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chart.ChartAreas[name].AxisX.LabelStyle.Font = new Font("", 10);           
            return area;
        }

        public static Series AddSeries(Chart chart, ChartArea area, string series_name,
            string chart_type)
        {
            Series series = new Series(series_name);
            if (area == null) area = chart.ChartAreas[0];
            chart.Series.Add(series);
            series.ChartArea = area.Name;
            //series.Color = color;
            series.LegendText = series_name;
            series.XValueType = ChartValueType.String;
            //series.YValueType = ChartValueType.Double;
            switch (chart_type.ToLower())
            {
                case "bar":
                    series.ChartType = SeriesChartType.Column;
                    break;
                case "pie":
                    chart.Legends.Add(series_name);
                    series.ChartType = SeriesChartType.Pie;
                    break;
                case "line":
                    series.ChartType = SeriesChartType.FastLine;
                    series.BorderWidth = 2;
                    break;
            }
            /*   
               //Chart1 style Seriery
               series.Palette = ChartColorPalette.BrightPastel;
               series.BackGradientStyle = GradientStyle.DiagonalLeft;
               series.BorderColor = Color.FromArgb(180, 26, 59, 105);
           */
            return series;
        }

        public static StripLine AddStripLine(Axis axis, Color color, double value, StringAlignment align)
        {
            StripLine sl = new StripLine();
            sl.BorderColor = color;
            sl.IntervalOffset = value;
            sl.TextLineAlignment = align;
            sl.Text = string.Format("{0:G}", value);
            axis.StripLines.Add(sl);
            return sl;
        }

        public static void Series_DataTrim(Series series, int maxlength)
        {
            if (maxlength > 0)
            {
                while (series.Points.Count > maxlength)
                    series.Points.RemoveAt(0);
            }
        }

        public static void Series_DataAppend(Series series, int length, int[] data, out int min, out int max,
           int max_points = 0, bool is_clear = true)
        {
            if (is_clear) series.Points.Clear();
            Series_DataTrim(series, max_points - length);
            int data_size = data.Length;
            min = max = data[0];
            int value;
            for (int index = 0; index < length; index++)
            {
                if (index >= data_size) break;
                value = data[index];
                if (value < min) min = value;
                else if (value > max) max = value;
                series.Points.Add(value);
            }
        }

        public static void Series_DataAppend(Series series,int length, float[] data,out float min,out float max,
            int max_points=0, bool is_clear=true)
        {
            if (is_clear) series.Points.Clear();
            Series_DataTrim(series, max_points - length);
            int data_size = data.Length;
            min = max = data[0];
            float value;
            for (int index = 0; index < length; index++)
            {
                if (index >= data_size) break;
                value = data[index];
                if (value < min) min = value;
                else if (value > max) max = value;
                series.Points.Add(value);
            }
        }
    }
}
