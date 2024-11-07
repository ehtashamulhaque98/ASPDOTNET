using CRUDWithADONet.Models;
using CRUDWithADONet.DataAccessLayer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EmployeeData>();
builder.Services.AddScoped<StudentData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
    //name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
    //name: "default",
    //pattern: "{controller=EmployeeController1}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "studentRoute",
    pattern: "{controller=StudentController1}/{action=Index}/{id?}");

app.Run();
