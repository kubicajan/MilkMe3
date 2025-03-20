using UnityEngine;

namespace Managers
{
	public class BackgroundManager : MonoBehaviour
	{
		[SerializeField] private GameObject cameraInput;

		private float startPos, length;

		public float parallaxEffect;

		void Start()
		{
			startPos = transform.position.x;
			length = GetComponent<SpriteRenderer>().bounds.size.x;
		}

		void Update()
		{
			float distance = cameraInput.transform.position.x * parallaxEffect;
			float movement = cameraInput.transform.position.x * (1 - parallaxEffect);
			transform.position = new Vector2(startPos + distance, transform.position.y);
			if (movement > startPos + length)
			{
				startPos += length;
			}
			else if (movement < startPos - length)
			{
				startPos -= length;
			}
		}
	}
}