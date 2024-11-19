
namespace Catalog.API.Models.Products.CreateProduct;

public record CreateProductRequest(
    string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    Console.WriteLine("testsssssssssstttt");
                    Console.WriteLine($"request: {request}");

                    var command = request.Adapt<CreateProductCommand>();
                    Console.WriteLine($"Command: {command}");
                    var result = await sender.Send(command);
                    Console.WriteLine($"result {result}");

                    var response = result.Adapt<CreateProductResponse>();
                    Console.WriteLine($"response {response}");
                    
                    var created = Results.Created($"/products/{response.Id}", response);
                    Console.WriteLine($"created {response}");
                    return created;
                })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
};