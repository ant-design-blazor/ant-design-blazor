---
category: Components
subtitle: Global configuration
type: Other
title: ConfigProvider
---

ConfigProvider provides shared direction, theme, and form configuration for a component tree.

## When to use

Use it when an application needs consistent Ant Design theme, direction, or global behavior settings.

## API

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Direction | Text direction | string | -- |
| Theme | Global theme | GlobalTheme | -- |
| ThemeConfig | Design Token and component theme configuration | ThemeConfig | -- |
| Form | Global form configuration | FormConfig | -- |

## Design Token

ConfigProvider does not define component-specific style tokens. It provides the scope for global Seed, Map, and Alias tokens, preset algorithms (`default`, `dark`, `compact`), and per-component token overrides. See [Customize Theme](/docs/customize-theme) for the v6 token model and examples.
