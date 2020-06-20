using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using KmLog.Server.Dal;
using KmLog.Server.Dto;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class UserLogic
    {
        private readonly ILogger<UserLogic> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserLogic(ILogger<UserLogic> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupDto> AddGroup(GroupDto group)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                await _unitOfWork.GroupRepository.Add(group);

                await _unitOfWork.Save();
                transaction.Commit();

                return group;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new car");
                throw;
            }
        }

        public async Task<UserDto> LoadUser(string email)
        {
            try
            {
                var user = await CheckUser(email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user");
                throw;
            }
        }

        public async Task<IEnumerable<GroupDto>> LoadGroups()
        {
            try
            {
                var groups = await _unitOfWork.GroupRepository.LoadAll();
                return groups;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading groups");
                throw;
            }
        }

        public async Task<bool> JoinGroup(GroupDto group, string email)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();
                var user = await CheckUser(email);

                user.GroupId = group.Id;

                _unitOfWork.UserRepository.Update(user);

                await _unitOfWork.Save();
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining group");
                throw;
            }
        }

        private async Task<UserDto> CheckUser(string email)
        {
            var user = await _unitOfWork.UserRepository.LoadByEmail(email);
            return user ?? throw new AuthenticationException("Unknown user");
        }
    }
}
