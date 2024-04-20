using Core.Entity;

namespace Domain.Entities
{
    public class Episode : BaseEntity
    {
        public string? Name { get; set; }
        public string? AirDate { get; set; }
        public string? EpisodeCode { get; set; }
        public virtual ICollection<Character>? Characters { get; set; }
    }
}
