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
using Microsoft.Extensions.Options;

namespace AntDesign.Extensions.Localization
{
    internal class InteractiveStringLocalizer<TResourceSource>(IStringLocalizerFactory factory, ILocalizationService LocalizationService) : InteractiveStringLocalizer(factory, LocalizationService),
        IStringLocalizer<TResourceSource>,
        IStringLocalizer
    {
        public override void LanguageChanged(object sender, CultureInfo culture)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }
    }

    internal class InteractiveStringLocalizer : IStringLocalizer, IDisposable
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ILocalizationService _LocalizationService;
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

        public InteractiveStringLocalizer(IStringLocalizerFactory factory, ILocalizationService LocalizationService)
        {
            _factory = factory;
            _LocalizationService = LocalizationService;
            _languageChanged = LanguageChanged;

            _LocalizationService.LanguageChanged += _languageChanged;

            if (_localizer == null)
            {
                LanguageChanged(this, CultureInfo.CurrentCulture);
            }
        }

        public InteractiveStringLocalizer(IOptions<BlazorLocalizationOptions> options, IStringLocalizerFactory factory, ILocalizationService LocalizationService) : this(factory, LocalizationService)
        {
            _options = options;

            _languageChanged = (object sender, CultureInfo culture) =>
            {
                _localizer = _factory.Create(_options.Value.ResourcesPath, _options.Value.ResourcesAssembly.GetName().Name);
            };

            if (_localizer == null)
            {
                _languageChanged.Invoke(this, CultureInfo.CurrentCulture);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }

        public virtual void LanguageChanged(object sender, CultureInfo culture)
        {
        }

        public void Dispose()
        {
            _LocalizationService.LanguageChanged -= _languageChanged;
        }
    }
}
