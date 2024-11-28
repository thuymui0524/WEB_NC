using G09.Models;
using Microsoft.AspNetCore.Mvc;

namespace G09.Controllers
{
    public class TimKiemController : Controller
    {
        private readonly DbG09foodContext _context;
        private NguoiDung us;
        public TimKiemController(DbG09foodContext context)
        {
            _context = context;
        }

        /* public IActionResult Index(string searchQuery)
         {
             var posts = from p in _context.BaiViets
                         select p;

             if (!string.IsNullOrEmpty(searchQuery))
             {
                 posts = posts.Where(p => p.NoiDung.Contains(searchQuery));
             }


             return View(posts.ToList());
         }

         [HttpPost]
         public IActionResult Index1(string? searchQuery)
         {    
             var posts = from p in _context.BaiViets
                         select p;

             if (!string.IsNullOrEmpty(searchQuery))
             {
                 posts = posts.Where(p => p.NoiDung.Contains(searchQuery));
                 return RedirectToAction("Index", new { searchQuery = searchQuery });
             }

             return View(posts.ToList());
         }*/


        public IActionResult Index(string? searchQuery)
        {
            // Lấy email của người dùng hiện tại từ Session
            var currentUserEmail = HttpContext.Session.GetString("Email");

            // Tìm người dùng hiện tại dựa trên email
            us = _context.NguoiDungs
                .FirstOrDefault(t => t.Email == currentUserEmail);

            // Lấy danh sách bài viết từ cơ sở dữ liệu
            var baiVietsQuery = _context.BaiViets.AsQueryable();

            // Nếu có từ khóa tìm kiếm, thực hiện lọc theo nội dung
            if (!string.IsNullOrEmpty(searchQuery))
            {
                baiVietsQuery = baiVietsQuery.Where(b => b.NoiDung.Contains(searchQuery));
            }

            // Thực hiện dự án bài viết và các thông tin liên quan
            var baiViets = baiVietsQuery
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
                })
                .ToList();

            return View(baiViets);
            //var currentUserEmail = HttpContext.Session.GetString("Email");
            //us = _context.NguoiDungs
            //   .FirstOrDefault(t => t.Email == currentUserEmail);
            //if (string.IsNullOrEmpty(searchQuery))
            //{
            //    searchQuery = "2";

            //}
            //var baiViets = _context.BaiViets
            //                             .Where(b => b.NoiDung.Contains(searchQuery))
            //                             .Select(b => new BaiViet
            //                             {
            //                                 MaBaiViet = b.MaBaiViet,
            //                                 MaNguoiDung = b.MaNguoiDung,
            //                                 TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
            //                                 AnhDaiDien = b.MaNguoiDungNavigation.AnhDaiDien,
            //                                 TenLoaiMonAn = b.MaLoaiMonAnNavigation.TenLoaiMonAn,
            //                                 NoiDung = b.NoiDung,
            //                                 AnhBaiViet = b.AnhBaiViet,
            //                                 NgayTao = b.NgayTao ?? DateTime.Now,
            //                                 Thiches = b.Thiches,
            //                                 IsLiked = _context.Thiches.Any(t => t.MaBaiViet == b.MaBaiViet && t.MaNguoiDung == us.MaNguoiDung),
            //                                 SoLuongLike = b.SoLuongLike
            //                             }).ToList();
            //return View(baiViets);


        }
    }
}
