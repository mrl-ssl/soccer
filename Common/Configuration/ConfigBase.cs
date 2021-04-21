using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MRL.SSL.Common.Configuration
{
    public class ConfigBase
    {
        protected static ConfigBase _default;
        public static string name;

        public string Name { get => name; }

        public ConfigBase()
        {

        }
        public void Load(string baseAddress)
        {
            name = GetType().Name.Substring(0, GetType().Name.LastIndexOf("Config"));

            _default = GetType().GetConstructor(new Type[] { }).Invoke(new object[] { }) as ConfigBase;
            var dom = new ConfigurationBuilder()
                            .SetBasePath(baseAddress)
                            .AddXmlFile(name + ".xml")
                            .Build();

            dom.Bind(name, _default);
        }
        public void Load()
        {
            var baseAddress = Assembly.GetEntryAssembly().Location;
            baseAddress = Path.Combine(baseAddress.Substring(0, baseAddress.LastIndexOf("bin")), "configs");

            Load(baseAddress);
        }
    }
}