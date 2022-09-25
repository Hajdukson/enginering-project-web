using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.DataAccess;
using MoneyManager.Services;
using MoneyManager.Services.Interfeces;
using MoneyManager.WWW.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<MoneyManagerWWWContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MoneyManagerWWWContext") ?? throw new InvalidOperationException("Connection string 'MoneyManagerWWWContext' not found.")));
builder.Services.AddDbContext<MoneyManagerWWWContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'MoneyManagerWWWContext' not found.")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICalculator, ExpenseCalculator>();


builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // To preserve the default behavior, capture the original delegate to call later.
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<Program>>();

            // Perform logging here.
            // ...

            // Invoke the default behavior, which produces a ValidationProblemDetails
            // response.
            // To produce a custom response, return a different implementation of 
            // IActionResult instead.
            return builtInFactory(context);
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();