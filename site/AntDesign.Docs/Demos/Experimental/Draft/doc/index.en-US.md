---
category: Experimental
type: Data Entry
title: Draft
subtitle: Draft Auto Save
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/3StSdUlSH/Modal.svg
---

Automatic draft saving to prevent data loss from accidental page closures.

## When To Use

- Long forms with risk of data loss
- Complex configuration editing
- Offline editing with local storage

## API

### DraftMonitor

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Data | Data to monitor | TData | - |
| DataChanged | Change callback | EventCallback&lt;TData&gt; | - |
| Options | Draft options | DraftOptions | - |
| Enabled | Enable draft | bool | true |
| Locale | Localization | DraftLocale | - |
| RecoveryContentTemplate | Custom dialog template | RenderFragment&lt;DraftData&lt;TData&gt;&gt; | - |
| OnRecovered | Recovery callback | EventCallback&lt;TData&gt; | - |
| OnDeleted | Deletion callback | EventCallback | - |

**Methods:**
- `SaveDraftAsync()` - Manually save draft
- `ClearDraftAsync()` - Clear draft

### DraftOptions

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Key | Draft identifier | string | - |
| DelayMilliseconds | Save delay (ms) | int | 1000 |
| RecoveryMode | Recovery mode | DraftRecoveryMode | Confirm |
| Version | Version number | string | "1.0.0" |
| EnableVersionCheck | Version check | bool | true |
| Enabled | Enable draft | bool | true |
| StorageProvider | Storage provider | IDraftStorageProvider | null |
| ShouldRecoverDraft | Custom version comparison function. (draftVersion, currentVersion) => bool | Func&lt;string, string, bool&gt; | null |

### DraftRecoveryMode

| Value | Description |
| --- | --- |
| Confirm | Show confirmation dialog |
| Auto | Auto-recover |
| Manual | Manual handling |

### DraftLocale

| Property | Description | Default |
| --- | --- | --- |
| RecoveryTitle | Dialog title | "Draft Found" |
| RecoveryContent | Dialog content template (supports `{time}` placeholder) | "An unfinished draft was detected. Would you like to recover it?\n\nSaved at: {time}" |
| RecoveryOkText | Confirm button | "Recover" |
| RecoveryCancelText | Cancel button | "Delete" |

## Advanced

### Custom Storage

```csharp
public class DbDraftProvider : IDraftStorageProvider
{
    public async Task SaveAsync(string key, string data) { /* DB save */ }
    public async Task<string> LoadAsync(string key) { /* DB load */ }
    public async Task RemoveAsync(string key) { /* DB remove */ }
    public async Task<bool> ExistsAsync(string key) { /* DB check */ }
}

builder.Services.AddAntDesign();
builder.Services.AddScoped<IDraftStorageProvider, DbDraftProvider>();
```

### Using IDraftService Directly

```csharp
@inject IDraftService DraftService

@code {
    private async Task SaveDraft()
    {
        await DraftService.SaveDraftAsync("my-key", myData, options);
    }
    
    private async Task<MyData> LoadDraft()
    {
        var draft = await DraftService.LoadDraftAsync<MyData>("my-key", options);
        return draft?.Data;
    }
    
    private async Task RemoveDraft()
    {
        await DraftService.RemoveDraftAsync("my-key");
    }
}
```

## Notes

1. **Version Control**: Update version when data structure changes
2. **Storage Limits**: LocalStorage ~5-10MB, use custom provider for larger data
3. **Security**: Don't store sensitive data or use encryption
4. **Cleanup**: Regularly clean expired drafts
5. **Multi-Tab**: LocalStorage shared across tabs, use unique keys if needed
