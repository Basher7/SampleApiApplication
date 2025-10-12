using Application;
using Domain.Abstraction;
using FluentValidation;
using LiteMediator;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using SampleApiApplication.ActionFilters;
using System.IO.Compression;

namespace SampleApiApplication;

internal sealed class Startup
{
    internal static void ConfigureServices(IServiceCollection services, bool isWindows)
    {
        services.AddResponseCompression(option =>
        {
            option.EnableForHttps = true;
            option.Providers.Add<BrotliCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(option =>
        {
            option.Level = CompressionLevel.Fastest;
        });

        //For Nginx Deploy
        if (!isWindows)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        //services.AddControllersWithViews()
        //.AddMvcOptions((options) =>
        //{
        //    options.Filters.Add(new ConsumesAttribute("application/json"));
        //});
        services
            .AddControllers((options) =>
            {
                options.Filters.Add<FluentValidationFilter>();
            })
            .AddMvcOptions((options) =>
            {
                options.Filters.Add(new ConsumesAttribute(MimeTypes.applicationjson));
            });

        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHttpClient();
        services.AddMemoryCache();

        // Register MediatR
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(ApplicationAssemblyReference.Assembly);
        });
        //services.AddLiteMediator(ApplicationAssemblyReference.Assembly);
    }

    internal static void ConfigureMethod(WebApplication app, bool isWindows)
    {
        bool isDevelopment = app.Environment.IsDevelopment();

        if (isDevelopment)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //For Nginx Deploy
        if (!isWindows)
        {
            app.UseForwardedHeaders();
        }

        // Configure the HTTP request pipeline.
        //if (!isDevelopment)
        //{
        //    app.UseHsts();
        //    app.UseHttpsRedirection();
        //}

        app.UseResponseCompression();
        app.UseStaticFiles();

        app.MapControllers();
        app.UseRouting();

        if (!isDevelopment)
            app.MapGet("/", () => "Welcome to Sample Application");
    }
}
