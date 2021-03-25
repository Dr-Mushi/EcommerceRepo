using TestingEFRelations.Models;
using TestingEFRelations.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestingEFRelations.Data;
using TestingEFRelations.Repositories.Interface;
using AutoMapper;
using System;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TestingEFRelations
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //Identity Framework
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //Identity Framework Configurations
            services.Configure<IdentityOptions>(options =>
            {

            });
            //redirect unregistered users to login page or give 401 error for API
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = Configuration.GetValue<string>("LogInPath");
                config.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api")
                    )
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
                //config.LoginPath = Configuration.GetValue<string>("LogInPath");
                //config.LoginPath = "/api/ProductApi/AccessDenied";
                
            });


            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();



            //for API consuming
            services.AddHttpClient();
            services.AddHttpClient("Ecommerce", c => {
                c.BaseAddress = new Uri(Configuration.GetValue<string>("EcommerceAPI"));
            });



            //for API handling deep tree branches and Patch requests
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //Deep tree branches
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                //PATCH
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            );
            

            //Auto mapper for DTO
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Map model to section in app settings NOT USED
            var jwt = Configuration.GetSection("JWT");
            services.Configure<AppSettings>(jwt);


            //JWT
            //take the secret key
            var secretKey = Encoding.UTF8.GetBytes(Constants.Secret);
            //give it a symmetric key
            var key = new SymmetricSecurityKey(secretKey);

            services.AddAuthentication(config =>
            {
                //for default Autherization checking
                //config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //add JWT bearer 
            }).AddJwtBearer(config =>
            {
                //this is where we will validate the token
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Constants.Issuer,
                    ValidAudience = Constants.Audiance,
                    IssuerSigningKey = key,
                    //make sure the default expire time is not set to more than zero, so that you can put custom EXP
                    ClockSkew = TimeSpan.Zero
                };

                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Token_AccessCookie"];
                        return Task.CompletedTask;
                    }
                };

            });


            // A distributed cache can improve the performance and scalability of an ASP.NET Core app 
            //check https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-5.0
            services.AddDistributedMemoryCache();

            //
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.IsEssential = true;
            });




            //Repositories
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IWishlistRepository, WishlistRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            // automatically enable session state for the application.
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Product}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapControllers();


            });
        }
    }
}













////to validate the to
//var appSettings = jwt.Get<AppSettings>();
//var key = Encoding.ASCII.GetBytes(appSettings.Secret);
////JWT 
//services.AddAuthentication(auth =>
//{
//    //because of this the authorization default will not go to line 51 and so... it will not direct the user to the login page.
//    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//})
//    .AddJwtBearer(jwt =>
//    {
//        jwt.RequireHttpsMetadata = false;
//        jwt.SaveToken = true;
//        jwt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//        //the API
//        //options.Audience = Configuration["AzureActiveDirectory:ResourceId"];
//        ////the one who gives the TOKENS on the API behalf which is Azure
//        //options.Authority = $"{Configuration["AzureActiveDirectory:InstanceId"]}{Configuration["AzureActiveDirectory:TenantId"]}";
//    });



////Aouth
//services.AddAuthentication(config =>
//{
//    //check the cookie to see if the user logged in
//    config.DefaultAuthenticateScheme = "CleintCookie";

//    //deal out a cookie when the user sings up
//    config.DefaultSignInScheme = "CleintCookie";

//    //check if the user is allowed to do something
//    config.DefaultChallengeScheme = "OurServer";
//})
//    .AddCookie("CleintCookie")
//    .AddOAuth("OurServer",config=>
//    {
//        config.ClientId = "client_id";
//        config.ClientSecret = "client_secret";
//        config.CallbackPath = "/Account";
//        config.AuthorizationEndpoint = "https://localhost:44324/Account/Authorize";
//        config.TokenEndpoint = "https://localhost:44324/Account/Token";

//    });


