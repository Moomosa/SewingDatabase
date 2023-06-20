﻿// <auto-generated />
using System;
using BackendDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackendDatabase.Migrations
{
    [DbContext(typeof(BackendDatabaseContext))]
    partial class BackendDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ModelLibrary.Models.Database.UserMapping", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("UserId")
                        .HasDatabaseName("IX_UserId");

                    b.ToTable("UserMapping", (string)null);
                });

            modelBuilder.Entity("ModelLibrary.Models.Thread.ThreadColor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ColorFamilyID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ColorFamilyID");

                    b.ToTable("ThreadColor", (string)null);
                });

            modelBuilder.Entity("ModelLibrary.Models.Thread.ThreadColorFamily", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("ColorFamily")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ThreadColorFamily", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.Elastic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ElasticTypeID")
                        .HasColumnType("int");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("ElasticTypeID");

                    b.ToTable("Elastic", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.ElasticTypes", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ElasticTypes", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.Fabric", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<string>("Appearance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FabricBrandID")
                        .HasColumnType("int");

                    b.Property<int>("FabricTypeID")
                        .HasColumnType("int");

                    b.Property<float>("PurchasePrice")
                        .HasColumnType("real");

                    b.Property<bool>("SolidOrPrint")
                        .HasColumnType("bit");

                    b.Property<float>("Value")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("FabricBrandID");

                    b.HasIndex("FabricTypeID");

                    b.ToTable("Fabric", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.FabricBrand", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("FabricBrand", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.FabricTypes", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("FabricTypes", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.Machine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("PurchasePrice")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("Machine", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.MiscItemType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Item")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("MiscItemType", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.MiscObjects", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("AdditionalNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemTypeID")
                        .HasColumnType("int");

                    b.Property<float?>("PurchasePrice")
                        .HasColumnType("real");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("SpecificInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Value")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("ItemTypeID");

                    b.ToTable("MiscObjects", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.Thread", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ColorFamilyID")
                        .HasColumnType("int");

                    b.Property<int>("ColorID")
                        .HasColumnType("int");

                    b.Property<bool>("MaxiLockStretch")
                        .HasColumnType("bit");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ThreadTypeID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ColorFamilyID");

                    b.HasIndex("ColorID");

                    b.HasIndex("ThreadTypeID");

                    b.ToTable("Thread", (string)null);
                });

            modelBuilder.Entity("SewingModels.Models.ThreadTypes", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ThreadTypes", (string)null);
                });

            modelBuilder.Entity("ModelLibrary.Models.Thread.ThreadColor", b =>
                {
                    b.HasOne("ModelLibrary.Models.Thread.ThreadColorFamily", "ColorFamily")
                        .WithMany()
                        .HasForeignKey("ColorFamilyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ColorFamily");
                });

            modelBuilder.Entity("SewingModels.Models.Elastic", b =>
                {
                    b.HasOne("SewingModels.Models.ElasticTypes", "ElasticType")
                        .WithMany()
                        .HasForeignKey("ElasticTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ElasticType");
                });

            modelBuilder.Entity("SewingModels.Models.Fabric", b =>
                {
                    b.HasOne("SewingModels.Models.FabricBrand", "Brand")
                        .WithMany()
                        .HasForeignKey("FabricBrandID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SewingModels.Models.FabricTypes", "FabricType")
                        .WithMany()
                        .HasForeignKey("FabricTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("FabricType");
                });

            modelBuilder.Entity("SewingModels.Models.MiscObjects", b =>
                {
                    b.HasOne("SewingModels.Models.MiscItemType", "ItemType")
                        .WithMany()
                        .HasForeignKey("ItemTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("SewingModels.Models.Thread", b =>
                {
                    b.HasOne("ModelLibrary.Models.Thread.ThreadColorFamily", "ColorFamily")
                        .WithMany()
                        .HasForeignKey("ColorFamilyID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ModelLibrary.Models.Thread.ThreadColor", "Color")
                        .WithMany()
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SewingModels.Models.ThreadTypes", "ThreadType")
                        .WithMany()
                        .HasForeignKey("ThreadTypeID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Color");

                    b.Navigation("ColorFamily");

                    b.Navigation("ThreadType");
                });
#pragma warning restore 612, 618
        }
    }
}
