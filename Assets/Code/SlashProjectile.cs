using UnityEngine;

namespace Code
{
	public class SlashProjectile : MonoBehaviour
	{
		private float speed = 10;
		private float direction = 1f;

		void Update()
		{
			Vector3 move = new Vector3(direction, 0, 0f);
			transform.Translate(move * (speed * Time.deltaTime));
		}
	}
}