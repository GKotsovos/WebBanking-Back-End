﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class UserServices
    {
        private UserManager userManager;
        public UserServices()
        {
            userManager = new UserManager();
        }

        public string Authenticate(string userId, string password)
        {
            return userManager.GetCustomerId(userId, password);
        }
    }
}
