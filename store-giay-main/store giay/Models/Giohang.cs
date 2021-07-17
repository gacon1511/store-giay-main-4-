using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace store_giay.Models
{
    public class Giohang
    {
        dbStoregiayDataContext data = new dbStoregiayDataContext();

        public int iMaSanpham { set; get; }
        public string sTenSanpham { set; get; }
        public string sHinhSanpham { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        public Giohang(int idSanpham)
        {
            iMaSanpham = idSanpham;
            Sanpham sanpham = data.Sanphams.Single(n => n.idSanpham == iMaSanpham);
            sTenSanpham = sanpham.tenSanpham;
            sHinhSanpham = sanpham.hinhSanpham;
            dDongia = double.Parse(sanpham.giabanSanpham.ToString());
            iSoluong = 1;
        }
    }
}