using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;

namespace MilestoneTG.NHibernate.Extensions.AspNetCore
{
    /// <summary>
    /// IServiceCollection extension methods for configuring and registering NHibernate.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds NHibernate to the services collection using the default mapping file name of "hibernate.hbm.xml", and
        /// options defined in the NHibernate section of appSettings.json.
        /// </summary>
        /// <param name="services">The IServerCollection instance.</param>
        /// <param name="filters">An array of global filters to enable when creating a session.</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="charp">
        /// public class Startup
        /// {
        ///     public Startup(IConfiguration configuration)
        ///     {
        ///         Configuration = configuration;
        ///     }
        /// 
        ///     public IConfiguration Configuration { get; }
        /// 
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        ///
        ///         services.AddNHibernate();
        ///     }
        ///     
        ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        ///     {
        ///         app.UseNHibernate();
        ///
        ///         if (env.IsDevelopment())
        ///         {
        ///             app.UseDeveloperExceptionPage();
        ///         }
        ///         else
        ///         {
        ///             app.UseHsts();
        ///         }
        /// 
        ///         app.UseHttpsRedirection();
        ///         app.UseMvc();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string[] filters = null)
        {
            return services.AddNHibernate("hibernate.hbm.xml", filters);
        }

        /// <summary>
        /// Adds NHibernate to the services collection using the mapping file provided, and
        /// options defined in the NHibernate section of appSettings.json.
        /// </summary>
        /// <param name="services">The IServerCollection instance.</param>
        /// <param name="xmlMappingFile">Filename (and path) of the hibernate mapping xml file.</param>
        /// <param name="filters">An array of global filters to enable when creating a session.</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="csharp">
        /// public class Startup
        /// {
        ///     public Startup(IConfiguration configuration)
        ///     {
        ///         Configuration = configuration;
        ///     }
        /// 
        ///     public IConfiguration Configuration { get; }
        /// 
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        ///
        ///         services.AddNHibernate("mappings.xml");
        ///     }
        ///     
        ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        ///     {
        ///         app.UseNHibernate();
        ///
        ///         if (env.IsDevelopment())
        ///         {
        ///             app.UseDeveloperExceptionPage();
        ///         }
        ///         else
        ///         {
        ///             app.UseHsts();
        ///         }
        /// 
        ///         app.UseHttpsRedirection();
        ///         app.UseMvc();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string xmlMappingFile, string[] filters = null)
        {
            services.AddNHibernate((provider, config) =>
            {
                IConfiguration appConfig = provider.GetService<IConfiguration>();
                NHibernateOptions options = appConfig.GetSection(NHibernateOptions.SECTION_NAME).Get<NHibernateOptions>();

                config.Properties.Add("connection.connection_string", appConfig.GetConnectionString(options.ConnectionStringName));
                config.AddProperties(options.Properties);
                config.AddXmlFile(xmlMappingFile);
            }, filters);

            return services;
        }

