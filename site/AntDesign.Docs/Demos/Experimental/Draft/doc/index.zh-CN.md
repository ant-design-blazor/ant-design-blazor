---
category: Experimental
type: 数据录入
title: Draft
subtitle: 草稿自动保存
cols: 1
cover: https://gw.alipayobjects.com/zos/alicdn/3StSdUlSH/Modal.svg
---

提供自动草稿保存功能，防止用户因意外关闭页面而丢失数据。

## 何时使用

- 需要填写长表单，防止数据丢失
- 需要编辑复杂配置，支持草稿恢复
- 需要离线编辑，本地保存草稿

## API

### DraftMonitor

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Data | 要监控的数据 | TData | - |
| DataChanged | 数据变更回调 | EventCallback<TData> | - |
| Options | 草稿配置选项 | DraftOptions | - |
| Enabled | 是否启用 | bool | true |
| Locale | 本地化配置 | DraftLocale | LocaleProvider.CurrentLocale.Draft |
| RecoveryContentTemplate | 自定义恢复对话框内容模板 | RenderFragment<DraftData<TData>> | - |
| OnRecovered | 草稿恢复回调 | EventCallback<TData> | - |
| OnDeleted | 草稿删除回调 | EventCallback | - |

### DraftLocale

| 参数 | 说明 | 类型 | 默认值 (zh-CN / en-US) |
| --- | --- | --- | --- |
| RecoveryTitle | 恢复确认框标题 | string | "发现草稿" / "Draft Found" |
| RecoveryContent | 恢复确认框内容 | string | "检测到未完成的草稿，是否恢复？" / "An unfinished draft was detected..." |
| RecoveryOkText | 恢复按钮文本 | string | "恢复" / "Recover" |
| RecoveryCancelText | 删除按钮文本 | string | "删除" / "Delete" |
| SavedAtLabel | 保存时间标签 | string | "保存时间" / "Saved at" |

### DraftOptions

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Key | 草稿唯一标识符 | string | - |
| DelayMilliseconds | 延迟保存时长（毫秒） | int | 1000 |
| RecoveryMode | 恢复模式 | DraftRecoveryMode | Confirm |
| Version | 版本号 | string | "1.0.0" |
| EnableVersionCheck | 是否启用版本检查 | bool | true |
| Enabled | 是否启用草稿功能 | bool | true |
| StorageProvider | 自定义存储提供者 | IDraftStorageProvider | null |
| ShouldRecoverDraft | 自定义版本比较函数。(draftVersion, currentVersion) => bool | Func&lt;string, string, bool&gt; | null |

### DraftRecoveryMode

| 值 | 说明 |
| --- | --- |
| Confirm | 弹出确认框，让用户选择是否恢复 |
| Auto | 自动恢复草稿 |
| Manual | 不自动恢复，需要手动处理 |

### DraftMonitor 方法

| 方法 | 说明 | 参数 | 返回值 |
| --- | --- | --- | --- |
| SaveDraftAsync | 手动立即保存草稿 | - | Task |
| ClearDraftAsync | 手动清除草稿 | - | Task |

## 高级用法

### 自定义存储提供者

```csharp
public class DatabaseDraftProvider : IDraftStorageProvider
{
    private readonly MyDbContext _db;
    
    public async Task SaveAsync(string key, string data)
    {
        // 保存到数据库
    }
    
    public async Task<string> LoadAsync(string key)
    {
        // 从数据库加载
    }
    
    public async Task RemoveAsync(string key)
    {
        // 从数据库删除
    }
    
    public async Task<bool> ExistsAsync(string key)
    {
        // 检查是否存在
    }
}

// 注册自定义提供者
builder.Services.AddAntDesign();
builder.Services.AddScoped<IDraftStorageProvider, DatabaseDraftProvider>();
```

### 直接使用 IDraftService

```csharp
@inject IDraftService DraftService

@code {
    private async Task SaveDraft()
    {
        await DraftService.SaveDraftAsync("key", data, options);
    }
    
    private async Task LoadDraft()
    {
        var draft = await DraftService.LoadDraftAsync<MyData>("key", options);
    }
}
```

## 注意事项

1. **版本控制**：建议为每个草稿设置版本号，当数据结构变更时更新版本号，避免恢复不兼容的旧草稿。

2. **存储限制**：LocalStorage 通常有 5-10MB 的存储限制，对于大型数据建议使用自定义存储提供者。

3. **敏感数据**：不要在草稿中存储敏感信息（如密码），或使用加密的自定义存储提供者。

4. **定期清理**：建议定期清理过期的草稿数据，避免占用过多存储空间。

5. **多标签页**：当前实现基于 LocalStorage，多个标签页会共享同一个草稿。如需隔离，请使用带有标签页标识的 Key。
