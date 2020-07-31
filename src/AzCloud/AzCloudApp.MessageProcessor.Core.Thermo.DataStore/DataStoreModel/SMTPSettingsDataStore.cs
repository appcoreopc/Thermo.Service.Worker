﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class SMTPSettingsDataStore
    {
        [Key]
        public long Nid { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public DateTime? TimeStamp { get; set; }

    }
}
