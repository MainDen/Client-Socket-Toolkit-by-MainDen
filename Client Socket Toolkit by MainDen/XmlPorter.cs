using System;
using System.Reflection;
using System.Xml;

namespace MainDen.ClientSocketToolkit
{
    public class XmlPorter
    {
        private XmlDocument document = null;
        public XmlDocument Document
        {
            get
            {
                return document ?? (document = new XmlDocument());
            }
            set
            {
                document = value;
            }
        }
        public XmlNode Add(string name, object value = null)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            XmlNode node = document.CreateElement(name);
            node.InnerText = value?.ToString() ?? "";
            
            return node;
        }
        public XmlNode Add(string sourceName, object source, params string[] propertyNames)
        {
            if (sourceName is null)
                throw new ArgumentNullException(nameof(sourceName));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (propertyNames is null)
                throw new ArgumentNullException(nameof(propertyNames));

            XmlNode xmlSource = document.CreateElement(sourceName);
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo;
            object propertyValue;
            XmlNode xmlProperty;

            foreach (var propertyName in propertyNames)
            {
                try
                {
                if (propertyName is null)
                    throw new ArgumentException("Property name must not be null.");
                propertyInfo = sourceType.GetProperty(propertyName);
                propertyValue = propertyInfo?.GetValue(source, null);
                xmlProperty = Add(propertyName, propertyValue);
                xmlSource.AppendChild(xmlProperty);
                } catch { }
            }

            return xmlSource;
        }
        public void Set(string sourceXPath, object source, params string[] propertyNames)
        {
            if (sourceXPath is null)
                throw new ArgumentNullException(nameof(sourceXPath));
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (propertyNames is null)
                throw new ArgumentNullException(nameof(propertyNames));

            XmlNode xmlSource = document.SelectSingleNode(sourceXPath);
            Type sourceType = source.GetType();
            PropertyInfo propertyInfo;
            MethodInfo parseInfo;
            object propertyValue;
            XmlNode xmlProperty;

            foreach (var propertyName in propertyNames)
            {
                try
                {
                    if (propertyName is null)
                        throw new ArgumentException("Property name must not be null.");
                    propertyInfo = sourceType.GetProperty(propertyName);
                    xmlProperty = xmlSource.SelectSingleNode(propertyName);
                    parseInfo = propertyInfo.PropertyType.GetMethod("Parse", BindingFlags.Public |
                        BindingFlags.Static, null, new Type[] { typeof(string) }, null);
                    propertyValue = parseInfo?.Invoke(null, new object[] { xmlProperty.InnerText }) ?? xmlProperty.InnerText;
                    propertyInfo.SetValue(source, propertyValue, null);
                } catch { }
            }
        }
    }
}
