using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Extentions;

public static class QueryExtentions
{
    public static IQueryable<TKey> ApplyParams<TKey>(
        this IQueryable<TKey> query,
        ListParams listParams) where TKey : Key => query
            .Skip(listParams.Limit * (listParams.Page - 1))
            .Take(listParams.Limit);
}
