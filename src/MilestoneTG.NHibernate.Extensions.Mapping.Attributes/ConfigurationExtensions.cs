using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using System;
using System.IO;
using System.Reflection;

namespace MilestoneTG.NHibernate.Mapping.Attributes
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds a mapping using an assembly containing classes decorated with NHibernate.Mapping.Attributes.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="mappedAssemblyName">The name of of the assembly.</param>
        /// <returns></returns>
        public static Configuration AddMappingAssembly(this Configuration configuration, string mappedAssemblyName)
        {
            if (string.IsNullOrEmpty(mappedAssemblyName.Trim()))
                throw new ArgumentException($"Parameter {nameof(mappedAssemblyName)} cannot be null or empty.", nameof(mappedAssemblyName));

            AssemblyName assemblyName = new AssemblyName(mappedAssemblyName);

            return configuration.AddMappingAssembly(assemblyName);
        }

        /// <summary>
        /// Adds a mapping using an assembly containing classes decorated with NHibernate.Mapping.Attributes.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="mappedAssemblyName">The name of of the assembly.</param>
        /// <returns></returns>
        public static Configuration AddMappingAssembly(this Configuration configuration, AssemblyName mappedAssemblyName)
        {
            if (mappedAssemblyName == null)
                throw new ArgumentNullException(nameof(mappedAssemblyName));

            Assembly mappedAssembly = Assembly.Load(mappedAssemblyName);

            if (mappedAssembly == null)
                throw new Exception(string.Format("Cannot load assembly {0}.  File not found.", mappedAssemblyName.FullName));

            return configuration.AddMappingAssembly(mappedAssembly);
        }

        /// <summary>
        /// Adds a mapping using an assembly containing classes decorated with NHibernate.Mapping.Attributes.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="mappedAssemblyName">The name of of the assembly.</param>
        /// <returns></returns>
        public static Configuration AddMappingAssembly(this Configuration configuration, Assembly mappedAssembly)
        {
            if (mappedAssembly == null)
                throw new ArgumentNullException(nameof(mappedAssembly));

            using (MemoryStream stream = new MemoryStream())
            {
                HbmSerializer.Default.HbmNamespace = mappedAssembly.FullName;
                HbmSerializer.Default.HbmAssembly = mappedAssembly.FullName;
                HbmSerializer.Default.Serialize(stream, mappedAssembly);

                stream.Position = 0;

                return configuration.AddInputStream(stream);
            }
        }
    }
}
