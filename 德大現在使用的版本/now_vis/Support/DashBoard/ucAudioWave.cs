using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSound;
using System.Windows.Forms.DataVisualization.Charting;

namespace Support.DashBoard
{
    public partial class ucAudioWave : UserControl
    {
        public ucAudioWave()
        {
            InitializeComponent();
            ucWaveFFT_main._ucChild = this;
            _IsAudioPlayBack = false;
            _Login = "dek";
            _Pass = "dek";
            _FFTBarCount = 64;
            _WaveInDev = _FormAudioWaveSetup._AudioWaveIn;
            _WaveOutDev = _FormAudioWaveSetup._AudioWaveOut;
            recorder.OnDataRecorded += Recorder_OnDataRecorded;
            recorder.OnRecordingStopped += Recorder_OnRecordingStopped;
            wave_stream.OnDataStream += Recorder_OnDataRecorded;
            AudioWaveDevConverter.callBack = TypeConverter_callBack;
        }
        //===========================================================
        public bool _IsStart
        {
            get
            {
                if (_IsIPStream) return wave_stream.IsStarted;
                else return recorder.Started;
            }
        }

        public bool _IsIPStream
        {
            get { return _WaveInDev != "" && _WaveInDev.ToLower().Contains("http://"); }
        }

        public bool _IsAudioPlayBack { get; set; }

        public bool _CtrlVisible
        {
            get { return ucWaveFFT_main._CtrlVisible; }
            set { ucWaveFFT_main._CtrlVisible = value; }
        }
        public Chart _WaveChart
        {
            get { return ucWaveFFT_main._WaveChart; }
        }
        public Chart _FFTChart
        {
            get { return ucWaveFFT_main._FFTChart; }
        }

        public string _Login { get; set; }
        public string _Pass { get; set; }
        public int _FFTBarCount
        {
            get { return ucWaveFFT_main._FFTBarCount; }
            set { ucWaveFFT_main._FFTBarCount = value; }
        }
        
     public string _Title
        {
            get { return ucWaveFFT_main._Title; }
            set { ucWaveFFT_main._Title = value; }
        }

        public Color _Title_BackColor
        {
            get { return ucWaveFFT_main._Title_BackColor; }
            set { ucWaveFFT_main._Title_BackColor = value; }
        }

        public bool _IsAutoScaleWave
        {
            get { return ucWaveFFT_main._IsAutoScaleWave; }
            set { ucWaveFFT_main._IsAutoScaleWave = value; }
        }

        public bool _IsAutoScaleFFT
        {
            get { return ucWaveFFT_main._IsAutoScaleFFT; }
            set { ucWaveFFT_main._IsAutoScaleFFT = value; }
        }

        public double _WaveMinimum
        {
            get { return ucWaveFFT_main._WaveChart.ChartAreas[0].AxisY.Minimum; }
            set { ucWaveFFT_main._WaveChart.ChartAreas[0].AxisY.Minimum = value; }
        }

        public double _WaveMaximum
        {
            get { return ucWaveFFT_main._WaveChart.ChartAreas[0].AxisY.Maximum; }
            set { ucWaveFFT_main._WaveChart.ChartAreas[0].AxisY.Maximum = value; }
        }

        public double _FFTMinimum
        {
            get { return ucWaveFFT_main._FFTChart.ChartAreas[0].AxisY.Minimum; }
            set { ucWaveFFT_main._FFTChart.ChartAreas[0].AxisY.Minimum = value; }
        }

        public double _FFTMaximum
        {
            get { return ucWaveFFT_main._FFTChart.ChartAreas[0].AxisY.Maximum; }
            set { ucWaveFFT_main._FFTChart.ChartAreas[0].AxisY.Maximum = value; }
        }

