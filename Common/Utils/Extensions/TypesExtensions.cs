

using System;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class CommonExtensions
    {
        public static T As<T>(this object obj)
        {
            if (obj == null)
                return default(T);
            else
                return (T)obj;
        }
    }
}