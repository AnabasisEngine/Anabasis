﻿using Anabasis.Platform.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Anabasis.Hosting;

public static class AnabasisHostBuilderExtensions
{
    public static IHostBuilder ConfigureAnabasis(this IHostBuilder hostBuilder, string[]? args, Action<IHostBuilder>? configureHost = null)
    {
        configureHost?.Invoke(hostBuilder);

        return hostBuilder
            .ConfigureLogging(logging => { })
            .ConfigureAppConfiguration(config => { })
            .ConfigureServices(services =>
            {
                services.AddAnabasisCore(args ?? GetCommandLineArguments());

                services.AddHostedService<AnabasisHostedService>();
            });
    }
    
    

    public static AnabasisAppHostBuilder UsingPlatform<TPlatform>(this AnabasisAppHostBuilder builder)
        where TPlatform : class, IAnabasisPlatform {
        builder.ConfigureServices(TPlatform.ConfigureServices);
        return builder;
    }
    
    private static string[] GetCommandLineArguments()
    {
        var args = System.Environment.GetCommandLineArgs();
        return args.Any()
            ? args.Skip(1).ToArray() // args[0] is the path to executable binary.
            : Array.Empty<string>();
    }
}