// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Extensions.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace AntDesign.Docs.Localization
{
    internal class BlazorStringLocalizer<TResourceSource>(IOptions<BlazorLocalizationOptions> options, IStringLocalizerFactory factory, ILanguageService languageService) : BlazorStringLocalizer(options, factory, languageService),
        IStringLocalizer<TResourceSource>,
        IStringLocalizer
    {
        public override void LanguageChanged(object sender, CultureInfo culture)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }
    }

    internal class BlazorStringLocalizer : IStringLocalizer, IDisposable
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ILanguageService _languageService;
        private readonly IOptions<BlazorLocalizationOptions> _options;
        private readonly EventHandler<CultureInfo> _languageChanged;

        protected IStringLocalizer _localizer;

        public virtual LocalizedString this[string name]
        {
            get
            {
                return _localizer[name];
            }
        }

        public virtual LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                return _localizer[name, arguments];
            }
        }

        public BlazorStringLocalizer(IOptions<BlazorLocalizationOptions> options, IStringLocalizerFactory factory, ILanguageService languageService)
        {
            _factory = factory;
            _languageService = languageService;
            _options = options;

            _languageChanged = LanguageChanged;

            _languageService.LanguageChanged += _languageChanged;

            LanguageChanged(this, CultureInfo.CurrentCulture);
        }


        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        public virtual void LanguageChanged(object sender, CultureInfo culture)
        {
            _localizer = _factory.Create(_options.Value.ResourcesPath, _options.Value.ResourcesAssembly.GetName().Name);
        }

        public void Dispose()
        {
            _languageService.LanguageChanged -= _languageChanged;
        }
    }
}
