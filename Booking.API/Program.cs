using Booking.API.Configurations;
using Booking.API.Conventions;
using Booking.API.Extensions;
using Booking.API.Middlewares;
using Booking.Application.Extensions;
using Booking.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new GlobalRoutePrefixConvention("api"));
});

builder.Services
    .AddSwaggerDocumentation()
    .AddFluentValidation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    // I wouldn't use this everywhere. For production, I'd use a different pipeline, 
    // separate the migration container first, 
    // and then update the server...
    await app.Services.ApplyMigrationsAsync();
}

app.UseExceptionHandling();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
