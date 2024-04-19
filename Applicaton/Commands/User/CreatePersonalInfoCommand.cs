using Apposite.Application.Dto.CuisinePreference;
using Apposite.Application.Dto.Ingredient;
using Apposite.Core.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apposite.Application.Commands.User
{
    public class CreatePersonalInfoCommand : IRequest<Response<NoContent>>
    {
        public required List<Guid> Ingredients { get; set;}
        public required List<Guid> Cuisines { get; set;}
        public required List<Guid> Health { get; set;}

        
    }
}
