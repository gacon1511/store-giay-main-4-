using store_giay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;


namespace store_giay.Controllers
{
    public class AdminController : Controller
    {

        dbStoregiayDataContext db = new dbStoregiayDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
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
        //San Pham
        public ActionResult Sanpham()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            
                return View(db.Sanphams.OrderByDescending(n => n.idSanpham).ToList());
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
        [ValidateInput(false)]
        public ActionResult Create(Sanpham sanpham, HttpPostedFileBase fileUpload)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                if(fileUpload == null)
                {
                    ViewBag.Thongbao = "Vui Lòng chọn ảnh";
                    return View();

                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //luu ten file
                        var fileName = Path.GetFileName(fileUpload.FileName);
                        //luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                        //kiem tra hinh anh ton tai chua
                        if (System.IO.File.Exists(path))
                        {
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        }
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileUpload.SaveAs(path);
                        }
                        sanpham.hinhSanpham = fileName;
                        db.Sanphams.InsertOnSubmit(sanpham);
                        db.SubmitChanges();
                        //return View("Sanpham", "Admin");
                    }
                    return RedirectToAction("Sanpham", "Admin");
                }
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
            return RedirectToAction("Sanpham", "Admin");

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
        [ValidateInput(false)]
        public ActionResult EditSanPham(int id)
        {
            Sanpham sanpham = db.Sanphams.SingleOrDefault(n => n.idSanpham == id);
            UpdateModel(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Sanpham", "Admin");

        }
        // Loai San Pham
        public ActionResult LoaiGiay()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.Loaisanphams.ToList());
        }
        [HttpGet]
        public ActionResult CreateLSP()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult CreateLSP(Loaisanpham loaisanpham)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.Loaisanphams.InsertOnSubmit(loaisanpham);
                db.SubmitChanges();
                return RedirectToAction("LoaiGiay", "Admin");
            }
        }

        public ActionResult DetailsLSP(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var loaisanpham = from lsp in db.Loaisanphams where lsp.idLoaisanpham  == id select lsp;
                return View(loaisanpham.SingleOrDefault());
            }
        }
        public ActionResult DeleteLSP(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var loaisanpham = from lsp in db.Loaisanphams where lsp.idLoaisanpham == id select lsp;
                return View(loaisanpham.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("DeleteLSP")]
        public ActionResult DeleteLSanPham(int id)
        {
            Loaisanpham loaisanpham = db.Loaisanphams.Where(n => n.idLoaisanpham == id).SingleOrDefault();
            db.Loaisanphams.DeleteOnSubmit(loaisanpham);
            db.SubmitChanges();
            return RedirectToAction("LoaiGiay", "Admin");

        }
        [HttpGet]
        public ActionResult EditLSP(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var loaisanpham = from lsp in db.Loaisanphams where lsp.idLoaisanpham == id select lsp;
                return View(loaisanpham.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("EditLSP")]
        public ActionResult EditLSanPham(int id)
        {
            Loaisanpham loaisanpham = db.Loaisanphams.Where(n => n.idLoaisanpham == id).SingleOrDefault();
            UpdateModel(loaisanpham);
            db.SubmitChanges();
            return RedirectToAction("LoaiGiay", "Admin");

        }

        //Nhan Hieu
        public ActionResult NhanHieu()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View(db.Nhanhieus.ToList());
        }

        [HttpGet]
        public ActionResult CreateNH()
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult CreateNH(Nhanhieu nhanhieu)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                db.Nhanhieus.InsertOnSubmit(nhanhieu);
                db.SubmitChanges();
                return RedirectToAction("NhanHieu", "Admin");
            }
        }

        public ActionResult DetailsNH(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhanhieu = from nh in db.Nhanhieus where nh.idNhanhieu == id select nh;
                return View(nhanhieu.SingleOrDefault());
            }
        }
        public ActionResult DeleteNH(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhanhieu = from nh in db.Nhanhieus where nh.idNhanhieu == id select nh;
                return View(nhanhieu.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("DeleteNH")]
        public ActionResult DeleteNH1(int id)
        {
            try
            {
                Nhanhieu nhanhieu = db.Nhanhieus.Where(n => n.idNhanhieu == id).SingleOrDefault();
                db.Nhanhieus.DeleteOnSubmit(nhanhieu);
                db.SubmitChanges();
                return RedirectToAction("NhanHieu", "Admin");
            }
            catch (Exception ex)
            {
                TempData["ThongBao"] = ex.Message;
                return RedirectToAction("DeleteNH", "Admin", new {id = id });
            }
           /* Nhanhieu nhanhieu = db.Nhanhieus.Where(n => n.idNhanhieu == id).SingleOrDefault();
            db.Nhanhieus.DeleteOnSubmit(nhanhieu);
            db.SubmitChanges();
            return RedirectToAction("NhanHieu", "Admin");*/

        }
        [HttpGet]
        public ActionResult EditNH(int id)
        {
            if (Session["TaiKhoanAdmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var nhanhieu = from nh in db.Nhanhieus where nh.idNhanhieu == id select nh;
                return View(nhanhieu.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("EditNH")]
        public ActionResult EditNH1(int id)
        {
            Nhanhieu nhanhieu = db.Nhanhieus.Where(n => n.idNhanhieu == id).SingleOrDefault();
            UpdateModel(nhanhieu);
            db.SubmitChanges();
            return RedirectToAction("NhanHieu", "Admin");

        }

    }
}