using AdvertisingBoard.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingBoard.Services
{
    public interface IAdvertisementService
    {
        Task<TaskResultViewModel> CreateAdvertisement(AdvertisementViewModel model, string login);
        Task<TaskResultViewModel> DeleteAdvertisement(int id, string login);
        Task<TaskResultViewModel> UpdateAdvertisement(int id, AdvertisementViewModel model, string login);
        Task<AdvertisementViewModel> GetAdvertisementById(int id);
    }

    public class AdvertisementService : IAdvertisementService
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AdvertisementService(IAdvertisementRepository advertisementRepository, IUserRepository userRepository, IMapper mapper)
        {
            _advertisementRepository = advertisementRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<TaskResultViewModel> CreateAdvertisement(AdvertisementViewModel model, string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
            }
            var advertisement = _mapper.Map<Advertisement>(model);
            advertisement.UserId = user.UserId;
            await _advertisementRepository.CreateAdvertisment(advertisement);
            return new TaskResultViewModel() { State = true, Message = "Объявление опубликовано!" };
        }
        public async Task<TaskResultViewModel> DeleteAdvertisement(int id, string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
            }
            var advertisement = await _advertisementRepository.GetAdvertisementById(id);
            if (advertisement == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Ошибка в получении объекта объявления" };
            }
            if (advertisement.UserId != user.UserId)
            {
                return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
            }
            await _advertisementRepository.DeleteAdvertisment(advertisement);
            return new TaskResultViewModel() { State = true, Message = "Объявление удалено!" };
        }

        public async Task<AdvertisementViewModel> GetAdvertisementById(int id)
        {
            return _mapper.Map<AdvertisementViewModel>(await _advertisementRepository.GetAdvertisementById(id));
        }

        public async Task<TaskResultViewModel> UpdateAdvertisement(int id, AdvertisementViewModel model, string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
            }
            var originalAdvertisement = await _advertisementRepository.GetAdvertisementById(id);
            if (originalAdvertisement == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Ошибка в получении объекта объявления" };
            }
            if (originalAdvertisement.UserId != user.UserId)
            {
                return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
            }
            _mapper.Map(model, originalAdvertisement);
            originalAdvertisement.UpdatedAt = DateTime.UtcNow;
            await _advertisementRepository.UpdateAdvertisment(originalAdvertisement);
            return new TaskResultViewModel() { State = true, Message = "Объявление обновлено!" };
        }
    }
}
