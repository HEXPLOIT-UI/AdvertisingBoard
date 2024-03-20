using AdvertisingBoard.Repositories;
using AutoMapper;

namespace AdvertisingBoard.Services
{
    public interface ICategoryService
    {
        Task<TaskResultViewModel> CreateCategory(CategoryViewModel model); 
        Task<TaskResultViewModel> DeleteCategory(int id); 
        Task<TaskResultViewModel> ModifyCategory(int id, CategoryViewModel model); 
        Task<IEnumerable<Category>> GetCategoriesByParentId(int parentId);
        Task<IEnumerable<Category>> GetAllParentsCategories();
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

        public async Task<TaskResultViewModel> CreateCategory(CategoryViewModel model)
        {
            var category = _mapper.Map<Category>(model);
            await _categoryRepository.CreateCategory(category);
            return new TaskResultViewModel() { State = true, Message = $"Создана категория {category.Name}, её айди: {category.Id}" };
        }

        public async Task<TaskResultViewModel> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return new TaskResultViewModel() { State = false, Message = $"Категория с айди {id} не найдена!" };
            }
            await _categoryRepository.DeleteAsync(category);
            return new TaskResultViewModel() { State = true, Message = $"Категория {category.Name} удалена!" };
        }

        public async Task<IEnumerable<Category>> GetCategoriesByParentId(int parentId)
        {
            return await _categoryRepository.GetCategoriesByParentId(parentId);
        }

        public async Task<TaskResultViewModel> ModifyCategory(int id, CategoryViewModel model)
        {
            if (await _categoryRepository.GetByIdAsync(id) == null)
            {
                return new TaskResultViewModel() { State = false, Message = $"Категория с айди {id} не найдена!" };
            }
            var category = _mapper.Map<Category>(model);
            category.Id = id;
            await _categoryRepository.UpdateCategory(id, category);
            return new TaskResultViewModel() { State = true, Message = "Категория обновлена!" };
        }

        public async Task<IEnumerable<Category>> GetAllParentsCategories()
        {
            return await _categoryRepository.GetAllParentsCategories();
        }
    }
}
