// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace AntDesign.Docs.Localization
{
    internal class BlazorStringLocalizer<TResourceSource>(IStringLocalizerFactory factory, ILanguageService languageService) : BlazorStringLocalizer(factory, languageService),
        IStringLocalizer<TResourceSource>,
        IStringLocalizer
    {
        public override void LanguageChanged(object sender, CultureInfo culture)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }
    }

    internal class BlazorStringLocalizer : IStringLocalizer
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ILanguageService _languageService;

        protected IStringLocalizer _localizer;

        private EventHandler<CultureInfo> _languageChanged;

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

        public BlazorStringLocalizer(IStringLocalizerFactory factory, ILanguageService languageService)
        {
            _factory = factory;
            _languageService = languageService;

            _languageService.LanguageChanged += LanguageChanged;

            LanguageChanged(this, CultureInfo.DefaultThreadCurrentUICulture);
        }

        public BlazorStringLocalizer(string baseName, string location, IStringLocalizerFactory factory, ILanguageService languageService)
        {
            _factory = factory;
            _languageService = languageService;

            _localizer = _factory.Create(baseName, location);

            _languageChanged = (_, _) =>
            {
                _localizer = _factory.Create(baseName, location);
            };

            _languageService.LanguageChanged += _languageChanged;
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        public virtual void LanguageChanged(object sender, CultureInfo culture)
        {
        }
    }
}
