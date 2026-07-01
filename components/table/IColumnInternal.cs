// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.TableModels;

namespace AntDesign
{
    /// <summary>
    /// Internal column interface for registering relation components.
    /// </summary>
    /// <remarks>
    /// This interface is used for internal communication between Column and Table components.
    /// It should not be used directly by external code.
    /// </remarks>
    internal interface IColumnInternal
    {

        internal IRelationComponent GetRelationComponent();

        internal void SetRelationComponent(IRelationComponent relationComponent);
    }

    /// <summary>
    /// Generic internal column interface.
    /// </summary>
    /// <typeparam name="TData">Column data type (field value type)</typeparam>
    /// <remarks>
    /// This interface inherits from <see cref="IColumnInternal"/> and provides type-safe field value accessors.
    /// Relation components can use this interface to get strongly-typed GetValue delegates, avoiding type conversions.
    /// </remarks>
    /// <example>
    /// Usage in RelationComponentBase:
    /// <code>
    /// if (Column is IColumnInternal&lt;TData&gt; columnInternal)
    /// {
    ///     var getValue = columnInternal.GetValueTyped;
    ///     var fieldValue = getValue(rowData);
    /// }
    /// </code>
    /// </example>
    internal interface IColumnInternal<TData> : IColumnInternal
    {
        /// <summary>
        /// Gets the field value accessor (generic version).
        /// </summary>
        /// <remarks>
        /// This property returns a strongly-typed delegate for extracting field values from RowData.
        /// Compared to the non-generic version, this provides type safety and avoids boxing/unboxing operations.
        /// </remarks>
        //Func<RowData, TData> GetValueTyped { get; }
    }
}
