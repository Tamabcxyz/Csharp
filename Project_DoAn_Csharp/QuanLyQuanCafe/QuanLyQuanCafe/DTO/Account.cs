using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        // ham khoi tao chua tham so can co trong bang account
        public Account(String userName,String displayName, int type, String passWord)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.PassWord = passWord;
        }
        // ham khoi tao voi tham so la DataRow phuc vu cho viec tra ve du lieu khi AccountDAO goi
        public Account(DataRow rows)
        {
            this.UserName = rows["UserName"].ToString();
            this.DisplayName = rows["DisplayName"].ToString();
            this.Type = (int)rows["Type"];
            this.PassWord = rows["PassWord"].ToString();
        }
        private int type;
        private String passWord;
        private String displayName;
        private String userName;

        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public int Type { get => type; set => type = value; }
    }
}
