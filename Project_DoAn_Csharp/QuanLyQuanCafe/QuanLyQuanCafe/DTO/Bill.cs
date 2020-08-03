using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        // kieu du lieu DateTime? ko can truyen vao thoi gian 
        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, int discount)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }
        public Bill(DataRow row)
        {
            this.Discount = (int)row["disCount"];
            this.ID = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckIn"];
            var dateCheckOutTemp = row["dateCheckOut"];// kiem tra thoi gian checkOut co nhan duoc vao chua
            if(dateCheckOutTemp.ToString()!="")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;
            this.Status = (int)status;
            
        }
        private int discount;
        private int status;
        private DateTime? dateCheckOut;
        private DateTime? dateCheckIn; // co dau ? truoc DateTime de co the cho phep null
        private int iD;

        public int ID { get => iD; set => iD = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}
