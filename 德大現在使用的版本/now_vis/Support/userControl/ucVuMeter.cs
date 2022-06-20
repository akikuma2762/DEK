using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Support
{
    public enum MeterScale { Analog, Log10 };
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucVuMeter : UserControl
    {
        int Value_Current,
            Value_Min = 0,
            Value_Max = 100,
            Value_Peak,
            Value_Scale = 1;
        int LedCount1 = 6, LedCount2 = 6, LedCount3 = 4;
        Color LedColorOn1 = Color.LimeGreen, 
              LedColorOn2 = Color.Yellow, 
              LedColorOn3 = Color.Red;
        Color LedColorOff1 = Color.DarkGreen, 
              LedColorOff2 = Color.Olive, 
              LedColorOff3 = Color.Maroon;
        Color Color_Border = Color.DimGray;
        Color Color_DialBack = Color.White;
        Color Color_DialTextLow = Color.Red;
        Color Color_DialTextNeutral = Color.DarkGreen;
        Color Color_DialTextHigh = Color.Black;
        Color Color_DialNeedle = Color.Black;
        Color Color_DialPeak = Color.Red;
        int  PeakHoldTime = 1000;
        bool ShowPeak = true;
        bool Vertical = false;
        bool MeterAnalog = false;
        string MeterText = "VU";
        string[] DialText = { "-40", "-20", "-10", "-5", "0", "+6" };
        bool ShowDialText = false;
        bool AnalogDialRegionOnly = false;
        bool UseLedLightInAnalog = false;
        bool ShowLedPeakInAnalog = false;

        double DegLow = Math.PI * 0.8, 
               DegHigh = Math.PI * 1.2;

        protected Timer timer1;
        Size Led = new Size(6, 14);
        int LedSpacing = 3;
        //==============================================================
       
        public ucVuMeter()
        {
            LicenseManager.Validate(typeof(ucVuMeter), this);
            this.Name = "VuMeter";
            CalcSize();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            timer1 = new Timer();
            timer1.Interval = PeakHoldTime;
            timer1.Enabled = false;
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool UseLedLight
        {
            get
            {
                return UseLedLightInAnalog;
            }
            set
            {
                UseLedLightInAnalog = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool ShowLedPeak
        {
            get
            {
                return ShowLedPeakInAnalog;
            }
            set
            {
                ShowLedPeakInAnalog = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Analog meter layout")]
        public bool AnalogMeter
        {
            get
            {
                return MeterAnalog;
            }
            set
            {
                if (value & !MeterAnalog) this.Size = new Size(100, 80);
                MeterAnalog = value;
                CalcSize();
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Text (max 10 letters)")]
        public string VuText
        {
            get
            {
                return MeterText;
            }
            set
            {
                if (value.Length < 11) MeterText = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Text in dial")]
        public string[] TextInDial
        {
            get
            {
                return DialText;
            }
            set
            {
                DialText = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Show textvalues in dial")]
        public bool ShowTextInDial
        {
            get
            {
                return ShowDialText;
            }
            set
            {
                ShowDialText = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Only show the Analog Dial Panel (Sets BackColor to Color_DialBack so antialias won't look bad)")]
        public bool ShowDialOnly
        {
            get
            {
                return AnalogDialRegionOnly;
            }
            set
            {
                AnalogDialRegionOnly = value;
                if (AnalogDialRegionOnly) this.BackColor = Color_DialBack;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on dial background")]
        public Color DialBackground
        {
            get
            {
                return Color_DialBack;
            }
            set
            {
                Color_DialBack = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on Value < 0")]
        public Color DialTextNegative
        {
            get
            {
                return Color_DialTextLow;
            }
            set
            {
                Color_DialTextLow = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on Value = 0")]
        public Color DialTextZero
        {
            get
            {
                return Color_DialTextNeutral;
            }
            set
            {
                Color_DialTextNeutral = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on Value > 0")]
        public Color DialTextPositive
        {
            get
            {
                return Color_DialTextHigh;
            }
            set
            {
                Color_DialTextHigh = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on needle")]
        public Color NeedleColor
        {
            get
            {
                return Color_DialNeedle;
            }
            set
            {
                Color_DialNeedle = value;
                this.Invalidate();
            }
        }

        [Category("Analog Meter")]
        [Description("Color on Peak needle")]
        public Color PeakNeedleColor
        {
            get
            {
                return Color_DialPeak;
            }
            set
            {
                Color_DialPeak = value;
                this.Invalidate();
            }
        }


        private MeterScale FormType = MeterScale.Analog;

        [Category("VU Meter")]
        [Description("Display value in analog or logarithmic scale")]
        public MeterScale MeterScale
        {
            get
            {
                return FormType;
            }
            set
            {
                FormType = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Led size (1 to 72 pixels)")]
        public Size LedSize
        {
            get
            {
                return Led;
            }
            set
            {
                if (value.Height < 1) Led.Height = 1;
                else if (value.Height > 72) Led.Height = 72;
                else Led.Height = value.Height;

                if (value.Width < 1) Led.Width = 1;
                else if (value.Width > 72) Led.Width = 72;
                else Led.Width = value.Width;

                CalcSize();
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Led spacing (0 to 72 pixels)")]
        public int LedSpace
        {
            get
            {
                return LedSpacing;
            }
            set
            {
                if (value < 0) LedSpacing = 0;
                else if (value > 72) LedSpacing = 72;
                else LedSpacing = value;
                CalcSize();
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Led bar is vertical")]
        public bool VerticalBar
        {
            get
            {
                return Vertical;
            }
            set
            {
                Vertical = value;
                CalcSize();
                this.Invalidate();
            }
        }

        public int _放大倍率數
        {
            get { return Value_Scale; }
        }

        public int _小數點位數
        {
            get
            {
                return Value_Scale / 10;
            }
            set
            {
                if (value < 0) value = 0;
                else if (value > 5) value = 5;
                Value_Scale = 1;
                while (--value >= 0) Value_Scale *= 10;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Max value from total LedCount to 65535")]
        public double Maximum
        {
            get
            {
                return (double) Value_Max/Value_Scale;
            }
            set
            {
                int new_value = (int)(value * Value_Scale);
                if (new_value < (Led1Count + Led2Count + Led3Count))
                    Value_Max = (Led1Count + Led2Count + Led3Count);
                else if (new_value < Value_Min) Value_Max = Value_Min;
                else Value_Max = new_value;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Minimum value ~ Maximum")]
        public double Minimum
        {
            get
            {
                return (double) Value_Min/Value_Scale;
            }
            set
            {
                int new_value = (int)(value * Value_Scale);
                if (new_value > Value_Max) Value_Min = Value_Max;
                else Value_Min = new_value;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("The level shown (between Minimum and Maximum)")]
        public double Value
        {
            get
            {
                return (double) Value_Current / Value_Scale;
            }

            set
            {
                int new_value = (int)(value * Value_Scale);
                if (new_value != Value_Current)
                {
                    if (new_value < Value_Min) Value_Current = Value_Min;
                    else if (new_value > Value_Max) Value_Current = Value_Max;
                    else Value_Current = new_value;

                    if ((Value_Current > Value_Peak) & (ShowPeak | ShowLedPeakInAnalog))
                    {
                        Value_Peak = Value_Current;
                        timer1.Stop();
                        timer1.Start();
                    }
                    this.Invalidate();
                }
            }
        }

        [Category("VU Meter")]
        [Description("How many mS to hold peak indicator (50 to 10000mS)")]
        public int Peakms
        {
            get
            {
                return PeakHoldTime;
            }
            set
            {
                if (value < 50) PeakHoldTime = 50;
                else if (value > 10000) PeakHoldTime = 10000;
                else PeakHoldTime = value;
                timer1.Interval = PeakHoldTime;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Use peak indicator")]
        public bool PeakHold
        {
            get
            {
                return ShowPeak;
            }
            set
            {
                ShowPeak = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Number of Leds in first area (0 to 32, default 6) Total Led count must be at least 1")]
        public int Led1Count
        {
            get
            {
                return LedCount1;
            }

            set
            {
                if (value < 0) LedCount1 = 0;
                else if (value > 32) LedCount1 = 32;
                else LedCount1 = value;
                if ((LedCount1 + LedCount2 + LedCount3) < 1) LedCount1 = 1;
                CalcSize();
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Number of Leds in middle area (0 to 32, default 6) Total Led count must be at least 1")]
        public int Led2Count
        {
            get
            {
                return LedCount2;
            }

            set
            {
                if (value < 0) LedCount2 = 0;
                else if (value > 32) LedCount2 = 32;
                else LedCount2 = value;
                if ((LedCount1 + LedCount2 + LedCount3) < 1) LedCount2 = 1;
                CalcSize();
                this.Invalidate();
            }
        }

        [Category("VU Meter")]
        [Description("Number of Leds in last area (0 to 32, default 4) Total Led count must be at least 1")]
        public int Led3Count
        {
            get
            {
                return LedCount3;
            }

            set
            {
                if (value < 0) LedCount3 = 0;
                else if (value > 32) LedCount3 = 32;
                else LedCount3 = value;
                if ((LedCount1 + LedCount2 + LedCount3) < 1) LedCount3 = 1;
                CalcSize();
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in first area (Led on)")]
        public Color Led1ColorOn
        {
            get
            {
                return LedColorOn1;
            }
            set
            {
                LedColorOn1 = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in middle area (Led on)")]
        public Color Led2ColorOn
        {
            get
            {
                return LedColorOn2;
            }
            set
            {
                LedColorOn2 = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in last area (Led on)")]
        public Color Led3ColorOn
        {
            get
            {
                return LedColorOn3;
            }
            set
            {
                LedColorOn3 = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in first area (Led off)")]
        public Color Led1ColorOff
        {
            get
            {
                return LedColorOff1;
            }
            set
            {
                LedColorOff1 = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in middle area (Led off)")]
        public Color Led2ColorOff
        {
            get
            {
                return LedColorOff2;
            }
            set
            {
                LedColorOff2 = value;
                this.Invalidate();
            }
        }

        [Category("VU Meter - Colors")]
        [Description("Color of Leds in last area (Led off)")]
        public Color Led3ColorOff
        {
            get
            {
                return LedColorOff3;
            }
            set
            {
                LedColorOff3 = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (MeterAnalog)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                DrawAnalogBorder(g);
                DrawAnalogDial(g);
            }
            else
            {
                Graphics g = e.Graphics;
                DrawBorder(g);
                DrawLeds(g);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (MeterAnalog)
            {
                base.OnResize(e);
            }
            CalcSize();
            base.OnResize(e);
            this.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Value_Peak = Value_Current;
            this.Invalidate();
            timer1.Start();
        }

        private void CalcSize()
        {
            if (MeterAnalog)
            {
                //this.Size = new Size(this.Width, (int)(this.Width * 0.8));
            }
            else if (Vertical)
            {
                this.Size = new Size(Led.Width + LedSpacing * 2, (LedCount1 + LedCount2 + LedCount3) * (Led.Height + LedSpacing) + LedSpacing);
            }
            else
            {
                this.Size = new Size((LedCount1 + LedCount2 + LedCount3) * (Led.Width + LedSpacing) + LedSpacing, Led.Height + LedSpacing * 2);
            }
        }

        private bool Circle_GetXYR(float x1, float y1, float x2, float y2, float x3, float y3,
                                out float x0, out float y0, out float radius)
        {
#if false
            (x−x0)^2+(y−y0)^2=r^2
            ----------------------------
            (x-x1)^2+(y-y1)^2=r^2  --- 1
            (x-x2)^2+(y-y2)^2=r^2  --- 2
            (x-x3)^2+(y-y3)^2=r^2  --- 3
            1 - 2 ---> (x1−x2)x0+(y1−y2)y0=((x1^2−x2^2)−(y2^2−y1^2))/2
            1 - 3 ---> (x1−x3)x0+(y1−y3)y0=((x1^2−x3^2)−(y3^2−y1^2))/2
            假設:
            a=x1−x2
            b=y1−y2
            c=x1−x3
            d=y1−y3
            ------------
            a*x0+b*y0=e
            c*x0+d*y0=f
            行列式 ==> det=b*c-a*d;
            //有唯一解的條件是係數行列式不為0 (即兩條線不平行)
            a/b ≠ c/d
            e=[(x1^2−x2^2)−(y2^2−y1^2)]/2
            f=[(x1^2−x3^2)−(y3^2−y1^2)]/2
            圓心(x0,y0):
            x0=−(de−bf)/(bc−ad)
            y0=−(af−ce)/(bc−ad)

#endif
            float a = x1 - x2;
            float b = y1 - y2;
            float c = x1 - x3;
            float d = y1 - y3;
            float x1_2 = x1 * x1;
            float y1_2 = y1 * y1;
            float e = ((x1_2 - x2 * x2) - (y2 * y2 - y1_2)) / 2.0f;
            float f = ((x1_2 - x3 * x3) - (y3 * y3 - y1_2)) / 2.0f;
            float det = b * c - a * d;
            //有唯一解的條件是係數行列式不為0
            if (Math.Abs(det) < 1e-5)
            {
                x0 = y0 = radius = 0f;
                return false;//無解
            }
            //x0 = −(de−bf) / (bc−ad)  ==> x0=(bf-de)/det
            //y0 = −(af−ce) / (bc−ad)  ==> y0=(ce-af)/det
            x0 = (b * f - d * e) / det;
            y0 = (c * e - a * f) / det;
            //(x−x0)^2+(y−y0)^2=r^2
            e = x1 - x0;
            f = y1 - y0;
            radius = (float)Math.Sqrt(e * e + f * f);
            return true;
        }

        private void DrawAnalogDial(Graphics g)
        {
            int delta = Value_Max - Value_Min;
            int LedCount = LedCount1 + LedCount2 + LedCount3;
            int calcValue = Value_Current - Value_Min;
            int calcPeak = Value_Peak - Value_Min;
            //Add code to draw "LED:s" by color in Dial (Analog and LED)
            if (UseLedLightInAnalog)
            {
                if (FormType == MeterScale.Log10)
                {
                    calcValue = calcValue / (delta / 10) + 1;
                    calcValue = (int)(Math.Log10((double)calcValue) * LedCount);
                    if (ShowLedPeakInAnalog)
                    {
                        calcPeak = Value_Peak - Value_Min;
                        calcPeak = calcPeak / (delta / 10) + 1;
                        calcPeak = (int)(Math.Log10((double)calcPeak) * LedCount);
                    }
                }
                else if (FormType == MeterScale.Analog)
                {
                    calcValue = (int)(((double)(calcValue / delta)) * LedCount + 0.5);
                    if (ShowLedPeakInAnalog)
                    {
                        calcPeak = Value_Peak - Value_Min;
                        calcPeak = (int)(((double)(calcPeak / delta)) * LedCount + 0.5);
                    }
                }

                Double DegStep = (DegHigh - DegLow) / (LedCount - 1);
                double i;
                double SinI, CosI;
                Pen scalePen;
                int lc = 0, x1, y1, x2, y2;
                int LedRadiusStart = (int)(this.Width * 0.6);
                if (!ShowTextInDial) LedRadiusStart = (int)(this.Width * 0.65);               

                for (i = DegHigh; i > DegLow - DegStep / 2; i = i - DegStep)
                {
                    if ((lc < calcValue) | (((lc + 1) == calcPeak) & ShowLedPeakInAnalog))
                    {
                        scalePen = new Pen(Led3ColorOn, Led.Width);
                        if (lc < LedCount1 + LedCount2) scalePen = new Pen(Led2ColorOn, Led.Width);
                        if (lc < LedCount1) scalePen = new Pen(Led1ColorOn, Led.Width);
                    }
                    else
                    {
                        scalePen = new Pen(Led3ColorOff, Led.Width);
                        if (lc < LedCount1 + LedCount2) scalePen = new Pen(Led2ColorOff, Led.Width);
                        if (lc < LedCount1) scalePen = new Pen(Led1ColorOff, Led.Width);
                    }

                    lc++;
                    SinI = Math.Sin(i);
                    CosI = Math.Cos(i);
                    x1 = (int)((LedRadiusStart - Led.Height) * SinI + this.Width / 2);
                    y1 = (int)((LedRadiusStart - Led.Height) * CosI + this.Height * 0.9);
                    x2 = (int)(LedRadiusStart * SinI + this.Width / 2);
                    y2 = (int)(LedRadiusStart * CosI + this.Height * 0.9);
                    g.DrawLine(scalePen, x1, y1, x2, y2);
                }
            }
            else if (FormType == MeterScale.Log10)
            {
                calcValue = (int)(Math.Log10((double)calcValue / (delta / 10) + 1) * delta);
                if (ShowPeak)
                {
                    calcPeak = Value_Peak - Value_Min;
                    calcPeak = (int)(Math.Log10((double)calcPeak / (delta / 10) + 1) * delta);
                }
            }
            else if (FormType == MeterScale.Analog)
            {
                calcValue = Value_Current - Value_Min;
                if (ShowPeak) calcPeak = Value_Peak - Value_Min;
            }
            int DialRadiusLow = (int)(this.Width * 0.3f), DialRadiusHigh = (int)(this.Width * 0.65f);

            //---------------------------------------------------
#if false
            float x0, y0;
            float r,w,h;
            w = Width;
            h = Height;
            Circle_GetXYR(0, h / 2, w / 2, h/8, w, h / 2, out x0, out y0, out r);
            g.DrawLine(Pens.Blue, 0, h / 2, w / 2, h/8);
            g.DrawLine(Pens.Blue, w / 2, h/8, w, h / 2);
            g.DrawEllipse(Pens.Blue, x0 - r, y0 - r, r*2, r*2);
#endif
            //--------------------------------------------------------

            Pen DialPen = new Pen(Color_DialNeedle, this.Width * 0.01f);
            double DialPos = DegHigh - (((double)calcValue / delta) * (DegHigh - DegLow));
            if (DialPos<DegLow) DialPos = DegLow;
            else if (DialPos>DegHigh) DialPos = DegHigh;
            Double SinD = Math.Sin(DialPos), CosD = Math.Cos(DialPos);

            g.DrawLine(DialPen, (int)(DialRadiusLow * SinD + this.Width * 0.5),
                (int)(DialRadiusLow * CosD + this.Height * 0.9),
                (int)(DialRadiusHigh * SinD + this.Width * 0.5),
                (int)(DialRadiusHigh * CosD + this.Height * 0.9));

            if (ShowPeak)
            {
                Pen PeakPen = new Pen(Color_DialPeak, this.Width * 0.01f);
                DialPos = DegHigh - (((double)calcPeak / delta) * (DegHigh - DegLow));
                if (DialPos < DegLow) DialPos = DegLow;
                else if (DialPos > DegHigh) DialPos = DegHigh;
                Double SinP = Math.Sin(DialPos), CosP = Math.Cos(DialPos);
                g.DrawLine(PeakPen, (int)(DialRadiusLow * SinP + this.Width * 0.5),
                    (int)(DialRadiusLow * CosP + this.Height * 0.9),
                    (int)(DialRadiusHigh * SinP + this.Width * 0.5),
                    (int)(DialRadiusHigh * CosP + this.Height * 0.9));
            }
            DialPen.Dispose();
        }

        private void DrawLeds(Graphics g)
        {
            int LedCount = LedCount1 + LedCount2 + LedCount3;
            int delta = Value_Max - Value_Min;
            int calcValue = Value_Current - Value_Min;
            int calcPeak = Value_Peak - Value_Min;
            if (FormType == MeterScale.Log10)
            {
                calcValue = (int)(Math.Log10((double)calcValue / (delta / 10) + 1) * LedCount);
                if (ShowPeak) calcPeak = (int)(Math.Log10((double)calcPeak / (delta / 10) + 1) * LedCount);
            }

            if (FormType == MeterScale.Analog)
            {
                calcValue = (int)(((double)calcValue / delta) * LedCount + 0.5);
                if (ShowPeak) calcPeak = (int)(((double)calcPeak / delta) * LedCount + 0.5);
            }


            for (int i = 0; i < LedCount; i++)
            {

                if (Vertical)
                {
                    Rectangle current = new Rectangle(this.ClientRectangle.X + LedSpacing,
                        this.ClientRectangle.Height - ((i + 1) * (Led.Height + LedSpacing)),
                        Led.Width, Led.Height);

                    if ((i < calcValue) | (((i + 1) == calcPeak) & ShowPeak))
                    {
                        if (i < LedCount1)
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn1), current);
                        }
                        else if (i < (LedCount1 + LedCount2))
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn3), current);
                        }
                    }
                    else
                    {
                        if (i < LedCount1)
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff1), current);
                        }
                        else if (i < (LedCount1 + LedCount2))
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff3), current);
                        }
                    }

                }
                else
                {
                    Rectangle current = new Rectangle(this.ClientRectangle.X + (i * (Led.Width + LedSpacing)) + LedSpacing,
                        this.ClientRectangle.Y + LedSpacing, Led.Width, Led.Height);

                    if ((i) < calcValue | (((i + 1) == calcPeak) & ShowPeak))
                    {
                        if (i < LedCount1)
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn1), current);
                        }
                        else if (i < (LedCount1 + LedCount2))
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(LedColorOn3), current);
                        }
                    }
                    else
                    {
                        if (i < LedCount1)
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff1), current);
                        }
                        else if (i < (LedCount1 + LedCount2))
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff2), current);
                        }
                        else
                        {
                            g.FillRectangle(new SolidBrush(LedColorOff3), current);
                        }
                    }

                }

            }
        }

        private void DrawBorder(Graphics g)
        {
            Rectangle Border = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            g.FillRectangle(new SolidBrush(this.BackColor), Border);
        }

        private void DrawAnalogBorder(Graphics g)
        {
            if (!AnalogDialRegionOnly)
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);

            double DegStep = (DegHigh * 1.05 - DegLow / 1.05) / 19;
            double i = DegHigh * 1.05;
            double SinI, CosI;

            PointF[] curvePoints = new PointF[40];
            for (int cp = 0; cp < 20; cp++)
            {
                i = i - DegStep;
                SinI = Math.Sin(i);
                CosI = Math.Cos(i);
                curvePoints[cp] = new PointF((float)(SinI * this.Width * 0.7 + this.Width / 2), (float)(CosI * this.Width * 0.7 + this.Height * 0.9));
                curvePoints[38 - cp] = new PointF((float)(SinI * this.Width * 0.3 + this.Width / 2), (float)(CosI * this.Width * 0.3 + this.Height * 0.9));
            }
            curvePoints[39] = curvePoints[0];
            System.Drawing.Drawing2D.GraphicsPath dialPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (AnalogDialRegionOnly)
                 dialPath.AddPolygon(curvePoints);
            else dialPath.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
            this.Region = new System.Drawing.Region(dialPath);
            g.FillPolygon(new SolidBrush(Color_DialBack), curvePoints);

            // Test moving this block
            if (!UseLedLightInAnalog)
            {
                DegStep = (DegHigh - DegLow) / (LedCount1 + LedCount2 + LedCount3 - 1);
                int lc = 0;
                int LedRadiusStart = (int)(this.Width * 0.6);
                if (!ShowTextInDial) LedRadiusStart = (int)(this.Width * 0.65);
                for (i = DegHigh; i > DegLow - DegStep / 2; i = i - DegStep)
                {
                    //Graphics scale = g.Graphics;
                    Pen scalePen = new Pen(Led3ColorOn, Led.Width);
                    if (lc < LedCount1 + LedCount2) scalePen = new Pen(Led2ColorOn, Led.Width);
                    if (lc < LedCount1) scalePen = new Pen(Led1ColorOn, Led.Width);
                    lc++;
                    SinI = Math.Sin(i);
                    CosI = Math.Cos(i);
                    g.DrawLine(scalePen, (int)((LedRadiusStart - Led.Height) * SinI + this.Width / 2),
                        (int)((LedRadiusStart - Led.Height) * CosI + this.Height * 0.9),
                        (int)(LedRadiusStart * SinI + this.Width / 2), (int)(LedRadiusStart * CosI + this.Height * 0.9));
                    scalePen.Dispose();
                }
            }
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            float MeterFontSize = this.Font.SizeInPoints;
            if (this.Width > 0) MeterFontSize = MeterFontSize * (float)(this.Width / 100f);
            if (MeterFontSize < 4) MeterFontSize = 4;
            if (MeterFontSize > 72) MeterFontSize = 72;
            Font MeterFont = new Font(this.Font.FontFamily, MeterFontSize);
            g.DrawString(this.MeterText, MeterFont, new SolidBrush(this.ForeColor), this.Width / 2, this.Height * 0.43f, format);

            if (ShowDialText)
            {
                double DialTextStep = (DegHigh - DegLow) / (DialText.Length - 1);
                int dt = 0;
                MeterFontSize = MeterFontSize * 0.1f;
                int TextRadiusStart = (int)(this.Width * 0.64);
                for (i = DegHigh; i > DegLow - DialTextStep / 2; i = i - DialTextStep)
                {
                    //Graphics scale = g.Graphics;
                    Brush dtColor = new SolidBrush(Color_DialTextHigh);
                    StringFormat dtformat = new StringFormat();
                    dtformat.Alignment = StringAlignment.Center;
                    dtformat.LineAlignment = StringAlignment.Center;
                    try
                    {
                        if (int.Parse(DialText[dt]) < 0) dtColor = new SolidBrush(Color_DialTextLow);
                        if (int.Parse(DialText[dt]) == 0) dtColor = new SolidBrush(Color_DialTextNeutral);
                    }
                    catch
                    {
                        dtColor = new SolidBrush(Color_DialTextHigh);
                    }
                    Font dtfont = new Font(this.Font.FontFamily, MeterFontSize);
                    SinI = Math.Sin(i);
                    CosI = Math.Cos(i);
                    g.DrawString(DialText[dt++], dtfont, dtColor, (int)(TextRadiusStart * SinI + this.Width / 2), (int)(TextRadiusStart * CosI + this.Height * 0.9), dtformat);

                }
            }


        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
