using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfo.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(o => 
                {
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                })
                //Below AddJsonOptions returns output in Pascal casing - for example "PointsOfInterest" instead of "pointsOfInterest"
                //.AddJsonOptions(o =>
                //{
                //    if (o.SerializerSettings.ContractResolver != null)
                //    {
                //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                //        castedResolver.NamingStrategy = null;
                //    }
                //})
                ;

            //services.AddTransient();
            //services.AddScoped();
            //services.AddSingleton();

#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
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
                //app.UseExceptionHandler();
                /*  Using .net core 2.1, the above line app.UseExceptionHandler() in my Startup.cs class in my API causes the 
                 *  following error to occur when we start the app:

                    System.InvalidOperationException: An error occurred when configuring the exception handler 
                    middleware. Either the 'ExceptionHandlingPath' or the 'ExceptionHandler' option must be 
                    set in 'UseExceptionHandler()'.
                    at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware..ctor(RequestDelegate next, ILoggerFactory loggerFactory, IOptions`1 options, DiagnosticSource diagnosticSource)
                    --- End of stack trace from previous location where exception was thrown ---
                    at Microsoft.Extensions.Internal.ActivatorUtilities.ConstructorMatcher.CreateInstance(IServiceProvider provider)
                    at Microsoft.Extensions.Internal.ActivatorUtilities.CreateInstance(IServiceProvider provider, Type instanceType, Object[] parameters)
                    at Microsoft.AspNetCore.Builder.UseMiddlewareExtensions.<>c__DisplayClass4_0.<UseMiddleware>b__0(RequestDelegate next)
                    at Microsoft.AspNetCore.Builder.Internal.ApplicationBuilder.Build()
                    at Microsoft.AspNetCore.Hosting.Internal.WebHost.BuildApplication()

                    The problem went away with one of the following lines:
                    app.UseExceptionHandler("/");
                    app.UseExceptionHandler("/error");
                    Ref: https://github.com/JosephWoodward/GlobalExceptionHandlerDotNet/issues/18

                    I need to understand why. March 3, 2020 11:42 pm.

                */
                app.UseExceptionHandler("/error");
            }

            app.UseStatusCodePages();

            app.UseMvc();
        }
    }
}
