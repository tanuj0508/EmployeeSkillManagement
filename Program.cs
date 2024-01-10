using EmployeeManagement.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("localDB")));

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{  
    options.AddPolicy("RequireAdminRole", policy =>{
        policy.RequireAuthenticatedUser(); // Require authentication
        policy.RequireRole("Admin"); // Required Admin authorization
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using var scope = app.Services.CreateScope();
 
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
 
var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
if (!adminRoleExists)
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}
 
var adminUser = await userManager.FindByNameAsync("admin");
// if (adminUser == null)
// {
//     adminUser = new IdentityUser { UserName = "anuj@gmail.com", Email = "anuj@gmail.com" };
//     //  password
//     // await userManager.CreateAsync(adminUser,);

//      var result = await userManager.CreateAsync(adminUser,  "Anuj#123");
//      await userManager.AddToRoleAsync(adminUser,"Admin");

//     if (result.Succeeded)
//     {
//         Console.WriteLine("User created successfully!");
//         Console.WriteLine($"User Id: {adminUser.Id}");
//         Console.WriteLine($"Username: {adminUser.UserName}");
//         Console.WriteLine($"Email: {adminUser.Email}");
//         // Add more properties as needed

//         // You can also retrieve and print additional user information if needed
//         // var additionalUserInfo = await userManager.GetClaimsAsync(adminUser);
//         // Console.WriteLine($"Additional Info: {string.Join(", ", additionalUserInfo.Select(c => $"{c.Type}: {c.Value}"))}");
//     }
//     else
//     {
//         Console.WriteLine("User creation failed!");
//         foreach (var error in result.Errors)
//         {
//             Console.WriteLine($"Error: {error.Description}");
//         }
//     }

    
// }
if (adminUser == null)
{
    adminUser = new IdentityUser { UserName = "anuj@gmail.com", Email = "anuj@gmail.com" };
    //  password
    await userManager.CreateAsync(adminUser, "Anuj#123"); 
    await userManager.AddToRoleAsync(adminUser, "Admin");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