        public Support.Forms.FormAudioWaveIO _FormAudioWaveSetup = new Forms.FormAudioWaveIO();
        public DialogResult ShowDialog()
        {
            bool isStart = recorder.Started;
            if (isStart) Stop();
            if (_WaveInDev != null && _WaveInDev!="")
                _FormAudioWaveSetup._AudioWaveIn = _WaveInDev;
            if (_WaveOutDev != null && _WaveOutDev!="")
                _FormAudioWaveSetup._AudioWaveOut = _WaveOutDev;
            if (_WaveSamplesPerSec != null)
                _FormAudioWaveSetup._SamplesPerSec = _WaveSamplesPerSec;
            if (_WaveBitsPerSample != null)
                _FormAudioWaveSetup._BitsPerSample = _WaveBitsPerSample;
            if (_WaveChannels != null)
                _FormAudioWaveSetup._Channels = _WaveChannels;
            DialogResult result = _FormAudioWaveSetup.ShowDialog();
            if (result == DialogResult.OK)
            {
                _WaveInDev = _FormAudioWaveSetup._AudioWaveIn;
                _WaveOutDev = _FormAudioWaveSetup._AudioWaveOut;
                _WaveSamplesPerSec = _FormAudioWaveSetup._SamplesPerSec;
                _WaveBitsPerSample = _FormAudioWaveSetup._BitsPerSample;
                _WaveChannels = _FormAudioWaveSetup._Channels;
            }
            //if (isStart) Start();
            return result;
        }   

        [TypeConverter(typeof(AudioWaveDevConverter))]
        public string _WaveOutDev
        {
            get;//{ return _FormAudioWaveSetup._AudioWaveOut; }
            set;// { _FormAudioWaveSetup._AudioWaveOut = value; }
        }

        [TypeConverter(typeof(AudioWaveDevConverter))]
        public string _WaveInDev
        {
            get;// { return _FormAudioWaveSetup._AudioWaveIn; }
            set;// { _FormAudioWaveSetup._AudioWaveIn = value; }
        }

        [TypeConverter(typeof(AudioWaveDevConverter))]
        public string _WaveSamplesPerSec { get; set; }

        [TypeConverter(typeof(AudioWaveDevConverter))]
        public string _WaveBitsPerSample { get; set; }

        [TypeConverter(typeof(AudioWaveDevConverter))]
        public string _WaveChannels { get; set; }


        private string SetObjValueByList(string obj, List<string> list, int index = 0)
        {
            if (obj != null && obj != "")
            {
                if (list.IndexOf(obj) < 0)
                    list.Insert(index, obj);
            }
            else if (list.Count > index)
                obj = list[index];
            return obj;
        }

        private void TypeConverter_callBack(string item_name, ListStringTypeConverter obj)
        {
            if (obj.Items.Count > 0) return;
            if (item_name.Contains("_WaveIn"))
            {
                obj.IsReadOnly = false;
                //obj.Items = Win32mm.WaveIn_DevNameList();
                obj.Items.AddRange(_FormAudioWaveSetup._AudioWaveInList);
                _WaveInDev = _FormAudioWaveSetup._AudioWaveIn;
            }
            else if (item_name.Contains("_WaveOut"))
            {
                //obj.IsReadOnly = false;
                //obj.Items = Win32mm.WaveOut_DevNameList();
                obj.Items.AddRange(_FormAudioWaveSetup._AudioWaveOutList);
                _WaveOutDev = _FormAudioWaveSetup._AudioWaveOut;
            }
            else if (item_name.Contains("SamplesPerSec"))
            {
                //obj.Items.Add("5000");
                //obj.Items.Add("8000");
                //obj.Items.Add("11025");
                //obj.Items.Add("22050");
                //obj.Items.Add("44100");
                obj.Items.AddRange(_FormAudioWaveSetup._SamplesPerSecList);
                _WaveSamplesPerSec = _FormAudioWaveSetup._SamplesPerSec;
            }
            else if (item_name.Contains("_WaveBitsPerSample"))
            {
                //obj.Items.Add("16");
                //obj.Items.Add("8");
                obj.Items.AddRange(_FormAudioWaveSetup._BitsPerSampleList);
                _WaveBitsPerSample = _FormAudioWaveSetup._BitsPerSample;
            }
            else if (item_name.Contains("_WaveChannels"))
            {
                //obj.Items.Add("1");
                //obj.Items.Add("2");
                obj.Items.AddRange(_FormAudioWaveSetup._ChannelsList);
                _WaveChannels = _FormAudioWaveSetup._Channels;
            }
        }

        //======================================================================
        Recorder recorder = new Recorder();
        Player player = new Player();
        IPStream wave_stream = new IPStream();

        private void chart_MouseClick(object sender, MouseEventArgs e)
        {
            Stop();
            _CtrlVisible = !_CtrlVisible;
        }

        private void toolStripMenuItem_輸出入設定_Click(object sender, EventArgs e)
        {
            ShowDialog();
        }

