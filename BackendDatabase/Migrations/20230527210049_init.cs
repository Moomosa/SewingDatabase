using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendDatabase.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElasticTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElasticTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FabricBrand",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricBrand", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FabricTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FabricTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MiscItemType",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscItemType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThreadColorFamily",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorFamily = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadColorFamily", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThreadTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserMapping",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMapping", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Elastic",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElasticTypeID = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Width = table.Column<float>(type: "real", nullable: false),
                    Length = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elastic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Elastic_ElasticTypes_ElasticTypeID",
                        column: x => x.ElasticTypeID,
                        principalTable: "ElasticTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fabric",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FabricTypeID = table.Column<int>(type: "int", nullable: false),
                    FabricBrandID = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<float>(type: "real", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    SolidOrPrint = table.Column<bool>(type: "bit", nullable: false),
                    Appearance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fabric", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Fabric_FabricBrand_FabricBrandID",
                        column: x => x.FabricBrandID,
                        principalTable: "FabricBrand",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fabric_FabricTypes_FabricTypeID",
                        column: x => x.FabricTypeID,
                        principalTable: "FabricTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MiscObjects",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemTypeID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AdditionalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecificInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<float>(type: "real", nullable: false),
                    PurchasePrice = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscObjects", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MiscObjects_MiscItemType_ItemTypeID",
                        column: x => x.ItemTypeID,
                        principalTable: "MiscItemType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThreadColor",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorFamilyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadColor", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ThreadColor_ThreadColorFamily_ColorFamilyID",
                        column: x => x.ColorFamilyID,
                        principalTable: "ThreadColorFamily",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thread",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadTypeID = table.Column<int>(type: "int", nullable: false),
                    ColorFamilyID = table.Column<int>(type: "int", nullable: false),
                    ColorID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MaxiLockStretch = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thread", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Thread_ThreadColor_ColorID",
                        column: x => x.ColorID,
                        principalTable: "ThreadColor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Thread_ThreadColorFamily_ColorFamilyID",
                        column: x => x.ColorFamilyID,
                        principalTable: "ThreadColorFamily",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Thread_ThreadTypes_ThreadTypeID",
                        column: x => x.ThreadTypeID,
                        principalTable: "ThreadTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elastic_ElasticTypeID",
                table: "Elastic",
                column: "ElasticTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Fabric_FabricBrandID",
                table: "Fabric",
                column: "FabricBrandID");

            migrationBuilder.CreateIndex(
                name: "IX_Fabric_FabricTypeID",
                table: "Fabric",
                column: "FabricTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_MiscObjects_ItemTypeID",
                table: "MiscObjects",
                column: "ItemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_ColorFamilyID",
                table: "Thread",
                column: "ColorFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_ColorID",
                table: "Thread",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_Thread_ThreadTypeID",
                table: "Thread",
                column: "ThreadTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ThreadColor_ColorFamilyID",
                table: "ThreadColor",
                column: "ColorFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_UserId",
                table: "UserMapping",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elastic");

            migrationBuilder.DropTable(
                name: "Fabric");

            migrationBuilder.DropTable(
                name: "Machine");

            migrationBuilder.DropTable(
                name: "MiscObjects");

            migrationBuilder.DropTable(
                name: "Thread");

            migrationBuilder.DropTable(
                name: "UserMapping");

            migrationBuilder.DropTable(
                name: "ElasticTypes");

            migrationBuilder.DropTable(
                name: "FabricBrand");

            migrationBuilder.DropTable(
                name: "FabricTypes");

            migrationBuilder.DropTable(
                name: "MiscItemType");

            migrationBuilder.DropTable(
                name: "ThreadColor");

            migrationBuilder.DropTable(
                name: "ThreadTypes");

            migrationBuilder.DropTable(
                name: "ThreadColorFamily");
        }
    }
}
