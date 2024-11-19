namespace Catalog.API.Models.Products.GetProducts;

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10); //: IRequest<IEnumerable<Product>>;
public record GetProductsResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request ,ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                
            var results = await sender.Send(query);
            var response = results.Adapt<GetProductsResponse>();
            return Results.Ok(response);
        }).WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem (StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}