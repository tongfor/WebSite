using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YhcdWebsite
{
    /// <summary>
    /// EF数据库连接配置
    /// </summary>
    public class EntityFrameWorkConfigure : IOnDatabaseConfiguring
    {
        private readonly IOptions<DatabaseOption> _dataBaseOption;
        private readonly ILoggerFactory _loggerFactory;
        public EntityFrameWorkConfigure(IOptionsSnapshot<DatabaseOption> dataBaseOption, ILoggerFactory loggerFactory)
        {
            _dataBaseOption = dataBaseOption;
            _loggerFactory = loggerFactory;
        }

        public void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            switch (_dataBaseOption.Value.DbType)
            {
                case DbTypes.MsSql:
                    {
                        optionsBuilder.UseSqlServer(_dataBaseOption.Value.ConnectionString);
                        break;
                    }
                case DbTypes.MsSqlEarly:
                    {
                        optionsBuilder.UseSqlServer(_dataBaseOption.Value.ConnectionString, option => option.UseRowNumberForPaging());
                        break;
                    }
                case DbTypes.Sqlite:
                    {
                        optionsBuilder.UseSqlite(_dataBaseOption.Value.ConnectionString);
                        break;
                    }
                case DbTypes.MySql:
                    {
                        optionsBuilder.UseMySql(_dataBaseOption.Value.ConnectionString);
                        break;
                    }
            }

            optionsBuilder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }
    }
}
