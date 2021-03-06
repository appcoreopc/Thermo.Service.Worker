﻿using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thermo.Web.WebApi.Model.UserModel;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public class PersonDataService : IPersonDataService
    {
        private readonly ThermoDataContext _thermoDataContext;

        public PersonDataService(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }
        public virtual UserGetResponse Authenticate(UserAuthenticateRequest source)
        {
            if (source != null && !string.IsNullOrWhiteSpace(source.Username) 
                && !string.IsNullOrWhiteSpace(source.Password))
            {
                var userAuthorized = this._thermoDataContext.Users.Where(x => x.Username == source.Username).FirstOrDefault();

                if (userAuthorized != null)
                {
                    if (EncryptionUtil.IsPasswordAMatch(source.Password.Trim(), userAuthorized.Password.Trim()))
                        return userAuthorized.MapTo();
                }
            }

            return null;
        }

        public virtual Task<int> UpdateUserAsync(UserUpdateRequest source)
        {
            var targetRecord = GetUserByName(source.Username);

            if (targetRecord != null)
            {
                UserConverter.UpateModel(ref targetRecord, source);
                return this._thermoDataContext.SaveChangesAsync();
            }

            return Task.FromResult(-1);
        }

        public virtual Task<int> RegisterUserAsync(UserNewRequest source)
        {
            var targetRecord = GetUserByName(source.Username);

            if (targetRecord == null)
            {
                this._thermoDataContext.Users.Add(source.ToModel());
                return this._thermoDataContext.SaveChangesAsync();
            }
            else
            {
                return Task.FromResult(-1);
            }

        }

        public virtual UserGetResponse GetUserByIdAsync(int? source)
        {
            if (source.HasValue)
            {
                var result = this._thermoDataContext.Users.Where(x => x.Nid == source.Value).FirstOrDefault();

                return result.MapTo();
            }

            return null;
        }

        public virtual IEnumerable<UserGetResponse> GetUsersAsync()
        {
            return this._thermoDataContext.Users.MapTo().ToList();
        }

        public virtual Task<int> DeleteUserAsync(UserDeleteRequest source)
        {
             if (source == null)
                 return Task.FromResult(-1);
            
            var isRecordDelete = false;

            var usersToRemove = DataTypeHelper.ConvertToIntegerArray(source?.TargetUsers);

            foreach (var targetUserId in usersToRemove)
            {
                var targetRecord = GetUserByIdRaw(targetUserId);

                if (targetRecord != null)
                {
                    isRecordDelete = true;
                    _thermoDataContext.Users.Remove(targetRecord);
                }
            }

            if (isRecordDelete)
                return _thermoDataContext.SaveChangesAsync();
            else
                return Task.FromResult(-1);
        }

        private UsersDataStore GetUserByIdRaw(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.Users.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }

        public UsersDataStore GetUserByName(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                return this._thermoDataContext.Users.Where(x => x.Username == username).FirstOrDefault();
            }

            return null;
        }
    }
}

