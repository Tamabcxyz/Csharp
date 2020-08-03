using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thật sự muốn thoát","Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;// khi nguoi ta khong bấm ok thì event này không được thực thi
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String userName = txbUserName.Text;
            String passWord = txbPassWord.Text;
            if (checklogin(userName,passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                TableManeger f = new TableManeger(loginAccount);
                this.Hide();
                f.ShowDialog();// xu ly truoc thang tablemaneger moi tới những thằng khác đó là tính chất đặt biệt của showdialog
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
            }
        }

        // kiem tra dang nhap
        bool checklogin(String userName, String passWord)
        {
            return AccountDAO.Instance.CheckLogin(userName,passWord);
        }
    }
}
