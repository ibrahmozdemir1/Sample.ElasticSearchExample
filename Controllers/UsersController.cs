using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample.ElasticSearchExample.Models;
using Sample.ElasticSearchExample.Services;

namespace Sample.ElasticSearchExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IElasticService _elasticService;
        public UsersController(ILogger<UsersController> logger, IElasticService elasticService)
        {
            _logger = logger;
            _elasticService = elasticService;
        }

        [HttpPost("create-index")]

        public async Task<IActionResult> CreateIndex(string indexName)
        {
            await _elasticService.CreateIndexIfNotExistsAsync(indexName);

            return Ok($"Index {indexName} created");
        }

        [HttpPost("add-user")]

        public async Task<IActionResult> AddUser(User user)
        {
            var result = await _elasticService.AddOrUpdateAsync(user);

            return result ? Ok(result) : BadRequest();
        }

        [HttpPost("update-user")]

        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await _elasticService.AddOrUpdateAsync(user);

            return result ? Ok(result) : BadRequest();
        }

        [HttpGet("get-user")]

        public async Task<IActionResult> GetUser(string key)
        {
            var result = await _elasticService.GetAsync(key);

            return result != null ? Ok(result) : NotFound();
        }


        [HttpGet("get-all-user")]

        public async Task<IActionResult> GetAllUser()
        {
            var result = await _elasticService.GetAllAsync();

            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete("delete-user")]

        public async Task<IActionResult> DeleteUser(string key)
        {
            var result = await _elasticService.RemoveAsync(key);

            return result ? Ok(result) : NotFound();
        }

    }
}
