using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using store_giay.Models;

namespace store_giay.Controllers
{
    public class SanphamController : Controller
    {
        // GET: Sanpham
        dbStoregiayDataContext db = new dbStoregiayDataContext();
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
                return View("Index","Sanpham");
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