using Supermarket.Core.Context;
using Supermarket.Core.Dtos.Response;
using Supermarket.Core.Entities;
using Supermarket.Core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Supermarket.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SupermarketDbContext _context;

        public CategoryRepository(SupermarketDbContext context) => _context = context;

        public IList<Category> GetAll() => _context.Categories
            .Where(category => category.DeletedAt == null)
            .OrderBy(category => category.CreatedAt)
            .ToList();

        public IList<Category> GetByNameContains(string name) => _context.Categories
            .Where(category => category.DeletedAt == null && category.Name.Contains(name))
            .OrderBy(category => category.CreatedAt)
            .ToList();

        public Category GetById(Guid id) => _context.Categories
            .Where(category => category.DeletedAt == null)
            .FirstOrDefault(category => category.Id == id)
            ?? throw new Exception($"Category with id {id} not found.");

        public Category Add(Category category)
        {
            if (category.Id == Guid.Empty) category.Id = Guid.NewGuid();
            category.CreatedAt = DateTime.Now;
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category UpdateById(Category category, Guid id)
        {
            Category categoryToUpdate = GetById(id);
            categoryToUpdate.Name = category.Name;
            if (_context.Entry(categoryToUpdate).State == EntityState.Modified)
                categoryToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return categoryToUpdate;
        }

        public Category DeleteById(Guid id)
        {
            Category categoryToDelete = GetById(id);
            categoryToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return categoryToDelete;
        }

        public IList<CategoryValueResponse> ComputeCategoryValues() => _context.Categories
            .Include(c => c.Products.Select(p => p.Stocks))
            .Select(category => new CategoryValueResponse { CategoryName = category.Name, Value = category.Products.Sum(product => product.Stocks.Sum(stock => stock.SalePrice * stock.Quantity)) })
            .ToList();
    }
}

//using Supermarket.Core.Context;
//using Supermarket.Core.Dtos.Response;
//using Supermarket.Core.Entities;
//using Supermarket.Core.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.SqlClient;
//using System.Linq;

//namespace Supermarket.Core.Repositories
//{
//    public class CategoryRepository : ICategoryRepository
//    {
//        private readonly SupermarketDbContext _context;

//        public CategoryRepository(SupermarketDbContext context) => _context = context;

//        public IList<Category> GetAll() => _context.Database.SqlQuery<Category>("GetAllCategories").ToList();

//        public IList<Category> GetByNameContains(string name)
//        {
//            var param = new SqlParameter("@Name", name);
//            return _context.Database.SqlQuery<Category>("GetCategoryByNameContains @Name", param).ToList();
//        }

//        public Category GetById(Guid id)
//        {
//            var param = new SqlParameter("@Id", id);
//            return _context.Database.SqlQuery<Category>("GetCategoryById @Id", param).FirstOrDefault() ?? throw new Exception($"Category with id {id} not found.");
//        }

//        public Category Add(Category category)
//        {
//            if (category.Id == Guid.Empty) category.Id = Guid.NewGuid();
//            category.CreatedAt = DateTime.Now;
//            var idParam = new SqlParameter("@Id", category.Id);
//            var nameParam = new SqlParameter("@Name", category.Name);
//            var createdAtParam = new SqlParameter("@CreatedAt", category.CreatedAt);
//            _context.Database.ExecuteSqlCommand("AddCategory @Id, @Name, @CreatedAt", idParam, nameParam, createdAtParam);
//            return category;
//        }

//        public Category UpdateById(Category category, Guid id)
//        {
//            Category categoryToUpdate = GetById(id);
//            categoryToUpdate.Name = category.Name;
//            categoryToUpdate.UpdatedAt = DateTime.Now;
//            var idParam = new SqlParameter("@Id", id);
//            var nameParam = new SqlParameter("@Name", category.Name);
//            var updatedAtParam = new SqlParameter("@UpdatedAt", categoryToUpdate.UpdatedAt);
//            _context.Database.ExecuteSqlCommand("UpdateCategory @Id, @Name, @UpdatedAt", idParam, nameParam, updatedAtParam);
//            return categoryToUpdate;
//        }

//        public Category DeleteById(Guid id)
//        {
//            Category categoryToDelete = GetById(id);
//            categoryToDelete.DeletedAt = DateTime.Now;
//            var idParam = new SqlParameter("@Id", id);
//            var deletedAtParam = new SqlParameter("@DeletedAt", categoryToDelete.DeletedAt);
//            _context.Database.ExecuteSqlCommand("DeleteCategory @Id, @DeletedAt", idParam, deletedAtParam);
//            return categoryToDelete;
//        }

//        public IList<CategoryValueResponse> ComputeCategoryValues() => _context.Database.SqlQuery<CategoryValueResponse>("ComputeCategoryValues").ToList();
//    }
//}