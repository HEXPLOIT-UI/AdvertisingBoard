using AdvertisingBoard.ModelsDTO;
using AdvertisingBoard.Repositories;
using AutoMapper;

namespace AdvertisingBoard.Services
{
    public interface ICategoryService
    {
        Task<TaskResultViewModel> CreateCategory(CategoryViewModel model); 
        Task<TaskResultViewModel> DeleteCategory(string name); 
        Task<TaskResultViewModel> ModifyCategory(CategoryViewModel model); 
        Task<CategoryViewModel> GetCategory(string name);
        Task<bool> CategoryExist(string name);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<bool> CategoryExist(string name)
        {
            return await _categoryRepository.GetByNameAsync(name) != null;
        }

        public async Task<TaskResultViewModel> CreateCategory(CategoryViewModel model)
        {
            var category = _mapper.Map<Category>(model);
            await _categoryRepository.AddAsync(category);
            return new TaskResultViewModel() { State = true, Message = $"Категория {model.Name} создана" };
        }

        public async Task<TaskResultViewModel> DeleteCategory(string name)
        {
            var category = await _categoryRepository.GetByNameAsync(name);
            if (category == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Категория не найдена" };
            }
            await _categoryRepository.DeleteAsync(category);
            return new TaskResultViewModel() { State = true, Message = $"Категория {name} удалена!" };
        }

        public async Task<CategoryViewModel> GetCategory(string name)
        {
            return _mapper.Map<CategoryViewModel>(await _categoryRepository.GetByNameAsync(name));
        }

        public async Task<TaskResultViewModel> ModifyCategory(CategoryViewModel model)
        {
            if (!await CategoryExist(model.Name))
            {
                return new TaskResultViewModel() { State = false, Message = "Категория не найдена" };
            }
            var category = _mapper.Map<Category>(model);
            await _categoryRepository.UpdateAsync(category);
            return new TaskResultViewModel() { State = true, Message = "Категория обновлена" };
        }
    }
}
