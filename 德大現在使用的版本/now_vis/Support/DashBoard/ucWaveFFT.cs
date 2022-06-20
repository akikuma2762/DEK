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

namespace Support.DashBoard
{
    public partial class ucWaveFFT : ucVisObj
    {
        public ucWaveFFT()
        {
            InitializeComponent();
            _ucChild = this;
            _FFTBarCount = 32;
            _WaveChart.ChartAreas[0].AxisY.Minimum = 0;
            _WaveChart.ChartAreas[0].AxisY.Maximum = 1;
            _FFTChart.ChartAreas[0].AxisY.Minimum = 0;
            _FFTChart.ChartAreas[0].AxisY.Maximum = 1;
        }

        //---------------------------------
        public int _FFTBarCount { get; set; }
        public bool _IsAutoScaleWave { get; set; }
        public bool _IsAutoScaleFFT { get; set; }
        public Chart _WaveChart { get { return chart_TimeDomain; } }
        public Chart _FFTChart { get { return chart_FreqDomain; } }

        public userControl.ucPanel3D _WaveText { get { return ucPanel3D_WaveLeft; } }
        public userControl.ucPanel3D _FFTText { get { return ucPanel3D_Freq; } }

        public void Data_Clear()
        {
            _WaveChart.Series[0].Points.Clear();
            _FFTChart.Series[0].Points.Clear();
        }
        public void Data_Update(int[] data,int length=0)
        {
            int wave_min, wave_max;
            if (length <= 0) length = data.Length;
            ChartUtils.Series_DataAppend(_WaveChart.Series[0], length, data, out wave_min, out wave_max);
            if (_IsAutoScaleWave)
            {
                if (wave_min < _WaveChart.ChartAreas[0].AxisY.Minimum)
                    _WaveChart.ChartAreas[0].AxisY.Minimum = wave_min;
                if (wave_max > _WaveChart.ChartAreas[0].AxisY.Maximum)
                    _WaveChart.ChartAreas[0].AxisY.Maximum = wave_max;
            }
            double fft_min, fft_max;
            ChartSeries_FFT(_FFTChart.Series[0], length, data, _FFTBarCount, out fft_min, out fft_max);
            if (_IsAutoScaleFFT)
            {
                // min is alway 0
                //if (wave_min < _FFTChart.ChartAreas[0].AxisY.Minimum)
                //    _FFTChart.ChartAreas[0].AxisY.Minimum = wave_min;
                if (wave_max > _FFTChart.ChartAreas[0].AxisY.Maximum)
                    _FFTChart.ChartAreas[0].AxisY.Maximum = wave_max;
            }
        }

        public void Data_Update(float[] data, int length = 0)
        {
            float wave_min, wave_max;
            if (length <= 0) length = data.Length;
            ChartUtils.Series_DataAppend(_WaveChart.Series[0], length, data, out wave_min, out wave_max);
            if (_IsAutoScaleWave)
            {
                if (wave_min < _WaveChart.ChartAreas[0].AxisY.Minimum)
                    _WaveChart.ChartAreas[0].AxisY.Minimum = wave_min;
                if (wave_max > _WaveChart.ChartAreas[0].AxisY.Maximum)
                    _WaveChart.ChartAreas[0].AxisY.Maximum = wave_max;
            }
            double fft_min, fft_max;
            ChartSeries_FFT(_FFTChart.Series[0], length, data, _FFTBarCount, out fft_min, out fft_max);
            if (_IsAutoScaleFFT)
            {
                // min is alway 0
                //if (wave_min < _FFTChart.ChartAreas[0].AxisY.Minimum)
                //    _FFTChart.ChartAreas[0].AxisY.Minimum = wave_min;
                if (wave_max > _FFTChart.ChartAreas[0].AxisY.Maximum)
                    _FFTChart.ChartAreas[0].AxisY.Maximum = wave_max;
            }
        }
        //-------------------------------------------------------------
        private void ucWaveFFT_SizeChanged(object sender, EventArgs e)
        {
            panel_Wave.Height = panel_main.ClientSize.Height / 2;
        }

        private void ucPanel3D1__Panel_Click(object sender, EventArgs e)
        {
            _CtrlVisible = !_CtrlVisible;
        }

        //-----------------------------------------------------------

        //===========================================================

        private void ChartSeries_TimeData(Series series, int[] data,out int min,out int max)
        {
            /*
            int max_count = data.Length * 5 / 4;
            while (series.Points.Count > max_count)
                series.Points.RemoveAt(0);
            */
            int value;
            series.Points.Clear();
            min = max = data[0];
            for (int index = 0; index < data.Length; index++)
            {
                value = data[index];
                series.Points.AddY(value);
                if (value < min) min = value;
                else if (value > max) max = value;
            }
        }

        private void ChartSeries_TimeData(Series series, float[] data, out float min, out float max)
        {
            /*
            int max_count = data.Length * 5 / 4;
            while (series.Points.Count > max_count)
                series.Points.RemoveAt(0);
            */
            float value;
            series.Points.Clear();
            min = max = data[0];
            for (int index = 0; index < data.Length; index++)
            {
                value = data[index];
                series.Points.AddY(value);
                if (value < min) min = value;
                else if (value > max) max = value;
            }
        }

        LomontFFT fft = new LomontFFT();
        private void ChartSeries_FFT(Series series,int data_size, int[] data, int bar_steps, out double min, out double max)
        {
            double[] complex_data = fft.ComplexArray(data_size, data);
            ChartSeries_ComplexFFT(series, complex_data, bar_steps, out min, out max);
        }

        private void ChartSeries_FFT(Series series, int data_size, float[] data, int bar_steps, out double min, out double max)
        {
            double[] complex_data = fft.ComplexArray(data_size, data);
            ChartSeries_ComplexFFT(series, complex_data, bar_steps, out min, out max);
        }

        private void ChartSeries_FFT(Series series,int data_size, double[] data, int bar_steps, out double min, out double max)
        {
            double[] complex_data = fft.ComplexArray(data_size, data);
            ChartSeries_ComplexFFT(series, complex_data, bar_steps, out min, out max);
        }

        private void ChartSeries_ComplexFFT(Series series,double[] complex_data, int bar_steps, out double min, out double max)
        {
            int complex_size = complex_data.Length;
            fft.FFT_ByTable(complex_data, true);
            //---------------------- 
            double real, imagenary;
            double local_max;
            min = max = 0;
            complex_size /= 2;
            int data_steps = complex_size / bar_steps / 2;
            if (data_steps <= 0) data_steps = 1;
            series.Points.Clear();
            for (int index = 0; index < complex_size;) // IGNORE the SAME HALF
            {
                local_max = 0;
                for (int i = 0; i < data_steps; i++)
                {
                    real = complex_data[index++];
                    imagenary = complex_data[index++];
                    local_max += Math.Sqrt(real * real + imagenary * imagenary);
                }
                series.Points.Add(local_max);
                if (local_max > max) max = local_max;
            }
        }
    }
}
