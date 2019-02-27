using UnityEngine;

namespace Utilities
{
    public class PooledObject : MonoBehaviour
    {
        private ObjectPool parentPool;

        public ObjectPool ParentPool
        {
            set {
                parentPool = value;
            }
        }

        public void ReturnObject() {
            parentPool.ReturnObject(this.gameObject);
        }
    }
}