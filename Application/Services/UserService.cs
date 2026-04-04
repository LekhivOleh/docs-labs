using docs_project.Application.Interfaces.Repositories;
using docs_project.Application.Interfaces.Services;
using docs_project.Domain.Entities;

namespace docs_project.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICsvReader _csvReader;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserService(IUserRepository userRepository, ICsvReader csvReader)
        {
            _userRepository = userRepository;
            _csvReader = csvReader;
        }

        public async Task<User> CreateAsync(User user)
        {
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<(List<User> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            return await _userRepository.GetPagedAsync(page, pageSize);
        }

        public async Task<int> ImportFromCsvAsync(Stream stream)
        {
            var records = await _csvReader.ReadAllAsync(stream);
            var count = 0;
            foreach (var r in records)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = r.Username,
                    Email = r.Email,
                    Password = r.Password
                };
                await _userRepository.AddAsync(user);
                count++;
            }
            return count;
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task DeleteAllAsync()
        {
            await _userRepository.DeleteAllAsync();
        }
    }
}
