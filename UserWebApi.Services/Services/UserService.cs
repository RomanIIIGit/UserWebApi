using AutoMapper;
using UserWebApi.Data.Models;
using UserWebApi.Repositories;
using UserWebApi.Services.Dtos;

namespace UserWebApi.Services.Services
{
	public interface IUserService
	{
		Task<IEnumerable<UserResponseDto>> GetUsersAsync();
		Task<UserResponseDto> GetUserByEmailAsync(string email);
		Task CreateUserAsync(UserRequestDto userRequestDto);
		Task UpdateUserAsync(int id, UserRequestDto userRequestDto);
		Task DeleteUserAsync(int id);
	}

	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
		{
			var users = await _userRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<UserResponseDto>>(users);
		}

		public async Task CreateUserAsync(UserRequestDto userRequestDto)
		{
			if (!await _userRepository.IsEmailUniqueAsync(userRequestDto.Email))
			{
				throw new ArgumentException("Email is already in use");
			}

			var user = _mapper.Map<User>(userRequestDto);
			await _userRepository.AddAsync(user);
		}

		public async Task UpdateUserAsync(int id, UserRequestDto userRequestDto)
		{
			var existingUser = await _userRepository.GetByIdAsync(id);
			if (existingUser == null)
			{
				throw new KeyNotFoundException($"User with ID {id} not found.");
			}

			if (existingUser.Email != userRequestDto.Email && !await _userRepository.IsEmailUniqueAsync(userRequestDto.Email))
			{
				throw new ArgumentException("Email is already in use");
			}

			var user = _mapper.Map<User>(userRequestDto);
			user.Id = id;

			await _userRepository.UpdateAsync(user);
		}

		public async Task DeleteUserAsync(int id)
		{
			await _userRepository.DeleteAsync(id);
		}

		public async Task<UserResponseDto> GetUserByEmailAsync(string email)
		{
			var user = await _userRepository.GetByEmailAsync(email);
			return _mapper.Map<UserResponseDto>(user);
		}
	}
}
