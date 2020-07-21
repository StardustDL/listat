# listat

![CI](https://github.com/StardustDL/listat/workflows/CI/badge.svg) ![CD](https://github.com/StardustDL/listat/workflows/CD/badge.svg) ![License](https://img.shields.io/github/license/StardustDL/listat.svg) [![Listat](https://buildstats.info/nuget/Listat)](https://www.nuget.org/packages/Listat/)

Listat is a Light STATistic service.

## API

- **Post** `/` with *Statistic* body: Create statistic, return id
- **Post** `/query` with *StatisticQuery* body: Query statistics, return list of statistics
- **Post** `/count` with *StatisticQuery* body: Query and count statistics, return the number of statistics
- **Get** `/id`: Get statistic by id, return statistic
- **Delete** `/id`: Delete statistic by id, return if done
- **Put** `/id` with *Statistic* body: Update statistic by id, return if done

## Models

```go
type Statistic struct {
	Id               string
	CreationTime     time.Time
	ModificationTime time.Time
	Payload          string
	Uri              string
	Category         string
}

type StatisticQuery struct {
	Id               string
	CreationTime     time.Time
	ModificationTime time.Time
	Payload          string
	Uri              string
	Category         string
	Offset           int
	Limit            int
}
```

## SDK

For C#.

```sh
dotnet add package Listat
```

API:

```csharp
public interface IListatService
{
    Task<string?> Create(Statistic statistic, CancellationToken cancellationToken = default);

	Task<long> Count(StatisticQuery query, CancellationToken cancellationToken = default);

    Task<IList<Statistic>> Query(StatisticQuery query, CancellationToken cancellationToken = default);

    Task<Statistic?> Get(string id, CancellationToken cancellationToken = default);

    Task<bool> Delete(string id, CancellationToken cancellationToken = default);

    Task<bool> Update(Statistic statistic, CancellationToken cancellationToken = default);
}
```

## Status

![](https://buildstats.info/github/chart/StardustDL/listat?branch=master)