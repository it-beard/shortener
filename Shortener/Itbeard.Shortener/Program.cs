using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Itbeard.Di;
using Itbeard.Shortener.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(
    containerBuilder => containerBuilder.RegisterModule(new WebApiDiModule()));

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddHeadElementHelper();

builder.Services.AddCustomAutoMapper();
builder.Services.AddCustomSqlContext(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //todo: need to be moved to Autofac DI
builder.Services.AddLocalization();

var app = builder.Build();

// configure Culture Info
var supportedCultures = new[] { "en", "ru", "be" };
var defaultCulture = CultureInfo.CurrentCulture.Name switch
{
    string s when s.StartsWith("en") => supportedCultures[0],
    string s when s.StartsWith("ru") => supportedCultures[1],
    string s when s.StartsWith("be") => supportedCultures[2],
    _ => supportedCultures[0]
};
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(defaultCulture)
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    app.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();