using System.Collections.Generic;

namespace MilestoneTG.NHibernate.Extensions.AspNetCore
{
    /// <summary>
    /// NHibernate configuration for use with Microsoft.Extensions.Configuration.
    /// </summary>
    /// <example>
    /// NHibernate section in appsettings.json:
    /// 
    /// <code lang="js"> 
    /// "NHibernate": {
    ///   "ConnectionStringName":  "Default",
    ///     "Properties": {
    ///       "dialect": "NHibernate.Dialect.MsSql2012Dialect",
    ///       "hbm2ddl.keywords": "auto-quote",
    ///       "show_sql": "true",
    ///       "generate_statistics": "true"
    ///     }
    /// }
    /// </code>
    /// </example>
    public class NHibernateOptions
    {
        /// <summary>
        /// Configuration Section name
        /// </summary>
        public const string SECTION_NAME = "NHibernate";

        /// <summary>
        /// The name/key of the connection string to use from the ConnectionStrings section of appsettings.json.
        /// </summary>
        public string ConnectionStringName { get; set; }

        /// <summary>
        /// Dictionary of NHibernate configuration properties.
        /// </summary>
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
    }
}
