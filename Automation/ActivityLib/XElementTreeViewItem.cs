using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Windows.Threading;
using Common;

namespace ActivityLib
{
    public class XElementTreeViewItem : TreeViewItem, IEnumerable<XElementTreeViewItem>
    {
        private int _xthFound;

        public XElementTreeViewItem(XElement element)
        {
            Element = element;
            InitNode();
        }

        public XElement Element { get; set; }

        #region IEnumerable<XElementTreeViewItem> Members

        IEnumerator<XElementTreeViewItem> IEnumerable<XElementTreeViewItem>.GetEnumerator()
        {
            var xTreeViewItemEnumerator = new XTreeViewItemEnumerator(this);
            return xTreeViewItemEnumerator;
        }

        public IEnumerator GetEnumerator()
        {
            var xTreeViewItemEnumerator = new XTreeViewItemEnumerator(this);
            return xTreeViewItemEnumerator;
        }

        #endregion

/*
        public static void LoadTree(TreeView treeView, string content)
        {
            treeView.Items.Clear();

            var root = new XElementTreeViewItem(XElement.Parse(content));
            treeView.Items.Add(root);
        }
*/

        private TreeView GetRootParent()
        {
            DependencyObject parent = Parent;
            while (true)
            {
                if (parent == null)
                    return null;
                if (parent is TreeView)
                    return (TreeView)parent;
                parent = ((TreeViewItem)parent).Parent;
            }
        }

        private void UpdateLayOut()
        {
            TreeView root = GetRootParent();
            if (root != null)
            {
                root.UpdateLayout();
                root.Items.Refresh();
            }
        }
        public bool FindByDetail(string filter, int xth, bool isDetailSearch)
        {
            foreach (object item in Items)
            {
                if (item is XElementTreeViewItem)
                {
                    bool ret = isDetailSearch ? IsPartOfElement((XElementTreeViewItem)item, filter) : IsPartOfName((XElementTreeViewItem)item, filter);

                    if (ret)
                    {
                        _xthFound++;
                        if (_xthFound == xth)
                        {
                            var xtvItem = (XElementTreeViewItem)item;
                            xtvItem.ExpendToMe();
                            xtvItem.Focus();
                            xtvItem.IsSelected = true;
                            xtvItem.UpdateNodeLayout();
                            xtvItem.UpdateLayOut();

                            _xthFound = 0;
                            return true;
                        }
                    }
                    if (((XElementTreeViewItem)item).FindByDetail(filter, xth, isDetailSearch)) return true;
                }
            }

            _xthFound = 0;
            return false;
        }
        public bool FindByName(string filter, int xth)
        {
            return FindByDetail(filter, xth, false);
        }

        private bool IsPartOfName(XElementTreeViewItem source, string target)
        {
            return source.Element.Attributes().Where(xa => xa.Name.ToString().Equals(Const.AttributeName)).Where(xa => !string.IsNullOrEmpty(xa.Value)).Any(xa => xa.Value.ToLower().Contains(target.Trim().ToLower()));
        }

        private bool IsPartOfElement(XElementTreeViewItem source, string target)
        {
            return source.Element.Attributes().Where(xa => xa != null).Where(xa => !string.IsNullOrEmpty(xa.Value)).Any(xa => xa.Value.ToLower().Contains(target.Trim().ToLower()));
        }


        //public XElementTreeViewItem GetSimpleVersion()
        //{
        //    var simpleXe = new XElement(Element.Name);
        //    if (Element != null)
        //    {
        //        //simpleXe.SetAttributeValue(ConstString.AttributeId, Element.Attribute(ConstString.AttributeId).Value);
        //        XAttribute nameAttr = Element.Attribute(ConstString.AttributeName);
        //        if (nameAttr != null)
        //        {
        //            simpleXe.SetAttributeValue(ConstString.AttributeName, nameAttr.Value);
        //        }
        //        SetOneAttribute(simpleXe, ConstString.AttributeId);
        //        SetOneAttribute(simpleXe, ConstString.AttributeName);
        //        SetOneAttribute(simpleXe, ConstString.AttributeDescription);
        //        SetOneAttribute(simpleXe, "CreatedUser");
        //    }
        //    return new XElementTreeViewItem(simpleXe);
        //}

        /*
                private void SetOneAttribute(XElement simpleXe, string attributeName)
                {
                    XAttribute idAttr = Element.Attribute(attributeName);
                    if (idAttr != null)
                    {
                        simpleXe.SetAttributeValue(attributeName, idAttr.Value);
                    }
                }
        */

/*
        public void Add(XElementTreeViewItem newKid)
        {
            Element.Add(newKid.Element);
            AddChild(newKid);
        }
*/

        public void AddNode(XElementTreeViewItem newNode)
        {
            AddChild(newNode);
        }

