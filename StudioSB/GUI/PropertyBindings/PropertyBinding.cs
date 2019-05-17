﻿using System;
using System.Reflection;

namespace StudioSB.GUI
{
    /// <summary>
    /// Custom binding class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PropertyBinding<T>
    {
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if(BoundType != null)
                {
                    Property.SetValue(null, _value);
                }
                if(BoundObject != null)
                {
                    Property.SetValue(BoundObject, _value);
                }
            }
        }
        private T _value
        {
            get;set;
        }

        public bool IsBound { get => Property != null; }

        private PropertyInfo Property;
        private object BoundObject;
        private Type BoundType;
        
        public static implicit operator T(PropertyBinding<T> p)
        {
            return p.Value;
        }

        public static implicit operator PropertyBinding<T>(T d)
        {
            return new PropertyBinding<T>() { Value = d };
        }

        /// <summary>
        /// Binds a given objects parameter to this value
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="PropertyName"></param>
        public void Bind(object Object, string PropertyName)
        {
            UnBind();
            if(Object is Type)
            {
                BoundType = (Type)Object;

                foreach (var prop in BoundType.GetProperties())
                {
                    if (prop.Name.Equals(PropertyName) && prop.PropertyType == typeof(T))
                    {
                        Property = prop;
                        _value = (T)Property.GetValue(BoundObject);
                        break;
                    }
                }
                return;
            }
            if (Object == null)
            {
                BoundObject = null;
                Property = null;
                return;
            }
            foreach (var prop in Object.GetType().GetProperties())
            {
                if (prop.Name.Equals(PropertyName) && prop.PropertyType == typeof(T))
                {
                    BoundObject = Object;
                    Property = prop;
                    _value = (T)Property.GetValue(BoundObject);
                    break;
                }
            }
        }

        /// <summary>
        /// Releases the binding on the object
        /// </summary>
        public void UnBind()
        {
            BoundObject = null;
            BoundType = null;
            Property = null;
        }
    }
}
