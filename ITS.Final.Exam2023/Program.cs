using ITS.Final.Exam2023.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(options =>
    {
        options
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins([ "https://localhost:8080", "http://localhost:8181" ]);
    });
});

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSqlServer<WarehouseDBContext>(builder.Configuration.GetConnectionString("Default"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetService<WarehouseDBContext>();
    var logger = scope.ServiceProvider.GetService<ILogger<WarehouseDBContext>>();
    try
    {
        if (ctx != null)
        {
            ctx.Database.Migrate();
            if (ctx.Products.Any())
            {
                ctx.Products.RemoveRange(ctx.Products);
                ctx.SaveChanges();
            }
            string json = File.ReadAllText("Products.json");
            List<JsonProduct>? jsonProducts = JsonSerializer.Deserialize<List<JsonProduct>>(json);

            if (jsonProducts != null)
            {
                //jsonProducts = jsonProducts.DistinctBy(j => new { j.PartNumber, j.Description }).ToList();
                var dirtyData = jsonProducts.GroupBy(j => j.PartNumber, (key, list) => new { key, total = list.Count() })
                    .Where(g => g.total > 1).Select(g => g.key);
                jsonProducts = jsonProducts.ExceptBy(dirtyData, j => j.PartNumber).ToList();
                
                List<Product> products = jsonProducts
                    .Where(j => j.PartNumber != null && j.PartNumber.Length <= 20 && j.PartNumber.Length >= 5)
                    .Select(j => new Product
                    {
                        Description = j.Description ?? "",
                        ProductId = j.PartNumber!
                    }).ToList();
                ctx.AddRange(products);
                ctx.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogCritical($"Fatal Database Error: {ex.Message}", ex);
    }
}
app.UseCors();

app.UseFileServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
