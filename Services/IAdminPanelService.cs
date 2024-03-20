using AdvertisingBoard.ModelsDTO;
using AdvertisingBoard.Repositories;
using AutoMapper;

namespace AdvertisingBoard.Services
{
    public interface IAdminPanelService
    {
        Task<TaskResultViewModel> SetAdmin(string login, bool state);
    }

    public class AdminPanelService : IAdminPanelService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AdminPanelService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<TaskResultViewModel> SetAdmin(string login, bool state)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Пользователь не найден" };
            }
            user.Role = state ? "Admin" : "User";
            await _userRepository.UpdateAsync(user);
            return new TaskResultViewModel() { State = true, Message = $"Пользователь теперь имеет роль {user.Role}" };
        }
    }
}
