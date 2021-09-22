using Apache.Ignite.Core;
using Apache.Ignite.Core.Configuration;
using Apache.Ignite.Core.Discovery.Tcp;
using Apache.Ignite.Core.Discovery.Tcp.Multicast;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IIgnite>(sp =>
{
    var ignite = Ignition.Start(new IgniteConfiguration
    {
        DataStorageConfiguration = new DataStorageConfiguration
        {
            DefaultDataRegionConfiguration = new DataRegionConfiguration
            {
                Name = "Default_Region",
                PersistenceEnabled = true
            }
        },
        IsActiveOnStart = true,
        DiscoverySpi = new TcpDiscoverySpi
        {
            IpFinder = new TcpDiscoveryMulticastIpFinder
            {
                MulticastGroup = "228.10.10.157"
            }
        }
    });
    ignite.GetCluster().SetActive(true);
    return ignite;
});

builder.Services.AddCors(c => c.AddPolicy("cors", builder =>
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "IT.Projectmanagement", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IT.Projectmanagement v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("cors");

app.Run();
