using System;
using Microsoft.ApplicationServer.Caching;
using RoReader.Infrastructure.Core;

namespace RoReader.Infrastructure.Cache
{
    public class AzureCache : IAzureCache
    {
        DataCache _cache;

        public AzureCache()
        {
            _cache = new DataCache("default");
        }


        public bool Exists<T>(string key)
        {
            var result = (T)_cache.Get(key);

            if (result is T)
            {
                return true;
            }

            return false;
        }

        public T Get<T>(string key)
        {
            var result = (T)_cache.Get(key);

            if (result is T)
            {
                return (T)result;
            }

            return default(T);
        }

        public void Put<T>(string key, object value)
        {
            _cache.Put(key, value);
        }

        public void Put<T>(string key, object value, TimeSpan timeOut)
        {
            _cache.Put(key, value, timeOut);
        }

        public T MakeCached<T>(string key, Func<string, T> resultFunc)
        {
            return MakeCached(key, resultFunc, TimeSpan.FromDays(365));
        }

        public T MakeCached<T>(string key, Func<string, T> resultFunc, TimeSpan absoluteExpiry)
        {
            var value = Get<T>(key);

            if (Equals(default(T), value))
            {
                value = resultFunc(key);

                if (!Equals(default(T), value))
                {
                    Put<T>(key, value, absoluteExpiry);
                }
            }

            return value;
        }
    }
}
