using BuildingBlocks.Behaviors;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
//register 3rd party apps into DI container
builder.Services.AddCarter();
//register mediatr
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});
// register marten
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

//If the environment is development, initialize the database with initial data
if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

// register validator 
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// register exception handler before building the app
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//Add health checks
builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!);


// build the app
var app = builder.Build();

// configure and expose http endpoints 
app.MapCarter();

// configure global exception handling
app.UseExceptionHandler(options => { });

// configure health checks
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapGet("/", () => "Hello World!");

app.Run();