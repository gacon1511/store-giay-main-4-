using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using store_giay.Models;

namespace store_giay.Controllers
{
    public class GiohangController : Controller
    {
        dbStoregiayDataContext db = new dbStoregiayDataContext();
        // GET: Giohang
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGiohang(int iMaSanpham, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();

            Giohang sanpham = lstGiohang.Find(n => n.iMaSanpham == iMaSanpham);
            if (sanpham == null)
            {
                sanpham = new Giohang(iMaSanpham);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int TongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                TongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return TongSoLuong;
        }
        private double TongTien()
        {
            double TongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                TongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return TongTien;
        }
        private double TongTienUSD()
        {
            double TongTienUSD = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                TongTienUSD = 23000;
            }
            return TongTienUSD;
        }
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GiayStore");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaSP)
        {

            List<Giohang> lstGiohang = Laygiohang();
  
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSanpham == iMaSP);

            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSanpham == iMaSP);
                return RedirectToAction("GioHang");

            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaSanpham == iMaSP);
            
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "GiayStore");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "GiayStore");
            }

            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            Donhang ddh = new Donhang();
            Khachhang kh = (Khachhang)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.idKhachhang = kh.idKhachhang;
            ddh.ngayDathang = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.ngayGiaohang = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            db.Donhangs.InsertOnSubmit(ddh);
            db.SubmitChanges();
         
            foreach (var item in gh)
            {
                Chitietdonhang ctdh = new Chitietdonhang();
                ctdh.idDonhang = ddh.idDonhang;
                ctdh.idSanpham = item.iMaSanpham;
                ctdh.Soluong = item.iSoluong;
                ctdh.dongia = (decimal)item.dDongia;
                db.Chitietdonhangs.InsertOnSubmit(ctdh);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}