using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    public partial class CdyhcdDBContext : DbContext
    {
        public virtual DbSet<ArticleView> ArticleView { get; set; }
        public virtual DbSet<AdminUserMenuView> AdminUserMenuView { get; set; }
        public virtual DbSet<AdminUserView> AdminUserView { get; set; }
        public virtual DbSet<AdminMenuRoleButtonView> AdminMenuRoleButtonView { get; set; }

        public virtual DbSet<PageData<Article>> ArticlePageData { get; set; }
    }
}
