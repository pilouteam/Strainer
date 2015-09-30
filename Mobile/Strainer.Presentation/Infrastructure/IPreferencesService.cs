using System;

namespace Strainer.Infrastructure
{
    public interface IPreferencesService
    {
        void Set(string key, bool value);
        void Set(string key, string value);
        void Set(string key, int value);
        void Set(string key, float value);
        T Get<T>(string key);
    }
}

