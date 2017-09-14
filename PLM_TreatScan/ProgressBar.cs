using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace PLM_TreatScan
{
    class DataGridViewProgressColumn : DataGridViewImageColumn
    {

        public DataGridViewProgressColumn()
        {
            this.CellTemplate = new DataGridViewProgressCell();

        }
    }

    class DataGridViewProgressCell : DataGridViewImageCell
    {
        //public UpdateProcessValue updateValue; 

        public object progressCellLock = new object();

        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public DataGridViewProgressCell()
        {
            this.ValueType = typeof(double);
        }

        public string ShowText { get; set; } //如果要显示独立的文字而不是百分比，设置此属性。 
        protected override object GetFormattedValue(object value,
        int rowIndex, ref DataGridViewCellStyle cellStyle,
        TypeConverter valueTypeConverter,
        TypeConverter formattedValueTypeConverter,
        DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        public new double Value
        {
            set
            {
                lock (progressCellLock)
                {
                    try
                    {
                        base.Value = Math.Round(value, 2);
                    }
                    catch { }
                }
            }
            get
            {
                return double.Parse(base.Value.ToString());
            }
        }



        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (value == null)
            {
                value = "0";
                ShowText = "0%";
            }
            string tValue = value.ToString();
            if (tValue == "") tValue = "0";


            double progressVal;
            try { progressVal = Convert.ToDouble(tValue); }
            catch
            {
                progressVal = 0;
            }

            float percentage = ((float)progressVal / 100.0f);
            Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);

            lock (progressCellLock)
            {
                base.Paint(g, clipBounds, cellBounds,
                rowIndex, cellState, value, formattedValue, errorText,
                cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));

                string DrawStringStr = progressVal.ToString() + "%";
                if (ShowText != "")
                {
                    DrawStringStr = ShowText;
                }

                if (percentage > 0.0)
                {
                    //g.FillRectangle(new SolidBrush(Color.FromArgb(163, 189, 242)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4); 
                    g.FillRectangle(new SolidBrush(Color.Green), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
                    g.DrawString(DrawStringStr, cellStyle.Font, foreColorBrush, cellBounds.X + 30, cellBounds.Y + 5);
                }

                else
                {
                    if (this.DataGridView.CurrentRow.Index == rowIndex)
                        g.DrawString(DrawStringStr, cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 30, cellBounds.Y + 5);
                    else
                        g.DrawString(DrawStringStr, cellStyle.Font, foreColorBrush, cellBounds.X + 30, cellBounds.Y + 5);
                }
            }
        }
    }
}
