using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace Support.userControl
{
#if DEBUG != true
    [LicenseProvider(typeof(dekLicenseProvider))]
#endif
    [Designer(typeof(ucPanel3DControlDesigner))]
    public partial class ucPanel3D : UserControl
    {
        Color border_topleft,
            border_bottomdown;

        public Color _Border_ColorTopLeft
        {
            get { return border_topleft; }
            set
            {
                border_topleft = value;
                Invalidate();
            }
        }
        public Color _Border_ColorBottomRight
        {
            get { return border_bottomdown; }
            set
            {
                border_bottomdown = value;
                Invalidate();
            }
        }

        /*
         * Margin.All=0
           (0,0)          (Width,0)
            +--------------+
            | rect_inner   |
            | [0]+----+[1] |
            |    |    |    |    
            | [3]+----+[2] |
            |              |
            +--------------+
          (0,Height)   (Width,Height)
         */
        int border_line_width;
        public int _Border_LineWidth
        {
            get { return border_line_width; }
            set
            {
                border_line_width = value;
                this.Padding = new Padding(value);
                Invalidate();
            }
        }

        string text = "";
        public string _Text
        {
            get { return text; }
            set
            {
                text = value;
                Invalidate();
            }
        }

        Orientation text_orient;
        public Orientation _TextOrientation
        {
            get { return text_orient; }
            set { text_orient = value;Invalidate(); }
        }
        public Color _TextColor
        {
            get { return _Panel.ForeColor; }
            set { _Panel.ForeColor = value;Invalidate(); }
        }
        public Color _BackColor
        {
            get { return _Panel.BackColor; }
            set { _Panel.BackColor = value; Invalidate(); }
        }
        public Font _Font
        {
            get { return _Panel.Font; }
            set { _Panel.Font = value; Invalidate(); }
        }



        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel _Panel
        {
            get { return panel_main; }
        }

        public ucPanel3D()
        {
            InitializeComponent();
            DoubleBuffered = true;
            LicenseManager.Validate(typeof(ucPanel3D), this);
            _Border_ColorTopLeft = Color.LightGray;
            _Border_ColorBottomRight = Color.Gray;
            _Border_LineWidth = 3;
            _Text = "";
            _TextColor = Color.Black;
            _TextOrientation = Orientation.Horizontal;

        }

        Point[] triangle = new Point[3];
        private void ReDraw(Graphics gr, int width, int height)
        {
            if (_Border_LineWidth < 1) return;
            int dw = _Border_LineWidth / 2;
            int dw2 = _Border_LineWidth * 2;
            int left = 0, top = 0,
                right = width,
                bottom = height;
            //----------------------------------------------------
            Pen pen = new Pen(_Border_ColorTopLeft, _Border_LineWidth);
            gr.DrawLine(pen, left, top + dw, right, top + dw);
            gr.DrawLine(pen, left + dw, top, left + dw, bottom);
            //-----------------------------------------
            pen.Color = _Border_ColorBottomRight;
            gr.DrawLine(pen, right - dw, top + _Border_LineWidth, right - dw, bottom);
            gr.DrawLine(pen, right, bottom - dw, left + _Border_LineWidth, bottom - dw);
            //------------------------------------------------------------
            SolidBrush brush = new SolidBrush(_Border_ColorBottomRight);
            triangle[0].X = right;
            triangle[0].Y = top;
            triangle[1].X = right - _Border_LineWidth;
            triangle[1].Y = top + _Border_LineWidth;
            triangle[2].X = right;
            triangle[2].Y = triangle[1].Y;
            gr.FillPolygon(brush, triangle);
            triangle[0].X = left;
            triangle[0].Y = bottom;
            triangle[1].X = left + _Border_LineWidth;
            triangle[1].Y = bottom - _Border_LineWidth;
            triangle[2].X = triangle[1].X;
            triangle[2].Y = triangle[0].Y;
            gr.FillPolygon(brush, triangle);
        }

        private void panel_main_Paint(object sender, PaintEventArgs e)
        {
            if (_Text == null || _Text == "") return;
            Graphics gr = Graphics.FromHwnd(_Panel.Handle);
            SolidBrush brush = new SolidBrush(_TextColor);
            SizeF size;
            if (_TextOrientation == Orientation.Horizontal)
            {
                size = gr.MeasureString(_Text, _Panel.Font);
                float x = _Panel.Width / 2 - size.Width / 2,
                    y = _Panel.Height / 2 - size.Height / 2;
                gr.DrawString(_Text, _Font, brush, x, y);
            }
            else
            {
                string text;
                float x, y, height = 0;
                foreach (char ch in _Text)
                {
                    size = gr.MeasureString(ch.ToString(), _Panel.Font);
                    height += size.Height;
                }
                x = _Panel.Width / 2;
                y= _Panel.Height / 2 - height / 2;
                foreach (char ch in _Text)
                {
                    text = ch.ToString();
                    size = gr.MeasureString(text, _Panel.Font);
                    gr.DrawString(text, _Font, brush,
                        x - size.Width / 2, y);
                    y += size.Height;
                }
            }
        }

        private void ucPanel3D_Paint(object sender, PaintEventArgs e)
        {
            ReDraw(e.Graphics, Width, Height);
        }
        
    }
    public class ucPanel3DControlDesigner : ParentControlDesigner
    {
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            if (this.Control is ucPanel3D)
            {
                this.EnableDesignMode((
                   (ucPanel3D)this.Control)._Panel, "_Panel");
            }
        }
    }
}
