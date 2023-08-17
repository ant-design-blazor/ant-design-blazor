---
order: 6.55
title:
  zh-CN: �Զ���ɸѡ��
  en-US: Custom field filter
---

## zh-CN

�������Ͳ�������ɸѡ��֧�ֵ�����ʱ�����������޸�ɸѡ���ıȽϲ���������ʹ���Զ���ɸѡ�� `FieldFilterType`��

�ڱ�ʾ���У�Color �п��԰�"����"ɸѡ ����HUEֵ���򣩡�

�Զ���ɸѡ����Ҫʵ�� `IFieldFilterType`�����߼̳� `BaseFieldFilterType` �� `DateFieldFilterType` ��������д��ط�����

���⣬������ͨ������ `FieldFilterTypeResolver` ����Ϊ���������������Զ���ɸѡ�������߿���ͨ��������ע�������ע�᲻ͬ�� `IFilterTypeResolver` ʵ����Ϊ����Ӧ�ó����еı����������Զ���ɸѡ����`DefaultFieldFilterTypeResolver` ��Ĭ��ʵ�֡�����

## en-US

For types that are not supported by the built-in filters, or when you want different semantics like customizing the default operator, you can specify a custom `FieldFilterType` on the column.

In this example, the Color column can be filtered by the colors brightness (or sorted by its hue).

The custom Filter Types should implement `IFieldFilterType`, but may use the provided `BaseFieldFilterType` or `DateFieldFilterType` as a base implementation.

Your custom filter types can also be applied for a whole table, by setting its `FieldFilterTypeResolver` property, or globally by registering a different `IFilterTypeResolver` service. (See `DefaultFieldFilterTypeResolver` as a reference/fallback implementation.)
