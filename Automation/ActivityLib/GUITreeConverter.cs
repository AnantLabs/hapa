using System;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Linq;

namespace ActivityLib
{
    public class GuiTreeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new XElementTreeViewItem(new XElement("NoGUIObjectAssigned"));
            }
            const string dataContextName = "GUIData";
            string id = value.ToString();
            //XElement xe = DataContextManager.GetInstance().GetDataContext(dataContextName).GetElementById(id);
            return new XElementTreeViewItem(XElement.Parse("<Data data='check GuiTreeConverter' />"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}