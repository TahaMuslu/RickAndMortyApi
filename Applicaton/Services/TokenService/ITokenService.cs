﻿using Microsoft.AspNetCore.Identity;

namespace Application.Services.TokenService
{
    public interface ITokenService
    {
        public Guid GetUserId();

    }
}