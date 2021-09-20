using Apache.Ignite.Core;
using IT.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT.Tasks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IIgnite _ignite;

        public TodoController(IIgnite ignite)
        {
            _ignite = ignite;
        }

        [HttpPut]
        public async Task Create([FromBody]Todo task)
        {
            task.Id = Guid.NewGuid();

            var cache = _ignite.GetOrCreateCache<Guid, Todo>(nameof(Todo));
            await cache.PutAsync(task.Id, task);
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> Get()
        {
            var cache = _ignite.GetOrCreateCache<Guid, Todo>(nameof(Todo));
            var query = cache.AsQueryable();
            return query.ToList().Select(x => x.Value);
        }
    }
}
