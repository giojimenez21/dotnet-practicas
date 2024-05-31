using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PeliculasAPI.Context;
using PeliculasAPI.Dtos;
using PeliculasAPI.Interfaces;
using PeliculasAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<List<ActorMovieCreateDTO>>(() =>
    new OpenApiSchema
    {
        Type = "array",
        Items =
        new OpenApiSchema
        {
            Reference =
            new OpenApiReference
            {
                Type = ReferenceType.Schema,
                Id = "ActorMovieCreateDTO"
            }
        }
    });
});

//Enable connection with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


//Enable autoMapper
builder.Services.AddAutoMapper(typeof(Program));

//List of services 
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IGenderService, GenderService>();
builder.Services.AddTransient<IActorService, ActorService>();
//builder.Services.AddTransient<IFileService, FileAzureService>();
builder.Services.AddTransient<IFileService, FileLocalService>();
builder.Services.AddTransient<IMovieService, MovieService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
