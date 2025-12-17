// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Draft monitor component for automatic draft saving and recovery of any data type.
    /// </summary>
    /// <typeparam name="TData">The type of data to monitor</typeparam>
    public partial class DraftMonitor<TData> : AntDomComponentBase
    {
        private TData _currentData;
        private string _currentDataJson;
        private bool _initialized;
        private DraftData<TData> _pendingDraft;

        /// <summary>
        /// Gets or sets the data to monitor.
        /// </summary>
        [Parameter]
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the callback when data changes.
        /// </summary>
        [Parameter]
        public EventCallback<TData> DataChanged { get; set; }

        /// <summary>
        /// Gets or sets whether draft functionality is enabled.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the draft configuration options.
        /// </summary>
        [Parameter]
        public DraftOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the callback when draft is recovered.
        /// </summary>
        [Parameter]
        public EventCallback<TData> OnRecovered { get; set; }

        /// <summary>
        /// Gets or sets the callback when draft is deleted.
        /// </summary>
        [Parameter]
        public EventCallback OnDeleted { get; set; }

        /// <summary>
        /// Gets or sets the callback to update data (optional).
        /// </summary>
        [Parameter]
        public EventCallback<TData> OnUpdate { get; set; }

        [Inject]
        private IDraftService DraftService { get; set; }

        [Inject]
        private ModalService ModalService { get; set; }

        /// <summary>
        /// Gets or sets the localization configuration.
        /// </summary>
        [Parameter]
        public Locales.DraftLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Draft;

        /// <summary>
        /// Gets or sets the custom recovery dialog content template.
        /// </summary>
        [Parameter]
        public RenderFragment<DraftData<TData>> RecoveryContentTemplate { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && Enabled && !_initialized)
            {
                _initialized = true;
                await InitializeDraft();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (_initialized && Enabled && Data != null)
            {
                var newDataJson = JsonSerializer.Serialize(Data);
                if (_currentDataJson != newDataJson)
                {
                    _currentDataJson = newDataJson;
                    _currentData = Data;
                    await SaveDraft();
                }
            }
        }

        private string GetDraftKey()
        {
            return Options?.Key ?? $"draft-monitor-{typeof(TData).Name}";
        }

        private async Task InitializeDraft()
        {
            if (!Enabled || DraftService == null)
                return;

            var key = GetDraftKey();
            var options = Options ?? new DraftOptions { Key = key };
            if (string.IsNullOrEmpty(options.Key))
                options.Key = key;

            var hasDraft = await DraftService.HasDraftAsync(key, options);
            if (hasDraft)
            {
                var draft = await DraftService.LoadDraftAsync<TData>(key, options);
                if (draft != null)
                {
                    // Check if draft should be recovered based on version comparison
                    bool shouldRecover = true;

                    if (options.EnableVersionCheck)
                    {
                        if (options.ShouldRecoverDraft != null)
                        {
                            // Use custom version comparison logic
                            shouldRecover = options.ShouldRecoverDraft(draft.Version, options.Version);
                        }
                        else
                        {
                            // Default: simple equality check
                            shouldRecover = draft.Version == options.Version;
                        }
                    }

                    if (shouldRecover)
                    {
                        _pendingDraft = draft;
                        if (options.RecoveryMode == DraftRecoveryMode.Auto)
                        {
                            await RecoverDraft();
                        }
                        else if (options.RecoveryMode == DraftRecoveryMode.Confirm)
                        {
                            await ShowRecoveryConfirm(draft);
                        }
                    }
                    else
                    {
                        // Draft version doesn't match, clear it
                        await DraftService.RemoveDraftAsync(key, options);
                    }
                }
            }

            if (Data != null)
            {
                _currentData = Data;
                _currentDataJson = JsonSerializer.Serialize(Data);
            }
        }

        private Task ShowRecoveryConfirm(DraftData<TData> draft)
        {
            if (ModalService == null)
                return Task.CompletedTask;

            var title = Locale.RecoveryTitle;
            var okText = Locale.RecoveryOkText;
            var cancelText = Locale.RecoveryCancelText;

            var confirmOptions = new ConfirmOptions
            {
                Title = title,
                OkText = okText,
                CancelText = cancelText,
                OkType = ButtonType.Primary,
                OnOk = async (e) => await RecoverDraft(),
                OnCancel = async (e) => await DeleteDraft()
            };

            if (RecoveryContentTemplate != null)
            {
                confirmOptions.Content = RecoveryContentTemplate(draft);
            }
            else
            {
                var content = Locale.RecoveryContent;
                var savedTime = draft.SavedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                content = content.Replace("${time}", savedTime);
                confirmOptions.Content = content;
            }

            ModalService.Confirm(confirmOptions);
            return Task.CompletedTask;
        }

        private async Task RecoverDraft()
        {
            if (_pendingDraft != null)
            {
                var recoveredData = _pendingDraft.Data;

                if (OnUpdate.HasDelegate)
                {
                    await OnUpdate.InvokeAsync(recoveredData);
                }
                else if (DataChanged.HasDelegate)
                {
                    await DataChanged.InvokeAsync(recoveredData);
                }

                if (OnRecovered.HasDelegate)
                {
                    await OnRecovered.InvokeAsync(recoveredData);
                }

                _currentData = recoveredData;
                _currentDataJson = JsonSerializer.Serialize(recoveredData);
                _pendingDraft = null;

                StateHasChanged();
            }
        }

        private async Task DeleteDraft()
        {
            var key = GetDraftKey();
            var options = Options ?? new DraftOptions { Key = key };
            if (string.IsNullOrEmpty(options.Key))
                options.Key = key;

            await DraftService.RemoveDraftAsync(key, options);

            if (OnDeleted.HasDelegate)
            {
                await OnDeleted.InvokeAsync(null);
            }

            _pendingDraft = null;
        }

        private async Task SaveDraft()
        {
            if (!Enabled || DraftService == null || Data == null)
                return;

            var key = GetDraftKey();
            var options = Options ?? new DraftOptions { Key = key };
            if (string.IsNullOrEmpty(options.Key))
                options.Key = key;

            await DraftService.SaveDraftAsync(key, Data, options);
        }

        /// <summary>
        /// Manually save the draft immediately.
        /// </summary>
        public async Task SaveDraftAsync()
        {
            if (!Enabled || DraftService == null || Data == null)
                return;

            var key = GetDraftKey();
            var options = Options ?? new DraftOptions { Key = key };
            if (string.IsNullOrEmpty(options.Key))
                options.Key = key;

            await DraftService.SaveDraftImmediateAsync(key, Data, options);
        }

        /// <summary>
        /// Manually clear the draft.
        /// </summary>
        public async Task ClearDraftAsync()
        {
            var key = GetDraftKey();
            var options = Options ?? new DraftOptions { Key = key };
            if (string.IsNullOrEmpty(options.Key))
                options.Key = key;

            await DraftService.RemoveDraftAsync(key, options);
        }
    }
}
