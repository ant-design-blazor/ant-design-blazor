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

            _currentCulture = culture;
            // Do NOT set CultureInfo.DefaultThreadCurrentCulture/DefaultThreadCurrentUICulture: they are
            // process-wide static defaults. This service is scoped per circuit in Blazor Server, so
            // mutating those statics here would leak the language change into every other user's circuit.
            CultureInfo.CurrentUICulture = culture;

            // A localizer may have been created before the route selected its locale. Notify on every
            // explicit SetLanguage call so it can rebind even when the service already has this culture.
            LanguageChanged?.Invoke(this, culture);
        }
    }
}
