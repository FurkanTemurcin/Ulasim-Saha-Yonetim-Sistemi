using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portal.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Birimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birimler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KanGrubus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanGrubus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lookups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Controller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mudurlukler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mudurlukler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mudurlukler_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MudurlukId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subeler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subeler_Mudurlukler_MudurlukId",
                        column: x => x.MudurlukId,
                        principalTable: "Mudurlukler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Puan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TcKimlik = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastQuizDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KanGrubuId = table.Column<int>(type: "int", nullable: true),
                    Cinsiyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ehliyet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgrenimDurumu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedeniDurum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubeId = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_KanGrubus_KanGrubuId",
                        column: x => x.KanGrubuId,
                        principalTable: "KanGrubus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_Users_Subeler_SubeId",
                        column: x => x.SubeId,
                        principalTable: "Subeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LookupLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookUpId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Attributes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LookupListId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupLists_LookupLists_LookupListId",
                        column: x => x.LookupListId,
                        principalTable: "LookupLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LookupLists_Lookups_LookUpId",
                        column: x => x.LookUpId,
                        principalTable: "Lookups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LookupLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CezaYonetmelikleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Madde = table.Column<int>(type: "int", nullable: false),
                    Fikra = table.Column<int>(type: "int", nullable: true),
                    Bent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Yonetmelik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CezaPuani = table.Column<int>(type: "int", nullable: false),
                    PlakaTuruId = table.Column<int>(type: "int", nullable: true),
                    AnahtarKelime = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CezaYonetmelikleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CezaYonetmelikleri_LookupLists_PlakaTuruId",
                        column: x => x.PlakaTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CezaYonetmelikleri_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fotolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Veri = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DosyaAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DosyaTuruId = table.Column<int>(type: "int", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotolar_LookupLists_DosyaTuruId",
                        column: x => x.DosyaTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Fotolar_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Guzergahlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IlceId = table.Column<int>(type: "int", nullable: true),
                    GeometriId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guzergahlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guzergahlar_LookupLists_GeometriId",
                        column: x => x.GeometriId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Guzergahlar_LookupLists_IlceId",
                        column: x => x.IlceId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlakaSahipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KayitNumarasi = table.Column<int>(type: "int", nullable: true),
                    KimlikNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VergiNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Soyad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unvan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CinsiyetId = table.Column<int>(type: "int", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: true),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlakaSahipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlakaSahipleri_LookupLists_CinsiyetId",
                        column: x => x.CinsiyetId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlakaSahipleri_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Videolar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Veri = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DosyaAdi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DosyaTuruId = table.Column<int>(type: "int", nullable: true),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videolar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videolar_LookupLists_DosyaTuruId",
                        column: x => x.DosyaTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Videolar_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AracPlakalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlakaNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VizeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KoltukKapasitesi = table.Column<int>(type: "int", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotorNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SasiNumarasi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlakaTuruId = table.Column<int>(type: "int", nullable: true),
                    GuzergahId = table.Column<int>(type: "int", nullable: true),
                    AracPlakaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracPlakalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracPlakalari_AracPlakalari_AracPlakaId",
                        column: x => x.AracPlakaId,
                        principalTable: "AracPlakalari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AracPlakalari_Guzergahlar_GuzergahId",
                        column: x => x.GuzergahId,
                        principalTable: "Guzergahlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AracPlakalari_LookupLists_PlakaTuruId",
                        column: x => x.PlakaTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UcretTakvimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuzergahId = table.Column<int>(type: "int", nullable: false),
                    Ucret = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UcretTakvimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UcretTakvimleri_Guzergahlar_GuzergahId",
                        column: x => x.GuzergahId,
                        principalTable: "Guzergahlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZamanCizelgeleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuzergahId = table.Column<int>(type: "int", nullable: false),
                    YerTuruId = table.Column<int>(type: "int", nullable: false),
                    GunId = table.Column<int>(type: "int", nullable: true),
                    BaslangicSaati = table.Column<TimeSpan>(type: "time", nullable: true),
                    BitisSaati = table.Column<TimeSpan>(type: "time", nullable: true),
                    Mesafe = table.Column<double>(type: "float", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZamanCizelgeleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZamanCizelgeleri_Guzergahlar_GuzergahId",
                        column: x => x.GuzergahId,
                        principalTable: "Guzergahlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZamanCizelgeleri_LookupLists_GunId",
                        column: x => x.GunId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZamanCizelgeleri_LookupLists_YerTuruId",
                        column: x => x.YerTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZamanCizelgeleri_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AracDenetimleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracPlakaId = table.Column<int>(type: "int", nullable: false),
                    KullaniciId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracDenetimleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracDenetimleri_AracPlakalari_AracPlakaId",
                        column: x => x.AracPlakaId,
                        principalTable: "AracPlakalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AracDenetimleri_Users_KullaniciId",
                        column: x => x.KullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AracSahipleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracPlakaId = table.Column<int>(type: "int", nullable: false),
                    PlakaSahipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracSahipleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracSahipleri_AracPlakalari_AracPlakaId",
                        column: x => x.AracPlakaId,
                        principalTable: "AracPlakalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracSahipleri_PlakaSahipleri_PlakaSahipId",
                        column: x => x.PlakaSahipId,
                        principalTable: "PlakaSahipleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Denetimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracPlakaId = table.Column<int>(type: "int", nullable: false),
                    CezaDurumId = table.Column<int>(type: "int", nullable: false),
                    DenetimTuruId = table.Column<int>(type: "int", nullable: false),
                    CezaDelilTipId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    LookupListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Denetimler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Denetimler_AracPlakalari_AracPlakaId",
                        column: x => x.AracPlakaId,
                        principalTable: "AracPlakalari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denetimler_LookupLists_CezaDelilTipId",
                        column: x => x.CezaDelilTipId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denetimler_LookupLists_CezaDurumId",
                        column: x => x.CezaDurumId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denetimler_LookupLists_DenetimTuruId",
                        column: x => x.DenetimTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Denetimler_LookupLists_LookupListId",
                        column: x => x.LookupListId,
                        principalTable: "LookupLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Denetimler_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AracDenetimGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AracDenetimId = table.Column<int>(type: "int", nullable: false),
                    KontrolTuruId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracDenetimGecmisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisleri_AracDenetimleri_AracDenetimId",
                        column: x => x.AracDenetimId,
                        principalTable: "AracDenetimleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisleri_LookupLists_KontrolTuruId",
                        column: x => x.KontrolTuruId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisleri_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DenetimCezalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenetimId = table.Column<int>(type: "int", nullable: false),
                    CezaYonetmelikId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenetimCezalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DenetimCezalari_CezaYonetmelikleri_CezaYonetmelikId",
                        column: x => x.CezaYonetmelikId,
                        principalTable: "CezaYonetmelikleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                  
                    table.ForeignKey(
                        name: "FK_DenetimCezalari_Denetimler_DenetimId",
                        column: x => x.DenetimId,
                        principalTable: "Denetimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
          
                    table.ForeignKey(
                        name: "FK_DenetimCezalari_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DenetimGecmisleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenetimId = table.Column<int>(type: "int", nullable: false),
                    Yorum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CezaDurumId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    DurumTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UyariSuresi = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenetimGecmisleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DenetimGecmisleri_Denetimler_DenetimId",
                        column: x => x.DenetimId,
                        principalTable: "Denetimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DenetimGecmisleri_LookupLists_CezaDurumId",
                        column: x => x.CezaDurumId,
                        principalTable: "LookupLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DenetimGecmisleri_Users_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AracDenetimGecmisiFotoVideolari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DenetimGecmisiId = table.Column<int>(type: "int", nullable: false),
                    FotoId = table.Column<int>(type: "int", nullable: true),
                    VideoId = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AracDenetimGecmisiFotoVideolari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisiFotoVideolari_DenetimGecmisleri_DenetimGecmisiId",
                        column: x => x.DenetimGecmisiId,
                        principalTable: "DenetimGecmisleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisiFotoVideolari_Fotolar_FotoId",
                        column: x => x.FotoId,
                        principalTable: "Fotolar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                   
                    table.ForeignKey(
                        name: "FK_AracDenetimGecmisiFotoVideolari_Videolar_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videolar",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisiFotoVideolari_DenetimGecmisiId",
                table: "AracDenetimGecmisiFotoVideolari",
                column: "DenetimGecmisiId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisiFotoVideolari_FotoId",
                table: "AracDenetimGecmisiFotoVideolari",
                column: "FotoId");

           

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisiFotoVideolari_VideoId",
                table: "AracDenetimGecmisiFotoVideolari",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisleri_AracDenetimId",
                table: "AracDenetimGecmisleri",
                column: "AracDenetimId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisleri_KontrolTuruId",
                table: "AracDenetimGecmisleri",
                column: "KontrolTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimGecmisleri_UserId",
                table: "AracDenetimGecmisleri",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimleri_AracPlakaId",
                table: "AracDenetimleri",
                column: "AracPlakaId");

            migrationBuilder.CreateIndex(
                name: "IX_AracDenetimleri_KullaniciId",
                table: "AracDenetimleri",
                column: "KullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_AracPlakalari_AracPlakaId",
                table: "AracPlakalari",
                column: "AracPlakaId");

            migrationBuilder.CreateIndex(
                name: "IX_AracPlakalari_GuzergahId",
                table: "AracPlakalari",
                column: "GuzergahId");

            migrationBuilder.CreateIndex(
                name: "IX_AracPlakalari_PlakaTuruId",
                table: "AracPlakalari",
                column: "PlakaTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_AracSahipleri_AracPlakaId",
                table: "AracSahipleri",
                column: "AracPlakaId");

            migrationBuilder.CreateIndex(
                name: "IX_AracSahipleri_PlakaSahipId",
                table: "AracSahipleri",
                column: "PlakaSahipId");

            migrationBuilder.CreateIndex(
                name: "IX_CezaYonetmelikleri_OlusturanKullaniciId",
                table: "CezaYonetmelikleri",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_CezaYonetmelikleri_PlakaTuruId",
                table: "CezaYonetmelikleri",
                column: "PlakaTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DenetimCezalari_CezaYonetmelikId",
                table: "DenetimCezalari",
                column: "CezaYonetmelikId");

   

            migrationBuilder.CreateIndex(
                name: "IX_DenetimCezalari_DenetimId",
                table: "DenetimCezalari",
                column: "DenetimId");



            migrationBuilder.CreateIndex(
                name: "IX_DenetimCezalari_UserId",
                table: "DenetimCezalari",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DenetimGecmisleri_CezaDurumId",
                table: "DenetimGecmisleri",
                column: "CezaDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_DenetimGecmisleri_DenetimId",
                table: "DenetimGecmisleri",
                column: "DenetimId");

            migrationBuilder.CreateIndex(
                name: "IX_DenetimGecmisleri_OlusturanKullaniciId",
                table: "DenetimGecmisleri",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_AracPlakaId",
                table: "Denetimler",
                column: "AracPlakaId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_CezaDelilTipId",
                table: "Denetimler",
                column: "CezaDelilTipId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_CezaDurumId",
                table: "Denetimler",
                column: "CezaDurumId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_DenetimTuruId",
                table: "Denetimler",
                column: "DenetimTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_LookupListId",
                table: "Denetimler",
                column: "LookupListId");

            migrationBuilder.CreateIndex(
                name: "IX_Denetimler_OlusturanKullaniciId",
                table: "Denetimler",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_DosyaTuruId",
                table: "Fotolar",
                column: "DosyaTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Fotolar_OlusturanKullaniciId",
                table: "Fotolar",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Guzergahlar_GeometriId",
                table: "Guzergahlar",
                column: "GeometriId");

            migrationBuilder.CreateIndex(
                name: "IX_Guzergahlar_IlceId",
                table: "Guzergahlar",
                column: "IlceId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupLists_LookUpId",
                table: "LookupLists",
                column: "LookUpId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupLists_LookupListId",
                table: "LookupLists",
                column: "LookupListId");

            migrationBuilder.CreateIndex(
                name: "IX_LookupLists_UserId",
                table: "LookupLists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Mudurlukler_BirimId",
                table: "Mudurlukler",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_PlakaSahipleri_CinsiyetId",
                table: "PlakaSahipleri",
                column: "CinsiyetId");

            migrationBuilder.CreateIndex(
                name: "IX_PlakaSahipleri_OlusturanKullaniciId",
                table: "PlakaSahipleri",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subeler_MudurlukId",
                table: "Subeler",
                column: "MudurlukId");

            migrationBuilder.CreateIndex(
                name: "IX_UcretTakvimleri_GuzergahId",
                table: "UcretTakvimleri",
                column: "GuzergahId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_KanGrubuId",
                table: "Users",
                column: "KanGrubuId");


            migrationBuilder.CreateIndex(
                name: "IX_Users_SubeId",
                table: "Users",
                column: "SubeId");

            migrationBuilder.CreateIndex(
                name: "IX_Videolar_DosyaTuruId",
                table: "Videolar",
                column: "DosyaTuruId");

            migrationBuilder.CreateIndex(
                name: "IX_Videolar_OlusturanKullaniciId",
                table: "Videolar",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamanCizelgeleri_GunId",
                table: "ZamanCizelgeleri",
                column: "GunId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamanCizelgeleri_GuzergahId",
                table: "ZamanCizelgeleri",
                column: "GuzergahId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamanCizelgeleri_OlusturanKullaniciId",
                table: "ZamanCizelgeleri",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_ZamanCizelgeleri_YerTuruId",
                table: "ZamanCizelgeleri",
                column: "YerTuruId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AracDenetimGecmisiFotoVideolari");

            migrationBuilder.DropTable(
                name: "AracDenetimGecmisleri");

            migrationBuilder.DropTable(
                name: "AracSahipleri");

            migrationBuilder.DropTable(
                name: "DenetimCezalari");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UcretTakvimleri");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "ZamanCizelgeleri");

            migrationBuilder.DropTable(
                name: "DenetimGecmisleri");

            migrationBuilder.DropTable(
                name: "Fotolar");

            migrationBuilder.DropTable(
                name: "Videolar");

            migrationBuilder.DropTable(
                name: "AracDenetimleri");

            migrationBuilder.DropTable(
                name: "PlakaSahipleri");

            migrationBuilder.DropTable(
                name: "CezaYonetmelikleri");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Denetimler");

            migrationBuilder.DropTable(
                name: "AracPlakalari");

            migrationBuilder.DropTable(
                name: "Guzergahlar");

            migrationBuilder.DropTable(
                name: "LookupLists");

            migrationBuilder.DropTable(
                name: "Lookups");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "KanGrubus");

            migrationBuilder.DropTable(
                name: "Subeler");

            migrationBuilder.DropTable(
                name: "Mudurlukler");

            migrationBuilder.DropTable(
                name: "Birimler");
        }
    }
}
