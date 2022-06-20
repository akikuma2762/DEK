using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Support
{
    public class CtrlUtils
    {
        //------------------------------------------------------------
        delegate void PBXSetImage(PictureBox pbx, Bitmap bmp);
        public static void PictureBox_SetImage(PictureBox pbx, Bitmap bmp)
        {
            if (pbx.InvokeRequired)
            {
                PBXSetImage handler = new PBXSetImage(PictureBox_SetImage);
                pbx.Invoke(handler, pbx, bmp);
            }
            else pbx.Image = bmp;
        }
        //------------------------------------------------------------
        public static bool setControlText(Control ctrl, string value)
        {
            if (ctrl == null) return false;
            else if (ctrl is TextBox) ((TextBox)ctrl).Text = value;
            else if (ctrl is ComboBox) ((ComboBox)ctrl).Text = value;
            else if (ctrl is NumericUpDown) ((NumericUpDown)ctrl).Text = DataTableUtils.toString(value);
            else if (ctrl is DateTimePicker) ((DateTimePicker)ctrl).Text = value;
            else if (ctrl is Label) ((Label)ctrl).Text = value;
            else return false;
            return true;
        }

        public static string getControlText(Control ctrl)
        {
            string value = "";
            if (ctrl != null)
            {
                if (ctrl is TextBox) value = ((TextBox)ctrl).Text;
                else if (ctrl is ComboBox) value = ((ComboBox)ctrl).Text;
                else if (ctrl is NumericUpDown) value = ((NumericUpDown)ctrl).Value.ToString();
                else if (ctrl is DateTimePicker) value = ((DateTimePicker)ctrl).Text;
            }
            return value;
        }

        public static bool CtrlList_setControlText(Dictionary<string,Control> list,string name,string value)
        {
            Control ctrl;
            try
            {
                ctrl = list[name];
                setControlText(ctrl, value);
            }
            catch { return false; }
            return true;
        }

        public static void ComboBox_AddItems(ComboBox cbx, List<string> list, bool is_clear = true)
        {
            if (cbx == null) return;
            if (is_clear) cbx.Items.Clear();
            foreach (string item in list)
                cbx.Items.Add(item);
            if (cbx.Items.Count > 0)
            {
                if (cbx.SelectedIndex < 0 ||
                    cbx.SelectedIndex >= cbx.Items.Count)
                {
                    cbx.SelectedIndex = 0;
                    cbx.Text = list[0];
                }
            }
        }

        public static List<string> ComboBox_ToListString(ComboBox cbx)
        {
            List<string> list = new List<string>();
            foreach (string item in cbx.Items)
                list.Add(item);
            return list;
        }
    }

    //==============================================================================================

    public delegate void TypeConverterCallBack(string item_name, ListStringTypeConverter obj);
    public class ListStringTypeConverter : TypeConverter
    {
        public bool IsReadOnly = true;
        public List<string> Items = new List<string>();
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return IsReadOnly;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
           return new StandardValuesCollection(Items);
        }

        //-------------------------------------------------
        // Overrides the CanConvertFrom method of TypeConverter.
        // The ITypeDescriptorContext interface provides the context for the
        // conversion. Typically, this interface is used at design time to 
        // provide information about the design-time container.
        public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return value;
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
