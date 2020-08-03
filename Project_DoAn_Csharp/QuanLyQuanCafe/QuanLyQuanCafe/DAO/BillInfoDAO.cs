using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instace;

        public static BillInfoDAO Instace {
            get { if (instace == null) instace = new BillInfoDAO(); return BillInfoDAO.instace; }
            private set { BillInfoDAO.instace = value; } 
        }
        private BillInfoDAO() { }
        public List<BillInfo> GetListBillInfo(int id)// truyen vao id Bill
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from BillInfo where idBill= "+id);
            foreach(DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }
        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("InsertBillInfo @idBill , @idFood , @count", new object[] { idBill,idFood,count });
        }
        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete BillInfo where idFood= " + id);
        }
    }
}
