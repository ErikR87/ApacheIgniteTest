using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Configuration;
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

            await GetCache().PutAsync(project.Id, project);
        }

        [HttpGet]
        public async Task<IEnumerable<Project>> Get()
        {
            var query = GetCache().AsQueryable();
            return query.ToList().Select(x => x.Value);
        }

        private ICache<Guid, Project> GetCache()
        {
            var projectCfg = new CacheConfiguration(nameof(Project))
            {
                KeyConfiguration = new[]
                {
                    new CacheKeyConfiguration
                    {
                        TypeName = nameof(Project),
                        AffinityKeyFieldName = nameof(Project.TodoId)
                    }
                }
            };

            return _ignite.GetOrCreateCache<Guid, Project>(projectCfg);
        }
    }
}
