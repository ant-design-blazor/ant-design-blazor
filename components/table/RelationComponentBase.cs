// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    /// <summary>
    /// Base class for relation data components that automatically load and display related data in table columns.
    /// </summary>
    /// <typeparam name="TItem">Table row data type</typeparam>
    /// <typeparam name="TData">Column field value type (foreign key type)</typeparam>
    /// <remarks>
    /// <para>
    /// This base class provides automatic batch loading of related data and supports two implementation approaches:
    /// </para>
    /// <list type="number">
    /// <item><description><b>C# Class Implementation</b>: Inherit this class and override OnLoadBatch and RenderContent methods</description></item>
    /// <item><description><b>Razor File Implementation</b>: Inherit this class and use CurrentFieldValue and CurrentRowData properties in Razor markup</description></item>
    /// </list>
    /// <para>
    /// <b>Key Features</b>:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Zero-reflection field access via Column.GetValue delegate</description></item>
    /// <item><description>Batch loading optimization to avoid N+1 query problems</description></item>
    /// <item><description>Shared cache support to avoid duplicate data loading</description></item>
    /// <item><description>Two OnLoadBatch overloads for simple and complex scenarios</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para><b>Example 1: C# Class Implementation</b></para>
    /// <code>
    /// public class UserRelation : RelationComponentBase&lt;Order, int&gt;
    /// {
    ///     [Inject] private IUserService UserService { get; set; }
    ///     private Dictionary&lt;int, User&gt; _userCache = new();
    ///     
    ///     protected override async Task OnLoadBatch(IEnumerable&lt;int&gt; userIds)
    ///     {
    ///         var users = await UserService.GetUsersByIdsAsync(userIds);
    ///         _userCache = users.ToDictionary(u => u.Id);
    ///     }
    ///     
    ///     protected override RenderFragment RenderContent(int userId, Order order)
    ///     {
    ///         return builder =>
    ///         {
    ///             if (_userCache.TryGetValue(userId, out var user))
    ///                 builder.AddContent(0, user.Name);
    ///         };
    ///     }
    /// }
    /// </code>
    /// 
    /// <para><b>Example 2: Razor File Implementation</b></para>
    /// <code>
    /// @inherits RelationComponentBase&lt;Order, int&gt;
    /// @inject IUserService UserService
    /// 
    /// @if (_userCache.TryGetValue(CurrentFieldValue, out var user))
    /// {
    ///     &lt;span&gt;@user.Name&lt;/span&gt;
    /// }
    /// 
    /// @code {
    ///     private Dictionary&lt;int, User&gt; _userCache = new();
    ///     protected override async Task OnLoadBatch(IEnumerable&lt;int&gt; userIds)
    ///     {
    ///         var users = await UserService.GetUsersByIdsAsync(userIds);
    ///         _userCache = users.ToDictionary(u => u.Id);
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class RelationComponentBase<TItem, TData> : IComponent, IRelationComponent<TItem>
    {
        private RenderHandle _renderHandle;
        private bool _initialized;
        private bool _hasNeverRendered = true;
        private bool _hasPendingQueuedRender;

        /// <summary>
        /// Gets the shared relation data cache.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This cache is provided by the Table component and may be shared by multiple RelationComponent instances.
        /// </para>
        /// <para>
        /// It is recommended to check the cache before loading data to avoid duplicate loads.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// protected override async Task OnLoadBatch(IEnumerable&lt;int&gt; userIds)
        /// {
        ///     var uncachedIds = userIds.Where(id => 
        ///         !SharedCache.ContainsKey($"User_{id}")).ToList();
        ///     
        ///     if (uncachedIds.Any())
        ///     {
        ///         var users = await UserService.GetUsersByIdsAsync(uncachedIds);
        ///         foreach (var user in users)
        ///         {
        ///             SharedCache[$"User_{user.Id}"] = user;
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        [CascadingParameter(Name = "RelationCache")]
        protected ConcurrentDictionary<string, object> SharedCache { get; set; }

        /// <summary>
        /// Gets the Column component associated with this relation.
        /// </summary>
        /// <remarks>
        /// Use this property to access column configuration such as the GetValue delegate, Title, etc.
        /// </remarks>
        [CascadingParameter]
        protected IColumn Column { get; set; }

        /// <summary>
        /// Gets the Table component associated with this relation.
        /// </summary>
        /// <remarks>
        /// Use this property to access table configuration and the data source.
        /// </remarks>
        [CascadingParameter]
        protected ITable Table { get; set; }

        /// <summary>
        /// The row data currently being rendered.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is especially useful in Razor file implementations where the template can directly access the full row data.
        /// </para>
        /// <para>
        /// Note: This property is only valid during rendering (when RenderContent is invoked).
        /// </para>
        /// </remarks>
        protected TItem CurrentRowData { get; private set; }

        /// <summary>
        /// The field value (foreign key) currently being rendered.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is especially useful in Razor file implementations where the template can access the current field value.
        /// </para>
        /// <para>
        /// Note: This property is only valid during rendering (when RenderContent is invoked).
        /// </para>
        /// </remarks>
        protected TData CurrentFieldValue { get; private set; }

        // Data source cache (for the non-generic interface)
        private IEnumerable<TItem> _cachedDataSource;
        private QueryModel<TItem> _cachedQueryModel;

        /// <summary>
        /// Get the field value (foreign key) from a row data item.
        /// </summary>
        /// <param name="rowData">The table row data.</param>
        /// <returns>The field value; returns default(TData) if the Column does not provide a GetValue delegate.</returns>
        /// <remarks>
        /// <para>
        /// This method uses the Column.GetValue delegate to obtain the field value, avoiding reflection.
        /// </para>
        /// <para>
        /// Performance note: Delegate invocation is significantly faster than reflection.
        /// </para>
        /// </remarks>
        protected TData GetFieldValue(TItem rowData)
        {
            if (Column is Column<TData> columnInternal)
            {
                // Create RowData<TItem> wrapper object
                //var rowDataWrapper = new RowData<TItem> { DataItem = new TableDataItem<TItem>(default, default) { Data = rowData } };
                return columnInternal.GetItemValueExpression<TItem>()(rowData);
            }
            return default;
        }

        /// <summary>
        /// Batch load related data (simplified version).
        /// </summary>
        /// <param name="fieldValues">A collection of field values to load (distinct).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>
        /// Usage: In most cases you only need to override this method.
        /// </para>
        /// <para>
        /// Invocation: The Table calls this method when data is loaded or refreshed.
        /// </para>
        /// <para>
        /// Performance tips:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Use SharedCache to store previously loaded data.</description></item>
        /// <item><description>Batch database queries to avoid N+1 problems.</description></item>
        /// <item><description>Consider using an IN query or bulk API from your data source.</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// protected override async Task OnLoadBatch(IEnumerable&lt;int&gt; userIds)
        /// {
        ///     // Batch load users
        ///     var users = await UserService.GetUsersByIdsAsync(userIds);
        ///     
        ///     // Cache into local dictionary
        ///     _userCache = users.ToDictionary(u => u.Id);
        /// }
        /// </code>
        /// </example>
        protected virtual Task OnLoadBatch(IEnumerable<TData> fieldValues)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Batch load related data (full version).
        /// </summary>
        /// <param name="dataSource">The full data source for the current page.</param>
        /// <param name="queryModel">The query model containing paging, sorting and filtering information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// <para>
        /// Usage: Override this method when you need access to the full row data or query parameters.
        /// </para>
        /// <para>
        /// Default implementation: collects field values, deduplicates them and calls the simplified OnLoadBatch.
        /// </para>
        /// <para>
        /// QueryModel includes:
        /// </para>
        /// <list type="bullet">
        /// <item><description>PageIndex: current page index</description></item>
        /// <item><description>PageSize: page size</description></item>
        /// <item><description>SortModel: sorting information</description></item>
        /// <item><description>FilterModel: filtering information</description></item>
        /// </list>
        /// </remarks>
        /// <example>
        /// <code>
        /// protected override async Task OnLoadBatch(
        ///     IEnumerable&lt;Order&gt; dataSource, 
        ///     QueryModel queryModel)
        /// {
        ///     // 访问完整行数据
        ///     var ordersWithDetails = dataSource.Where(o => o.Status == "Active");
        ///     
        ///     // 使用查询参数优化加载
        ///     Console.WriteLine($"Loading page {queryModel.PageIndex}");
        ///     
        ///     // 批量加载
        ///     var userIds = ordersWithDetails.Select(o => o.UserId).Distinct();
        ///     var users = await UserService.GetUsersByIdsAsync(userIds);
        ///     
        ///     _userCache = users.ToDictionary(u => u.Id);
        /// }
        /// </code>
        /// </example>
        protected virtual async Task OnLoadBatch(IEnumerable<TItem> dataSource, QueryModel<TItem> queryModel)
        {
            // 默认实现：收集字段值并调用简化版
            var fieldValues = dataSource
                .Select(item => GetFieldValue(item))
                .Where(v => v != null && !EqualityComparer<TData>.Default.Equals(v, default))
                .Distinct()
                .ToList();

            await OnLoadBatch(fieldValues);
        }

        /// <summary>
        /// Render the cell content.
        /// </summary>
        /// <param name="fieldValue">The current cell's field value (foreign key).</param>
        /// <param name="rowData">The current row's full data.</param>
        /// <returns>A RenderFragment representing the content to render.</returns>
        /// <remarks>
        /// <para>
        /// C# class implementations: You must override this method and return the content to render.
        /// </para>
        /// <para>
        /// Razor file implementations: No override is required; the base class will set CurrentFieldValue and CurrentRowData.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// protected override RenderFragment RenderContent(int userId, Order order)
        /// {
        ///     return builder =>
        ///     {
        ///         if (_userCache.TryGetValue(userId, out var user))
        ///         {
        ///             builder.OpenElement(0, "div");
        ///             builder.AddAttribute(1, "class", "user-info");
        ///             builder.AddContent(2, user.Name);
        ///             builder.CloseElement();
        ///         }
        ///         else
        ///         {
        ///             builder.AddContent(0, "Loading...");
        ///         }
        ///     };
        /// }
        /// </code>
        /// </example>
        protected virtual RenderFragment RenderContent(TData fieldValue, TItem rowData) => RenderContentInternal(fieldValue, rowData);

        /// <summary>
        /// Render cell content (virtual overload for Razor file support).
        /// </summary>
        /// <param name="fieldValue">The current cell's field value (foreign key).</param>
        /// <param name="rowData">The current row's full data.</param>
        /// <returns>A RenderFragment representing the content to render.</returns>
        /// <remarks>
        /// <para>
        /// This method provides a default implementation for Razor files: it sets CurrentFieldValue and CurrentRowData
        /// and then calls BuildRenderTree to render the Razor template.
        /// </para>
        /// <para>
        /// C# class implementations typically do not override this method; override the abstract RenderContent instead.
        /// </para>
        /// </remarks>
        private RenderFragment RenderContentInternal(TData fieldValue, TItem rowData)
        {
            // Default implementation for Razor files
            return builder =>
            {
                CurrentFieldValue = fieldValue;
                CurrentRowData = rowData;
                // 调用 BuildRenderTree，Razor 编译器会生成这个方法的重写
                BuildRenderTree(builder);
            };
        }

        /// <summary>
        /// Build the render tree (intended to be overridden by Razor files).
        /// </summary>
        /// <remarks>
        /// <para>
        /// Important: This method is intended to be used by code generated by the Razor compiler.
        /// </para>
        /// <para>
        /// When a Razor file inherits RelationComponentBase, the compiler will generate an override of this method.
        /// </para>
        /// <para>
        /// Do not call this method directly. All rendering should be performed through RenderContent, which calls this method at the appropriate time.
        /// </para>
        /// </remarks>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder)
        {
            // 默认空实现，由 Razor 文件生成的代码重写
        }

        #region IComponent implementation

        /// <summary>
        /// Attach the render handle to the component.
        /// </summary>
        void IComponent.Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        /// <summary>
        /// Set component parameters and handle initialization.
        /// </summary>
        async Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (!_initialized)
            {
                _initialized = true;
                await OnInitializedAsync();
            }

            await OnParametersSetAsync();

            // Note: RelationComponent does not trigger rendering here.
            // Rendering is fully controlled by the Table via RenderContent.
        }

        /// <summary>
        /// Called when the component is initialized (synchronous, only once).
        /// </summary>
        protected virtual void OnInitialized()
        {
            if (Column is IColumnInternal columnInternal)
            {
                columnInternal.SetRelationComponent(this);
            }
        }

        /// <summary>
        /// Called when the component is initialized (asynchronous, only once).
        /// </summary>
        protected virtual Task OnInitializedAsync()
        {
            OnInitialized();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called after parameters have been set (synchronous).
        /// </summary>
        protected virtual void OnParametersSet()
        {
        }

        /// <summary>
        /// Called after parameters have been set (asynchronous).
        /// </summary>
        protected virtual Task OnParametersSetAsync()
        {
            OnParametersSet();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Request a re-render of the component.
        /// </summary>
        /// <remarks>
        /// When called from within OnLoadBatch or other async operations, this will trigger
        /// the Table to re-render all rows and cells, allowing updated data to be displayed.
        /// </remarks>
        protected void StateHasChanged()
        {
            if (_hasPendingQueuedRender)
                return;

            if (_hasNeverRendered || ShouldRender())
            {
                _hasPendingQueuedRender = true;

                try
                {
                    // Trigger Table to re-render all rows and cells
                    Table?.Refresh();
                }
                catch
                {
                    _hasPendingQueuedRender = false;
                    throw;
                }
                finally
                {
                    _hasPendingQueuedRender = false;
                }
            }
        }

        /// <summary>
        /// Determine whether the component should render.
        /// </summary>
        protected virtual bool ShouldRender() => true;

        #endregion

        #region IRelationComponent<TItem> explicit interface implementation

        /// <summary>
        /// Explicit implementation of IRelationComponent&lt;TItem&gt;.OnLoadBatch.
        /// </summary>
        async Task IRelationComponent<TItem>.OnLoadBatch(IEnumerable<TItem> dataSource, QueryModel<TItem> queryModel)
        {
            await OnLoadBatch(dataSource, queryModel);
        }

        /// <summary>
        /// Explicit implementation of IRelationComponent&lt;TItem&gt;.RenderCell.
        /// </summary>
        RenderFragment IRelationComponent<TItem>.RenderCell(TItem rowData)
        {
            var fieldValue = GetFieldValue(rowData);
            return RenderContent(fieldValue, rowData);
        }

        #endregion

        #region IRelationComponent non-generic interface implementation

        /// <summary>
        /// Explicit implementation of IRelationComponent.SetDataSource.
        /// </summary>
        void IRelationComponent.SetDataSource(IEnumerable dataSource, QueryModel queryModel)
        {
            _cachedDataSource = dataSource.Cast<TItem>();
            _cachedQueryModel = queryModel as QueryModel<TItem>;
        }

        /// <summary>
        /// Explicit implementation of IRelationComponent.OnLoadBatchAsync.
        /// </summary>
        async Task IRelationComponent.OnLoadBatchAsync()
        {
            if (_cachedDataSource != null)
            {
                await OnLoadBatch(_cachedDataSource, _cachedQueryModel);
                //Table.Refresh();
            }
        }

        /// <summary>
        /// Explicit implementation of IRelationComponent.RenderCell.
        /// </summary>
        RenderFragment IRelationComponent.RenderCell(RowData rowData)
        {
            var typedRowData = ((RowData<TItem>)rowData).Data;
            var fieldValue = GetFieldValue(typedRowData);
            return RenderContent(fieldValue, typedRowData);
        }

        #endregion
    }
}
