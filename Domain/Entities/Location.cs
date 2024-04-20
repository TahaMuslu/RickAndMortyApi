﻿
using Core.Entity;

namespace Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Dimension { get; set; }
        public virtual ICollection<Character> Characters { get; set; }
        //public string Url { get; set; }

    }
}
