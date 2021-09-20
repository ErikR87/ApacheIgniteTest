
namespace IT.Shared
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name {  get; set; }
        public IEnumerable<Todo> Todos { get; set; }
    }
}
