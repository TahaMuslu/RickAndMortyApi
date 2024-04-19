using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apposite.Application.Dto.Recipe
{
    public class RecipeIngredientDto
    {
        public Guid IngredientId { get; set; }
        public string QuantityType { get; set; }
        public double Quantity { get; set; }
    }
}
