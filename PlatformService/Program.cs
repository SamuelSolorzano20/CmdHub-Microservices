using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction()) 
{
    Console.WriteLine("--> Using SqlServer DB");

    builder.Services.AddDbContext<AppDbContext>(x => 
        x.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else {
    Console.WriteLine("--> Using InMem DB");

    builder.Services.AddDbContext<AppDbContext>(x => 
        x.UseInMemoryDatabase("InMemDB"));
}

builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();