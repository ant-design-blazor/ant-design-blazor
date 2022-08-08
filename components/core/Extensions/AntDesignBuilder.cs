// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign.core.Extensions
{
    public class AntDesignBuilder
    {
        private readonly IServiceCollection _services;
        internal AntDesignBuilder(IServiceCollection services)
        {
            _services = services;
            //添加一个默认接口 ICustomHttpHeaders
            AddCustomHttpHeaders<DefaultCustomHttpHeaders>();
        }

        public AntDesignBuilder AddCustomHttpHeaders<T>() where T : class
        {
            if (!typeof(ICustomHttpHeaders).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("AddCustomHttpHeaders必须是实现ICustomHttpHeaders的类");
            }
            //判断是否有默认实现，如果有，则替换它
            var tmp = _services.FirstOrDefault(s => s.ServiceType == typeof(ICustomHttpHeaders));
            if (tmp != null)
            {
                _services.Remove(tmp);
            }
            _services.AddScoped(typeof(ICustomHttpHeaders), typeof(T));

            return this;
        }

    }
    #region 允许自定义HTTP头
    /// <summary>
    /// 自定义HTTP头
    /// </summary>
    public interface ICustomHttpHeaders
    {
        Task<Dictionary<string, string>> GetCustomHeaders();
    }

    public class DefaultCustomHttpHeaders : ICustomHttpHeaders
    {
        public Task<Dictionary<string, string>> GetCustomHeaders()
        {
            return Task.FromResult(new Dictionary<string, string>());
        }
    }
    #endregion
}
