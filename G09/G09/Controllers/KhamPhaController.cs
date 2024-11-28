using G09.Models;
using Microsoft.AspNetCore.Mvc;

namespace G09.Controllers
{
    [Route("g09/")]
    public class KhamPhaController : Controller
    {
        private readonly DbG09foodContext _context;
        private NguoiDung us;
        public KhamPhaController(DbG09foodContext context)
        {
            _context = context;
        }

        [Route("khampha")]
        public IActionResult Index(int? loaiMonAnId)
        {
            var currentUserEmail = HttpContext.Session.GetString("Email");
            us = _context.NguoiDungs
               .FirstOrDefault(t => t.Email == currentUserEmail);
            var loaiMonAns = _context.LoaiMonAns.ToList();

            if (!loaiMonAnId.HasValue && loaiMonAns.Any())
            {
                loaiMonAnId = loaiMonAns.First().MaLoaiMonAn;
            }

            var baiViets = _context.BaiViets
                .Where(b => !loaiMonAnId.HasValue || b.MaLoaiMonAn == loaiMonAnId.Value)
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
            var cmts = _context.BinhLuans.Select(b => new BinhLuan
            {
                MaBinhLuan = b.MaBinhLuan,
                MaBaiViet = b.MaBaiViet,
                MaNguoiDung = b.MaNguoiDung,
                TenNguoiDung = b.MaNguoiDungNavigation.TenNguoiDung,
                NoiDung = b.NoiDung,
                NgayTao = b.NgayTao



            }).ToList();

            ViewBag.LoaiMonAns = loaiMonAns;
            ViewBag.SelectedLoaiMonAnId = loaiMonAnId;
            ViewBag.cmts = cmts;

            return View(baiViets);
        }
    }
}
