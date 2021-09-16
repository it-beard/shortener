using System;
using System.Threading.Tasks;
using Autofac;
using Itbeard.Di;
using Itbeard.Shortener.AppStart;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Itbeard.Shortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddAutoMapperCustom();
            services.AddDatabaseContext(Configuration);
            services.Configure<CookiePolicyOptions>(options =>
              {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
              });

              // Add authentication services
              services.AddAuthentication(options =>
              {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              })
              .AddCookie()
              .AddOpenIdConnect("Auth0", options =>
              {
                  // Set the authority to your Auth0 domain
                  options.Authority = $"https://{Configuration["Auth0:Domain"]}";

                  // Configure the Auth0 Client ID and Client Secret
                  options.ClientId = Configuration["Auth0:ClientId"];
                  options.ClientSecret = Configuration["Auth0:ClientSecret"];

                  // Set response type to code
                  options.ResponseType = "code";

                  // Configure the scope
                  options.Scope.Clear();
                  options.Scope.Add("openid");

                  // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
                  // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
                  options.CallbackPath = new PathString("/admin/callback");

                  // Configure the Claims Issuer to be Auth0
                  options.ClaimsIssuer = "Auth0";

                  options.Events = new OpenIdConnectEvents
                  {
                      // handle the logout redirection
                      OnRedirectToIdentityProviderForSignOut = (context) =>
                      {
                          var logoutUri =
                              $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                          var postLogoutUri = context.Properties.RedirectUri;
                          if (!string.IsNullOrEmpty(postLogoutUri))
                          {
                              if (postLogoutUri.StartsWith("/"))
                              {
                                  // transform to absolute
                                  var request = context.Request;
                                  postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                              }

                              logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                          }

                          context.Response.Redirect(logoutUri);
                          context.HandleResponse();

                          return Task.CompletedTask;
                      }
                  };
              });
            services.AddHttpContextAccessor();
            //todo: need to be moved to Autofac DI
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new WebApiDiModule());
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}