using API;
using Application;
using Infrastructure;
using Domain.Identity;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


{
    builder
        .Services.AddPresentation()
        .AddApplication()
        .AddInfrastructureServices(builder.Configuration);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.InitialiseDatabaseAsync();
}


app.UseInfrastructure();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
