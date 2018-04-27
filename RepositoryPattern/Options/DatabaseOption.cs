using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryPattern
{
    /// <summary>
    /// 数据库设置类
    /// </summary>
    public class DatabaseOption
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbTypes DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
