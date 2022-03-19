// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Reflection.Emit;

namespace AntDesign.Core.Reflection
{
    internal static class ReflectUtility
    {
        private static volatile Dictionary<string, PropertyInfo> _getpropertyhash = new Dictionary<string, PropertyInfo>();
        public static bool SetProperty(object instance, string propertyName, object value)
        {
            if (instance == null)
                return false;
            if (string.IsNullOrEmpty(propertyName))
                return false;

            if (instance is DataRow)
            {
                try
                {
                    ((DataRow)instance)[propertyName] = value;
                    return true;
                }
                catch { return false; }
            }
            else if (instance is DataRowView)
            {
                try
                {
                    ((DataRowView)instance)[propertyName] = value;
                    return true;
                }
                catch { return false; }
            }
            Type t = instance.GetType();
            string pname = t.FullName + "." + propertyName;
            PropertyInfo pi = null;
            lock (_getpropertyhash)
            {
                if (_getpropertyhash.ContainsKey(pname))
                    pi = _getpropertyhash[pname];
                else
                {
                    pi = t.GetProperty(propertyName);
                    if (pi == null)
                        pi = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                    _getpropertyhash.Add(pname, pi);
                }
            }
            if (pi == null)
                return false;
            try
            {
                if (pi.CanWrite)
                {
                    pi.SetValue(instance, value, null);
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static object GetProperty(object instance, string propertyName)
        {
            if (instance == null)
                return null;
            if (string.IsNullOrEmpty(propertyName))
                return null;
            if (instance is DataRow)
                return ((DataRow)instance)[propertyName];
            else if (instance is DataRowView)
                return ((DataRowView)instance)[propertyName];

            Type t = instance.GetType();

            string pname = t.FullName + "." + propertyName;
            PropertyInfo pi = null;
            lock (_getpropertyhash)
            {
                if (_getpropertyhash.ContainsKey(pname))
                    pi = _getpropertyhash[pname];
                else
                {
                    pi = t.GetProperty(propertyName);
                    if (pi == null)
                        pi = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                    _getpropertyhash.Add(pname, pi);
                }
            }
            if (pi == null)
            {
                return null;
            }
            try
            {
                if (pi.CanRead)
                    return pi.GetValue(instance, null);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        public static T GetPropertyCustomAttribute<T>(Type t, string propertyName) where T : Attribute
        {
            if (string.IsNullOrEmpty(propertyName) || t == null)
                return default(T);

            string pname = t.FullName + "." + propertyName;
            PropertyInfo pi = null;
            if (_getpropertyhash.ContainsKey(pname))
                pi = _getpropertyhash[pname];
            else
            {
                pi = t.GetProperty(propertyName);
                if (pi == null)
                    pi = t.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
                _getpropertyhash.Add(pname, pi);
            }
            if (pi != null)
            {
                var l = pi.GetCustomAttributes(typeof(T), true);
                if (l != null && l.Length > 0)
                    return (T)l[0];
                else
                    return default(T);
            }
            return default(T);
        }

        public static T GetTypeCustomAttribute<T>(Type t) where T : Attribute
        {
            if (t != null)
            {
                var l = t.GetCustomAttributes(typeof(T), true);
                if (l != null && l.Length > 0)
                    return (T)l[0];
                else
                    return default(T);
            }
            return default(T);
        }

        public static object Invoke(object obj, string functionName, Type[] paramTypes, Type[] genericTypes, params object[] paramVals)
        {
            if (obj == null)
                return null;
            if (string.IsNullOrEmpty(functionName))
                return null;
            Type t = obj.GetType();
            MethodInfo mi = null;
            if (paramTypes == null || paramTypes.Length == 0)
            {
                mi = t.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public);
                if (mi == null)
                    mi = t.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic);
            }
            else
            {
                mi = t.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
                if (mi == null)
                    mi = t.GetMethod(functionName, BindingFlags.Instance | BindingFlags.NonPublic, null, paramTypes, null);
            }
            if (mi == null)
                return null;
            if (genericTypes != null && genericTypes.Length > 0)
                mi = mi.MakeGenericMethod(genericTypes);
            return mi.Invoke(obj, paramVals);
        }

        public static object Invoke(Type t, string functionName, Type[] paramTypes, Type[] genericTypes, params object[] paramVals)
        {
            if (t == null)
                return null;
            if (string.IsNullOrEmpty(functionName))
                return null;
            MethodInfo mi = null;
            if (paramTypes == null || paramTypes.Length == 0)
            {
                mi = t.GetMethod(functionName, BindingFlags.Static | BindingFlags.Public);
                if (mi == null)
                    mi = t.GetMethod(functionName, BindingFlags.Static | BindingFlags.NonPublic);
            }
            else
            {
                mi = t.GetMethod(functionName, BindingFlags.Static | BindingFlags.Public, null, paramTypes, null);
                if (mi == null)
                    mi = t.GetMethod(functionName, BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);
            }
            if (mi == null)
                return null;
            if (genericTypes != null && genericTypes.Length > 0)
                mi = mi.MakeGenericMethod(genericTypes);
            return mi.Invoke(null, paramVals);
        }

        public static object Invoke(Type t, string functionName, Type[] paramTypes, params object[] paramVals)
        {
            return Invoke(t, functionName, paramTypes, null, paramVals);
        }

        public static object Invoke(object obj, string functionName, Type[] paramTypes, params object[] paramVals)
        {
            return Invoke(obj, functionName, paramTypes, null, paramVals);
        }

        public static object Invoke(Type t, string functionName, params object[] paramVals)
        {
            return Invoke(t, functionName, null, null, paramVals);
        }

        public static object Invoke(object obj, string functionName, params object[] paramVals)
        {
            return Invoke(obj, functionName, null, null, paramVals);
        }

        public static bool IsSubTypeOf(this Type type, Type parent)
        {
            if (type == null || parent == null)
                return false;
            if (parent.IsInterface)
            {
                return type.GetInterface(parent.FullName) != null;
            }
            while (true)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == parent)
                    return true;
                if (type.IsSubclassOf(parent))
                    return true;
                type = type.BaseType;
                if (type == null || type == typeof(object))
                    return false;
            }
        }

        public static T Convert<T>(string s, T defautvalue, string format)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)s;
            if (string.IsNullOrEmpty(s))
                return defautvalue;
            if (typeof(T).IsEnum)
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), s, true);
                }
                catch
                {
                    return defautvalue;
                }
            }
            IFormatProvider pf = GetFormatProvider(format);
            Type[] types = null;
            object[] val = null;
            if (pf == null)
            {
                if (string.IsNullOrEmpty(format))
                {
                    types = new Type[1];
                    types.SetValue(typeof(string), 0);
                    val = new object[] { s };
                    try
                    {
                        return (T)Invoke(typeof(T), "Parse", types, val);
                    }
                    catch
                    {
                        return defautvalue;
                    }
                }
                else
                {
                    try
                    {
                        return (T)Invoke(typeof(T), "ParseExact", new Type[] { typeof(string), typeof(string), typeof(IFormatProvider) }, s, format, (IFormatProvider)null);
                    }
                    catch
                    {
                        return defautvalue;
                    }
                }
            }
            else
            {
                types = new Type[2];
                val = new object[2];
                types.SetValue(typeof(string), 0);
                types.SetValue(typeof(IFormatProvider), 1);
                val = new object[] { s, pf };
                try
                {
                    return (T)Invoke(typeof(T), "Parse", types, val);
                }
                catch
                {
                    return defautvalue;
                }
            }
        }

        public static T Convert<T>(string s)
        {
            return Convert<T>(s, default(T), null);
        }

        public static T Convert<T>(string s, T defautvalue)
        {
            return Convert<T>(s, defautvalue, null);
        }

        public static T ConvertNullable2Value<T>(T? value) where T : struct
        {
            if (value == null || !value.HasValue)
                return default(T);
            else
                return value.Value;
        }

        public static bool CanConvert<T>(string s)
        {
            if (typeof(T) == typeof(string))
                return true;
            if (string.IsNullOrEmpty(s))
                return false;
            if (typeof(T).IsEnum)
            {
                try
                {
                    var e1 = (T)Enum.Parse(typeof(T), s, true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            Type[] types = null;
            object[] val = null;
            types = new Type[1];
            types.SetValue(typeof(string), 0);
            val = new object[] { s };
            try
            {
                var e2 = (T)Invoke(typeof(T), "Parse", types, val);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IFormatProvider GetFormatProvider(string format)
        {
            if (string.IsNullOrEmpty(format))
                return null;
            try
            {
                IFormatProvider f = System.Globalization.CultureInfo.GetCultureInfo(format);
                return f;
            }
            catch
            {
                return null;
            }
        }

        public static int Compare(object v1, object v2)
        {
            if (v1 == null && v2 == null)
                return 0;
            if (v1 == null && v2 != null)
                return -1;
            if (v1 != null && v2 == null)
                return 1;
            if (v1.GetType() != v2.GetType())
                return int.MinValue;
            var ct = typeof(Comparer<>).MakeGenericType(v1.GetType());
            if (ct == null)
                return int.MaxValue;
            var ctd = ct.GetProperty("Default", BindingFlags.Static | BindingFlags.Public).GetValue(null, null);
            if (ctd == null)
                return int.MaxValue;
            return (int)Invoke(ctd, "Compare", v1, v2);
        }

    }
}
