using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static int tableWidth = 90;
        public static int tableHeight = 90;
        internal static TableDAO Instance {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();// chua danh sach cac table
            DataTable data = DataProvider.Instance.ExecuteQuery("exec GetTableList");
            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }
            return tableList;
        }
        // chuyen ban
        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("exec SwitchTable @idTable1 , @idTable2", new object[] { id1, id2});
        }
        public bool InsertTable(String ten, String trangthai)
        {
            String query = "exec InsertTable @name , @status";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { ten,trangthai });
            return resutl > 0;
        }
        public bool UpdateTable(int id, String ten, String trangthai)
        {
            String query = "exec UpdateTable @id , @name , @status";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] {id, ten, trangthai });
            return resutl > 0;
        }
        public bool DeleteTable(int id)
        {
            String query = "delete TableFood where id= " + id;
            int resutl = DataProvider.Instance.ExecuteNonQuery(query);
            return resutl > 0;
        }
    }
}
