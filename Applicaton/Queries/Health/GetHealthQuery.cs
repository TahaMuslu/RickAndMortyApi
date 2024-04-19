using Apposite.Application.Dto.Health;
using Apposite.Core.Dtos;
using MediatR;

namespace Apposite.Application.Queries.Health
{
    public class GetHealthQuery : IRequest<Response<List<HealthDto>>>
    {
        public string SearchText { get; init; } = string.Empty;
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
