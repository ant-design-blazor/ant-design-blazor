---
order: 5
title:
  zh-CN: 版本号对比
  en-US: Version Comparison
---

## zh-CN

通过自定义版本比较函数,实现只恢复版本号相同或更新的草稿,自动删除过期草稿。

### 使用场景

1. **防止旧数据覆盖新数据**:服务器数据已更新,避免用户恢复旧草稿覆盖最新数据
2. **多用户协作**:在多人编辑场景下,确保只恢复当前版本的草稿
3. **版本化管理**:基于版本号进行草稿管理,自动清理不兼容的旧草稿

### 测试步骤

1. 编辑文档内容,等待自动保存草稿
2. 点击"模拟服务器更新(版本+1)"按钮,模拟服务器端数据版本升级
3. 刷新页面,旧版本草稿将被自动删除(不会弹出恢复对话框)

### 核心配置

```csharp
private DraftOptions draftOptions => new DraftOptions
{
    Version = formData.Version.ToString(),
    EnableVersionCheck = true,
    // 自定义版本比较:只有草稿版本 >= 当前版本才恢复
    ShouldRecoverDraft = (draftVer, currentVer) =>
    {
        if (int.TryParse(draftVer, out var dv) && 
            int.TryParse(currentVer, out var cv))
        {
            return dv >= cv; // 草稿版本必须 >= 当前版本
        }
        return false;
    }
};
```

## en-US

Implement custom version comparison to only recover drafts with matching or newer versions, automatically deleting outdated drafts.

### Use Cases

1. **Prevent Old Data Overwrite**: Server data has been updated, prevent users from recovering old drafts that would overwrite the latest data
2. **Multi-User Collaboration**: In collaborative editing scenarios, ensure only current version drafts are recovered
3. **Version-Based Management**: Manage drafts based on version numbers, automatically clean up incompatible old drafts

### Test Steps

1. Edit the document content and wait for auto-save
2. Click "Simulate Server Update (Version++)" to simulate server-side data version upgrade
3. Refresh the page - the old version draft will be automatically deleted (no recovery dialog)

### Core Configuration

```csharp
private DraftOptions draftOptions => new DraftOptions
{
    Version = formData.Version.ToString(),
    EnableVersionCheck = true,
    // Custom version comparison: only recover if draft version >= current version
    ShouldRecoverDraft = (draftVer, currentVer) =>
    {
        if (int.TryParse(draftVer, out var dv) && 
            int.TryParse(currentVer, out var cv))
        {
            return dv >= cv; // Draft version must be >= current version
        }
        return false;
    }
};
```
