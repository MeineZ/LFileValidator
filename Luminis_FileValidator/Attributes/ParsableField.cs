using System;
using System.Reflection;

namespace LFV.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal class ParsableField : Attribute
    {
        public string XmlName { get; private set; }
        public string CsvName { get; private set; }

        public ParsableField(string xmlName = "", string csvName = "")
        {
            XmlName = xmlName ?? string.Empty;
            CsvName = csvName ?? string.Empty;
        }

        public void SetValueFromString(FieldInfo fieldInfo, object obj, string value) 
        {
            if (string.IsNullOrEmpty(value)) return;

            // switch case?
            fieldInfo.SetValue(obj, value);
        }

    }
}
