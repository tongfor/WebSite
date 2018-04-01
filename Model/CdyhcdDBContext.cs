using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    public partial class CdyhcdDBContext : DbContext
    {
        public CdyhcdDBContext(DbContextOptions<CdyhcdDBContext> options) : base(options)
        {
        }

        public virtual DbSet<AdminBug> AdminBug { get; set; }
        public virtual DbSet<AdminButton> AdminButton { get; set; }
        public virtual DbSet<AdminDepartment> AdminDepartment { get; set; }
        public virtual DbSet<AdminLoginLog> AdminLoginLog { get; set; }
        public virtual DbSet<AdminMenu> AdminMenu { get; set; }
        public virtual DbSet<AdminMenuAdminButton> AdminMenuAdminButton { get; set; }
        public virtual DbSet<AdminOperateLog> AdminOperateLog { get; set; }
        public virtual DbSet<AdminRole> AdminRole { get; set; }
        public virtual DbSet<AdminRoleAdminMenuButton> AdminRoleAdminMenuButton { get; set; }
        public virtual DbSet<AdminUser> AdminUser { get; set; }
        public virtual DbSet<AdminUserAdminDepartment> AdminUserAdminDepartment { get; set; }
        public virtual DbSet<AdminUserAdminRole> AdminUserAdminRole { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleClass> ArticleClass { get; set; }
        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySql("Server=localhost;Database=CdyhcdDB;User Id=cdyhcd;Password=123456;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminBug>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.IsShow).HasColumnType("tinyint(4)");

                entity.Property(e => e.IsSolve).HasColumnType("tinyint(4)");

                entity.Property(e => e.UserIp).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminButton>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Sort).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminDepartment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Path).HasColumnType("text");

                entity.Property(e => e.Sort).HasColumnType("int(11)");

                entity.Property(e => e.Tier).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminLoginLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.IsSuccess).HasColumnType("tinyint(4)");

                entity.Property(e => e.LoginTime).HasColumnType("datetime");

                entity.Property(e => e.UserIp).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminMenu>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.LinkAddress).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Sort).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminMenuAdminButton>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ButtonId).HasColumnType("int(11)");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.MenuId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminOperateLog>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.IsSuccess).HasColumnType("tinyint(4)");

                entity.Property(e => e.OperateInfo).HasMaxLength(64);

                entity.Property(e => e.OperateTime).HasColumnType("datetime");

                entity.Property(e => e.UserIp).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminRoleAdminMenuButton>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.ButtonId).HasColumnType("int(11)");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.MenuId).HasColumnType("int(11)");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsAble)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsChangePwd)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsFromThird).HasColumnType("tinyint(4)");

                entity.Property(e => e.MemberLevel).HasColumnType("int(11)");

                entity.Property(e => e.Mobile).HasMaxLength(32);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Postcode).HasMaxLength(32);

                entity.Property(e => e.Qq)
                    .HasColumnName("QQ")
                    .HasMaxLength(32);

                entity.Property(e => e.ThirdToken).HasMaxLength(500);

                entity.Property(e => e.ThirdType).HasMaxLength(50);

                entity.Property(e => e.ThirdUrl).HasMaxLength(500);

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserPwd).HasMaxLength(50);
            });

            modelBuilder.Entity<AdminUserAdminDepartment>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.DepartmentId).HasColumnType("int(11)");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<AdminUserAdminRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnType("int(11)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddHtmlurl)
                    .HasColumnName("AddHTMLUrl")
                    .HasMaxLength(500);

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Author).HasMaxLength(100);

                entity.Property(e => e.ClassId).HasColumnType("int(11)");

                entity.Property(e => e.Content).IsRequired();

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.Introduce).HasMaxLength(1000);

                entity.Property(e => e.IntroduceImg).HasMaxLength(200);

                entity.Property(e => e.IsDel)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsMarquee)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.IsTop)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.LookCount).HasColumnType("int(11)");

                entity.Property(e => e.Origin).HasMaxLength(500);

                entity.Property(e => e.Sort).HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TitleColor).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            modelBuilder.Entity<ArticleClass>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnType("tinyint(4)");

                entity.Property(e => e.IsDel)
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ParentId).HasColumnType("int(11)");

                entity.Property(e => e.Path).HasMaxLength(2000);

                entity.Property(e => e.Sort).HasColumnType("int(11)");

                entity.Property(e => e.Tier).HasColumnType("int(11)");
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.HomePage).HasMaxLength(255);

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.IsChecked).HasColumnType("tinyint(4)");

                entity.Property(e => e.IsDel).HasColumnType("tinyint(4)");

                entity.Property(e => e.Qq)
                    .HasColumnName("QQ")
                    .HasMaxLength(32);

                entity.Property(e => e.Title).HasMaxLength(255);
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.AddTime).HasColumnType("datetime");

                entity.Property(e => e.EditTime).HasColumnType("datetime");

                entity.Property(e => e.ParExplain).HasMaxLength(128);

                entity.Property(e => e.ParHierarchy).HasColumnType("int(11)");

                entity.Property(e => e.ParKey).HasMaxLength(32);

                entity.Property(e => e.ParName).HasMaxLength(32);

                entity.Property(e => e.ParParentId)
                    .HasColumnName("ParParentID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ParPath).HasColumnType("text");

                entity.Property(e => e.ParSequence).HasColumnType("int(11)");

                entity.Property(e => e.ParValue).HasMaxLength(64);

                entity.Property(e => e.ParVersion).HasColumnType("int(11)");
            });
        }
    }
}
