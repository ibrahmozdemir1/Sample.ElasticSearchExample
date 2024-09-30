using Sample.ElasticSearchExample.Models;

namespace Sample.ElasticSearchExample.Services
{
    public interface IElasticService
    {
        // CREATE INDEX

        Task CreateIndexIfNotExistsAsync(string indexName);

        Task<bool> AddOrUpdateAsync(User user);

        Task<bool> AddOrUpdateBulkAsync(IEnumerable<User> users,string indexName);

        Task<User> GetAsync(string key);

        Task<List<User>?> GetAllAsync();

        Task<bool> RemoveAsync(string key);

        Task<long?> RemoveAllAsync();
    }
}
