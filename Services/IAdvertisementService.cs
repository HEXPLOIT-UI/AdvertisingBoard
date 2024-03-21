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
        Task<IEnumerable<AdvertisementViewModel>> GetAllAdvertisementsOnPage(int page, int categoryId = -1);
        Task<IEnumerable<AdvertisementViewModel>> SearchAdvertisementByKeywordsOnPage(int page, string keyword, int categoryId = -1, string contacts = "");
        Task<TaskResultViewModel> DeleteAllAdvertisements();
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
            if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
            var advertisement = _mapper.Map<Advertisement>(model);
            advertisement.UserId = user.UserId;
            await _advertisementRepository.CreateAdvertisment(advertisement);
            return new TaskResultViewModel() { State = true, Message = "Объявление опубликовано!" };
        }
        public async Task<TaskResultViewModel> DeleteAdvertisement(int id, string login)
        {
            return await ValidateAction(id, login, async (user, advertisement) =>
            {
                await _advertisementRepository.DeleteAdvertisment(advertisement);
                return new TaskResultViewModel() { State = true, Message = "Объявление удалено!" };
            });
        }

        public async Task<TaskResultViewModel> UpdateAdvertisement(int id, AdvertisementViewModel model, string login)
        {
            return await ValidateAction(id, login, async (user, originalAdvertisement) =>
            {
                _mapper.Map(model, originalAdvertisement);
                originalAdvertisement.UpdatedAt = DateTime.UtcNow;
                await _advertisementRepository.UpdateAdvertisment(originalAdvertisement);
                return new TaskResultViewModel() { State = true, Message = "Объявление обновлено!" };
            });
        }

        private async Task<TaskResultViewModel> ValidateAction(int id, string login, Func<User, Advertisement, Task<TaskResultViewModel>> action)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);
            if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
            var advertisement = await _advertisementRepository.GetAdvertisementById(id);
            if (advertisement == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении объекта объявления" };
            if (advertisement.UserId != user.UserId) return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
            return await action(user, advertisement);
        }

        public async Task<AdvertisementViewModel> GetAdvertisementById(int id)
        {
            return _mapper.Map<AdvertisementViewModel>(await _advertisementRepository.GetAdvertisementById(id));
        }

        public async Task<IEnumerable<AdvertisementViewModel>> GetAllAdvertisementsOnPage(int page, int categoryId = -1)
        {
            var ads = await _advertisementRepository.GetAllAdvertisementOnPage(page, categoryId);
            return ads.Select(_mapper.Map<AdvertisementViewModel>).ToList();
        }
        
        public async Task<IEnumerable<AdvertisementViewModel>> SearchAdvertisementByKeywordsOnPage(int page, string keyword, int categoryId = -1, string contacts = "")
        {
            var ads = await _advertisementRepository.SearchAdvertisementByKeywordsOnPage(page, keyword, categoryId, contacts);
            return ads.Select(_mapper.Map<AdvertisementViewModel>).ToList();
        }

        public async Task<TaskResultViewModel> DeleteAllAdvertisements()
        {
            await _advertisementRepository.DeleteAll();
            return new TaskResultViewModel() { State = true, Message = "Все объявления удалены" };
        }

    }
}
