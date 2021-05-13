

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class TypesExtensions
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