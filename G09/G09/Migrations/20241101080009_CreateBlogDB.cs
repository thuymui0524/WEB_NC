using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G09.Migrations
{
    /// <inheritdoc />
    public partial class CreateBlogDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiMonAn",
                columns: table => new
                {
                    MaLoaiMonAn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiMonAn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoaiMonA__AF2559D340F86737", x => x.MaLoaiMonAn);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TieuSu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NguoiDun__C539D762303B0343", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "BaiViet",
                columns: table => new
                {
                    MaBaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhBaiViet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaLoaiMonAn = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    luotthich = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BaiViet__AEDD56473B060195", x => x.MaBaiViet);
                    table.ForeignKey(
                        name: "FK__BaiViet__MaLoaiM__52593CB8",
                        column: x => x.MaLoaiMonAn,
                        principalTable: "LoaiMonAn",
                        principalColumn: "MaLoaiMonAn");
                    table.ForeignKey(
                        name: "FK__BaiViet__MaNguoi__5165187F",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "TheoDoi",
                columns: table => new
                {
                    MaTheoDoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiTheoDoi = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDuocTheoDoi = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TheoDoi__3156C079A2F63E05", x => x.MaTheoDoi);
                    table.ForeignKey(
                        name: "FK__TheoDoi__MaNguoi__5FB337D6",
                        column: x => x.MaNguoiTheoDoi,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK__TheoDoi__MaNguoi__60A75C0F",
                        column: x => x.MaNguoiDuocTheoDoi,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "BinhLuan",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BinhLuan__87CB66A0B5F49EF9", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "FK__BinhLuan__MaBaiV__5629CD9C",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet");
                    table.ForeignKey(
                        name: "FK__BinhLuan__MaNguo__571DF1D5",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "Thich",
                columns: table => new
                {
                    MaThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: true),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Thich__985232E7041EB93A", x => x.MaThich);
                    table.ForeignKey(
                        name: "FK__Thich__MaBaiViet__5AEE82B9",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViet",
                        principalColumn: "MaBaiViet");
                    table.ForeignKey(
                        name: "FK__Thich__MaNguoiDu__5BE2A6F2",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_MaLoaiMonAn",
                table: "BaiViet",
                column: "MaLoaiMonAn");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViet_MaNguoiDung",
                table: "BaiViet",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaBaiViet",
                table: "BinhLuan",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaNguoiDung",
                table: "BinhLuan",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "UQ__LoaiMonA__7AD8299C135BB907",
                table: "LoaiMonAn",
                column: "TenLoaiMonAn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__57E5A81DADD4AFC0",
                table: "NguoiDung",
                column: "TenNguoiDung",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NguoiDun__A9D10534472D9BA7",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TheoDoi_MaNguoiDuocTheoDoi",
                table: "TheoDoi",
                column: "MaNguoiDuocTheoDoi");

            migrationBuilder.CreateIndex(
                name: "UQ__TheoDoi__1DBD570C78F5600C",
                table: "TheoDoi",
                columns: new[] { "MaNguoiTheoDoi", "MaNguoiDuocTheoDoi" },
                unique: true,
                filter: "[MaNguoiTheoDoi] IS NOT NULL AND [MaNguoiDuocTheoDoi] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Thich_MaBaiViet",
                table: "Thich",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_Thich_MaNguoiDung",
                table: "Thich",
                column: "MaNguoiDung");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinhLuan");

            migrationBuilder.DropTable(
                name: "TheoDoi");

            migrationBuilder.DropTable(
                name: "Thich");

            migrationBuilder.DropTable(
                name: "BaiViet");

            migrationBuilder.DropTable(
                name: "LoaiMonAn");

            migrationBuilder.DropTable(
                name: "NguoiDung");
        }
    }
}
