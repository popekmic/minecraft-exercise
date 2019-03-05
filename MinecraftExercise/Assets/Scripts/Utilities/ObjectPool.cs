using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities
{
    public class ObjectPool
    {
        private readonly GameObject pooledObject;
        private readonly Queue<GameObject> freeObjects = new Queue<GameObject>();


        public ObjectPool(GameObject pooledObject, int initialCapacity)
        {
            this.pooledObject = pooledObject;
            for (int i = 0; i < initialCapacity; i++)
            {
                CreateObject();
            }
        }

        public GameObject GetObject()
        {
            if (freeObjects.Count == 0)
            {
                CreateObject();
            }

            GameObject returnValue = freeObjects.Dequeue();
            //returnValue.SetActive(true);
            return returnValue;
        }

        private void CreateObject()
        {
            GameObject newObject = Object.Instantiate(pooledObject);
            PooledObject pooledComponent = newObject.AddComponent<PooledObject>();
            pooledComponent.ParentPool = this;
            //newObject.SetActive(false);
            freeObjects.Enqueue(newObject);
        }

        public void ReturnObject(GameObject gameObject)
        {
            freeObjects.Enqueue(gameObject);
            //gameObject.SetActive(false);
        }

        public void DestroyObjects()
        {
            foreach (var freeObject in freeObjects)
            {
                Object.Destroy(freeObject);
            }
        }
    }
}