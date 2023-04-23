using System.Text.Json.Serialization;
using DinkToPdf;
using DinkToPdf.Contracts;
using ExpenseTracker.Server.AppDbContext;
using ExpenseTracker.Server.Entities;
using ExpenseTracker.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();  // Combine razor pages and api

// For entity Framework
builder.Services.AddDbContext<ExpenseTrackerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// For DI registration
builder.Services.AddTransient<IExpenseTrackerService, ExpenseTrackerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // to laod wasm static files

app.UseBlazorFrameworkFiles(); //  a special middleware component to serve the client 

app.UseAuthorization();


app.MapRazorPages(); // Combine razor pages and api

app.MapControllers();// handle /api

app.MapFallbackToFile("index.html");  // handle  everything else

app.Run();
