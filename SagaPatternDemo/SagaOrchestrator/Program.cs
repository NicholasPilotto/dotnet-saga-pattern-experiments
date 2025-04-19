using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaOrchestrator.Persistence;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<NpgsqlDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

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
}

app.UseHttpsRedirection();

app.Run();
