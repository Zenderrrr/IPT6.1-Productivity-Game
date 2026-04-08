using FocusUp.Infrastructure.Repositories;
using System;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;
    public CategoryService(CategoryRepository categoryRepository) => _categoryRepository = categoryRepository;

    public Category? GetCategoryById(int id) => _categoryRepository.GetById(id);

    public List<Category> GetCategoriesByUser(int userId) => _categoryRepository.GetAllByUserId(userId);

    public int CreateCategory(Category category) => _categoryRepository.Insert(category);

    public void UpdateCategory(Category category) => _categoryRepository.Update(category);

    public void DeleteCategory(int id) => _categoryRepository.Delete(id);

    public bool CategoryExistsByName(int userId, string name) => _categoryRepository.ExistsByName(userId, name);
}