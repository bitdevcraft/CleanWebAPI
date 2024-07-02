using API;
using Application;
using Domain.Identity;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});


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

app.UseAuthentication();

app.UseInfrastructure();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
