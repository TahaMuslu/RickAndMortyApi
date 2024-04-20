using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Dto.Episode
{
    public class EpisodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AirDate { get; set; }
        public string Episode { get; set; }
        public virtual List<string> Characters { get; set; }
        public string Url { get; set; }
        public string Created { get; set; }
    }
}
