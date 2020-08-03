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
// acceptbutton va cancelbutton dung de có thể cho nhấn enter để chon
namespace QuanLyQuanCafe
{
    public partial class AccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangAccount(loginAccount); }
        }
        void ChangAccount(Account acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }
        public AccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccountInfo()
        {
            String userName = txbUserName.Text;
            String passWord = txbPassWord.Text;
            String displayName = txbDisplayName.Text;
            String newPassWord = txbNewPassWord.Text;
            String reEnterPass = txbEnterNewPassWord.Text;
            if (!newPassWord.Equals(reEnterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu giống với mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, passWord, newPassWord))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                    {
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount{
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
    }
    // phuc vu cap nhat ten hien thi ngay sau khi doi ten hien thi
    public class AccountEvent: EventArgs
    {
        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }
    }

}
