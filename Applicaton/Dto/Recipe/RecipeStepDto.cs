

namespace Apposite.Application.Dto.Recipe
{
    public class RecipeStepDto
    {
        public string Description { get; set; }
        public int StepNumber { get; set; }
        public ICollection<Guid> MediaFiles { get; set; }
    }
}