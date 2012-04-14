using System;
using System.Globalization;
using System.Windows.Data;
using System.Xml.Linq;
using Common;

namespace ActivityLib
{
    public class DataTreeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new XElementTreeViewItem(new XElement("NoDataAssigned"));
            }
            const string dataContextName = "MainData";
            string id = value.ToString();
            //XElement xe = DataContextManager.GetInstance().GetDataContext(dataContextName).GetElementById(id);
            return new XElementTreeViewItem(XElement.Parse("<Data data='check datatreeconverter' />"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}