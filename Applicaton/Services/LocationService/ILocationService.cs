using Applicaton.Dto.Character;
using Applicaton.Dto.Location;
using Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Services.LocationService
{
    public interface ILocationService
    {
        Response<List<LocationDto>> GetLocations(int page, string? name = null, string? type = null, string? dimension = null);
        Response<List<LocationDto>> GetLocationsById(List<int> ids);
    }
}
