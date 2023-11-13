using System.Linq.Expressions;
using Sonab.Core.Dto;
using Sonab.Core.Entities;

namespace Sonab.DbRepositories.Extensions;

internal static class QueryExtensions
{
    public static IQueryable<TKey> ApplyParams<TKey>(
        this IQueryable<TKey> query,
        ListParams listParams) where TKey : Entity => query
            .Skip(listParams.Limit * (listParams.Page - 1))
            .Take(listParams.Limit);

    public static IQueryable<TKey> WhereIfHasValue<TKey>(
        this IQueryable<TKey> query,
        Expression<Func<TKey, int>> expression,
        int? intValue) where TKey : Entity
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
