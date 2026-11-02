// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Threading;
using Xunit;

namespace AntDesign.Tests.LocaleProvider
{
    public class LocaleProviderTests
    {
        [Fact]
        public void SetLocale_does_not_change_process_wide_default_thread_culture()
        {
            var originalDefaultUICulture = CultureInfo.DefaultThreadCurrentUICulture;
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                const int MaxAttempts = 10;
                for (var attempt = 1; attempt <= MaxAttempts; attempt++)
                {
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                    AntDesign.LocaleProvider.SetLocale("zh-CN");

                    var observed = CultureInfo.DefaultThreadCurrentUICulture.Name;

                    if (observed == "zh-CN")
                    {
                        Assert.Fail("LocaleProvider.SetLocale must not mutate the process-wide CultureInfo.DefaultThreadCurrentUICulture.");
                    }

                    if (observed == "en-US")
                    {
                        return;
                    }

                    // Some other concurrently-running test mutated the shared static in the middle
                    // of this attempt (unrelated noise, e.g. AddAntDesign() capturing the environment's
                    // default locale) - retry.
                }

                Assert.Fail("Could not obtain a conclusive result due to unrelated concurrent test interference.");
            }
            finally
            {
                CultureInfo.DefaultThreadCurrentUICulture = originalDefaultUICulture;
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [Fact]
        public void SetLocale_in_one_circuit_does_not_leak_into_a_brand_new_thread()
        {
            // Simulates the reported scenario: user A (existing circuit) switches language while
            // user B's request runs on a brand-new thread that never flowed an ExecutionContext
            // from user A (e.g. a new SignalR circuit dispatch). A thread with no explicit culture
            // falls back to CultureInfo.DefaultThreadCurrentUICulture, so that static must remain
            // untouched by SetLocale.
            //
            // Note: other unrelated tests in this assembly legitimately touch
            // CultureInfo.DefaultThreadCurrentUICulture too (e.g. AddAntDesign() captures the
            // environment's default locale once at "startup", which bUnit exercises per test). To
            // keep this test deterministic under parallel test execution, we retry a few times and
            // only fail when the leaked value is unambiguously the culture set via SetLocale below.
            var originalDefaultUICulture = CultureInfo.DefaultThreadCurrentUICulture;
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                const int MaxAttempts = 10;
                for (var attempt = 1; attempt <= MaxAttempts; attempt++)
                {
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");

                    AntDesign.LocaleProvider.SetLocale("zh-CN");

                    string? cultureOnNewThread = null;

                    // Suppress ExecutionContext flow so the new thread starts with no explicit
                    // culture, just like an unrelated circuit that never inherited the current
                    // async flow.
                    using (ExecutionContext.SuppressFlow())
                    {
                        var newThread = new Thread(() =>
                        {
                            cultureOnNewThread = CultureInfo.CurrentUICulture.Name;
                        });
                        newThread.Start();
                        newThread.Join();
                    }

                    if (cultureOnNewThread == "zh-CN")
                    {
                        Assert.Fail("CultureInfo.DefaultThreadCurrentUICulture leaked the culture set via LocaleProvider.SetLocale into an unrelated thread.");
                    }

                    if (cultureOnNewThread == "en-US")
                    {
                        return;
                    }

                    // Some other concurrently-running test mutated the shared static in the middle
                    // of this attempt (unrelated noise) - retry.
                }

                Assert.Fail("Could not obtain a conclusive result due to unrelated concurrent test interference.");
            }
            finally
            {
                CultureInfo.DefaultThreadCurrentUICulture = originalDefaultUICulture;
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }

        [Fact]
        public void GetLocale_returns_locale_for_requested_culture()
        {
            var locale = AntDesign.LocaleProvider.GetLocale("zh-CN");

            Assert.NotNull(locale);
            Assert.Equal("zh-CN", locale.CurrentCulture.Name);
        }
    }
}
