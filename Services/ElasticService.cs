using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Sample.ElasticSearchExample.Configuration;
using Sample.ElasticSearchExample.Models;
using System;

namespace Sample.ElasticSearchExample.Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ElasticSettings _elasticSettings;

        public ElasticService(IOptions<ElasticSettings> optionsMonitor)
        {
            _elasticSettings = optionsMonitor.Value;

            var settings = new ElasticsearchClientSettings(new Uri(_elasticSettings.Url))
                //.Authentication()
                .DefaultIndex(_elasticSettings.DefaultIndex);

            _client = new ElasticsearchClient(settings);
        }

        public async Task<bool> AddOrUpdateAsync(User user)
        {
            var response = await _client.IndexAsync(user, idx =>
            {
                idx.Index(_elasticSettings.DefaultIndex)
                .OpType(OpType.Index);
            });

            return response.IsValidResponse;
        }

        public async Task<bool> AddOrUpdateBulkAsync(IEnumerable<User> users, string indexName)
        {
            var response = await _client.BulkAsync(idx =>
            {
                idx.Index(_elasticSettings.DefaultIndex)
                .UpdateMany(users, (ud, u) => ud.Doc(u).DocAsUpsert(true));
            });

            return response.IsValidResponse;
        }

        public async Task CreateIndexIfNotExistsAsync(string indexName)
        {
            if (!_client.Indices.Exists(indexName).Exists)
                await _client.Indices.CreateAsync(indexName);


        }

        public async Task<List<User>?> GetAllAsync()
        {
            var response = await _client.SearchAsync<User>(g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });

            return response.IsValidResponse ? response.Documents.ToList() : default;
        }

        public async Task<User> GetAsync(string key)
        {
            var response = await _client.GetAsync<User>(key, g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });

            return response.Source;
        }

        public async Task<long?> RemoveAllAsync()
        {
            var response = await _client.DeleteByQueryAsync<User>(g =>
            {
                g.Indices(_elasticSettings.DefaultIndex);
            });

            return response.IsValidResponse ? response.Deleted : default;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            var response = await _client.DeleteAsync(key, g =>
            {
                g.Index(_elasticSettings.DefaultIndex);
            });

            return response.IsValidResponse;
        }
    }
}
