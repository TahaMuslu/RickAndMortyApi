using Applicaton.Dto.Character;
using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Services.CharacterService
{
    public interface ICharacterService
    {
        Response<List<CharacterDto>> GetCharacters(int page, string? name = null, string? status = null, string? species = null, string? type = null, string? gender = null);

        Response<List<CharacterDto>> GetCharactersById(List<int> ids);
    }
}
