// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using AntDesign.TableModels;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class QueryModelJsonConverterTests
    {
        [Fact]
        public void Deserialize_ReadsPaginationProperties_UnderTurkishCulture()
        {
            var originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

                var json = JsonSerializer.Serialize(
                    new QueryModel<int>(3, 10, 25, new List<ITableSortModel>(), new List<ITableFilterModel>()));

                var model = JsonSerializer.Deserialize<QueryModel<int>>(json);

                Assert.Equal(3, model.PageIndex);
                Assert.Equal(10, model.PageSize);
                Assert.Equal(25, model.StartIndex);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }
    }
}
