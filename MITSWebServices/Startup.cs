using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MITSDataLib.Contexts;
using MITSDataLib.Models;
using MITSDataLib.Seeds;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OpenIddict.Validation;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphiQl;
using static MITSWebServices.GraphQL;
using GraphQL.Validation;
using MITSBusinessLib.Business;
using MITSBusinessLib.GraphQL;
using MITSBusinessLib.GraphQL.Types;
using MITSBusinessLib.GraphQL.Types.Inputs;
using MITSBusinessLib.Repositories;
using MITSBusinessLib.Repositories.Interfaces;

namespace MITSWebServices
{
    public class Startup
    {


        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //TODO: Only for development
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options => 
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                });
     
            //Setup up database and use OpenIddict
            services.AddDbContext<MITSContext>(options => {
                options.UseSqlServer(_config.GetConnectionString("DevConnectionString"));
                
                options.UseOpenIddict();
                //options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Scoped);

            services.AddIdentity<User, IdentityRole>(option =>
            {
                option.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MITSContext>();


            //services.AddAuthorization(options => {
            //    options.AddPolicy("Faculty", policy => policy.RequireClaim("Role", "Faculty"));
            //    options.AddPolicy("Student", policy => policy.RequireClaim("Role", "Student"));
            //    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));

            //});

            services.AddGraphQLAuth(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireClaim("role", "Admin"));
            });

            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = true;
            }).AddUserContextBuilder(context => new GraphQLUserContext { User = context.User });


            services.AddOpenIddict()
                
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<MITSContext>();
                })
                
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();
                    // Enable the token endpoint.
                    options.EnableTokenEndpoint("/connect/token");

                    // Enable the password flow.
                    options.AllowPasswordFlow();

                    // Accept anonymous clients (i.e clients that don't send a client_id).
                    options.AcceptAnonymousClients();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();

                    // Note: to use JWT access tokens instead of the default
                    // encrypted format, the following lines are required:
                    //
                    options.UseJsonWebTokens();
                    //Update for production...
                    options.AddEphemeralSigningKey();

                });

            // If you prefer using JWT, don't forget to disable the automatic
            // JWT -> WS-Federation claims mapping used by the JWT middleware:
            //
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(options => 
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:50000/";
                    options.Audience = "mits_server";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = OpenIdConnectConstants.Claims.Subject,
                        RoleClaimType = OpenIdConnectConstants.Claims.Role
                    };
                });


            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            

            


            services.AddTransient<MITSSeeder>();
            //What does this do?
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddScoped<ISchema, MITSSchema>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<IDaysRepository, DaysRepository>();
            services.AddScoped<ISectionsRepository, SectionsRepository>();
            services.AddScoped<IAuditRepository, AuditRepository>();
            services.AddScoped<ISpeakersRepository, SpeakersRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWaRepository, WaRepository>();
            services.AddScoped<IEventRegistrationBusinessLogic, EventRegistrationBusinessLogic>();
            services.AddScoped<MITSQuery>();
            services.AddScoped<MITSMutation>();
            services.AddScoped<EventType>();
            services.AddScoped<EventInputType>();
            services.AddScoped<RegistrationType>();
            services.AddScoped<RegistrationInputType>();
            services.AddScoped<CheckInAttendeeInputType>();
            services.AddScoped<CheckInAttendeeType>();
            services.AddScoped<DayType>();
            services.AddScoped<DayInputType>();
            services.AddScoped<SectionType>();
            services.AddScoped<SpeakerType>();
            services.AddScoped<SpeakerInputType>();
            services.AddScoped<TagType>();
            services.AddScoped<WaEventType>();
            services.AddScoped<WaRegistrationType>();
            services.AddScoped<UserType>();
            services.AddScoped<UserRoleType>();

            //var sp = services.BuildServiceProvider();
            //services.AddSingleton<ISchema>(new MITSSchema(new FuncDependencyResolver(type => sp.GetService(type))));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            var validationRules = app.ApplicationServices.GetService<IValidationRule>();
            app.UseGraphQL<ISchema>("/graphql");

            app.UseGraphiQl("/graphiql");

            //Only for development
            //Todo: Remove for production
            app.UseCors("CorsPolicy");

            app.UseMvc(
            //routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //}
            );

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                //if (env.IsDevelopment())
                //{
                //    //spa.UseAngularCliServer(npmScript: "start");
                //    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                //}
            });

            
        }
    }
}
