using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyQuanCafe.DTO.Menu;

namespace QuanLyQuanCafe
{
    public partial class TableManeger : Form
    {
        private Account loginAccount;

        public Account LoginAccount {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }// an nut admin
        }

        public TableManeger(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwichTable);
        }
        void ChangeAccount(int type)
        {
            // neu =1 la true =0 la false
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }
        //
        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCatagory.DataSource = listCategory;
            cbCatagory.DisplayMember = "Name";// giup hien thi truong muon hien thi len cb
        }
        //
        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        public void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach(Table item in tableList)
            {
                Button btn = new Button()
                {
                    Width = TableDAO.tableWidth,
                    Height = TableDAO.tableHeight
                };
                
                btn.Text = item.Name + Environment.NewLine + item.Status;// Environment.NewLine xuong dong
                btn.Click += btn_Click;
                btn.Tag = item;// luu table duoc click vao
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Violet;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }
                flpTable.Controls.Add(btn);
               
            }

        }
        void ShowBill(int id)// show bill cua ban id
        {
            float totalPrice = 0;
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            foreach(QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo curtual = new CultureInfo("vi");
            Thread.CurrentThread.CurrentCulture = curtual;
            txbTotalPrice.Text = totalPrice.ToString("c");
            LoadTable();
        }
        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID=((sender as Button).Tag as Table).ID;

            // moi khi click vao ban nao thi button se co Tag cua dua do
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;// lay table
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount =(int) nmDiscount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có thực sự thanh toán hóa đơn cho bàn {0}\n Tổng tiền -(Tổng tiền/100)*Giảm giá\n {1}-({1}/100)*{2}={3}", table.Name,totalPrice,discount,finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill,discount,(float)finalTotalPrice);
                    ShowBill(table.ID);
                }
            }
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile(loginAccount);
            f.UpdateAccount += F_UpdateAccount;

            f.ShowDialog();
        }

        private void F_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "(Thông tin tài khoản " + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.loginAccount = LoginAccount;// phuc vu cho ko xoa tai khoan dang dang nhap hien tai
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCatagory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCatagory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCatagory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
        }

        // khi selected duoc chon thi danh sach
        private void cbCatagory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id=0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int FoodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;
            // kiem tra chua co Bill nao khong neu ko =-1
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instace.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(),FoodID,count);
            }
            else// bill da ton tai
            {
                BillInfoDAO.Instace.InsertBillInfo(idBill, FoodID, count);
            }
            ShowBill(table.ID);
        }
        private void btnSwichTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwichTable.SelectedItem as Table).ID;
            if(MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bàn {0} vào bàn {1}", (lsvBill.Tag as Table).Name, (cbSwichTable.SelectedItem as Table).Name),"Thông báo",MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }

        #endregion

        private void themMonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());// goi den event cua btn
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }
    }
}
