using System.Linq.Expressions;
using Sonab.Core.Entities;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Extensions;

public static class QueryExtensions
{
    public static IQueryable<TKey> ApplyParams<TKey>(
        this IQueryable<TKey> query,
        ListParams listParams) where TKey : Key => query
            .Skip(listParams.Limit * (listParams.Page - 1))
            .Take(listParams.Limit);

    public static IQueryable<TKey> WhereIfHasValue<TKey>(
        this IQueryable<TKey> query,
        Expression<Func<TKey, int>> expression,
        int? intValue) where TKey : Key
    {
        if (!intValue.HasValue)
        {
            return query;
        }

        Expression<Func<int, bool>> isEquals = x => x == intValue.Value;
        ParameterExpression tkeyParameter = Expression.Parameter(typeof(TKey));
        return query.Where(Expression.Lambda<Func<TKey, bool>>(
            Expression.Invoke(
                isEquals,
                Expression.Invoke(expression, tkeyParameter)),
                tkeyParameter
        ));
    }

    public static IQueryable<Post> WhereTopic(this IQueryable<Post> query, int? topicId) =>
        topicId.HasValue
            ? query.Where(x => x.Topics.Any(y => y.Id == topicId.Value))
            : query;
}
