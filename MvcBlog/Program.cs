using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MvcBlog.DbRepository;
using MvcBlog.DbRepository.Implementations;
using MvcBlog.DbRepository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepositoryContextFactory, RepositoryContextFactory>();

builder.Services.AddScoped<IUsersRepository>(
    provider => new UsersRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

builder.Services.AddScoped<IPostsRepository>(
    provider => new PostsRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

builder.Services.AddScoped<ICategoriesRepository>(
    provider => new CategoriesRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

builder.Services.AddScoped<ITagsRepository>(
    provider => new TagsRepository(config.GetConnectionString("DefaultConnection"),
    provider.GetService<IRepositoryContextFactory>())
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var factory = services.GetRequiredService<IRepositoryContextFactory>();

    factory.CreateDbContext(config.GetConnectionString("DefaultConnection")).Database.Migrate();
}

app.Run();
