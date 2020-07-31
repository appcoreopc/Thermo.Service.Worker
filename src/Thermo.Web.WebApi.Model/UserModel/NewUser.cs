﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Web.WebApi.Model.UserModel
{
    public class NewUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string NotificationToken { get; set; }
    }
}