using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


//Identity server config **
builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{

    options.Authority = "https://localhost:7219/";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false
    };

});

//Identity server config --end **

builder.Services.AddOcelot();
var app = builder.Build();

await app.UseOcelot();

app.Run();
