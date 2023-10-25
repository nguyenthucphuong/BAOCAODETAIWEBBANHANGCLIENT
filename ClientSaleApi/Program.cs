
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ClientSaleApi.Models.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Đăng ký IHttpClientFactory để tạo ra các đối tượng HttpClient gửi các yêu cầu HTTP đến các API
builder.Services.AddHttpClient();

// Đăng ký IHttpContextAccessor để tạo Header cho Request
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("Client")
		.AddHttpMessageHandler<AuthorizationHeaderHandler>();
builder.Services.AddTransient<AuthorizationHeaderHandler>();


builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
	options.LoginPath = "/thanh-vien/dang-nhap";
	options.AccessDeniedPath = "/thanh-vien/404";
	//options.Cookie.HttpOnly = true;
	//options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// khai báo Friendly Url
app.MapControllerRoute(
	name: "product",
	pattern: "{friendlyUrl}",
	defaults: new { controller = "Shops", action = "Chitiet" });

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
