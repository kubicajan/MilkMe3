using UnityEngine;

namespace Helpers
{
    public class DestroyObject : MonoBehaviour
    {
        [SerializeField] private float timeToDestruction;

        private void Start()
        {
            Destroy(gameObject, timeToDestruction);
        }
    }
}
