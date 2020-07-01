using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using KmLog.Server.Dal;
using KmLog.Server.Dto;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.Logic
{
    public class UserLogic
    {
        private readonly ILogger<UserLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserLogic(ILogger<UserLogic> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GroupDto> AddGroup(GroupDto group)
        {
            try
            {
                using var transaction = _unitOfWork.BeginTransaction();

                var entity = _mapper.Map<Group>(group);
                await _unitOfWork.GroupRepository.Add(entity);

                await _unitOfWork.Save();
                transaction.Commit();

                _mapper.Map(entity, group);

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
                var groups = await _unitOfWork.GroupRepository.Query().ToListAsync();
                return _mapper.Map<IEnumerable<GroupDto>>(groups);
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

                var entity = _mapper.Map<User>(user);
                _unitOfWork.UserRepository.Update(entity);

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
            var user = await _unitOfWork.UserRepository.Query().FirstOrDefaultAsync(u => u.Email == email);
            return user != null
                ? _mapper.Map<UserDto>(user)
                : throw new AuthenticationException("Unknown user");
        }
    }
}
