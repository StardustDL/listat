using Listat.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Listat
{
    public interface IListatService
    {
        Task<string?> Create(Statistic statistic, CancellationToken cancellationToken = default);

        Task<IList<Statistic>> Query(StatisticQuery query, CancellationToken cancellationToken = default);

        Task<Statistic?> Get(string id, CancellationToken cancellationToken = default);

        Task<bool> Delete(string id, CancellationToken cancellationToken = default);

        Task<bool> Update(Statistic statistic, CancellationToken cancellationToken = default);
    }

    public class ListatService : IListatService
    {
        public ListatService(HttpClient client) => Client = client;

        public HttpClient Client { get; }

        public async Task<string?> Create(Statistic statistic, CancellationToken cancellationToken = default)
        {
            var response = await Client.PostAsJsonAsync("/", statistic, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> Delete(string id, CancellationToken cancellationToken = default)
        {
            var response = await Client.DeleteAsync($"/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }

        public async Task<Statistic?> Get(string id, CancellationToken cancellationToken = default)
        {
            var response = await Client.GetAsync($"/{Uri.EscapeDataString(id)}", cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Statistic?>(cancellationToken: cancellationToken);
        }

        public async Task<IList<Statistic>> Query(StatisticQuery query, CancellationToken cancellationToken = default)
        {
            var response = await Client.PostAsJsonAsync("/query", query, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<IList<Statistic>>(cancellationToken: cancellationToken);
            if(result is null)
            {
                return Array.Empty<Statistic>();
            }
            else
            {
                return result;
            }
        }

        public async Task<bool> Update(Statistic statistic, CancellationToken cancellationToken = default)
        {
            var response = await Client.PutAsJsonAsync($"/{Uri.EscapeDataString(statistic.Id)}", statistic, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken);
        }
    }
}
