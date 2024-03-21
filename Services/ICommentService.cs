using AdvertisingBoard.Repositories;
using AdvertismentBoard.Repositories;
using AutoMapper;

namespace AdvertisingBoard.Services;

public interface ICommentService
{
    Task<TaskResultViewModel> CreateComment(CommentViewModel model, string login);
    Task<TaskResultViewModel> DeleteComment(int id, string login);
    Task<TaskResultViewModel> UpdateComment(int id, CommentViewModel model, string login);
    Task<IEnumerable<CommentViewModel>> GetCommentsByAdId(int adId);
}

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAdvertisementRepository _advertisementRepository;
    private readonly IMapper _mapper;
    public CommentService(ICommentRepository commentRepository, IUserRepository userRepository,IAdvertisementRepository advertismentRepository, IMapper mapper)
    {
        _advertisementRepository = advertismentRepository;
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<TaskResultViewModel> CreateComment(CommentViewModel model, string login)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
        var advertisement = await _advertisementRepository.GetAdvertisementById(model.AdvertismentId);
        if (advertisement == null) return new TaskResultViewModel() { State = false, Message = "Объявление к которому оставлен комментарий не найдено" };
        var comment = _mapper.Map<Comment>(model);
        comment.UserId = user.UserId;
        await _commentRepository.CreateComment(comment);
        return new TaskResultViewModel() { State = true, Message = "Комментарий опубликован!" };
    }

    public async Task<IEnumerable<CommentViewModel>> GetCommentsByAdId(int adId)
    {
        var comments = await _commentRepository.GetCommentsByAdId(adId);
        var commentViewModelTasks = comments.Select(async comment =>
        {
            var commentViewModel = _mapper.Map<CommentViewModel>(comment);
            var user = await _userRepository.GetByIdAsync(comment.UserId);
            commentViewModel.OwnerName = user.FullName;
            return commentViewModel;
        });
        var commentViewModels = await Task.WhenAll(commentViewModelTasks);
        return commentViewModels.ToList();
    }

    public async Task<TaskResultViewModel> DeleteComment(int id, string login)
    {
        return await ValidateAction(id, login, async (user, comment) =>
        {
            await _commentRepository.DeleteComment(comment);
            return new TaskResultViewModel() { State = true, Message = "Комментарий удалён!" };
        });
    }

    public async Task<TaskResultViewModel> UpdateComment(int id, CommentViewModel model, string login)
    {
        return await ValidateAction(id, login, async (user, comment) =>
        {
            _mapper.Map(model, comment);
            await _commentRepository.UpdateComment(comment);
            return new TaskResultViewModel() { State = true, Message = "Комментарий обновлён!" };
        });
    }

    private async Task<TaskResultViewModel> ValidateAction(int id, string login, Func<User, Comment, Task<TaskResultViewModel>> operation)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);
        if (user == null) return new TaskResultViewModel() { State = false, Message = "Ошибка в получении отправителя запроса" };
        var comment = await _commentRepository.GetCommentById(id);
        if (comment == null) return new TaskResultViewModel() { State = false, Message = "Комментарий не найден!" };
        if (comment.UserId != user.UserId) return new TaskResultViewModel() { State = false, Message = "Нет доступа" };
        return await operation(user, comment);
    }
}