        private void 啟動停止ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (recorder.Started) Stop();
            else Start();
        }

        private void Recorder_OnRecordingStopped()
        {

        }

        private int[] AudioData_ToInt(byte[] data, bool isWord, bool is2Chan, out int min, out int max)
        {
            //8-bit samples are stored as unsigned bytes, ranging from 0 to 255
            //16-bit samples are stored as 2's-complement signed integers, ranging from -32768 to 32767.
            //DATA: little-endian, 16bit: 2's complement, 8bit: 0-255
            int array_size = data.Length;
            int data_step = 1;
            if (isWord)
            {
                array_size /= 2;
                data_step = 2;
            }
            if (is2Chan)
            {
                array_size /= 2;
                data_step *= 2;
            }
            int [] out_array = new int [array_size];
            int value;
            int out_index = 0;
            min = max = 0;
            for (int index = 0; index < data.Length; index += data_step)
            {
                if (isWord)
                    value = (short)((data[index + 1] << 8) | data[index]);
                else
                {
                    value = (short)(128 - data[index]);
                }
                out_array[out_index++] = value;
                if (index == 0) { max = min = value; }
                else
                {
                    if (value < min) min = value;
                    if (value > max) max = value;
                }
            }
            return out_array;
        }
        bool IsWord = true;
        bool Is2Channels = false;
        private void Recorder_OnDataRecorded(byte[] data)
        {
            try
            {
                if (_IsAudioPlayBack)
                {
                    if (player.IsOpened!=true)
                    { 
                        if (wave_stream.IsStarted)
                        {
                            player.Start(_WaveOutDev,
                                    (int)wave_stream.WaveFileHeader.SamplesPerSecond,
                                    wave_stream.WaveFileHeader.BitsPerSample, 
                                    wave_stream.WaveFileHeader.Channels, 4);
                        }
                    }
                    if (player.IsOpened)
                        player.PlayData(data, false);
                }

                this.Invoke(new MethodInvoker(delegate ()
                {
                    int min, max;
                    int[] audio_data = AudioData_ToInt(data,IsWord, Is2Channels, out min, out max);
                    ucWaveFFT_main.Data_Update(audio_data);
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("FormMain.cs | On_DataRecorded() | {0}", ex.Message));
            }
        }

        private void Chart_Init()
        {
            if (_IsAutoScaleWave)
            {
                _WaveChart.ChartAreas[0].AxisY.Maximum = 1;
                _WaveChart.ChartAreas[0].AxisY.Minimum = 0;
            }
            if (_IsAutoScaleFFT)
            {
                _FFTChart.ChartAreas[0].AxisY.Maximum = 1;
                _FFTChart.ChartAreas[0].AxisY.Minimum = 0;
            }
        }

        public bool Start()
        {
            Stop();
            Chart_Init();
            string waveIn = _WaveInDev.Trim();
            string waveOut = _WaveOutDev.Trim();
            if (_IsIPStream)
            {
                return wave_stream.Start_WaveStream(_WaveInDev, _Login, _Pass);
            }
            
            //Determine values
            int SamplesPerSecond = Convert.ToInt32(_WaveSamplesPerSec);
            int BitsPerSample = Convert.ToInt32(_WaveBitsPerSample);
            int Channels = Convert.ToInt32(_WaveChannels);
            int buffer_count = 8;
            Is2Channels = (Channels > 1);
            IsWord = (BitsPerSample == 16);
            //--------------------------
            if (_IsAudioPlayBack)
            {
                player.Start(waveOut, SamplesPerSecond, BitsPerSample, Channels, buffer_count);
            }
            //Start recording
            return recorder.Start(waveIn, SamplesPerSecond, BitsPerSample, Channels, buffer_count);
        }

        public bool Stop()
        {
            bool ok = recorder.Stop();
            if (!wave_stream.Stop()) ok = false;
            if (!player.Stop()) ok = false;
            return ok;
        }

        private void panel_main_SizeChanged(object sender, EventArgs e)
        {
            ///panel_TimeDomain.Height = panel_main.ClientSize.Height / 2;
        }

        
    }

    public class AudioWaveDevConverter : ListStringTypeConverter
    {
        public static TypeConverterCallBack callBack = null;
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            callBack?.Invoke(context.PropertyDescriptor.Name, this);
            return true;
        }
    }
}
