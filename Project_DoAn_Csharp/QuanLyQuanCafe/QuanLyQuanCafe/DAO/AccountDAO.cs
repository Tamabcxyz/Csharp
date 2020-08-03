using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class AccountDAO
    {
        // singleton thay vi khoi tao nhieu lan ta chi can .Instance la ok
        private static AccountDAO instance;

        internal static AccountDAO Instance {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }// khong duoc thay gan gia tri chi duoc doc thoi
        }
        private AccountDAO() { }
        public bool CheckLogin(String userName, String passWord)
        {
            /*
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);// lay ra 1 mang byte tu chuoi passWord
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);// has them may tinh lay ra mang byte hasData da chua mat khau sau khi duoc bam ra
            String hasPass = "";
            foreach(byte item in hasData)
            {
                hasPass += item;
            }*/

            String query = "exec Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query,new object[] { userName,passWord});
            return result.Rows.Count>0;
        }
        public Account GetAccountByUserName(String userName)
        {
            DataTable data=DataProvider.Instance.ExecuteQuery("select * from Account where UserName= '" + userName + "'");
            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(String userName, String displayName, String pass, String newPass)
        {

            int result = DataProvider.Instance.ExecuteNonQuery("exec UpdateAccount @userName , @displayName , @passWord , @newPassWord ", new object[] { userName,displayName,pass,newPass });
            return result > 0;
        }
        public DataTable GetListAccount()
        {
            String sql = "select UserName, DisplayName, Type from Account";
            return DataProvider.Instance.ExecuteQuery(sql);
        }








        public bool InsertAccount(String name, String displayName, int type)
        {
            String query = "exec InsertAccount @name , @displayName , @type ";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name,displayName,type });
            return resutl > 0;
        }

        public bool EditAccount(String name, String displayName, int type)
        {
            String query = "exec EditAccount  @name , @displayName , @type";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name,displayName,type });
            return resutl > 0;
        }
        public bool DeleteAccount(String name)
        {
            String query = "exec DeleteAccount  @name";
            int resutl = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name});
            return resutl > 0;

        }
        public bool ResetPassWord(String name)
        {
            String query = "exec ResetPass @name";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name });
            return result > 0;
        }
    }
}
