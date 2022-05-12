﻿using Anabasis.Builder.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Anabasis.Builder
{
    public class AnabasisAppBuilder
    {
        private readonly HostBuilder                 _hostBuilder = new HostBuilder();
        private readonly ConfigureHostBuilder        _configureHostBuilder;
        private readonly Action<AnabasisAppOptions>? _configureOptions;
        private          AnabasisApp?                _application;

        public IServiceCollection Services { get; } = new ServiceCollection();

        public ILoggingBuilder Logging { get; }
        public IHostBuilder Host => _configureHostBuilder;
        public IHostEnvironment Environment { get; }
        public ConfigurationManager Configuration { get; }
    
        internal AnabasisAppBuilder(string[]? args, Action<AnabasisAppOptions>? configureOptions = null)
        {
            Configuration = new ConfigurationManager();
            _configureOptions = configureOptions;

            // Sets the default configuration values for the application host.
            // such as EnvironmentName, ApplicationName, ContentRoot...
            BootstrapHostBuilder bootstrapHostBuilder = new(Services);
            bootstrapHostBuilder.ConfigureDefaultAnabasis(args);
            (HostBuilderContext hostBuilderContext, ConfigurationManager _) = bootstrapHostBuilder.Apply(Configuration, _hostBuilder);

            _configureHostBuilder = new ConfigureHostBuilder(hostBuilderContext, Configuration, Services);
            Environment = hostBuilderContext.HostingEnvironment;
            Logging = new LoggingBuilder(Services);

            Services.AddSingleton<IConfiguration>(_ => Configuration);
        }
    }
}