using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Auth;
using ProductCatalog.Api.CQRS;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


var httpPort = builder.Configuration.GetValue<int?>("Port") ?? 0; // 0 = leave default
var httpsPort = builder.Configuration.GetValue<int?>("HttpsPort") ?? 0; // 0 = disable HTTPS
var useDevCert = builder.Configuration.GetValue<bool>("UseDevCert");

if (httpPort != 0 || httpsPort != 0)
{
    builder.WebHost.ConfigureKestrel(opts =>
    {
        // Add explicit HTTP listener only if you supplied Port
        if (httpPort != 0)
            opts.ListenAnyIP(httpPort);               // http://*:{Port}

        // Add explicit HTTPS listener only if you supplied HttpsPort
        if (httpsPort != 0)
        {
            if (useDevCert)                           // dotnet dev-cert
                opts.ListenAnyIP(httpsPort, lo => lo.UseHttps());
            else
            {
                // Replace with your production cert if desired (incorporate into appsettings/pipeline):
                // opts.ListenAnyIP(httpsPort, lo => lo.UseHttps("cert.pfx","password"));
            }
        }
    });
}


builder.Services.AddDbContext<ProductCatalogDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDispatcher, Dispatcher>();

// Scan CQRS handlers
var asm = typeof(Program).Assembly;
builder.Services.Scan(s => s
    .FromAssemblies(asm)
    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

builder.Services.Scan(s => s
    .FromAssemblies(asm)
    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithTransientLifetime());

// Controllers & JSON settings
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductCatalog.Api", Version = "v1" });
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "ApiKey header. Example: ApiKey: YOUR_API_KEY",
        In = ParameterLocation.Header,
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "ApiKey"
                },
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();              // no-op if HTTPS listener not added
app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();
app.MapControllers();

// Create and seed database.  EnsureDeleted/EnsureCreated will drop and
// recreate the schema to match the current model on startup.  This
// avoids schema mismatches when the domain model evolves.
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ProductCatalogDbContext>();
    // WARNING: EnsureDeleted will drop the existing database.  In a
    // development environment this is acceptable to keep the schema in
    // sync with the model.  For production, replace with migrations.
    await ctx.Database.EnsureDeletedAsync();
    await ctx.Database.EnsureCreatedAsync();
    await DbInitializer.Initialize(ctx);
}

app.Run();
