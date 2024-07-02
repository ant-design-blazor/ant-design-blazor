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
    internal class InteractiveStringLocalizer<TResourceSource>(IStringLocalizerFactory factory, ILocalizationService localizationService) : InteractiveStringLocalizer(factory, localizationService),
        IStringLocalizer<TResourceSource>,
        IStringLocalizer
    {
        public override void LanguageChanged(object sender, CultureInfo culture)
        {
            _localizer = factory.Create(typeof(TResourceSource));
        }
    }

    /// <summary>
    /// Support change the localizer at runtime. 
    /// </summary>
    internal class InteractiveStringLocalizer : IStringLocalizer, IDisposable
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ILocalizationService _localizationService;
        private readonly IOptions<SimpleStringLocalizerOptions> _options;
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

        public InteractiveStringLocalizer(IStringLocalizerFactory factory, ILocalizationService localizationService)
        {
            _factory = factory;
            _localizationService = localizationService;
            _languageChanged = LanguageChanged;

            _localizationService.LanguageChanged += _languageChanged;

            if (_localizer == null)
            {
                LanguageChanged(this, CultureInfo.CurrentCulture);
            }
        }

        public InteractiveStringLocalizer(IOptions<SimpleStringLocalizerOptions> options, IStringLocalizerFactory factory, ILocalizationService localizationService) : this(factory, localizationService)
        {
            _options = options;

            _languageChanged = (object sender, CultureInfo culture) =>
            {
                _localizer = _factory.Create(_options.Value.ResourcesPath, _options.Value.ResourcesAssembly.GetName().Name);
            };

            _localizationService.LanguageChanged += _languageChanged;

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
            _localizationService.LanguageChanged -= _languageChanged;
        }
    }
}
