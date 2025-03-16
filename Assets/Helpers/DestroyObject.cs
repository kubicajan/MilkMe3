using UnityEngine;

namespace Helpers
{
    public class DestroyObject : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 3f);
        }
    }
}
