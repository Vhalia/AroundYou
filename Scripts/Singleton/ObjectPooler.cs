using System.Collections.Generic;

namespace AroundYou.Scripts.Singleton
{
    public class ObjectPooler
    {
        public Dictionary<string, object> Pool = new();

        public ObjectPooler _instance;
        public ObjectPooler Instance
        {
            get
            {
                _instance ??= new ObjectPooler();
                return _instance;
            }
        }

        /*public T Instantiate<T>(PackedScene scene) where T : class
        {
            if(Pool.TryGetValue(typeof(T).Name, out object existingInstance))
            {

            }
            else
            {
                Pool.Add(typeof(T).Name, scene.Instantiate<T>());
            }
        }*/
    }
}
