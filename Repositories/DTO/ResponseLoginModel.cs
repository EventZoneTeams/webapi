﻿namespace Repositories.DTO
{
    public class ResponseLoginModel
    {
        public bool Status { get; set; } = false;

        public string Message { get; set; } = "";

        public string JWT { get; set; } = "";

        public DateTime? Expired { get; set; }

        public string? JWTRefreshToken { get; set; } = "";

        public int? UserId { get; set; }

    }
}
