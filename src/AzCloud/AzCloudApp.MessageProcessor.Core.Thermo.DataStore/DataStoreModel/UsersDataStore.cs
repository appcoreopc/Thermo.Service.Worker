﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class UsersDataStore
    {
        [Key]
        public int Nid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string NotificationToken { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
