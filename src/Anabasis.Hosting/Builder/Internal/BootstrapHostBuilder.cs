﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Anabasis.Hosting.Builder.Internal;

internal class BootstrapHostBuilder : IHostBuilder
{
    private readonly IServiceCollection _services;

    private readonly List<Action<IConfigurationBuilder>>                     _configureHostConfigs = new();
    private readonly List<Action<HostBuilderContext, IServiceCollection>>    _configureServices    = new();
    private readonly List<Action<HostBuilderContext, IConfigurationBuilder>> _configureAppConfigs  = new();
    private readonly List<Action<IHostBuilder>>                              _remainingOperations  = new();

    public IDictionary<object, object> Properties { get; } = new Dictionary<object, object>();

    public BootstrapHostBuilder(IServiceCollection services) {
        _services = services;
    }

    public IHost Build() => throw new InvalidOperationException();

    public (HostBuilderContext HostBuilderContext, ConfigurationManager HostConfiguration) Apply(
        ConfigurationManager configuration, HostBuilder hostBuilder) {
        // Use default services/configurations for HostBuilder
        this.ConfigureDefaults(Array.Empty<string>());

        ConfigurationManager hostConfiguration = new();

        foreach (Action<IConfigurationBuilder> action in _configureHostConfigs) {
            action(hostConfiguration);
        }

        string contentRootPath =
            ResolveContentRootPath(hostConfiguration[HostDefaults.ContentRootKey]!, AppContext.BaseDirectory);
        HostBuilderContext hostBuilderContext = new HostBuilderContext(new Dictionary<object, object>()) {
            Configuration = hostConfiguration,
            HostingEnvironment = new HostEnvironment() {
                ApplicationName = hostConfiguration[HostDefaults.ApplicationKey],
                EnvironmentName = hostConfiguration[HostDefaults.EnvironmentKey] ?? Environments.Production,
                ContentRootPath = contentRootPath,
                ContentRootFileProvider = new PhysicalFileProvider(contentRootPath),
            },
        };

        configuration.SetBasePath(hostBuilderContext.HostingEnvironment.ContentRootPath);
        configuration.AddConfiguration(hostConfiguration, true);
        foreach (Action<HostBuilderContext, IConfigurationBuilder> action in _configureAppConfigs) {
            action(hostBuilderContext, configuration);
        }

        hostBuilderContext.Configuration = configuration;

        foreach (Action<HostBuilderContext, IServiceCollection> action in _configureServices) {
            action(hostBuilderContext, _services);
        }

        foreach (Action<IHostBuilder> action in _remainingOperations) {
            action(hostBuilder);
        }

        return (hostBuilderContext, hostConfiguration);
    }

    // https://github.com/dotnet/runtime/blob/312c66f1fc2f749f56612999cb1adab9ca7fde59/src/libraries/Microsoft.Extensions.Hosting/src/HostBuilder.cs#L198
    private string ResolveContentRootPath(string contentRootPath, string basePath) {
        if (string.IsNullOrEmpty(contentRootPath)) {
            return basePath;
        }

        if (Path.IsPathRooted(contentRootPath)) {
            return contentRootPath;
        }

        return Path.Combine(Path.GetFullPath(basePath), contentRootPath);
    }

    private class HostEnvironment : IHostEnvironment
    {
        public string? ApplicationName { get; set; }
#pragma warning disable CS8766
        public IFileProvider? ContentRootFileProvider { get; set; }
        public string? ContentRootPath { get; set; }
        public string? EnvironmentName { get; set; }
#pragma warning restore CS8766
    }

    public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate) {
        _configureAppConfigs.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
        return this;
    }

    public IHostBuilder ConfigureContainer<TContainerBuilder>(
        Action<HostBuilderContext, TContainerBuilder> configureDelegate) {
        _remainingOperations.Add(builder => builder.ConfigureContainer(configureDelegate));
        return this;
    }

    public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate) {
        _configureHostConfigs.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
        return this;
    }

    public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate) {
        _configureServices.Add(configureDelegate ?? throw new ArgumentNullException(nameof(configureDelegate)));
        return this;
    }

    public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        where TContainerBuilder : notnull {
        _remainingOperations.Add(builder => builder.UseServiceProviderFactory(factory));
        return this;
    }

    public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(
        Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        where TContainerBuilder : notnull {
        _remainingOperations.Add(builder => builder.UseServiceProviderFactory(factory));
        return this;
    }
}