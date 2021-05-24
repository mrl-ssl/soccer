using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MRL.SSL.Common.Utils
{
    public class XorShift
    {
        private static int _seedCount = 0;

        public static ThreadLocal<XorShift> CreateInstance()
        {
            return new ThreadLocal<XorShift>(() => new XorShift(GenerateSeed()));
        }

        private static uint GenerateSeed()
        {
            // note the usage of Interlocked, remember that in a shared context we can't just "_seedCount++"
            return (uint)((Environment.TickCount << 4) + (Interlocked.Increment(ref _seedCount)));
        }
        private IEnumerator<uint> r;
        public static readonly Dictionary<string, uint> defaults = new Dictionary<string, uint>(){
            {"x",123456789},
            {"y",362436069},
            {"z",521288629},
            {"w",88675123}
        };
        public readonly Dictionary<string, uint> seeds;
        public uint randCount = 0;

        public XorShift(uint? _w = null, uint? _x = null, uint? _y = null, uint? _z = null)
        {
            uint w = _w ?? GenerateSeed();
            uint x = _x ?? w << 13;
            uint y = _y ?? (w >> 9) ^ (x << 6);
            uint z = _z ?? y >> 7;
            seeds = new Dictionary<string, uint>(){
                {"x",x},{"y",y},{"z",z},{"w",w}
            };
            r = RandGen(w, x, y, z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerator<uint> RandGen(uint w, uint x, uint y, uint z)
        {
            uint t;
            for (; ; )
            {
                t = x ^ (x << 11);
                x = y;
                y = z;
                z = w;
                yield return w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Rand()
        {
            randCount++;
            r.MoveNext();
            return r.Current;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int RandInt(int min = 0, int max = 0x7FFFFFFF)
        {
            return (int)(Rand() % (max - min)) + min;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float RandFloat(float min = 0, float max = 1)
        {
            return (float)(Rand() % 0xFFFF) / 0xFFFF * (max - min) + min;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] Shuffle<T>(T[] _arr)
        {
            var arr = (T[])_arr.Clone();
            for (int i = 0; i <= arr.Length - 2; i++)
            {
                int r = RandInt(i, arr.Length - 1);
                T tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }
            return arr;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public List<T> Shuffle<T>(List<T> _arr)
        {
            var arr = new List<T>(_arr);
            for (int i = 0; i <= arr.Count - 2; i++)
            {
                int r = RandInt(i, arr.Count - 1);
                T tmp = arr[i];
                arr[i] = arr[r];
                arr[r] = tmp;
            }
            return arr;
        }
    }
}