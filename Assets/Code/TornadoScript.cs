using UnityEngine;

namespace Code
{
	public class TornadoScript : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D rigidBody;
		private float speed = 8;
		private float direction = 1f;

		public static void Initialize(TornadoScript prefab, Vector3 position, Quaternion rotation, float direction)
		{
			TornadoScript tornado = Instantiate(prefab, position, rotation);
			tornado.direction = direction;
		}

		private void Start()
		{
			rigidBody.linearVelocity = (transform.right * speed * direction);
		}
	}
}