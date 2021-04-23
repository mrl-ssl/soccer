using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace MRL.SSL.Common.Configuration
{
    public class ConfigBase
    {
        protected static Dictionary<int, ConfigBase> _default = new Dictionary<int, ConfigBase>();
        public static ConfigBase Default { get { return null; } }
        public string Name { get; private set; }
        public virtual ConfigType Id { get; }

        public ConfigBase()
        {

        }
        public void Load(string baseAddress)
        {
            string name = GetType().Name.Substring(0, GetType().Name.LastIndexOf("Config"));
            var t = GetType().GetConstructor(new Type[] { }).Invoke(new object[] { }) as ConfigBase;
            var dom = new ConfigurationBuilder()
                            .SetBasePath(baseAddress)
                            .AddJsonFile(name + ".json")
                            .Build();

            Name = name;
            _default.Add((int)t.Id, t);
            dom.Bind(t);
        }
        public void Load()
        {
            var baseAddress = Assembly.GetEntryAssembly().Location;
            baseAddress = Path.Combine(baseAddress.Substring(0, baseAddress.LastIndexOf("bin")), "configs");

            Load(baseAddress);
        }
    }
    public enum ConfigType
    {
        Connection = 0,
        MergerTracker = 1
    }
}