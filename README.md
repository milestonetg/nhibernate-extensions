# NHibernate Extensions

[![Build Status](https://milestonetg.visualstudio.com/Milestone/_apis/build/status/nhibernate-extensions?branchName=master)](https://milestonetg.visualstudio.com/Milestone/_build/latest?definitionId=35&branchName=master)
 ![GitHub](https://img.shields.io/github/license/milestonetg/nhibernate-extensions.svg)

## Packages

### MilestoneTG.NHibernate.Extensions.AspNetCore

![Nuget](https://img.shields.io/nuget/v/MilestoneTG.NHibernate.Extensions.AspNetCore.svg)
![Nuget](https://img.shields.io/nuget/dt/MilestoneTG.NHibernate.Extensions.AspNetCore.svg)

Configuration classes and DI extensions for configuring NHibernate in an Asp.Net Core application.

#### Configuration

``` js
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Default": "server=(localdb)\\mssqlserver;"
  },
  "NHibernate": {
    "ConnectionStringName":  "Default",
    "Properties": {
      "connection.driver_class": "MilestoneTG.NHibernate.TransientFaultHandling.SqlServer.SqlAzureClientDriver, MilestoneTG.NHibernate.TransientFaultHandling.SqlServer",
      "dialect": "NHibernate.Dialect.MsSql2012Dialect",
      "hbm2ddl.keywords": "auto-quote",
      "show_sql": "true",
      "generate_statistics": "true"
    }
  }
}
```

#### Dependency Injection

To use the default filename of `hibernate.hbm.xml`:

``` cs
services.AddNHibernate();
```

Or you can specify your own file:

``` cs
services.AddNHibernate("mapping.xml");
```

Or you can pass in a custom configuration:

``` cs
services.AddNHibernate((provider, config)=>{
    IDictionary&lt;string, string&gt; properties = new Dictionary&lt;string, string&gt; {
        { "connection.connection_string", Configuration.GetConnectionString("Default") },
        { "dialect", "NHibernate.Dialect.SQLiteDialect" },
        { "hbm2ddl.keywords", "auto-quote"},
        { "show_sql", "true"},
        { "generate_statistics", "true"}
    };

    config.AddProperties(properties);
    config.AddXmlFile("mappings.xml");
});
```

### MilestoneTG.NHibernate.Extensions.Mapping.Attributes

![Nuget](https://img.shields.io/nuget/v/MilestoneTG.NHibernate.Extensions.Mapping.Attributes.svg)
![Nuget](https://img.shields.io/nuget/dt/MilestoneTG.NHibernate.Extensions.Mapping.Attributes.svg)

Configuration extensions for configuring NHibernate with NHibernate.Mapping.Attributes.

Example:

``` cs
var config = new Configuration();

config.AddMappingAssembly("Acme.Domain");
```

Using mapping attributes with Asp.Net Core:

``` cs
services.AddNHibernate((provider, config)=>{
    var options = Configuration.GetSection(NHibernateOptions.SECTION_NAME).Get<NHibernateOptions>();
    config.Properties.Add("connection.connection_string", Configuration.GetConnectionString(options.ConnectionStringName))
    config.AddProperties(options.Properties);
    config.AddMappingAssembly("Acme.Domain");
});
```
