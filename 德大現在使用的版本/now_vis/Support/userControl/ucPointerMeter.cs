using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Support
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    public partial class ucPointerMeter : UserControl
    {
        public ucPointerMeter()
        {
            InitializeComponent();
            LicenseManager.Validate(typeof(ucPointerMeter), this);
            this.Name = "PointerMeter";
            CalcSize();
            timer1 = new Timer();
            timer1.Interval = PeakHoldTime;
            timer1.Enabled = false;
            timer1.Tick += new EventHandler(timer1_Tick);
        }
        //========================================================
        int Value_Current,
            Value_Min = 0,
            Value_Delta = 100,
            Value_Peak,
            Value_Scale = 1;
        int LedCount1 = 6, LedCount2 = 6, LedCount3 = 4;
        Color LedColorOn1 = Color.Lime,
              LedColorOn2 = Color.Yellow,
              LedColorOn3 = Color.Red;
        Color LedColorOff1 = Color.Green,
              LedColorOff2 = Color.Olive,
              LedColorOff3 = Color.Maroon;
        Color Color_Border = Color.DimGray;
        Color Color_DialBack = Color.White;
        Color Color_DialTextLow = Color.Red;
        Color Color_DialTextNeutral = Color.DarkGreen;
        Color Color_DialTextHigh = Color.Black;
        Color Color_DialNeedle = Color.Black;
        Color Color_DialPeak = Color.Red;
        
        int PeakHoldTime = 1000;
        bool ShowPeak = true;
        string MeterText = "VU";
        bool ShowDialText = true;
        bool UseLedLightInAnalog = false;
        bool ShowLedPeakInAnalog = false;
        bool AnalogDialRegionOnly = false;

        float  X0, Y0, Radius_Outer,Radius_Inner;
        double Deg_Low, Deg_Delta;
        double Deg_High
        {
            get { return Deg_Low + Deg_Delta; }
            set
            {
                Deg_Delta = value - Deg_Low;
            }
        }

        protected Timer timer1;
        Size Led = new Size(6, 14);
        int LedSpacing = 3;
        //==============================================================
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            DrawAnalogBorder(g);
            DrawAnalogDial(g);
        }

        protected override void OnResize(EventArgs e)
        {
            if (Width < 42) Width = 42;
            if (Height < 42) Height = 42;
            CalcSize();
            base.OnResize(e);
            this.Invalidate();
        }

        private void CalcSize()
        {
            float w = Width - 32;
            float h = Height;
            float sx0 = 16;
            float sy0 = h / 3;
            float sy1 = 12;
            Circle_GetXYR(sx0, sy0, Width / 2, sy1, Width - sx0, sy0, out X0, out Y0, out Radius_Outer);
            Deg_Low = Math.Acos((X0 - sx0) / Radius_Outer);
            Deg_High = Math.PI - Deg_Low;
            Radius_Inner = Radius_Outer * 0.5f;
            if ((Radius_Outer-Radius_Inner) > Height / 2)
                Radius_Inner = Radius_Outer - Height / 2.0f;
        }

        private double MapStepValue(int step, int s_min, int s_delta, double v_min, double v_delta)
        {
            return v_min + v_delta * (step - s_min) / s_delta;
        }

        private int MapStepValue_Int(int step, int s_min, int s_delta, int v_min, int v_delta)
        {
            return v_min + v_delta * (step - s_min) / s_delta;
        }

        private void Circle_ValueMapXY(float radius,int value,out float dx,out float dy)
        {
            double degree = MapStepValue(value, Value_Min, Value_Delta, Deg_Low, Deg_Delta);
            dx = (float)Math.Cos(degree) * radius;
            dy = (float)Math.Sin(degree) * radius;
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

        private void Draw_ColorBars(Graphics g)
        {
            float radius = Radius_Outer-12;
            //if (ShowTextInDial) radius -= 12;
            float ratio = (float)(radius - Led.Height) / radius;
            double theta = (Led.Width + LedSpace) / radius;// s=r*theta
            int value_step = Value_Delta / (int)(Deg_Delta/theta);
            int value_max = Value_Min + Value_Delta;
            int led_count = Led1Count + Led2Count + Led3Count;
            int v1 = (int) MapStepValue(Led1Count, 0, led_count, Value_Min, Value_Delta);
            int v2 = (int) MapStepValue(Led1Count + Led2Count, 0, led_count, Value_Min, Value_Delta);
            Pen scalePen = null;
            Color color;
            float dx, dy;
            for (int value = Value_Min; value <= value_max; value += value_step)
            {
                if (value >= v2)
                {
                    if (Value_Current >= value)
                        color = Led3ColorOn;
                    else color = Led3ColorOff;
                }
                else if (value >= v1)
                {
                    if (Value_Current >= value)
                        color = Led2ColorOn;
                    else color = Led2ColorOff;
                }
                else
                {
                    if (Value_Current >= value)
                        color = Led1ColorOn;
                    else color = Led1ColorOff;
                }
                scalePen = new Pen(color, Led.Width);
                Circle_ValueMapXY(radius, value, out dx, out dy);
                g.DrawLine(scalePen, X0 - dx, Y0 - dy, X0 - dx * ratio, Y0 - dy * ratio);
                scalePen.Dispose();
            }
        }

        private void Draw_Needle(Graphics g, Color color,int value)
        {
            float radius = Radius_Outer-12;
            //if (ShowDialText) radius -= 12;
            float dx, dy, ratio = Radius_Inner / radius;
            Pen DialPen = new Pen(color, this.Width * 0.01f);//this.Width * 0.01f
            Circle_ValueMapXY(radius, value, out dx, out dy);
            g.DrawLine(DialPen, X0 - dx, Y0 - dy, X0 - dx * ratio, Y0 - dy * ratio);
            DialPen.Dispose();
        }

        private void DrawAnalogDial(Graphics g)
        {
            /*
            int LedCount = LedCount1 + LedCount2 + LedCount3;
            int calcValue = Value_Current - Value_Min;
            int calcPeak = Value_Peak - Value_Min;
            //Add code to draw "LED:s" by color in Dial (Analog and LED)
            if (UseLedLightInAnalog)
            {
                if (FormType == MeterScale.Log10)
                {
                    calcValue = calcValue / (Value_Delta / 10) + 1;
                    calcValue = (int)(Math.Log10((double)calcValue) * LedCount);
                    if (ShowLedPeakInAnalog)
                    {
                        calcPeak = Value_Peak - Value_Min;
                        calcPeak = calcPeak / (Value_Delta / 10) + 1;
                        calcPeak = (int)(Math.Log10((double)calcPeak) * LedCount);
                    }
                }
                else if (FormType == MeterScale.Analog)
                {
                    calcValue = (int)(((double)(calcValue / Value_Delta)) * LedCount + 0.5);
                    if (ShowLedPeakInAnalog)
                    {
                        calcPeak = Value_Peak - Value_Min;
                        calcPeak = (int)(((double)(calcPeak / Value_Delta)) * LedCount + 0.5);
                    }
                }

                Double DegStep = (Deg_High - Deg_Low) / (LedCount - 1);
                double i;
                double SinI, CosI;
                Pen scalePen;
                int lc = 0, x1, y1, x2, y2;
                int LedRadiusStart = (int)(this.Width * 0.6);
                if (!ShowTextInDial) LedRadiusStart = (int)(this.Width * 0.65);

                for (i = Deg_High; i > Deg_Low - DegStep / 2; i = i - DegStep)
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
                calcValue = (int)(Math.Log10((double)calcValue / (Value_Delta / 10) + 1) * Value_Delta);
                if (ShowPeak)
                {
                    calcPeak = Value_Peak - Value_Min;
                    calcPeak = (int)(Math.Log10((double)calcPeak / (Value_Delta / 10) + 1) * Value_Delta);
                }
            }
            else if (FormType == MeterScale.Analog)
            {
                calcValue = Value_Current - Value_Min;
                if (ShowPeak) calcPeak = Value_Peak - Value_Min;
            }
            */
            Draw_Needle(g, Color_DialNeedle, Value_Current);
            if (ShowPeak)
            {
                Draw_Needle(g, Color_DialPeak, Value_Peak);
            }
            
        }

        private void DrawAnalogBorder(Graphics g)
        {
            if (!AnalogDialRegionOnly)
                g.FillRectangle(new SolidBrush(this.BackColor), 0, 0, this.Width, this.Height);
            double SinI, CosI;
            double deg;
            const int STEPS = 10;
            PointF[] curvePoints = new PointF[STEPS * 2 + 1];
            float dx, dy, ratio = Radius_Inner / Radius_Outer;           
            for (int index = 0; index < STEPS; index++)
            {
                deg = MapStepValue(index, 0, STEPS - 1, Deg_Low, Deg_High - Deg_Low);
                SinI = Math.Sin(deg);
                CosI = Math.Cos(deg);
                dx = (float)CosI * Radius_Outer;
                dy = (float)SinI * Radius_Outer;
                curvePoints[index] = new PointF((float)(X0-dx), (float)(Y0-dy));
                curvePoints[STEPS * 2 - 1 - index] = new PointF((float)(X0 - dx * ratio), (float)(Y0 - dy * ratio));
            }
            curvePoints[STEPS * 2] = curvePoints[0];
            /*
            System.Drawing.Drawing2D.GraphicsPath dialPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (AnalogDialRegionOnly)
                 dialPath.AddPolygon(curvePoints);
            else dialPath.AddRectangle(new Rectangle(0, 0, this.Width, this.Height));
            this.Region = new System.Drawing.Region(dialPath);
            */
            g.FillPolygon(new SolidBrush(Color_DialBack), curvePoints);
            //--------------------------------------------------------------
            //if (!UseLedLightInAnalog)

                Draw_ColorBars(g);
            //---------------------------------------
            // Meter Text
            float MeterFontSize = this.Font.SizeInPoints;
            if (this.Width > 0) MeterFontSize = MeterFontSize * (float)(this.Width / 100f);
            if (MeterFontSize < 4) MeterFontSize = 4;
            else if (MeterFontSize > 72) MeterFontSize = 72;
            Font MeterFont = new Font(this.Font.FontFamily, MeterFontSize);
            SizeF text_size = g.MeasureString(this.MeterText, MeterFont);
            dy = Y0 - Radius_Inner;
            g.DrawString(this.MeterText, MeterFont, new SolidBrush(this.ForeColor),
                this.Width / 2 - text_size.Width / 2, dy);
            //---------------------------------------------
           if (ShowDialText)
            {
                
                MeterFontSize = 10;
                Font dtfont = new Font(this.Font.FontFamily, MeterFontSize);
                StringFormat dtformat = new StringFormat();
                dtformat.Alignment = StringAlignment.Center;
                dtformat.LineAlignment = StringAlignment.Center;
                double v_delta = Maximum - Minimum, dial_value;
                const int dialtext_len = 4;
                string dialtext;
                int value;
                for (int index = 0; index <= dialtext_len; index++)
                {
                    dial_value = MapStepValue(index, 0, dialtext_len, Minimum, v_delta);
                    dialtext = dial_value.ToString("F0");
                    Brush dtColor = new SolidBrush(Color_DialTextHigh);
                    try
                    {
                        if (dial_value < 0) dtColor = new SolidBrush(Color_DialTextLow);
                        else if (dial_value == 0) dtColor = new SolidBrush(Color_DialTextNeutral);
                    }
                    catch
                    {
                        dtColor = new SolidBrush(Color_DialTextHigh);
                    }
                    value = MapStepValue_Int(index, 0, dialtext_len, Value_Min, Value_Delta);
                    Circle_ValueMapXY(Radius_Outer, value, out dx, out dy);
                    g.DrawString(dialtext, dtfont, dtColor, X0 - dx, Y0 - dy, dtformat);
                }
            }
        }

        

        //--------------------------------------------------------------
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

        public string _MeterText
        {
            get
            {
                return MeterText;
            }
            set
            {
                if (value.Length < 11)
                {
                    MeterText = value;
                    this.Invalidate();
                }
            }
        }


        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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


        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        public int _放大倍率數
        {
            get { return Value_Scale; }
        }

        int digits = 0;
        public int _小數點位數
        {
            get
            {
                return digits;
            }
            set
            {
                if (value < 0) value = 0;
                else if (value > 5) value = 5;
                digits = value;
                int mul = 1;
                while (--value >= 0) mul *= 10;
                Value_Scale = mul;
                this.Invalidate();
            }
        }

        public double Maximum
        {
            get
            {
                return (double)(Value_Min+Value_Delta) / Value_Scale;
            }
            set
            {
                int new_value = (int)(value * Value_Scale);
                if (new_value < (Led1Count + Led2Count + Led3Count))
                    new_value = (Led1Count + Led2Count + Led3Count);
                else if (new_value < Value_Min) new_value = Value_Min;
                Value_Delta = new_value - Value_Min;
                this.Invalidate();
            }
        }

        public double Minimum
        {
            get
            {
                return (double)Value_Min / Value_Scale;
            }
            set
            {
                int max = Value_Min + Value_Delta;
                int new_value = (int)(value * Value_Scale);
                if (new_value > max)
                    new_value = max;
                Value_Min = new_value;
                Value_Delta = max - Value_Min;
                this.Invalidate();
            }
        }

        public double Value
        {
            get
            {
                return (double)Value_Current / Value_Scale;
            }

            set
            {
                int new_value = (int)(value * Value_Scale);
                if (new_value != Value_Current)
                {
                    int max = Value_Min + Value_Delta;
                    if (new_value < Value_Min) Value_Current = Value_Min;
                    else if (new_value > max) Value_Current = max;
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

        [Category("Pointer Meter")]
        [Description("How many mS to hold peak indicator (50 to 10000ms)")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter")]
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

        [Category("Pointer Meter - Colors")]
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

        [Category("Pointer Meter - Colors")]
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

        [Category("Pointer Meter - Colors")]
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

        [Category("Pointer Meter - Colors")]
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

        [Category("Pointer Meter - Colors")]
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

        [Category("Pointer Meter - Colors")]
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Value_Peak = Value_Current;
            this.Invalidate();
            timer1.Start();
        }

        private MeterScale FormType = MeterScale.Analog;
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
    }
}
