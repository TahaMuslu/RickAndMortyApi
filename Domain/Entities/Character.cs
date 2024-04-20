using Core.Entity;

namespace Domain.Entities
{
    public class Character : BaseEntity
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public virtual Location Origin { get; set; }
        public virtual Location Location { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        //public string Url { get; set; }
    }
}
