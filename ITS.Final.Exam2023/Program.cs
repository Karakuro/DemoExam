using ITS.Final.Exam2023.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

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
    var db = scope.ServiceProvider.GetService<WarehouseDBContext>();
    var logger = scope.ServiceProvider.GetService<ILogger<WarehouseDBContext>>();
    try
    {
        if (db != null)
        {
            db.Database.Migrate();
            if (db.Products.Any())
            {
                db.Products.RemoveRange(db.Products);
                db.SaveChanges();
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
                db.AddRange(products);
                db.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogCritical($"Fatal Database Error: {ex.Message}", ex);
    }
}

app.UseFileServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
