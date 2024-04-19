using Apposite.Application.Commands.User;
using Apposite.Application.Mapping;
using Apposite.Application.Services;
using Apposite.Application.Services.TokenService;
using Apposite.Core.Dtos;
using Apposite.Domain.Entities;
using Apposite.Persistence;
using MediatR;

namespace Apposite.Application.Handlers.User
{
    public class UserCommandHandler : IRequestHandler<CreatePersonalInfoCommand, Response<NoContent>>
    {
        private readonly RickAndMortyDbContext _dbContext;
        private readonly RedisService _redisService;
        private readonly ITokenService _tokenService;

        public UserCommandHandler(RickAndMortyDbContext dbContext, RedisService redisService, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _redisService = redisService;
            _tokenService = tokenService;
        }

        public async Task<Response<NoContent>> Handle(CreatePersonalInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _tokenService.GetUserId();

            var userIngredients = new List<UserIngredient>();
            foreach (var ingredient in request.Ingredients)
            {
                var userIngredient = new UserIngredient
                {
                    UserId = userId,
                    IngredientId = ingredient,
                };
                userIngredients.Add(userIngredient);
            }

            var userCuisines = new List<UserCuisinePreference>();
            foreach (var cuisine in request.Cuisines)
            {
                var userCuisine = new UserCuisinePreference
                {
                    UserId = userId,
                    CuisinePreferenceId = cuisine,
                };
                userCuisines.Add(userCuisine);
            }

            var userHealth = new List<UserHealth>();
            foreach (var health in request.Health)
            {
                var tempUserHealth = new UserHealth
                {
                    UserId = userId,
                    HealthId = health,
                };
                userHealth.Add(tempUserHealth);
            }


            await _dbContext.UserIngredients.AddRangeAsync(userIngredients, cancellationToken);
            await _dbContext.UserCuisinePreferences.AddRangeAsync(userCuisines, cancellationToken);
            await _dbContext.UserHealths.AddRangeAsync(userHealth, cancellationToken);
            await _dbContext.SaveChangesAsync();
            return Response<NoContent>.Success(204);
        }
    }
}
