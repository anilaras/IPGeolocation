using IPLocator.Models.Abstracts;

namespace IPLocator.Models.Concrete
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
