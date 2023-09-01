using ElBasset.DataBase;
using ElBasset.DataBase.DataBase;
using ElBasset.DTO;
using ElBasset.Repo.UnitOfWork;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Text;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
// service Configration for Tus Configration
builder.Services.AddSingleton(CreateTusConfiguration);

// Add Migration To DataBase 
builder.Services.AddDbContext<AppDbContext>(it => it.UseSqlServer(builder.Configuration["ConnectionStrings:myconn"]));
// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;

})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
// AutoMapper Configration
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Add IUnitOfWork and UnitOfWork Service 
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings


    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789- ._@+";
    options.User.RequireUniqueEmail = true;
});





// Service Configration for All files
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
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
app.UseTus(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
DefaultTusConfiguration CreateTusConfiguration(IServiceProvider serviceProvider)
{
    var env = (IWebHostEnvironment)serviceProvider.GetRequiredService(typeof(IWebHostEnvironment));

    //File upload path
    var tusFiles = Path.Combine(env.WebRootPath, "tusfiles");

    return new DefaultTusConfiguration
    {
        UrlPath = "/files",
        //File storage path
        Store = new TusDiskStore(tusFiles),
        //Does metadata allow null values
        MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
        //The file will not be updated after expiration
        Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(5)),
        //Event handling (various events, meet your needs)
        Events = new Events
        {
            //Upload completion event callback
            OnFileCompleteAsync = async ctx =>
            {
                //Get upload file
                var file = await ctx.GetFileAsync();

                //Get upload file=
                var metadatas = await file.GetMetadataAsync(ctx.CancellationToken);

                //Get the target file name in the above file metadata
                var fileNameMetadata = metadatas["name"];

                //The target file name is encoded in Base64, so it needs to be decoded here
                var fileName = fileNameMetadata.GetString(Encoding.UTF8);

                var extensionName = Path.GetExtension(fileName);

                //Convert the uploaded file to the actual target file
                File.Move(Path.Combine(tusFiles, ctx.FileId), Path.Combine(tusFiles, $"{ctx.FileId}{extensionName}"));
            }
        }
    };
}

async Task Initialize(IServiceProvider serviceProvider)
{
    var context = serviceProvider
               .GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    var roleManager = serviceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();
    var roleName = "Administrator";
    IdentityResult result;
    var roleExist = await roleManager.RoleExistsAsync(roleName);
    if (!roleExist)
    {
        result = await roleManager
            .CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var config = serviceProvider
                .GetRequiredService<IConfiguration>();
            var admin = await userManager
                .FindByEmailAsync(config["AdminCredentials:Email"]);

            if (admin == null)
            {
                admin = new IdentityUser()
                {
                    UserName = "User@emaill",
                    Email = "User@email.com",
                    EmailConfirmed = true
                };
                result = await userManager
                    .CreateAsync(admin, "PPpp  12");
                if (result.Succeeded)
                {
                    result = await userManager
                        .AddToRoleAsync(admin, roleName);
                    if (!result.Succeeded)
                    {
                        // todo: process errors
                    }
                }
            }
        }
    }
}