// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    partial class DynamicTable<TItem> where TItem : class, new()
    {
        private EntityClassAccessor _entityClassAccessor = null;
        private string _entityCheckedPropertyName;
        [Parameter]
        public TItem EditItem { get; set; } = new TItem();
        private string EditField { get; set; }
        [Parameter]
        public bool IsReadOnly { get; set; }
        private MarkupString EditFieldValidationMessage { get; set; }

        [CascadingParameter]
        public IEnumerable<VaildateErrorItem> ExternalVaildateErrors { get; set; }
        public DynamicTable()
        {
            _entityClassAccessor = new EntityClassAccessor();
            _entityClassAccessor.CurrentItem = new TItem();
            this.rowWidth = 500;
            foreach (var p in _entityClassAccessor.Fields)
            {
                if (p.Value.AutoGenerateBehaviorAttribute?.DynamicTableVisibility != false)
                {
                    this.rowWidth += p.Value.DataGridColumnSetupAttribute?.Width ?? 150;
                }
            }
        }

        internal PropertyValueAccessor GetColumn(string property)
        {
            if (_entityClassAccessor.Fields.ContainsKey(property))
            {
                return _entityClassAccessor.Fields[property];
            }

            return null;
        }

        internal bool IsRowValid(TItem data, out VaildateErrorItem firstError)
        {
            var ea = _entityClassAccessor.GetAccessor(data);
            foreach (var f in ea.Fields)
            {
                string[] errs = null;
                if (!f.Value.IsValid(data, out errs))
                {
                    firstError = new VaildateErrorItem(data, f.Key, errs.FirstOrDefault());
                    return false;
                }
            }

            firstError = null;
            return true;
        }

        internal bool IsAllRowValid(out VaildateErrorItem firstError)
        {
            if (this.DataSource == null || !this.DataSource.Any())
            {
                firstError = null;
                return true;
            }

            foreach (var f in this.DataSource)
            {
                if (!IsRowValid(f, out firstError))
                {
                    return false;
                }
            }

            firstError = null;
            return true;
        }

        public string GetFirstField()
        {
            return this._entityClassAccessor.Fields.FirstOrDefault(f => !string.Equals(f.Key, this.EntityCheckedPropertyName) && f.Value.PropertyInfo.PropertyType != typeof(BindEntityProperty)).Key;
        }

        private System.Linq.Expressions.Expression<Func<T>> GetPropertyExpression<T>(object data, PropertyInfo propertyInfo)
        {
            var myObject = Expression.Parameter(data.GetType(), "MyObject"); ;
            var myProperty = Expression.Property(myObject, propertyInfo);
            var myLamda = Expression.Lambda(myProperty);

            return (System.Linq.Expressions.Expression<Func<T>>)myLamda;
        }

        private T GetPropertyValue<T>(object data, PropertyInfo propertyinfo)
        {
            return (T)propertyinfo.GetValue(data, null);
        }

        private void SetPropertyValue<T>(object data, PropertyInfo propertyinfo, T value)
        {
            propertyinfo.SetValue(data, value);
        }

        private string GetPropertyStringValue(object data, PropertyValueAccessor property)
        {
            if (property.DataGridViewAttribute?.ConverterType?.IsSubTypeOf(typeof(IValueConverter)) == true)
            {
                var c = property.DataGridViewAttribute.GetConverter();
                if (c != null)
                {
                    return (c.Convert(property.PropertyInfo.GetValue(data, null), typeof(string), null, System.Globalization.CultureInfo.CurrentUICulture) ?? "").ToString();
                }
            }

            if (property.DisplayConverterAttribute?._converter?.IsSubTypeOf(typeof(IValueConverter)) == true)
            {
                var c = property.DisplayConverterAttribute.GetConverter();
                if (c != null)
                {
                    return (c.Convert(property.PropertyInfo.GetValue(data, null), typeof(string), property.DisplayConverterAttribute.ConvertorParameter, System.Globalization.CultureInfo.CurrentUICulture) ?? "").ToString();
                }
            }


            var obj = property.PropertyInfo.GetValue(data, null);
            if (!string.IsNullOrEmpty(property.DisplayFormatAttribute?.DataFormatString))
            {
                return string.Format("{0:" + property.DisplayFormatAttribute?.DataFormatString + "}", obj);
            }
            return (obj ?? "").ToString();
        }

        private RenderFragment CreateColumn(PropertyValueAccessor p, TItem context, ComponentBase page, RenderFragment title, RenderFragment edit, RenderFragment errtip, RenderFragment view, RenderFragment viewerrtip)
        {
            p.CurrentItem = context;
            void Foo(RenderTreeBuilder builder)
            {
                int idx = 0;
                builder.OpenComponent(idx++, typeof(Column<>).MakeGenericType(p.PropertyInfo.PropertyType));
                builder.AddAttribute(idx++, "FieldExpression", ReflectUtility.Invoke(this, "GetPropertyExpression", new Type[] { typeof(object), typeof(PropertyInfo) }, new Type[] { p.PropertyInfo.PropertyType }, context, p.PropertyInfo));
                builder.AddAttribute(idx++, "FieldChanged", p.GetPropertyChangedCallbackBoxing(page, context, true));
                //builder.AddAttribute(idx++, "Field", p.PropertyInfo.GetValue(context));
                builder.AddAttribute(idx++, "Sortable", (p.DataGridColumnSetupAttribute?.CanSort != false));
                builder.AddAttribute(idx++, "Fixed", ((p.DataGridColumnSetupAttribute?.Frozen == true) ? "left" : ""));
                builder.AddAttribute(idx++, "Filterable", true);
                builder.AddAttribute(idx++, "Align", ((ColumnAlign)Enum.Parse(typeof(ColumnAlign), p.DataGridViewAttribute?.TextAlignment.ToString() ?? "Left", true)));
                builder.AddAttribute(idx++, "OnCell", new Func<CellData, Dictionary<string, object>>(f => OnCell(f, p.PropertyInfo.Name)));

                if (p.DataGridColumnSetupAttribute?.Width > 0)
                {
                    builder.AddAttribute(idx++, "Width", $"{p.DataGridColumnSetupAttribute?.Width}px");
                }
                else
                {
                    builder.AddAttribute(idx++, "Width", $"150px");
                }
                //builder.AddAttribute(idx++, "Ellipsis", true);
                if (title != null)
                {
                    builder.AddAttribute(idx++, "TitleTemplate", title);
                }

                if (!this.IsReadOnly && IsFieldEditable(context, p.PropertyInfo.Name))
                {
                    builder.AddAttribute(idx++, "ChildContent", new RenderFragment((b) =>
                    {
                        if (!string.IsNullOrEmpty(EditFieldValidationMessage.Value))
                        {
                            b.AddContent(1, errtip);
                        }
                        b.AddContent(0, edit);
                    }
                        ));
                }
                else
                {
                    builder.AddAttribute(idx++, "ChildContent", new RenderFragment((b) =>
                    {
                        if (this.ExternalVaildateErrors?.Any(f => f.DataItem == context && (string.Equals(f.PropertyName, p.PropertyInfo.Name) || f.PropertyNames?.Contains(p.PropertyInfo.Name) == true)) == true)
                        {
                            b.AddContent(1, viewerrtip);
                        }
                        b.AddContent(0, view);
                    }
                        ));

                }
                builder.CloseComponent();
            }

            //void fooTitle(RenderTreeBuilder builder)
            //{
            //    builder.OpenElement(0, "TitleTemplate");
            //    builder.OpenComponent(1, typeof(AntDesign.Text));
            //    builder.AddAttribute(2, "Style", p.GetDataGridHeaderStyle);
            //    builder.AddContent(3, "123"); 
            //    builder.CloseComponent();
            //    builder.CloseElement();
            //}

            return Foo;

        }

        /// <summary>
        /// 指定返回的结果集中，用于表示“选中”状态的布尔型字段名。可以为空。若指定的正确，则返回结果集后，将在底部显示一个“全选”复选框。
        /// </summary>
        [Parameter]
        public string EntityCheckedPropertyName
        {
            get
            {
                if (typeof(TItem).IsSubTypeOf(typeof(IDynamicTableCheckedColumnDefinition)))
                {
                    return "Checked";
                }
                else
                {
                    return _entityCheckedPropertyName;
                }
            }

            set => _entityCheckedPropertyName = value;
        }

        /// <summary>
        /// 获取或设置查询框内的数据行是是否可以多选，默认为true，需要配合EntityCheckedPropertyName属性一起使用。
        /// </summary>
        [Parameter]
        public bool MulitSelect
        {
            get; set;
        } = true;

        private void UpdateSingleSelectStatus(TItem data, bool checkstate)
        {
            if (this.DataSource == null || !this.DataSource.Any())
            {
                return;
            }

            this.DataSource.ForEach(f => ReflectUtility.SetProperty(f, EntityCheckedPropertyName, false));
            if (checkstate && data != null)
            {
                ReflectUtility.SetProperty(data, EntityCheckedPropertyName, checkstate);
            }
        }

        /// <summary>
        /// 用于动态检查列可见性的方法委托，只对于DisplayAttribute.AutoGenerateField=True且AutoGenerateVisitily.DataGridExVisitily=True的列有效
        /// </summary>
        [Parameter]
        public Func<string, bool> CheckColumnVisibility { get; set; }
        /// <summary>
        /// 用于检查单个单元格是否可编辑的方法委托，对于一列中，部分可编辑的情况，可以使用本方法处理。第一个参数是列名，第二个参数是指定的行绑定的数据上下文
        /// </summary>
        [Parameter]
        public Func<string, TItem, bool> CheckCellEditable { get; set; }

        private bool IsFieldEditable(TItem data, string field)
        {
            if (data != EditItem)
            {
                return false;
            }
            if (!string.Equals(field, this.EditField))
            {
                return false;
            }

            var item = this._entityClassAccessor.GetAccessor(data).Fields[field];
            if (item?.DataGridColumnSetupAttribute?.IsReadOnly == true)
            {
                return false;
            }

            if (CheckCellEditable?.Invoke(field, data) == false)
            {
                return false;
            }

            return true;
        }

        private MarkupString ValidateField(TItem data, string field)
        {
            if (data == null || string.IsNullOrEmpty(field))
            {
                return new MarkupString("");
            }
            var item = this._entityClassAccessor.GetAccessor(data).Fields[field];
            string[] errs;
            if (!item.IsValid(data, out errs))
            {
                return new MarkupString(string.Join("<br />", errs));
            }
            else
            {
                return new MarkupString("");
            }
        }

        private MarkupString GetExternalVaildateErrors(TItem data, string field)
        {
            if (this.ExternalVaildateErrors == null || !this.ExternalVaildateErrors.Any())
            {
                return new MarkupString("");
            }

            var l = this.ExternalVaildateErrors.Where(f => f.DataItem == data && (string.Equals(field, f.PropertyName) || f.PropertyNames?.Contains(field) == true)).ToArray();
            return new MarkupString(string.Join("<br />", l.Select(f => f.ErrorMessage).ToArray()));
        }

        private bool CheckedAll
        {
            get
            {
                if (this.DataSource == null || !this.DataSource.Any())
                {
                    return false;
                }
                return !this.DataSource.Any(f => !(bool)ReflectUtility.GetProperty(f, this.EntityCheckedPropertyName));
            }
        }

        private bool PartialChecked
        {
            get
            {
                if (this.DataSource == null || !this.DataSource.Any())
                {
                    return false;
                }
                return this.DataSource.Any(f => !(bool)ReflectUtility.GetProperty(f, this.EntityCheckedPropertyName))
                    && this.DataSource.Any(f => (bool)ReflectUtility.GetProperty(f, this.EntityCheckedPropertyName));
            }
        }


        public void Add(IEnumerable<TItem> data)
        {
            if (data == null || !data.Any())
                return;
            if (this.DataSource == null)
            {
                this.DataSource = new List<TItem>();
            }
            ICollection<TItem> l = this.DataSource as ICollection<TItem>;
            foreach (var item in data)
            {
                if (item == null)
                    continue;
                l.Add(item);
            }
            this.StateHasChanged();
        }


        /// <summary>
        /// 将数据的OperatorType字段设置为Delete，但不真正从列表中去除数据，若数据的状态是Add，则直接从列表中删除这条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void Remove(IEnumerable<TItem> data)
        {
            if (data == null || !data.Any())
                return;
            ICollection<TItem> l = this.DataSource as ICollection<TItem>;
            if (l == null)
                return;
            TItem[] data1 = data.ToArray();
            foreach (var item in data1)
            {
                if (item == null)
                    continue;
                l.Remove(item);
            }
        }

        /// <summary>
        /// 将数据从列表中移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void RemoveFromList(IEnumerable<TItem> data)
        {
            if (data == null || !data.Any())
                return;
            ICollection<TItem> l = this.DataSource as ICollection<TItem>;
            if (l == null)
                return;
            TItem[] data1 = data.ToArray();
            foreach (var item in data1)
            {
                if (item == null)
                    continue;
                if (l.Contains(item))
                    l.Remove(item);
            }
        }

        private Dictionary<string, object> OnCell(CellData cell, string field)
        {
            var row = (RowData<TItem>)cell.RowData;
            var data = row.Data;
            var dic = new Dictionary<string, object>();
            if (data is ICompareEntityProperty compareData && compareData != null)
            {
                CompareValueEntity compareValueEntity = new CompareValueEntity();
                compareValueEntity.Value1 = ReflectUtility.GetProperty(data, field);
                compareValueEntity.Value2 = ReflectUtility.GetProperty(compareData.CompareEntity, field);
                if (!compareValueEntity.Match)
                {
                    dic.Add("style", "border-bottom-width: 2px;border-bottom-color: orange;");
                }
            }

            return dic;
        }
    }

}
