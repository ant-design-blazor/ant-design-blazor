---
order: 10.1
title:
  en-US: OData Querying
  zh-CN: OData 查询
---

## zh-CN

由于 `QueryModel` 已经提供了查询所需的所有信息（页码、排序、筛选），所以可以很好地支持 OData 等查询协议。

> 推荐使用 [OData Client](https://learn.microsoft.com/en-us/odata/client/getting-started?WT.mc_id=DT-MVP-5003987) 方式结合 `IQueryable<T>` 的扩展方法 `ExecuteTableQuery` 进行流畅地查询。
> 但需要解决一个[已知问题](https://github.com/OData/odata.net/issues/2544)。
>  ```
>    DefaultContainer dsc = new DefaultContainer(new Uri("https://services.odata.org/V4/(S(uvf1y321yx031rnxmcbqmlxw))/TripPinServiceRW/"));
>    var _data = dsc.People.ExecuteTableQuery(queryModel).ToArray();
>  ```

## en-US

Since the `QueryModel` already provides all the information needed for a query (page number, sort, filter), query protocols such as OData are well supported. 

> It is recommended to use [OData Client](https://learn.microsoft.com/en-us/odata/client/getting-started?WT.mc_id=DT-MVP-5003987) and the 'IQueryable<T>' extension method 'ExecuteTableQuery' performs fluid queries. 
> But there's a [known problem](https://github.com/OData/odata.net/issues/2544) that needs to be solved.
>  ```
>    DefaultContainer dsc = new DefaultContainer(new Uri("https://services.odata.org/V4/(S(uvf1y321yx031rnxmcbqmlxw))/TripPinServiceRW/"));
>    var _data = dsc.People.ExecuteTableQuery(queryModel).ToArray();
>  ```