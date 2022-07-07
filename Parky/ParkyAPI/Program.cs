using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParkyAPI;
using ParkyAPI.Data;
using ParkyAPI.Models.Repository;
using ParkyAPI.Models.Repository.IRepository;
using ParkyAPI.ParkyMapper;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.AddAutoMapper(typeof(ParkyMappings));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VV");
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen();

//builder.Services.AddSwaggerGen(options =>
//{
//    options.SwaggerDoc("ParkyOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
//    {
//        Title = "Parky API",
//        Version = "1",
//        Description = "Udemy Parky API",
//        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
//        {
//            Email = "ishanpradhan.ip@gmail.com",
//            Name = "Ishan Pradhan",
//            Url = new Uri("https://www.ishan.com")
//        },
//        License = new Microsoft.OpenApi.Models.OpenApiLicense()
//        {
//            Name = "MIT License",
//            Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
//        },
//    });
//    //options.SwaggerDoc("ParkyOpenAPISpecNP", new Microsoft.OpenApi.Models.OpenApiInfo()
//    //{
//    //    Title = "Parky API NP",
//    //    Version = "1",
//    //    Description = "Udemy Parky API NP",
//    //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
//    //    {
//    //        Email = "ishanpradhan.ip@gmail.com",
//    //        Name = "Ishan Pradhan",
//    //        Url = new Uri("https://www.ishan.com")
//    //    },
//    //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
//    //    {
//    //        Name = "MIT License",
//    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
//    //    },
//    //});
//    //options.SwaggerDoc("ParkyOpenAPISpecTrails", new Microsoft.OpenApi.Models.OpenApiInfo()
//    //{
//    //    Title = "Parky API Trails",
//    //    Version = "1",
//    //    Description = "Udemy Parky API Trails",
//    //    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
//    //    {
//    //        Email = "ishanpradhan.ip@gmail.com",
//    //        Name = "Ishan Pradhan",
//    //        Url = new Uri("https://www.ishan.com")
//    //    },
//    //    License = new Microsoft.OpenApi.Models.OpenApiLicense()
//    //    {
//    //        Name = "MIT License",
//    //        Url = new Uri("https://en.wikipedia.org/wiki/MIT License")
//    //    },
//    //});
//    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
//    options.IncludeXmlComments(cmlCommentsFullPath);
//});
var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
IWebHostEnvironment environment = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }

    });
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/ParkyOpenAPISpec/swagger.json", "Parky API NP");
    //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecNP/swagger.json", "Parky API NP");
    //    //options.SwaggerEndpoint("/swagger/ParkyOpenAPISpecTrails/swagger.json", "Parky API Trails");

    //});
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
