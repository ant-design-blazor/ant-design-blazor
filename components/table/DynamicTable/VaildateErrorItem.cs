// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    [DataContract(Name = "VaildateErrorItem", IsReference = true)]
    public class VaildateErrorItem
    {
        public VaildateErrorItem()
        { }

        public VaildateErrorItem(object data, string property, string message, VaildateErrorLevel level)
        {
            DataItem = data;
            PropertyName = property;
            ErrorMessage = message;
            Level = level;
        }

        public VaildateErrorItem(object data, string property, string message)
            : this(data, property, message, VaildateErrorLevel.Error)
        {

        }

        [DataMember]
        public string PropertyName
        {
            get
            {
                if (_propertyNames == null || _propertyNames.Length == 0)
                    return null;
                return _propertyNames[0];
            }
            set
            {
                if (_propertyNames == null || _propertyNames.Length == 0)
                {
                    _propertyNames = new string[] { value };
                }
                else
                {
                    _propertyNames[0] = value;
                }
            }
        }
        private string[] _propertyNames;
        [DataMember]
        public string[] PropertyNames
        {
            get { return _propertyNames; }
            set
            {
                if (_propertyNames != value)
                {
                    _propertyNames = value;
                }
            }
        }
        private object _dataItem;
        [DataMember]
        public object DataItem
        {
            get { return _dataItem; }
            set
            {
                if (_dataItem != value)
                {
                    _dataItem = value;
                }
            }
        }

        public string PropertyDisplayName
        {
            get
            {
                if (PropertyNames == null || PropertyName.Length == 0 || DataItem == null)
                    return PropertyName;
                var p = DataItem.GetType().GetProperties().Where(f => PropertyNames.Contains(f.Name)).Select(f => ReflectUtility.GetPropertyCustomAttribute<DisplayAttribute>(DataItem.GetType(), f.Name)).Where(f => f != null).ToArray();
                if (p == null || p.Length == 0)
                    return string.Join(", ", PropertyNames);
                return string.Join(", ", p.Select(f => f.GetName()).ToArray());
            }
        }

        private VaildateErrorLevel _level;
        [DataMember]
        public VaildateErrorLevel Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                }
            }
        }

        private string _errorMessage;
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                }
            }
        }

        public static System.Collections.Generic.IEnumerable<VaildateErrorItem> Vaildate(object data)
        {
            if (data == null)
            {
                yield break;
            }

            EntityClassAccessor eca = new EntityClassAccessor(); ;
            eca.CurrentItem = data;
            foreach (var item in eca.Fields)
            {
                string[] errs = null;
                if (!item.Value.IsValid(data, out errs))
                {
                    foreach (var err in errs)
                    {
                        yield return new VaildateErrorItem(data, item.Key, err);
                    }
                }
            }
        }

        [DataContract(Name = "VaildateErrorLevel")]
        public enum VaildateErrorLevel
        {
            [EnumMember]
            NoError = 0,
            [EnumMember]
            Warning = 1,
            [EnumMember]
            Error = 2
        }
    }
}
