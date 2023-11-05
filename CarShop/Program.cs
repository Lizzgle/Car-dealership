using CarShop;
using CarShop.Services.CarCategoryService;
using CarShop.Services.CarService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICarCategoryService, MemoryCarCategoryService>();
builder.Services.AddScoped<ICarService, MemoryCarService>();
builder.Services.AddHttpContextAccessor();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddHttpClient<ICarService, ApiCarService>
    (opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddHttpClient<ICarCategoryService, ApiCategoryService>
    (opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
    
        // Получить Claims пользователя
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
