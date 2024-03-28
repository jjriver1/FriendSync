using System.Reflection;
using System.Text;
using FriendSync.Models;
using FriendSync.Services;
using FriendSync.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

#pragma warning disable CS0162 // Unreachable code detected

//note: remove this line from launch settings.json  "launchUrl": "swagger" from http 

const bool useSwagger = true;

Main();
return;

void Main() {
    const string policyName = "CorsPolicy";
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    
    ConfigureRestApi(builder);
    if (useSwagger) {
        ConfigureSwagger(builder);
    }

    CreateCorsPolicy(builder, policyName);
    AddWebAuthentication(builder);
    
    WebApplication app = CreateWebApp(builder, policyName, useSwagger);
    app.Run();
}

WebApplication CreateWebApp(WebApplicationBuilder webApplicationBuilder, string policyName, bool swaggerEnabled)
{
    
    WebApplication app = webApplicationBuilder.Build();
    app.UseCors(policyName);
    
    if (swaggerEnabled)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    return app;
}

void CreateCorsPolicy(WebApplicationBuilder builder, string policyName) {
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: policyName,
            policy  =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:3000/*")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });
}

void AddWebAuthentication(WebApplicationBuilder builder) {
    //Jwt configuration starts here
    var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
    var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

    if (jwtKey == null || jwtIssuer == null)
    {
        throw new Exception("Jwt:Key or Jwt:Issuer not found in appsettings.json");
    }

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });
}

void ConfigureRestApi(WebApplicationBuilder builder)
{
    builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
    builder.Services.AddControllers();
    builder.Services.AddSingleton<MongoDBService>();
    builder.Services.AddEndpointsApiExplorer();
}

void ConfigureSwagger(IHostApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Description = "Please enter a valid JWT token.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });

        options.OperationFilter<AuthorizationOperationFilter>();
    });
}
