db first 生成实体类命令:
Scaffold-DbContext "Server=127.0.0.1;Database=CdyhcdDB;User ID=cdyhcd;Password=123456" Pomelo.EntityFrameworkCore.MySql -Project Models
强制覆盖
Scaffold-DbContext "Server=127.0.0.1;Database=CdyhcdDB;User ID=cdyhcd;Password=123456" Pomelo.EntityFrameworkCore.MySql -Project Models -Force

DbContext文件上增加构造函数注入：
public BloggingContext(DbContextOptions<BloggingContext> options)
    : base(options)
{ }