using AdvertisingBoard.Repositories;
using AutoMapper;

namespace AdvertisingBoard.Services;
public interface IPhotoService
{
    Task<TaskResultViewModel> CreatePhotoInAdvertisement(List<IFormFile> files, int adId, string login);
    Task<TaskResultViewModel> DeletePhotoFromAdvertisement(int photoId, string login);
    Task<IEnumerable<PhotoViewModel>> GetPhotosOnAdvertisement(int adId);
}

public class PhotoService : IPhotoService
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAdvertisementRepository _advertisementRepository;
    private readonly IMapper _mapper;
    private readonly string _uploadsFolder;

    public PhotoService(IPhotoRepository photoRepository, IUserRepository userRepository, IAdvertisementRepository advertisementRepository, IMapper mapper)
    {
        _photoRepository = photoRepository;
        _userRepository = userRepository;
        _advertisementRepository = advertisementRepository;
        _mapper = mapper;
        _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images");
    }

    public async Task<TaskResultViewModel> CreatePhotoInAdvertisement(List<IFormFile> files, int adId, string login)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
        var advertisement = await _advertisementRepository.GetAdvertisementById(adId);
        if (advertisement == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении объекта объявления" };
        if (advertisement.UserId != user.UserId) return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
        if (!Directory.Exists(_uploadsFolder))
        {
            Directory.CreateDirectory(_uploadsFolder);
        }
        foreach (var file in files)
        {
            var fileName = Guid.NewGuid().ToString()+".jpg";
            var filePath = Path.Combine(_uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(stream);
            }
            var Photo = new Photo()
            {
                PhotoURL = fileName,
                AdvertisementId = adId,
                UserId = user.UserId,
            };
            await _photoRepository.CreatePhoto(Photo);
            Console.WriteLine($"Было зарегистрировано фото {Photo.PhotoURL} для объявления {advertisement.Title}");
        }
        return new TaskResultViewModel() { State = true, Message = $"Успешно добавлено {files.Count} фотографий к объявлению {advertisement.Title}" };
    }

    public async Task<TaskResultViewModel> DeletePhotoFromAdvertisement(int photoId, string login)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
        var photo = await _photoRepository.GetPhotoById(photoId);
        if (photo == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении объекта фотографии" };
        if (photo.UserId != user.UserId) return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
        File.Delete(Path.Combine(_uploadsFolder, photo.PhotoURL));
        await _photoRepository.DeletePhoto(photo);
        return new TaskResultViewModel() { State = true, Message = "Фотография удалена" };
    }

    public async Task<IEnumerable<PhotoViewModel>> GetPhotosOnAdvertisement(int adId)
    {
        var photos = await _photoRepository.GetPhotosByAdid(adId);
        return photos.Select(_mapper.Map<PhotoViewModel>).ToList();
    }
}