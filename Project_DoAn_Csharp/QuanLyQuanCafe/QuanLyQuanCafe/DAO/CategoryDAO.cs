using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance { 
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; } 
        }
        private CategoryDAO() { }
        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();
            String query = "select * from FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                Category category = new Category(item);
                list.Add(category);
            }
            return list;
        }
        public Category GetCategoryByID(int id)
        {
            Category category = null;
            String query = "select * from FoodCategory where id= "+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }
        public bool InsertCategory(String namecategory)
        {
            String query = "exec InsertCategory @name";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { namecategory });
            return resutl > 0;
        }
        public bool UpdateFoodCategory(int id, String ten)
        {
            String query = "exec UpdateFoodCategory @id , @name ";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, ten});
            return resutl > 0;
        }
        public bool DeleteFoodCategory(int id)
        {
            // truoc khi xoa mon phai xoa tu billinfo truoc 
            BillInfoDAO.Instace.DeleteBillInfoByFoodID(id);
            String query = "delete FoodCategory where id= " + id;
            int resutl = DataProvider.Instance.ExecuteNonQuery(query);
            return resutl > 0;
        }
    }
}
