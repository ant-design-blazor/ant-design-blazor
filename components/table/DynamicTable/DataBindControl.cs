using AntDesign.DataAnnotations;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace AntDesign
{
    public interface IBindEntityControl
    {
        [Parameter]
        object DataObject { get; set; }
        void OnDataObjectChanged(object newDataObject, object oldDataObject);
    }

    public abstract class BindEntityButton : AntDesign.Button, IBindEntityControl
    {
        private object _dataObject;

        public BindEntityButton()
        {
            this.Type = AntDesign.ButtonType.Link;
        }

        public abstract void OnDataObjectChanged(object newDataObject, object oldDataObject);

        [Parameter]
        public object DataObject
        {
            get => _dataObject;
            set
            {
                if (_dataObject != value)
                {
                    this.OnDataObjectChanged(value, _dataObject);
                    _dataObject = value;
                }
            }
        }

    }

    public abstract class BindEntityLabel : AntDesign.Text, IBindEntityControl
    {
        private object _dataObject;
        public abstract void OnDataObjectChanged(object newDataObject, object oldDataObject);

        [Parameter]
        public object DataObject
        {
            get => _dataObject;
            set
            {
                if (_dataObject != value)
                {
                    this.OnDataObjectChanged(value, _dataObject);
                    _dataObject = value;
                }
            }
        }

    }

    public abstract class BindEntityContentControl : AntDesign.AntComponentBase, IBindEntityControl
    {
        private object _dataObject;
        public abstract void OnDataObjectChanged(object newDataObject, object oldDataObject);

        [Parameter]
        public object DataObject
        {
            get => _dataObject;
            set
            {
                if (_dataObject != value)
                {
                    this.OnDataObjectChanged(value, _dataObject);
                    _dataObject = value;
                }
            }
        }
    }

    public abstract class BindEntityCheckBox : AntDesign.Checkbox, IBindEntityControl
    {
        private object _dataObject;
        public abstract void OnDataObjectChanged(object newDataObject, object oldDataObject);

        [Parameter]
        public object DataObject
        {
            get => _dataObject;
            set
            {
                if (_dataObject != value)
                {
                    this.OnDataObjectChanged(value, _dataObject);
                    _dataObject = value;
                }
            }
        }
    }

    /// <summary>
    /// In a data model(entity), the property with this type will show as a ActionColumn, you can define the inner component by inherit <see cref="IBindEntityControl"/>(eg:BindEntityButton), and declear by using <see cref="DynamicTableViewAttribute"/>  
    /// </summary>
    public class BindEntityProperty { }
}
