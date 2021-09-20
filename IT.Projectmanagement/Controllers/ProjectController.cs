using Apache.Ignite.Core;
using IT.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT.Projectmanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IIgnite _ignite;

        public ProjectController(IIgnite ignite)
        {
            _ignite = ignite;
        }

        [HttpPut]
        public async Task Create([FromBody] Project project)
        {
            project.Id = Guid.NewGuid();

            var cache = _ignite.GetOrCreateCache<Guid, Project>(nameof(Project));
            await cache.PutAsync(project.Id, project);
        }

        [HttpGet]
        public async Task<IEnumerable<Project>> Get()
        {
            var cache = _ignite.GetOrCreateCache<Guid, Project>(nameof(Project));
            var query = cache.AsQueryable();
            return query.ToList().Select(x => x.Value);
        }
    }
}
