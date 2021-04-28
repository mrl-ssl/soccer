using System.Reflection;
using System.IO;
using System.Linq;
using System;
using MRL.SSL.Common.Utils.Extensions;

namespace MRL.SSL.Common.Configuration
{
    public class ConfigurationLoader
    {
        static string dir;

        public static void Load(string section = "")
        {
            if (dir == null)
            {
                dir = Assembly.GetEntryAssembly().Location;
                dir = dir.Substring(0, dir.LastIndexOf("bin") - 1);
                var parent = Directory.GetParent(dir);
                if (parent == null)
                {
                    return;
                }
                dir = Path.Combine(parent.FullName, "configs");
            }
            var baseAdress = Path.Combine(dir, section);
            var fileEntries = Directory.GetFiles(baseAdress).ToList().Where(w => w.LastIndexOf(".json") > 0);
            var types = Assembly.GetAssembly(typeof(ConfigBase)).GetTypes().ToList().Where(t => t.IsClass && t.IsSubclassOf(typeof(ConfigBase))).ToList();

            foreach (var item in fileEntries)
            {
                string typeName = Path.GetFileNameWithoutExtension(item).ToPascalCase() + "Config";
                var t = types.Where(w => w.Name == typeName).FirstOrDefault();
                if (t != null)
                {
                    ConfigBase c = (ConfigBase)Activator.CreateInstance(t);
                    c.Load(item);
                }
                else
                    Console.WriteLine("Cannot find proper type for {0}.xml file", typeName.Substring(0, typeName.LastIndexOf("Config")));
            }
        }
    }
}