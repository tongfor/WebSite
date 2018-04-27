using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    /// <summary>
    /// 数据库设置接口
    /// </summary>
    public interface IOnDatabaseConfiguring
    {
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}
