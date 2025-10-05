using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace Tarea2_AdrianArayaG_UNED.Repositories
{
    public class JsonFileStore<T>
    {
        private readonly string _path;
        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        public JsonFileStore(string virtualPath)
        {
            _path = HttpContext.Current.Server.MapPath(virtualPath);
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            if (!File.Exists(_path)) File.WriteAllText(_path, "[]");
        }
        public List<T> Load()
        {
            _lock.EnterReadLock();
            try { return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(_path)) ?? new List<T>(); }
            finally { _lock.ExitReadLock(); }
        }
        public void Save(IEnumerable<T> items)
        {
            _lock.EnterWriteLock();
            try { File.WriteAllText(_path, JsonConvert.SerializeObject(items, Formatting.Indented)); }
            finally { _lock.ExitWriteLock(); }
        }
    }
}