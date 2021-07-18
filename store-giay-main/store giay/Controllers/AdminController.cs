using store_giay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace store_giay.Controllers
{
    public class AdminController : Controller
    {

        dbStoregiayDataContext db = new dbStoregiayDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["Username"];
            var matkhau = collection["Password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Nhập Tên Đăng Nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Nhập Mật Khẩu";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PasswordAdmin == matkhau);
                if (ad != null)
                {
                    Session["TaiKhoanAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult Sanpham()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.Sanphams.ToList());
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Create(Sanpham sanpham)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.Sanphams.InsertOnSubmit(sanpham);
                db.SubmitChanges();
                return View("Index", "Sanpham");
            }
        }

        public ActionResult Details(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sanpham = from sp in db.Sanphams where sp.idSanpham == id select sp;
                return View(sanpham.SingleOrDefault());
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sanpham = from sp in db.Sanphams where sp.idSanpham == id select sp;
                return View(sanpham.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteSanPham(int id)
        {
            Sanpham sanpham = db.Sanphams.Where(n => n.idSanpham == id).SingleOrDefault();
            db.Sanphams.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Index", "Sanpham");

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sanpham = from sp in db.Sanphams where sp.idSanpham == id select sp;
                return View(sanpham.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult EditSanPham(int id)
        {
            Sanpham sanpham = db.Sanphams.Where(n => n.idSanpham == id).SingleOrDefault();
            UpdateModel(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Index", "Sanpham");

        }

        public ActionResult LoaiGiay()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.Loaisanphams.ToList());
        }
       
        public ActionResult NhanHieu()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.Nhanhieus.ToList());
        }


    }
}