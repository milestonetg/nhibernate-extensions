using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;

namespace MilestoneTG.NHibernate.Extensions.AspNetCore
{
    /// <summary>
    /// IApplicationBuilder extension methods for NHiberate.
    /// </summary>
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        /// A convenience method for initializing the configuration and session factory on application startup.
        /// </summary>
        /// <param name="builder"></param>
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
        public static IApplicationBuilder UseNHibernate(this IApplicationBuilder builder)
        {
            if (builder.ApplicationServices.GetService<ISessionFactory>() == null)
                throw new HibernateConfigException("Unable to initialize the session factorey.");

            return builder;
        }
    }
}
