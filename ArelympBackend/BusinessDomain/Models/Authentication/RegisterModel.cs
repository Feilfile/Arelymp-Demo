using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Models.Authentication
{
    public class RegisterModel
    {
        public string UserName { get; set; }

        public RegisterModel(string userName, string email)
        {
            UserName = userName;
        }
    }
}
