// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace AntDesign.Extensions.Localization
{
    internal sealed class LocalizationService : ILocalizationService
    {
        private CultureInfo? _currentCulture;

        public CultureInfo CurrentCulture => _currentCulture ?? CultureInfo.CurrentCulture;

        public event EventHandler<CultureInfo> LanguageChanged = default!;

        public void SetLanguage(CultureInfo culture)
        {
            if (!culture.Equals(CultureInfo.CurrentCulture))
            {
                CultureInfo.CurrentCulture = culture;
            }

            if (_currentCulture == null || !_currentCulture.Equals(culture))
            {
                _currentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;

                LanguageChanged?.Invoke(this, culture);
            }
        }
    }
}
