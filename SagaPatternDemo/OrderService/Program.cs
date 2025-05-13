using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Commands;
using OrderService.Entities;
using OrderService.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    string dbConnectionString = builder.Configuration.GetConnectionString("Database") ??
        throw new InvalidOperationException();

    options.UseNpgsql(dbConnectionString);
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumers(typeof(Program).Assembly);

    busConfigurator.UsingRabbitMq((context, conf) =>
    {
        string rabbitMqConnection = builder.Configuration.GetConnectionString("RabbitMq") ?? string.Empty;
        conf.Host(new Uri(rabbitMqConnection));

        conf.UseInMemoryOutbox(context);
        conf.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(conf =>
    {
        conf.Theme = ScalarTheme.Moon;
        conf.Title = "Order API";
        conf.HideClientButton = true;
        conf.HideDarkModeToggle = true;
        conf.HiddenClients = true;
    });

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapPost("/order", async ([FromBody] Order payload, IBus bus) =>
{
    await bus.Publish(new OrderCreated(payload.Id, payload.Price));

    return Results.Accepted;
});

app.Run();

