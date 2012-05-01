using System;
using System.Linq;
using System.Xml.Linq;
//using System.Windows.Media.Imaging;

namespace Common
{
    public static class XElementExtension
    {
        public static string GetAttributeValue(this XElement e, string attrName)
        {
            if (e == null)
                return null;
            if (string.IsNullOrEmpty(attrName))
                return null;
            XAttribute xa = e.Attribute(attrName);
            if (xa == null)
                return null;
            return xa.Value;
        }

        public static string XElementToText(this XElement e)
        {
            if (e == null)
                return "Element is NULL";
            string retString = e.Name + "\n";
            if (!string.IsNullOrWhiteSpace(e.Value))
                retString += e.Value + "\n";
            return e.Attributes().Aggregate(retString, (current, a) => current + (" " + a.Name + " : " + a.Value + "\n"));
        }

        public static XElement GetRootElement(this XElement current)
        {
            XElement parent = current;
            while (true)
            {
                if (parent == null)
                    break;
                if (parent.Parent == null)
                    break;
                parent = parent.Parent;
            }
            return parent;
        }


        //public static void ExportTo(string content)
        //{
        //    var fileDialog = new SaveFileDialog();
        //    fileDialog.ShowDialog();
        //    if (!String.IsNullOrEmpty(fileDialog.FileName))
        //    {
        //        try
        //        {
        //            string file = fileDialog.FileName;
        //            File.WriteAllText(file, content);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Save File or File content wrong\n" + ex.Message);
        //        }
        //    }
        //}

        //public static XElement ImportFrom()
        //{
        //    XElement newXe = null;
        //    var fileDialog = new OpenFileDialog();

        //    fileDialog.ShowDialog();
        //    if (!String.IsNullOrEmpty(fileDialog.FileName))
        //    {
        //        try
        //        {
        //            string file = fileDialog.FileName;
        //            string fileContent = File.ReadAllText(file);
        //            newXe = XElement.Parse(fileContent);
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Open File or File content wrong\n" + ex.Message);
        //        }
        //    }
        //    return newXe;
        //}

        private static string GetNTab(int n)
        {
            string tab = "";
            for (int i = 0; i < n; i++)
            {
                tab += "\t";
            }
            return tab;
        }

        public static string GetSimpleDescriptionFromXElement(this XElement element)
        {
            return element.GetSimpleDescriptionFromXElement(0);
        }

        public static string GetSimpleDescriptionFromXElement(this XElement element, int level)
        {
            if (element == null)
                return "element is null!";
            string result = "";
            result += GetNTab(level) + element.Name;
            if (!String.IsNullOrEmpty(element.Value))
            {
                result += " : [" + element.Value + "] ";
            }
            result += "\n";
            foreach (XAttribute xa in element.Attributes())
            {
                result += GetNTab(level + 1) + xa.Name + "=" + xa.Value + "\n";
            }
            if (result.Length > 1024)
                return result.Substring(0, 1000) + " ...";
            foreach (XElement xe in element.Elements())
            {
                result += GetSimpleDescriptionFromXElement(xe, level + 1);
                if (result.Length > 1024)
                    return result.Substring(0, 1000) + " ...";
            }
            if (result.Length > 1024)
                return result.Substring(0, 1000) + " ...";
            return result;
        }


        //public static BitmapImage GetImage(string binary64)
        //{
        //    byte[] buffer = Convert.FromBase64String(binary64);

        //    BitmapImage bImage;
        //    using (var ms = new MemoryStream(buffer))
        //    {
        //        bImage = new BitmapImage {CacheOption = BitmapCacheOption.OnLoad};
        //        bImage.BeginInit();
        //        bImage.StreamSource = ms;
        //        bImage.EndInit();
        //    }

        //    return bImage;
        //}

        //public static string ClipboardImageToBase64String()
        //{
        //    string newBase64Str;
        //    BitmapSource bs = Clipboard.GetImage();
        //    if (bs == null)
        //        return "";
        //    var encoder = new PngBitmapEncoder();
        //    var frame = BitmapFrame.Create(bs);
        //    encoder.Frames.Add(frame);
        //    using (var stream = new MemoryStream())
        //    {
        //        encoder.Save(stream);
        //        newBase64Str = Convert.ToBase64String(stream.ToArray());
        //    }
        //    return newBase64Str;
        //}
    }
}