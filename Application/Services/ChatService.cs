using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;
using docs_project.Application.Dto;
using docs_project.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using docs_project.Application.Dto.Upsert;
using docs_project.Application.Dto.Read;
using docs_project.Application.Dto.Summary;

namespace docs_project.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public ChatService(IChatRepository chatRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        public async Task UpdateGroupTitleAsync(Guid id, string title)
        {
            var group = await _chatRepository.GetGroupByIdAsync(id);
            if (group is null) return;
            group.Title = title;
            await _chatRepository.UpdateAsync(group);
        }

        public async Task AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            await _chatRepository.AddUserToGroupAsync(groupId, userId);
        }

        public async Task<ChatDto> CreateGroupAsync(GroupChatUpsertDto dto)
        {
            var group = new GroupChat
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Title = dto.Title,
                OwnerId = dto.OwnerId
            };

            if (dto.UserIds != null)
            {
                foreach (var uid in dto.UserIds)
                {
                    var user = await _userRepository.GetByIdAsync(uid);
                    if (user != null) group.Users.Add(user);
                }
            }

            await _chatRepository.AddAsync(group);
            return await MapToDtoAsync(group);
        }

        public async Task<ChatDto> CreateDirectAsync(DirectChatUpsertDto dto)
        {
            var direct = new DirectChat
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                FirstUserId = dto.FirstUserId,
                SecondUserId = dto.SecondUserId
            };

            var u1 = await _userRepository.GetByIdAsync(dto.FirstUserId);
            var u2 = await _userRepository.GetByIdAsync(dto.SecondUserId);
            if (u1 != null) direct.Users.Add(u1);
            if (u2 != null) direct.Users.Add(u2);

            await _chatRepository.AddAsync(direct);
            return await MapToDtoAsync(direct);
        }

        public async Task<ChatDto?> GetByIdAsync(Guid id)
        {
            var chat = await _chatRepository.GetByIdAsync(id);
            if (chat is null) return null;
            return await MapToDtoAsync(chat);
        }

        public async Task<List<ChatDto>> GetAllAsync()
        {
            var chats = await _chatRepository.GetAllAsync();
            var list = new List<ChatDto>();
            foreach (var c in chats)
            {
                list.Add(await MapToDtoAsync(c));
            }
            return list;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _chatRepository.DeleteAsync(id);
        }

        public async Task<List<ChatDto>> GetAllGroupsAsync()
        {
            var chats = await _chatRepository.GetAllGroupsAsync();
            var list = new List<ChatDto>();
            foreach (var c in chats)
                list.Add(await MapToDtoAsync(c));
            return list;
        }

        public async Task<ChatDto?> GetGroupByIdAsync(Guid id)
        {
            var c = await _chatRepository.GetGroupByIdAsync(id);
            if (c is null) return null;
            return await MapToDtoAsync(c);
        }

        public async Task<List<ChatDto>> GetAllDirectsAsync()
        {
            var chats = await _chatRepository.GetAllDirectsAsync();
            var list = new List<ChatDto>();
            foreach (var c in chats)
                list.Add(await MapToDtoAsync(c));
            return list;
        }

        public async Task<ChatDto?> GetDirectByIdAsync(Guid id)
        {
            var c = await _chatRepository.GetDirectByIdAsync(id);
            if (c is null) return null;
            return await MapToDtoAsync(c);
        }

        private Task<ChatDto> MapToDtoAsync(Chat c)
        {
            var dto = new ChatDto
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
                Type = c.GetType().Name
            };

            if (c is GroupChat gc)
            {
                dto.Title = gc.Title;
                dto.OwnerId = gc.OwnerId;
            }
            if (c is DirectChat dc)
            {
                dto.FirstUserId = dc.FirstUserId;
                dto.SecondUserId = dc.SecondUserId;
            }

            if (c.Users != null)
            {
                foreach (var u in c.Users)
                {
                    dto.Users.Add(new UserSummaryDto { Id = u.Id, Username = u.Username, Email = u.Email });
                }
            }

            if (c.Messages != null)
            {
                var userLookup = c.Users?.ToDictionary(u => u.Id, u => u.Username) ?? new();
                foreach (var m in c.Messages)
                {
                    dto.Messages.Add(new MessageDto
                    {
                        Id = m.Id,
                        CreatedAt = m.CreatedAt,
                        Content = m.Content,
                        UserId = m.UserId,
                        Username = userLookup.TryGetValue(m.UserId, out var name) ? name : null,
                        ChatId = m.ChatId
                    });
                }
            }

            return Task.FromResult(dto);
        }
    }
}
