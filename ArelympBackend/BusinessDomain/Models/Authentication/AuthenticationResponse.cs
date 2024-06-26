using Google.Apis.Auth.OAuth2.Responses;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Models.Authentication
{
    public class AuthenticationResponse
    {
        public UserDto? User { get; set; }

        public string? Token { get; set; }

        public AuthenticationResponse(UserDto? user, string? token) 
        { 
            User = user;
            Token = token;
        }
    }
}
