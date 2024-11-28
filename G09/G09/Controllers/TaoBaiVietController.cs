using G09.Models;
using G09.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace G09.Controllers
{
    public class TaoBaiVietController : Controller
    {
        private readonly DbG09foodContext _context;

        public TaoBaiVietController(DbG09foodContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[Route("TaoBaiViet/CratePost")]
        //public async Task<IActionResult> CratePost([FromForm] IFormFile image, [FromForm] int postType, [FromForm] string postContent)
        //{

        //    if (postType != 0 && !string.IsNullOrWhiteSpace(postContent))
        //    {
        //        var filePath = Path.Combine("wwwroot/Post/img", image.FileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await image.CopyToAsync(stream);
        //        }

        //        DateTime now = DateTime.Now;
        //        string imageUrl = "/Post/img/" + image.FileName;

        //        BaiViet baiv = new BaiViet
        //        {
        //            MaNguoiDung = HttpContext.Session.GetInt32("id"),
        //            NoiDung = postContent,
        //            AnhBaiViet = imageUrl,
        //            MaLoaiMonAn = postType,
        //            NgayTao = now,
        //            SoLuongLike=0
        //        };
        //        NguoiDung nguoiDung = await _context.NguoiDungs.FindAsync(HttpContext.Session.GetInt32("id"));
        //        await _context.AddAsync(baiv);
        //        await _context.SaveChangesAsync();

        //        var response = new
        //        {
        //            success = true,
        //            UrlImg = imageUrl,
        //            Date = now.ToString("dd/MM/yyyy HH:mm:ss"),
        //            Us = new
        //            {
        //                nguoiDung.MaNguoiDung,
        //                nguoiDung.TenNguoiDung,
        //                nguoiDung.AnhDaiDien
        //            }
        //        };
        //        return Json(response);
        //        /*return View();*/
        //    }

        //    return Json(new { success = false });
        //    /*return View();*/
        //}
        [HttpPost]
        [Route("TaoBaiViet/CratePost")]
        public async Task<IActionResult> CratePost([FromForm] IFormFile image, [FromForm] int postType, [FromForm] string postContent)
        {
            // Check if the user ID exists in the session
            var userId = HttpContext.Session.GetInt32("id");
            if (userId == null)
            {
                return Json(new { success = false, message = "User is not logged in." });
            }

            // Validate post type and content
            if (postType == 0 || string.IsNullOrWhiteSpace(postContent))
            {
                return Json(new { success = false, message = "Invalid post type or content." });
            }

            // Check if an image is provided
            if (image != null && image.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Post/img", image.FileName);

                try
                {
                    // Save the image file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    // Handle file upload exceptions
                    return Json(new { success = false, message = $"Error uploading image: {ex.Message}" });
                }

                DateTime now = DateTime.Now;
                string imageUrl = "/Post/img/" + image.FileName;

                // Create new post
                BaiViet baiv = new BaiViet
                {
                    MaNguoiDung = userId.Value, // Ensure the userId is not null
                    NoiDung = postContent,
                    AnhBaiViet = imageUrl,
                    MaLoaiMonAn = postType,
                    NgayTao = now,
                    SoLuongLike = 0
                };

                // Save the post to the database
                try
                {
                    await _context.BaiViets.AddAsync(baiv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    // Handle database update exceptions
                    return Json(new { success = false, message = $"Database error: {ex.Message}" });
                }

                // Successful response
                var response = new
                {
                    success = true,
                    UrlImg = imageUrl,
                    Date = now.ToString("dd/MM/yyyy HH:mm:ss"),
                    Us = new
                    {
                        MaNguoiDung = userId.Value,
                        TenNguoiDung = HttpContext.Session.GetString("tenND"),
                        UrlAnhDD = HttpContext.Session.GetString("UrlAnhDD")
                    }
                };
                return Json(response);
            }

            return Json(new { success = false, message = "No image provided." });
        }

        [HttpGet]
        [Route("TaoBaiViet/GetPost/{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var baiV = await _context.BaiViets.FindAsync(id);
            if (baiV == null)
            {
                return Json(new { success = false });
            }
            var result = new
            {
                success = true,
                postType = baiV.MaLoaiMonAn,
                postContent = baiV.NoiDung , 
                imageUrl = baiV.AnhBaiViet  
            };
            return Json(result);
        }
        [HttpPost]
        [Route("TaoBaiViet/EditPost")]
        public async Task<IActionResult> EditPost([FromForm] IFormFile image = null, [FromForm] int postType = 0, [FromForm] string postContent ="", int id =0)
        {
            List<BaiViet> baiV = _context.BaiViets
                                        .Where(b => b.MaBaiViet == id)
                                        .ToList();
            if(image != null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", baiV[0].AnhBaiViet.TrimStart('/'));
                try
                {
                    // Xóa file
                    System.IO.File.Delete(filePath);
                }
                catch (IOException ex)
                {
                    // Xử lý lỗi nếu không thể xóa file
                    return StatusCode(500, $"Lỗi khi xóa ảnh: {ex.Message}");
                }
                var NewfilePath = Path.Combine("wwwroot/Post/img", image.FileName);

                using (var stream = new FileStream(NewfilePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                string imageUrl = "/Post/img/" + image.FileName;
                baiV[0].AnhBaiViet=imageUrl;
            }

            if (postType>0)
            {
                baiV[0].MaLoaiMonAn = postType;
            }

            if (!string.IsNullOrWhiteSpace(postContent))
            {
                baiV[0].NoiDung = postContent;
            }

            _context.Update(baiV[0]);
            await _context.SaveChangesAsync();


            /*return Json(new
            {
                success = true
            });*/
            return RedirectToAction("TrangCaNhan","TrangCaNhan");


        }
        [HttpGet]
        [Route("TaoBaiViet/DeletePost/{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            List<BaiViet> baiV = _context.BaiViets
                                        .Where(b => b.MaBaiViet == id)
                                        .ToList();
            /*BaiViet baiV =_context.BaiViets.Find(id);*/

            List<Thich> likes = _context.Thiches.Where(b => b.MaBaiViet == id).
                ToList();

            List<BinhLuan> cmts = _context.BinhLuans.Where(b => b.MaBaiViet == id).ToList();

            if (baiV[0] == null)
            {
                return Json(new { success = false,bai = baiV  });
            }
            // Tạo đường dẫn đầy đủ tới ảnh trong thư mục wwwroot
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", baiV[0].AnhBaiViet.TrimStart('/'));

            _context.Thiches.RemoveRange(likes);
            _context.BinhLuans.RemoveRange(cmts);

            // Kiểm tra xem file có tồn tại không
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    // Xóa file
                    System.IO.File.Delete(filePath);
                }
                catch (IOException ex)
                {
                    // Xử lý lỗi nếu không thể xóa file
                    return StatusCode(500, $"Lỗi khi xóa ảnh: {ex.Message}");
                }
            }
            
            _context.BaiViets.Remove(baiV[0]);
            await _context.SaveChangesAsync();

            return Json(new { success = true,img= filePath });
            
        }
        
    }
}
