using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private String connecttionStr = "Data Source=DESKTOP-46NLQ1J;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        // tao singleton khac phuc khi muon goi de phuong thuc trong class DataProvider tu class khac khong can khoi tao
        private static DataProvider instance;

        public static DataProvider Instance {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; } // private de chi co the get gia tri trong class DataProvider thoi
        }
        private DataProvider() { }
        // thuc thi lenh tra ve du lieu
        public DataTable ExecuteQuery(String query, object[] parameter=null)
        {
            DataTable data = new DataTable();
            // su dung using giup tu dong dong ket noi sau khi thuc hien
            using (SqlConnection conn = new SqlConnection(connecttionStr))// tao ket noi voi connectionstr o tren
            {
                conn.Open(); // mo ket noi
                SqlCommand s = new SqlCommand(query, conn);// thuc thi cau lenh query tren bang SqlCommand thuc thi dua tren ket noi conn

                if (parameter != null)
                {
                    String[] listPara = query.Split(' ');// tach chuoi query truyen vao theo khoan trang de xac dinh parameter
                    int i = 0;
                    foreach(String item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            // them parameter vao SqlCommand
                            s.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(s);// lay du lieu tu cau truy van cua SqlCommand
                adapter.Fill(data); // do du lieu vao datatable
                conn.Close(); // dong ket noi
            }
            return data;
        }
        // tra ve so dong tac dong
        public int ExecuteNonQuery(String query, object[] parameter = null)
        {
           int data = 0;
            // su dung using giup tu dong dong ket noi sau khi thuc hien
            using (SqlConnection conn = new SqlConnection(connecttionStr))// tao ket noi voi connectionstr o tren
            {
                conn.Open(); // mo ket noi
                SqlCommand s = new SqlCommand(query, conn);// thuc thi cau lenh query tren bang SqlCommand thuc thi dua tren ket noi conn

                if (parameter != null)
                {
                    String[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (String item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            s.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = s.ExecuteNonQuery();
                conn.Close(); // dong ket noi
            }
            return data;// tra ve so dong chiu tac dong 
        }

        public object ExecuteScalar(String query, object[] parameter = null)
        {
            object data = 0;
            // su dung using giup tu dong dong ket noi sau khi thuc hien
            using (SqlConnection conn = new SqlConnection(connecttionStr))// tao ket noi voi connectionstr o tren
            {
                conn.Open(); // mo ket noi
                SqlCommand s = new SqlCommand(query, conn);// thuc thi cau lenh query tren bang SqlCommand thuc thi dua tren ket noi conn

                if (parameter != null)
                {
                    String[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (String item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            s.Parameters.AddWithValue(item, parameter[i]);
                            i++;
                        }
                    }
                }
                data = s.ExecuteScalar();
                conn.Close(); // dong ket noi
            }
            return data;
        }
    }
}
