using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSound;

namespace Support.Forms
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class FormAudioWaveIO : Form
    {
        public FormAudioWaveIO()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(FormAudioWaveIO), this);
            CtrlUtils.ComboBox_AddItems(comboBox_audioIn, _AudioWaveInList);
            CtrlUtils.ComboBox_AddItems(comboBox_audioOut, _AudioWaveOutList);
        }

        public List<string> _AudioWaveInList
        {
            get
            {
                List<string> list=Win32mm.WaveIn_DevNameList();
                list.Add("http://192.168.0.1/audio.cgi");
                return list;
            }
        }

        public List<string> _AudioWaveOutList
        {
            get
            {
                List<string> list = Win32mm.WaveOut_DevNameList();
                return list;
            }
        }

        public List<string> _SamplesPerSecList
        {
            get
            {
                return CtrlUtils.ComboBox_ToListString(comboBox_samplesPerSec);
            }
        }

        public List<string> _BitsPerSampleList
        {
            get
            {
                return CtrlUtils.ComboBox_ToListString(comboBox_bitsPerSample);
            }
        }

        public List<string> _ChannelsList
        {
            get
            {
                return CtrlUtils.ComboBox_ToListString(comboBox_channels);
            }
        }

        public string _AudioWaveIn
        {
            get { return comboBox_audioIn.Text; }
            set
            {
                comboBox_audioIn.Text = value;
                if (comboBox_audioIn.Items.IndexOf(value) < 0)
                    comboBox_audioIn.Items.Add(value);
            }
        }

        public string _AudioWaveOut
        {
            get { return comboBox_audioOut.Text; }
            set
            {
                comboBox_audioOut.Text = value;
                if (comboBox_audioOut.Items.IndexOf(value) < 0)
                    comboBox_audioOut.Items.Add(value);
            }
        }

        public string _SamplesPerSec
        {
            get { return comboBox_samplesPerSec.Text; }
            set
            {
                if (comboBox_samplesPerSec.Items.IndexOf(value) >= 0)
                    comboBox_samplesPerSec.Text = value;
            }
        }

        public string _BitsPerSample
        {
            get { return comboBox_bitsPerSample.Text; }
            set
            {
                if (comboBox_bitsPerSample.Items.IndexOf(value) >= 0)
                    comboBox_bitsPerSample.Text = value;
            }
        }

        public string _Channels
        {
            get { return comboBox_channels.Text; }
            set
            {
                if (comboBox_channels.Items.IndexOf(value) >= 0)
                    comboBox_channels.Text = value;
            }
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (comboBox_audioIn.Items.IndexOf(_AudioWaveIn) < 0)
                comboBox_audioIn.Items.Add(_AudioWaveIn);
            if (comboBox_audioOut.Items.IndexOf(_AudioWaveOut) < 0)
                comboBox_audioOut.Items.Add(_AudioWaveOut);
            Close();
        }
    }
}
