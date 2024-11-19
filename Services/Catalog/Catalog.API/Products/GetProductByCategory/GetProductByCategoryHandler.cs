
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryQueryResult>;
public record GetProductByCategoryQueryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryHandler(IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryQueryResult>
{
    public async Task<GetProductByCategoryQueryResult> Handle(GetProductByCategoryQuery query,
        CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().
         Where(p=> p.Category.Contains(query.Category))
         .ToListAsync(cancellationToken);
        return new GetProductByCategoryQueryResult(products);
    }
};