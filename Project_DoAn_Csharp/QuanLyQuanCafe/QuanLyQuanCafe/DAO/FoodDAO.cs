using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }
        private FoodDAO() { } 
        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();
            String query = "select * from Food where idCategory="+id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            String query = "select * from Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
        public bool InsertFood(String ten, int loai, float gia)
        {
            String query = "exec InsertFood @name , @idcategory , @Price ";
            int resutl=DataProvider.Instance.ExecuteNonQuery(query, new object[] { ten, loai, gia });
            return resutl > 0;
        }

        public bool UpdateFood(int id,String ten, int loai, float gia)
        {
            String query = "exec UpdateFood @id , @name , @idcategory , @Price ";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] {id, ten, loai, gia });
            return resutl > 0;
        }
        public bool DeleteFood(int idFood)
        {
            // truoc khi xoa mon phai xoa tu billinfo truoc 
            BillInfoDAO.Instace.DeleteBillInfoByFoodID(idFood);
            String query = "delete Food where id= "+idFood;
            int resutl = DataProvider.Instance.ExecuteNonQuery(query);
            return resutl > 0;
        }
        public List<Food> SearchListFoodByName(String name)
        {
            List<Food> list = new List<Food>();
            String query = "select * from Food where name like '%"+name+"%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;
        }
    }
}