        public void InitNode()
        {
            if (HasItems)
            {
                Items.Clear();
            }
            if (Element != null)
            {
                UpdateNodeLayout();

                #region add children nodes recursive

                if (Element.HasElements)
                    foreach (XElement kid in Element.Elements())
                    {
                        //Dispatcher.Invoke(DispatcherPriority.Render, (Action)(() =>
                        //{
                        var xTreeNode = new XElementTreeViewItem(kid);
                        Items.Add(xTreeNode);
                        //}
                        //));


                    }

                #endregion add children nodes recursive
            }
        }

        // It just update a single node itself, not include its children
        public void UpdateNodeLayout()
        {
            string elementName = Element.Name.ToString();
            string title = "";
            XAttribute xAttribute = Element.Attribute(Const.AttributeName);
            if (xAttribute != null)
                if (!String.IsNullOrEmpty(xAttribute.Value))
                    title = xAttribute.Value;
            
            var head = new StackPanel { Orientation = Orientation.Horizontal };
            var text = new TextBlock();

            XAttribute iconAttr = Element.Attribute(Const.AttributeIcon);
            string iconName = iconAttr != null ? iconAttr.Value : elementName;
            var image = SetIconToNode(head, elementName, iconName, text);

            if (image == null)
                title = elementName + " " + title;
            text.Text = title;
            text.ToolTip = new ToolTip { Content = ToText() };

            if (image == null)
                title = elementName + " " + title;
            text.Text = title;
            text.ToolTip = new ToolTip { Content = ToText() };

            head.Children.Add(text);
            Header = head;

            //if (this.IsSelected)
            //    this.Background = Brushes.Gold;
            //else
            //    this.Background = Brushes.White;
        }

        private Image SetIconToNode(StackPanel head, string elementName, string iconName, TextBlock text)
        {
            Image image = null;
            if (!string.IsNullOrWhiteSpace(iconName))
            {
                BitmapImage bitmap = ImageList.GetInstance().Get(iconName);
                if (bitmap != null)
                {
                    // this is important, remove it, the tree will be 20 times slower
                    
                    Dispatcher.Invoke(DispatcherPriority.Normal, (System.Action)(() =>
                    {
                        image = new Image
                        {
                            Source = bitmap,
                            Stretch = Stretch.Uniform,
                            Width = text.FontSize,
                            Height = text.FontSize,
                            MinHeight = 16,
                            MinWidth = 16
                        };
                        XAttribute attribute = Element.Attribute(Const.AttributeDescription);
                        if (attribute != null)
                        {
                            string toolTip = elementName + "\n" + attribute.Value;
                            ToolTip = new ToolTip();
                            image.ToolTip = toolTip;
                        }

                        head.Children.Add(image);
                    }
                    ));
                }
            }
            return image;
        }

        private string ToText()
        {
            return Element.XElementToText();
        }

        public bool IsLeaf()
        {
            return HasItems;
        }

        public string GetPath()
        {
            var stack = new Stack();
            stack.Push(GetName(Element));
            XElement p = Element.Parent;
            while (p != null)
            {
                stack.Push(GetName(p));
                p = p.Parent;
            }
            string retString = (stack.Pop()).ToString();
            while (stack.Count > 0)
            {
                var e = (string)stack.Pop();
                retString = retString + "." + e;
            }
            return retString;
        }

        private string GetName(XElement element)
        {
            XAttribute nameAttr = element.Attribute("Name");
            if (nameAttr != null)
            {
                string name = nameAttr.Value;
                if (!string.IsNullOrEmpty(name.Trim()))
                    return name;
            }
            return element.Name.ToString();
        }

/*
        public void ExpendTo(string path)
        {
            throw new NotImplementedException();
        }
*/

        private void ExpendToMe()
        {
            if (IsSelected)
                return;
            if (IsExpanded)
                return;
            var stack = new Stack();

            var tnb = (XElementTreeViewItem)Parent;
            while (tnb != null)
            {
                stack.Push(tnb);
                if (tnb.Parent is XElementTreeViewItem)
                    tnb = (XElementTreeViewItem)tnb.Parent;
                else
                {
                    break;
                }
            }
            while (stack.Count > 0)
            {
                tnb = stack.Pop() as XElementTreeViewItem;
                if (tnb != null)
                {
                    tnb.IsExpanded = true;
                }
            }
            //tnb.Focus();
            //tnb.IsSelected = true;
        }
    }

    /// <summary>
    /// this class is very strange, it must be embeded in this file; or we will get compilation error.
    /// </summary>
    sealed class XTreeViewItemEnumerator : IEnumerator<XElementTreeViewItem>
    {
        private readonly XElementTreeViewItem _mainItem;
        private int _index = -1;

        public XTreeViewItemEnumerator(XElementTreeViewItem item)
        {
            _mainItem = item;
            _index = -1;
        }

        #region IEnumerator<XElementTreeViewItem> Members

        public XElementTreeViewItem Current
        {
            get
            {
                return _mainItem;
            }
        }

        public void Dispose()
        {
            _index = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return _mainItem;
            }
        }

        public bool MoveNext()
        {            
            return !(_index++ > 0);                
        }

        public void Reset()
        {
            _index = -1;
        }

        #endregion
    }
}