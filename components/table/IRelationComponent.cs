// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Relation component interface (internal use, non-generic base interface)
    /// </summary>
    /// <remarks>
    /// This interface is used by the Table component to uniformly manage relation components with different TItem types.
    /// Through the non-generic interface, the Table can call all relation components without knowing the specific TItem type.
    /// </remarks>
    internal interface IRelationComponent
    {
        /// <summary>
        /// Set the data source (called before OnLoadBatchAsync)
        /// </summary>
        /// <param name="dataSource">The table's data source</param>
        /// <param name="queryModel">Query model containing pagination, sorting, filtering information, etc.</param>
        /// <remarks>
        /// The Table will call this method after data is loaded, passing the currently displayed data source to the relation component.
        /// The relation component should cache this data for use in OnLoadBatchAsync.
        /// </remarks>
        void SetDataSource(IEnumerable dataSource, QueryModel queryModel);

        /// <summary>
        /// Batch load relation data (non-generic version, for unified Table invocation)
        /// </summary>
        /// <returns>Async loading task</returns>
        /// <remarks>
        /// The Table will call this method on all registered relation components in parallel.
        /// The relation component should execute data loading logic in this method and store results in SharedCache.
        /// </remarks>
        Task OnLoadBatchAsync();

        /// <summary>
        /// Render cell (non-generic version)
        /// </summary>
        /// <param name="rowData">Row data wrapper object</param>
        /// <returns>Render fragment for the cell</returns>
        /// <remarks>
        /// This method is called when the Table renders each cell.
        /// The relation component should retrieve related data from cache based on field values in rowData and render it.
        /// </remarks>
        RenderFragment RenderCell(RowData rowData);
    }

    /// <summary>
    /// Generic relation component interface
    /// </summary>
    /// <typeparam name="TItem">Table row data type</typeparam>
    /// <remarks>
    /// This interface provides type-safe APIs for relation components.
    /// Developers implement relation components by inheriting <see cref="RelationComponentBase{TItem, TData}"/>,
    /// without needing to implement this interface directly.
    /// </remarks>
    /// <example>
    /// Typically this interface is not used directly, instead inherit from RelationComponentBase:
    /// <code>
    /// public class UserNameRelation&lt;TItem&gt; : RelationComponentBase&lt;TItem, int&gt;
    /// {
    ///     protected override async Task OnLoadBatch(IEnumerable&lt;int&gt; userIds)
    ///     {
    ///         // Load user data
    ///     }
    ///     
    ///     protected override RenderFragment RenderContent(int userId, TItem rowData)
    ///     {
    ///         // Render user name
    ///     }
    /// }
    /// </code>
    /// </example>
    internal interface IRelationComponent<TItem> : IRelationComponent
    {
        /// <summary>
        /// Batch load relation data (generic version)
        /// </summary>
        /// <param name="dataSource">The table's typed data source</param>
        /// <param name="queryModel">Query model containing pagination, sorting, filtering information, etc.</param>
        /// <returns>Async loading task</returns>
        /// <remarks>
        /// This method provides type-safe data access.
        /// The relation component can access complete row data in this method to perform complex relation data queries.
        /// </remarks>
        Task OnLoadBatch(IEnumerable<TItem> dataSource, QueryModel queryModel);

        /// <summary>
        /// Render cell (generic version)
        /// </summary>
        /// <param name="rowData">Typed row data</param>
        /// <returns>Render fragment for the cell</returns>
        /// <remarks>
        /// This method provides type-safe row data access.
        /// The relation component can directly access strongly-typed properties of rowData.
        /// </remarks>
        RenderFragment RenderCell(TItem rowData);
    }
}
