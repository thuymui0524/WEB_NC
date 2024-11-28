using G09.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G09.Controllers
{

    public class TrangCaNhanController : Controller
    {

        private readonly DbG09foodContext _context;
        private NguoiDung us;
        public TrangCaNhanController(DbG09foodContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("TrangCaNhan/K_TrangCaNhan/{id}")]
        public IActionResult K_TrangCaNhan(int id)
        {
            /*id = 2;*/
            // Lấy thông tin người dùng 
            NguoiDung nguoiDung = _context.NguoiDungs.Find(id);

            // Lọc danh sách bài viết của người dùng 

            var baiViets_nd = _context.BaiViets
                                        .Where(b => b.MaNguoiDung == id)
                                        .Select(b => new BaiViet
                                        {
                                            MaBaiViet = b.MaBaiViet,
                                            MaNguoiDung = b.MaNguoiDung,
                                            TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                                            AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
                                            TenLoaiMonAn = b.MaLoaiMonAnNavigation.TenLoaiMonAn,
                                            NoiDung = b.NoiDung,
                                            AnhBaiViet = b.AnhBaiViet,
                                            NgayTao = b.NgayTao ?? DateTime.Now,
                                            Thiches = b.Thiches,
                                            IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == id),
                                            SoLuongLike = b.SoLuongLike
                                        }).ToList();
            // Lọc danh sách người được theo doi của người dùng 
            List<TheoDoi> dctheodoi_nd = _context.TheoDois
                                                .Where(theodoi => theodoi.MaNguoiTheoDoi == id)

                                                .ToList();
            // Lọc danh sách người theo doi của người dùng 
            List<TheoDoi> theodoi_nd = _context.TheoDois
                                                .Where(theodoi => theodoi.MaNguoiDuocTheoDoi == id)
                                                .ToList();
            //Lấy danh sách bình luận
            var cmts = _context.BinhLuans.Select(b => new BinhLuan
            {
                MaBinhLuan = b.MaBinhLuan,
                MaBaiViet = b.MaBaiViet,
                MaNguoiDung = b.MaNguoiDung,
                TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                NoiDung = b.NoiDung,
                NgayTao = b.NgayTao



            }).ToList();

            // Gán dữ liệu vào ViewBag
            ViewBag.nguoiD = nguoiDung;
            ViewBag.baiV = baiViets_nd;
            ViewBag.SoNguoi_DcTheoDoi = dctheodoi_nd.Count;
            ViewBag.SoNguoi_TheoDoi = theodoi_nd.Count;
            ViewBag.cmts = cmts;

            return View("TrangCaNhan");
        }
        public IActionResult TrangCaNhan()
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            us = _context.NguoiDungs.FirstOrDefault(t => t.Email == currentUserEmail);
            // Lấy thông tin người dùng 
            NguoiDung nguoiDung = _context.NguoiDungs.Find(us.MaNguoiDung);

            // Lọc danh sách bài viết của người dùng 

            var baiViets_nd = _context.BaiViets
                                        .Where(b => b.MaNguoiDung == us.MaNguoiDung)
                                        .Select(b => new BaiViet
                                        {
                                            MaBaiViet = b.MaBaiViet,
                                            MaNguoiDung = b.MaNguoiDung,
                                            TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                                            AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
                                            TenLoaiMonAn = b.MaLoaiMonAnNavigation.TenLoaiMonAn,
                                            NoiDung = b.NoiDung,
                                            AnhBaiViet = b.AnhBaiViet,
                                            NgayTao = b.NgayTao ?? DateTime.Now,
                                            Thiches = b.Thiches,
                                            IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == us.MaNguoiDung),
                                            SoLuongLike = b.SoLuongLike
                                        }).ToList();
            // Lọc danh sách người được theo doi của người dùng 
            List<TheoDoi> dctheodoi_nd = _context.TheoDois
                                                .Where(theodoi => theodoi.MaNguoiTheoDoi == us.MaNguoiDung)

                                                .ToList();
            // Lọc danh sách người theo doi của người dùng 
            List<TheoDoi> theodoi_nd = _context.TheoDois
                                                .Where(theodoi => theodoi.MaNguoiDuocTheoDoi == us.MaNguoiDung)
                                                .ToList();
            var cmts = _context.BinhLuans.Select(b => new BinhLuan
            {
                MaBinhLuan = b.MaBinhLuan,
                MaBaiViet = b.MaBaiViet,
                MaNguoiDung = b.MaNguoiDung,
                TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                NoiDung = b.NoiDung,
                NgayTao = b.NgayTao



            }).ToList();

            // Gán dữ liệu vào ViewBag
            ViewBag.nguoiD = nguoiDung;
            ViewBag.baiV = baiViets_nd;
            ViewBag.SoNguoi_DcTheoDoi = dctheodoi_nd.Count;
            ViewBag.SoNguoi_TheoDoi = theodoi_nd.Count;
            ViewBag.cmts = cmts;

            return View();
        }
        [HttpPost]
        [Route("TrangCaNhan/editProfile")]
        public async Task<IActionResult> EditProfile([FromForm] IFormFile image = null, [FromForm] string tenND = "", [FromForm] string TieuSu = "")
        {
            var nguoiDung = _context.NguoiDungs.Find(2);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            if (image != null)
            {
                var filePath = Path.Combine("wwwroot/User/img", image.FileName);
                // Kiểm tra xem ảnh đã tồn tại hay chưa
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }

                string imageUrl = "/User/img/" + image.FileName;
                nguoiDung.AnhDaiDien = imageUrl;
            }

            if (!string.IsNullOrWhiteSpace(tenND))
            {
                nguoiDung.TenNguoiDung = tenND;
            }

            if (!string.IsNullOrWhiteSpace(TieuSu))
            {
                nguoiDung.TieuSu = TieuSu;
            }

            _context.Update(nguoiDung);
            await _context.SaveChangesAsync();

            return RedirectToAction("TrangCaNhan"); // Redirect đến view TrangCaNhan
        }
        //[Route("TrangCaNhan/editProfile")]
        //public async Task<IActionResult> EditProfile(int userId, [FromForm] IFormFile image = null, [FromForm] string tenND = "", [FromForm] string TieuSu = "")
        //{
        //    var nguoiDung = await _context.NguoiDungs.FindAsync(userId);
        //    if (nguoiDung == null)
        //    {
        //        return NotFound();
        //    }

        //    if (image != null)
        //    {
        //        var filePath = Path.Combine("wwwroot/User/img", image.FileName);
        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await image.CopyToAsync(stream);
        //            }
        //        }
        //        nguoiDung.AnhDaiDien = "/User/img/" + image.FileName;
        //    }

        //    if (!string.IsNullOrWhiteSpace(tenND))
        //    {
        //        nguoiDung.TenNguoiDung = tenND;
        //    }

        //    if (!string.IsNullOrWhiteSpace(TieuSu))
        //    {
        //        nguoiDung.TieuSu = TieuSu;
        //    }

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // Log error and inform user
        //        ModelState.AddModelError("", "Error saving changes. Please try again.");
        //        return View(nguoiDung);
        //    }

        //    return RedirectToAction("TrangCaNhan");
        //}

    }
}
