using System.Runtime.InteropServices;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    private static IQueryable<T> ApplyBaseSpecification(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.IsDistinct)
            query = query.Distinct();

        return query;
    }

    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        return ApplyBaseSpecification(inputQuery, spec);
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> inputQuery, ISpecification<T, TResult> spec)
    {
        var query = ApplyBaseSpecification(inputQuery, spec);

        if (spec.Select != null)
        {
            var selectQuery = query.Select(spec.Select);

            if (spec.IsDistinct)
            {
                selectQuery = selectQuery?.Distinct();
            }

            return selectQuery ?? query.Cast<TResult>();
        }
        
        return query.Cast<TResult>();
    }
}
