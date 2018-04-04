using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IArticleClassDAL
    {
        List<ArticleClass> GetArticleClassByPage<TKey>(int pageIndex, int pageSize, Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, out int totalCount, bool isdesc = false);     
    }
}
