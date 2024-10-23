using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LibreriaGlobalHGInet.Funciones
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValueFromPropertyAttribute : Attribute
    {
        private string _propertyName = string.Empty;
        private System.Type _fromType = null;
        private System.Type _toType = null;

        public ValueFromPropertyAttribute(string propertyName)
        {
            this._propertyName = propertyName;
        }

        public ValueFromPropertyAttribute(string propertyName, System.Type
                  fromType)
        {
            this._propertyName = propertyName;
            this._fromType = fromType;
        }

        public ValueFromPropertyAttribute(string propertyName, System.Type fromType,
            System.Type toType)
        {
            this._propertyName = propertyName;
            this._fromType = fromType;
            this._toType = toType;
        }

        public string ValueFromProperty { get { return this._propertyName; } }

        public System.Type FromType { get { return this._fromType; } }
        public System.Type ToType { get { return this._toType; } }
    }


    public class ConvertirObjeto
    {



        public static object Convert(object fromObject, System.Type toType)
        {
            if (fromObject == null) return null;
            object returnObject = Activator.CreateInstance(toType);

            PropertyInfo[] infos = returnObject.GetType().GetProperties();

            foreach (PropertyInfo property in infos)
            {
                ValueFromPropertyAttribute[] attributes = (ValueFromPropertyAttribute[]) property.GetCustomAttributes(typeof(ValueFromPropertyAttribute), false);

                if (attributes.Length > 0)
                {
                    PropertyInfo fromProperty =
                     fromObject.GetType().
                      GetProperty(attributes[0].ValueFromProperty);

                    if (attributes[0].FromType == null)
                    {

                        property.SetValue(returnObject,
                 fromProperty.GetValue
                   (fromObject, null), null);
                    }
                    else
                    {
                        if (fromProperty.PropertyType.IsArray)
                        {
                            property.SetValue(returnObject,
                    ConvertAll((object[])
                    fromProperty.GetValue(fromObject, null),
                    attributes[0].ToType), null);
                        }
                        else
                        {
                            property.SetValue(returnObject,
                    Convert(fromProperty.GetValue(fromObject,
                    null), property.PropertyType), null);
                        }
                    }
                }
            }

            return returnObject;
        }

        public static object ConvertAll(object[] fromObjects, System.Type toType)
        {
            if (fromObjects != null)
            {
                ArrayList list = new ArrayList(fromObjects.Length);

                foreach (object obj in fromObjects)
                {
                    list.Add(Convert(obj, toType));
                }

                return list.ToArray(toType);

            }
            return null;
        }

    }
}
