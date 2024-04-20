using Application.ServiceExtensions;
using Applicaton.Dto.Character;
using AutoMapper;
using Core.Dtos;
using Core.Pagination;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly RickAndMortyDbContext _dbcontext;
        private readonly IMapper _mapper;

        public CharacterService(RickAndMortyDbContext dbcontext, IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
        }


        public Response<List<CharacterDto>> GetCharacters(int page, string? name = null, string? status = null, string? species = null, string? type = null, string? gender = null)
        {
            var paginationFilter = new PaginationFilter(page, 20);
            var characters = _dbcontext.Characters.Where(x => 
            (name == null || x.Name.ToLower().Contains(name)) &&
            (status == null || x.Status.ToLower() == status) &&
            (species == null || x.Species.ToLower() == species) &&
            (type == null || x.Type.ToLower() == type) &&
            (gender == null || x.Gender.ToLower() == gender))
                .ApplyPaginationQueryable(paginationFilter).Include(x => x.Origin).Include(x => x.Location).Include(x => x.Episodes).ToList();
            var characterDtos = _mapper.Map<List<CharacterDto>>(characters);
            var pager = new Pager(page, 20, _dbcontext.Characters.Where(x =>
            (name == null || x.Name.ToLower().Contains(name)) &&
            (status == null || x.Status.ToLower() == status) &&
            (species == null || x.Species.ToLower() == species) &&
            (type == null || x.Type.ToLower() == type) &&
            (gender == null || x.Gender.ToLower() == gender)).Count());
            return Response<List<CharacterDto>>.Success(200, characterDtos, pager);
        }

        public Response<List<CharacterDto>> GetCharactersById(List<int> ids)
        {
            var characters = _dbcontext.Characters.Where(x => ids.Contains(x.Id)).Include(x => x.Origin).Include(x => x.Location).Include(x => x.Episodes).ToList();
            var characterDtos = _mapper.Map<List<CharacterDto>>(characters);
            if (characterDtos.Count == 0)
                return Response<List<CharacterDto>>.Fail(404, "Characters not found");
            return Response<List<CharacterDto>>.Success(200, characterDtos);
        }
    }
}
