﻿using System.ComponentModel.DataAnnotations;

namespace MyContactsAPI.Dtos.User
{
    public class UserSignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}