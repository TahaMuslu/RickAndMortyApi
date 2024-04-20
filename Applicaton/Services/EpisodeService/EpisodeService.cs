using Application.ServiceExtensions;
using Applicaton.Dto.Character;
using Applicaton.Dto.Episode;
using AutoMapper;
using Core.Dtos;
using Core.Pagination;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Services.EpisodeService
{
    public class EpisodeService : IEpisodeService
    {
        private readonly RickAndMortyDbContext _dbcontext;
        private readonly IMapper _mapper;

        public EpisodeService(RickAndMortyDbContext dbcontext, IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
        }


        public Response<List<EpisodeDto>> GetEpisodes(int page, string? name = null, string? episode = null)
        {
            var paginationFilter = new PaginationFilter(page, 20);
            var episodes = _dbcontext.Episodes.Where(x =>
            (name == null || x.Name.ToLower().Contains(name)) &&
            (episode == null || x.EpisodeCode.ToLower() == episode))
                .ApplyPaginationQueryable(paginationFilter).Include(x => x.Characters).ToList();
            var episodeDtos = _mapper.Map<List<EpisodeDto>>(episodes);
            var pager = new Pager(page, 20, _dbcontext.Episodes.Where(x =>
            (name == null || x.Name.ToLower().Contains(name)) &&
            (episode == null || x.EpisodeCode.ToLower() == episode)).Count());
            return Response<List<EpisodeDto>>.Success(200, episodeDtos, pager);
        }

        public Response<List<EpisodeDto>> GetEpisodesById(List<int> ids)
        {
            var episodes = _dbcontext.Episodes.Where(x => ids.Contains(x.Id)).Include(x => x.Characters).ToList();
            var episodeDtos = _mapper.Map<List<EpisodeDto>>(episodes);
            if (episodeDtos.Count == 0)
                return Response<List<EpisodeDto>>.Fail(404, "Characters not found");
            return Response<List<EpisodeDto>>.Success(200, episodeDtos);
        }
    }
}
