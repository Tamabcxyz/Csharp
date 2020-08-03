using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    // co nhiem vu lay ra bill info tu id bill
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Bill where idTable="+id+" and status=0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;// la duo id bill
            }
            return -1;// khong co truong nao
        }
        public void InsertBill(int idTable)
        {
            DataProvider.Instance.ExecuteNonQuery("exec InsertBill @idTable", new object[] { idTable });
        }
        // do id bill them vao la id lon nhat
        public int GetMaxIDBill()
        {
            try{
               return (int)DataProvider.Instance.ExecuteScalar("select MAX(id) from Bill");
            }catch
            {
                return 1;
            }
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            String query = "update Bill set DateCheckOut=GETDATE(), status=1,"+"disCount = "+discount+", totalPrice="+totalPrice+" where id="+id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            // luu y tro @dateCheckIn , @dateCheckOut ko cach khoan ko chay
            return DataProvider.Instance.ExecuteQuery("exec GetListBillByDate @dateCheckIn , @dateCheckOut", new object[] { checkIn, checkOut });
        }
        public DataTable GetBillListByDateAndPage(DateTime checkIn, DateTime checkOut, int page)
        {
            
            return DataProvider.Instance.ExecuteQuery("exec GetListBillByDateAndPage @dateCheckIn , @dateCheckOut , @page", new object[] { checkIn, checkOut, page });
        }
        // tra ve so luong page
        public int GetNumBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            // luu y tro @dateCheckIn , @dateCheckOut ko cach khoan ko chay
            return (int)DataProvider.Instance.ExecuteScalar("exec GetNumBillByDate @dateCheckIn , @dateCheckOut", new object[] { checkIn, checkOut });
        }
    }
}
