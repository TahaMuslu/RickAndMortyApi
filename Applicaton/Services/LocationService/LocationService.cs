using Application.ServiceExtensions;
using Applicaton.Dto.Character;
using Applicaton.Dto.Location;
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

namespace Applicaton.Services.LocationService
{
    public class LocationService : ILocationService
    {
        private readonly RickAndMortyDbContext _dbcontext;
        private readonly IMapper _mapper;

        public LocationService(RickAndMortyDbContext dbcontext, IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
        }


        public Response<List<LocationDto>> GetLocations(int page, string? name = null, string? type = null, string? dimension = null)
        {
            var paginationFilter = new PaginationFilter(page, 20);
            var locations = _dbcontext.Locations.Where(x =>
            (name == null || x.Name.ToLower().Contains(name)) &&
            (type == null || x.Type.ToLower() == type) &&
            (dimension == null || x.Dimension.ToLower() == dimension))
                .ApplyPaginationQueryable(paginationFilter).Include(x => x.Residents).ToList();
            var locationDtos = _mapper.Map<List<LocationDto>>(locations);
            var pager = new Pager(page, 20, _dbcontext.Locations.Where(x =>
            (name == null || x.Name.ToLower().Contains(name)) &&
            (type == null || x.Type.ToLower() == type) &&
            (dimension == null || x.Dimension.ToLower() == dimension)).Count());
            return Response<List<LocationDto>>.Success(200, locationDtos, pager);
        }

        public Response<List<LocationDto>> GetLocationsById(List<int> ids)
        {
            var locations = _dbcontext.Locations.Where(x => ids.Contains(x.Id)).Include(x => x.Residents).ToList();
            var locationDtos = _mapper.Map<List<LocationDto>>(locations);
            if (locationDtos.Count == 0)
                return Response<List<LocationDto>>.Fail(404, "Characters not found");
            return Response<List<LocationDto>>.Success(200, locationDtos);
        }
    }
}
