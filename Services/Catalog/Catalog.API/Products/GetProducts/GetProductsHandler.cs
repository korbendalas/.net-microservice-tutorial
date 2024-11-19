using Marten.Pagination;

namespace Catalog.API.Models.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
    

internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
{
   public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancelationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancelationToken);
        return new GetProductsResult(products);
    }
}