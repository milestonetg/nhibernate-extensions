using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilestoneTG.NHibernate.Extensions.AspNetCore;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;

namespace MilestoneTG.NHibernate.Extensions.AspNetCore.Tests
{
    public class Foo
    {
        public virtual long Id { get; set; }

        public virtual long Bar { get; set; }

        public virtual long Version { get; set; }

        public virtual MyClass MyClassList { get; set; }
    }

    public class MyClass
    {
        public virtual long Id { get; set; }

        public virtual long Version { get; set; }

        public virtual long Bar { get; set; }
    }

    public class Bar
    {
        public Bar(string foo)
        {
            MyProperty = foo;
        }

        public string MyProperty { get; private set; }

    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddNHibernate()
        {
            IConfiguration appConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddSingleton(appConfig);
            services.AddNHibernate();

            var provider = services.BuildServiceProvider();
            var config = provider.GetService<Configuration>();
            Assert.IsNotNull(config);

            var factory = provider.GetService<ISessionFactory>();
            Assert.IsNotNull(factory);

            var session = provider.GetService<ISession>();
            Assert.IsNotNull(session);
        }

        [TestMethod]
        public void AddNHibernate_File()
        {
            IConfiguration appConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.AddSingleton(appConfig);
            services.AddNHibernate("mappings.xml");

            var provider = services.BuildServiceProvider();
            var config = provider.GetService<Configuration>();
            Assert.IsNotNull(config);

            var factory = provider.GetService<ISessionFactory>();
            Assert.IsNotNull(factory);

            var session = provider.GetService<ISession>();
            Assert.IsNotNull(session);
        }

        [TestMethod]
        public void AddNHibernate_Properties()
        {
            IDictionary<string, string> properties = new Dictionary<string, string> {
                { "connection.connection_string", "Data Source=nhibernate.db;" },
                { "connection.driver_class", "MilestoneTG.NHibernate.Driver.Sqlite.Microsoft.MicrosoftSqliteDriver, MilestoneTG.NHibernate.Driver.Sqlite.Microsoft" },
                { "dialect", "NHibernate.Dialect.SQLiteDialect" },
                { "hbm2ddl.keywords", "auto-quote"},
                { "show_sql", "true"},
                { "generate_statistics", "true"}
            };

            var services = new ServiceCollection();
            services.AddNHibernate("mappings.xml", properties);

            var provider = services.BuildServiceProvider();
            var config = provider.GetService<Configuration>();
            Assert.IsNotNull(config);

            var factory = provider.GetService<ISessionFactory>();
            Assert.IsNotNull(factory);

            var session = provider.GetService<ISession>();
            Assert.IsNotNull(session);
        }
    }
}
