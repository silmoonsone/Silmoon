using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;
using Silmoon.AspNetCore.Binders;
using Silmoon.AspNetCore.Extensions;
using Silmoon.AspNetCore.Test;
using Silmoon.AspNetCore.Test.Hubs;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.Business.AspNetCore;
using Silmoon.Business.AspNetCore.Binders;
using Silmoon.Business.Data.MongoDB.Converts;
using System.Numerics;
using System.Reflection;

// Newtonsoft.Json to MongoDB.Bson Converters
//BsonSerializer.RegisterSerializer(typeof(HexBigInteger), new HexBigIntegerConvertSerializer());
BsonSerializer.RegisterSerializer(typeof(BigInteger), new BigIntegerConvertSerializer());
BsonSerializer.RegisterSerializer(typeof(JObject), new JObjectBsonDocumentConvertSerializer());
BsonSerializer.RegisterSerializer(typeof(JArray), new JArrayBsonDocumentConvertSerializer());

var ProjectName = Assembly.GetExecutingAssembly().GetName().Name;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc(config =>
{
    config.ModelBinderProviders.Insert(0, new BigIntegerBinderProvider());
    config.ModelBinderProviders.Insert(0, new ObjectIdBinderProvider());
}).AddNewtonsoftJson();

//builder.Services.AddSwaggerGen(c =>
//{
//    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
//    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
//    c.IncludeXmlComments(xmlPath, true);
//}).AddSwaggerGenNewtonsoftSupport();

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));
builder.Services.AddSession(o => { o.Cookie.Name = ProjectName + "_" + "Session"; });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
{
    o.LoginPath = new PathString("/Account/Login");
    o.AccessDeniedPath = new PathString("/Home/Privacy");
    o.Cookie.Name = ProjectName + "_" + "Cookie";
});
builder.Services.AddSingleton<Core>();

//builder.Services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();
//builder.Services.AddSignalR().AddNewtonsoftJsonProtocol();
//builder.Services.AddServerSideBlazor();

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

var app = builder.Build();

Core core = app.Services.GetRequiredService<Core>();
ILogger logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseSwagger();
//app.UseSwaggerUI(c => c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None));

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseUserSession<User>((Username, NameIdentifier, User) =>
{
    Helper.Output(logger, "User \"" + Username + "\" session data is lost, using private api from passport pull user info.", LogLevel.Information);
    var user = core.GetUser(Username);
    if (user is null) return null;

    user.Password = "#hidden#";
    return user;
}, (Username, NameIdentifier, UserToken) =>
{
    User user = default;
    //Using UserToken Get user object and return
    return user;
});

app.UseApiDecrypt((string appId) =>
{
    return (null, null);
});
app.UseSession();
app.UseRouting();

//app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area:exists?}/{controller=Home}/{action=Index}");

app.MapRazorPages();

//app.MapBlazorHub();
//app.MapBlazorHub("/Backend/_blazor");

//app.MapHub<DemoHub>("/hubs/DemoHub");


Helper.Output(logger, "Server app run.", LogLevel.Information);
app.Run();
Helper.Output(logger, "Server shutdown.", LogLevel.Information);