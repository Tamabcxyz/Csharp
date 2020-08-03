using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class Admin : Form
    {
        public Account loginAccount;// phuc vu cho ko xoa tai khoa dang login
        BindingSource foodlist = new BindingSource();
        BindingSource accountlist = new BindingSource();
        BindingSource Categorylist = new BindingSource();
        BindingSource Tablelist = new BindingSource();
        public Admin()
        {
            
            InitializeComponent();


            Load();

        }
        void Load()
        {
            dtgvFood.DataSource = foodlist;
            dtgvAccount.DataSource = accountlist;
            dtgvCategory.DataSource = Categorylist;
            dtgvTable.DataSource = Tablelist;
            LoadDateTimePickerBill();
            //LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageNumber.Text));
            LoadListFood();
            AddFoodBinding();
            AddAccountBinding();
            AddCategoryBinding();
            AddTableBinding();
            LoadCategoryIntoComboBox(cbFoodCategory);
            LoadAccount();
            LoadListCategory();
            LoadListTable();
        }
        // load table list
        void LoadListTable()
        {
            Tablelist.DataSource = TableDAO.Instance.LoadTableList();
        }
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id"));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name"));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status"));
        }
        // load category
        void LoadListCategory()
        {
            Categorylist.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id"));
            txbNameCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name"));
            
        }
        List<Food>SearchListFoodByName(String str)
        {
            List<Food> listFood = FoodDAO.Instance.SearchListFoodByName(str);

            return listFood;
        }
        
        #region methods
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
           dtgvBill.DataSource= BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }


        void LoadListBillByDateAndPage(DateTime checkIn, DateTime checkOut, int page)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value,dtpkToDate.Value,Convert.ToInt32(txbPageNumber.Text));
        }



        void LoadListFood()
        {
            foodlist.DataSource = FoodDAO.Instance.GetListFood();
        }
        void AddFoodBinding()
        {
            // lay theo con tro chuot, thay doi gia tri Text bang thuoc tinh Name trong dtgvFood.DataSource
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true,DataSourceUpdateMode.Never));//true,DataSourceUpdateMode.Never du lieu se di1 chieu tu dtgv sang txb
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
            
        }
        void LoadAccount()
        {
            accountlist.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName"));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName"));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type"));
        }

        void LoadCategoryIntoComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddAccount(String name, String displayname, int type)
        {
            if(AccountDAO.Instance.InsertAccount(name, displayname, type))
            {
                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
            LoadAccount();
        }
        void EditAccount(String name,String displayname, int type)
        {
            if (AccountDAO.Instance.EditAccount(name, displayname, type))
            {
                MessageBox.Show("Sửa thành công");
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
            LoadAccount();
        }
        void DeleteAccount(String name)
        {
            if (loginAccount.UserName.Equals(name))
            {
                MessageBox.Show("Tài khoản đăng nhập không thể xóa");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(name))
            {
                MessageBox.Show("Xóa thành công");
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
            LoadAccount();
        }
        

        void ResetPassWord(String name)
        {
            if (AccountDAO.Instance.ResetPassWord(name))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }
        #endregion

        #region event
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            //LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageNumber.Text));
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txbFoodID_TextChange(object sender, EventArgs e)
        {
            try// try catch de neu tim ko thay thuc an cung ko bị loi
            {

                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryId"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    //cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            String name = txbFoodName.Text;
            int CategoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)(nmFoodPrice.Value);
            if (FoodDAO.Instance.InsertFood(name, CategoryID, price))
            {
                MessageBox.Show("Thêm thành công");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            String name = txbFoodName.Text;
            int CategoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)(nmFoodPrice.Value);
            if (FoodDAO.Instance.UpdateFood(id,name, CategoryID, price))
            {
                MessageBox.Show("Cập nhật thành công");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa thành công");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }

        // lam nhu vay de khoi phai tat chuong trinh
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler insertFoodCategory;
        public event EventHandler InsertFoodCategory
        {
            add { insertFoodCategory += value; }
            remove { insertFoodCategory -= value; }
        }

        private event EventHandler deleteFoodCategory;
        public event EventHandler DeleteFoodCategory
        {
            add { deleteFoodCategory += value; }
            remove { deleteFoodCategory -= value; }
        }

        private event EventHandler updateFoodCategory;
        public event EventHandler UpdateFoodCategory
        {
            add { updateFoodCategory += value; }
            remove { updateFoodCategory -= value; }
        }
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            // do dung data binding nen ta phai
            foodlist.DataSource=SearchListFoodByName(txbSearchFoodName.Text);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            String name = txbUserName.Text;
            String displayname = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;
            AddAccount(name, displayname, type);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            String name = txbUserName.Text;
            String displayname = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;
            EditAccount(name, displayname, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            String name = txbUserName.Text;
            DeleteAccount(name);
        }

        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            String name=txbUserName.Text;
            ResetPassWord(name);
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            txbPageNumber.Text = "1"; // trang dau tien la trang 1

        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value,dtpkToDate.Value);
            int lastPage = sumRecord / 5;
            if (sumRecord % 10 != 0)
                lastPage++;
            txbPageNumber.Text = lastPage.ToString();
        }

        private void txbPageNumber_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value,dtpkToDate.Value,Convert.ToInt32(txbPageNumber.Text));
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageNumber.Text);
            if (page > 1)
                page--;
            txbPageNumber.Text = page.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageNumber.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);
            if (page < sumRecord)
                page++;
            txbPageNumber.Text = page.ToString();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            
            String name = txbNameCategory.Text;
            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm thành công");
                LoadListCategory();
                if (insertFoodCategory != null)
                {
                    insertFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
            
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);
            String name = txbNameCategory.Text;
            if (CategoryDAO.Instance.UpdateFoodCategory(id,name))
            {
                MessageBox.Show("Sửa thành công");
                LoadListCategory();
                if (updateFoodCategory != null)
                {
                    updateFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);
            if (CategoryDAO.Instance.DeleteFoodCategory(id))
            {
                MessageBox.Show("Xóa thành công");
                LoadListCategory();
                if (deleteFoodCategory != null)
                {
                    deleteFoodCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            String tablename = txbTableName.Text;
            String tablestatus = cbTableStatus.SelectedItem.ToString();
            if (TableDAO.Instance.InsertTable(tablename,tablestatus))
            {
                MessageBox.Show("Thêm thành công");
                LoadListTable();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm thất bại");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            String name = txbTableName.Text;
            String tablestatus = cbTableStatus.SelectedItem.ToString();
            if (TableDAO.Instance.UpdateTable(id, name, tablestatus))
            {
                MessageBox.Show("Sửa thành công");
                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Sửa thất bại");
            }
        }

        private void tbnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa thành công");
                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa thất bại");
            }
        }


        #endregion

        /*
void LoadAccountList()
{
   // tao noi ket
   String query = "exec GetAccountByUserName  @userName";
   dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query,new object[]{"VoAnh"});
}
void LoadFoodList()
{
   String query = "exec GetFoodList;";
   dtgvFood.DataSource = DataProvider.Instance.ExecuteQuery(query);
}
*/

    }
}
