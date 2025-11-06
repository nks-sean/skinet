using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specification;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParams specParam) : base(x =>
        string.IsNullOrEmpty(specParam.Search) || x.Name.ToLower().Contains(specParam.Search) &&
        (specParam.Brands.Count == 0 || specParam.Brands.Contains(x.Brand)) &&
        (specParam.Types.Count == 0 || specParam.Types.Contains(x.Type)))
    {

        ApplyPaging((specParam.PageIndex - 1) * specParam.PageSize, specParam.PageSize);

        switch(specParam.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
