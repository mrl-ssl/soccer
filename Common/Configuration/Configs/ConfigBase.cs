using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace MRL.SSL.Common.Configuration
{
    public class ConfigBase
    {
        protected static Dictionary<int, ConfigBase> _default = new Dictionary<int, ConfigBase>();
        protected IConfigurationRoot dom;
        public static ConfigBase Default { get { return null; } }
        public string Name { get; private set; }
        public virtual ConfigType Id { get; }
        public bool HasChanged { get; private set; }

        public ConfigBase()
        {

        }
        public void Load(string address)
        {
            string name = GetType().Name.Substring(0, GetType().Name.LastIndexOf("Config"));
            // var t = GetType().GetConstructor(new Type[] { }).Invoke(new object[] { }) as ConfigBase;
            dom = new ConfigurationBuilder()
                             .AddJsonFile(address, false, true)
                             .Build();

            Name = name;
            _default.Add((int)Id, this);
            dom.Bind(this);
            Action onChange = () =>
            {
                HasChanged = true;
            };
            ChangeToken.OnChange(() => dom.GetReloadToken(), onChange);
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
        MergerTracker = 1,
        Field = 2,
        Robot = 3
    }
}