        /// <summary>
        /// Adds NHibernate to the services collection using the mapping file provided, and
        /// properties provided.
        /// </summary>
        /// <param name="services">The IServerCollection instance.</param>
        /// <param name="xmlMappingFile">Filename (and path) of the hibernate mapping xml file.</param>
        /// <param name="properties">A dictionary of properties used to configure the SessionFactory</param>
        /// <param name="filters">An array of global filters to enable when creating a session.</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="csharp">
        /// public class Startup
        /// {
        ///     public Startup(IConfiguration configuration)
        ///     {
        ///         Configuration = configuration;
        ///     }
        /// 
        ///     public IConfiguration Configuration { get; }
        /// 
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        ///
        ///         IDictionary&lt;string, string&gt; properties = new Dictionary&lt;string, string&gt; {
        ///             { "connection.connection_string", "Data Source=nhibernate.db;" },
        ///             { "connection.driver_class", "MilestoneTG.NHibernate.Driver.Sqlite.Microsoft.MicrosoftSqliteDriver, MilestoneTG.NHibernate.Driver.Sqlite.Microsoft" },
        ///             { "dialect", "NHibernate.Dialect.SQLiteDialect" },
        ///             { "hbm2ddl.keywords", "auto-quote"},
        ///             { "show_sql", "true"},
        ///             { "generate_statistics", "true"}
        ///         };
        ///         services.AddNHibernate("mappings.xml", properties);
        ///     }
        ///     
        ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        ///     {
        ///         app.UseNHibernate();
        ///
        ///         if (env.IsDevelopment())
        ///         {
        ///             app.UseDeveloperExceptionPage();
        ///         }
        ///         else
        ///         {
        ///             app.UseHsts();
        ///         }
        /// 
        ///         app.UseHttpsRedirection();
        ///         app.UseMvc();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string xmlMappingFile, IDictionary<string, string> properties, string[] filters = null)
        {
            services.AddNHibernate((provider, config) =>
            {
                config.AddProperties(properties);
                config.AddXmlFile(xmlMappingFile);
            }, filters);

            return services;
        }

        /// <summary>
        /// Adds NHibernate to the services collection using the configuration delegate provided.
        /// </summary>
        /// <param name="services">The IServerCollection instance.</param>
        /// <param name="configure">An action delegate used to configure the session factory.</param>
        /// <param name="filters">An array of global filters to enable when creating a session.</param>
        /// <returns></returns>
        /// <example>
        /// <code lang="charp">
        /// public class Startup
        /// {
        ///     public Startup(IConfiguration configuration)
        ///     {
        ///         Configuration = configuration;
        ///     }
        /// 
        ///     public IConfiguration Configuration { get; }
        /// 
        ///     public void ConfigureServices(IServiceCollection services)
        ///     {
        ///         services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        ///
        ///         services.AddNHibernate((provider, config)=>{
        ///             IDictionary&lt;string, string&gt; properties = new Dictionary&lt;string, string&gt; {
        ///                 { "connection.connection_string", "Data Source=nhibernate.db;" },
        ///                 { "connection.driver_class", "MilestoneTG.NHibernate.Driver.Sqlite.Microsoft.MicrosoftSqliteDriver, MilestoneTG.NHibernate.Driver.Sqlite.Microsoft" },
        ///                 { "dialect", "NHibernate.Dialect.SQLiteDialect" },
        ///                 { "hbm2ddl.keywords", "auto-quote"},
        ///                 { "show_sql", "true"},
        ///                 { "generate_statistics", "true"}
        ///             };
        ///             
        ///             config.AddProperties(properties);
        ///             config.AddXmlFile("mappings.xml");
        ///         });
        ///     }
        ///     
        ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        ///     {
        ///         app.UseNHibernate();
        ///
        ///         if (env.IsDevelopment())
        ///         {
        ///             app.UseDeveloperExceptionPage();
        ///         }
        ///         else
        ///         {
        ///             app.UseHsts();
        ///         }
        /// 
        ///         app.UseHttpsRedirection();
        ///         app.UseMvc();
        ///     }
        /// }
        /// </code>
        /// </example>
        public static IServiceCollection AddNHibernate(this IServiceCollection services, Action<IServiceProvider, Configuration> configure, string[] filters = null)
        {
            services.AddSingleton((provider) =>
            {
                Configuration config = new Configuration();
                configure(provider, config);
                return config;
            });

            services.AddSingleton((provider) =>
            {
                return provider.GetService<Configuration>().BuildSessionFactory();
            });

            services.AddScoped((provider) =>
            {

                ISessionFactory factory = provider.GetService<ISessionFactory>();
                ISession session = factory.OpenSession();

                if (filters != null)
                {
                    foreach (string filterName in filters)
                        session.EnableFilter(filterName);
                }

                return session;
            });

            return services;
        }
    }
}
