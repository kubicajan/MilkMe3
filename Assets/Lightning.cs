using System.Collections;
using Helpers;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	[SerializeField] private Transform endPoint;
	LineRenderer line;

	void Awake()
	{
		const float REFRESH_RATE = 2f;
		line = GetComponent<LineRenderer>();
		InvokeRepeating(nameof(UpdateLightning), 2f, REFRESH_RATE);
	}

	void UpdateLightning()
	{
		const int SEGMENTS = 12;
		const float JAGGEDNESS = 0.5f;
		line.positionCount = SEGMENTS;

		Vector2 start = new Vector2(endPoint.position.x, endPoint.position.y + 10);
		Vector2 end = Utility.GetGroundBelowLocation(endPoint.position);
		Vector2 direction = (end - start);
		Vector2 normal = new Vector2(-direction.y, direction.x).normalized;

		for (int i = 0; i < SEGMENTS; i++)
		{
			float t = i / (float)(SEGMENTS - 1);
			Vector2 pos = Vector2.Lerp(start, end, t);

			float offset = Random.Range(-JAGGEDNESS, JAGGEDNESS);
			pos += normal * offset;

			line.SetPosition(i, pos);
		}

		StartCoroutine(FadeLightning());
	}

	private IEnumerator FadeLightning()
	{
		yield return new WaitForSeconds(0.3f);
		float fadeDuration = 1f;
		float elapsed = 0f;
		Color startColor = line.startColor;
		Color endColor = line.endColor;

		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
			line.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
			line.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);
			yield return null;
		}

		line.startColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
		line.endColor = new Color(endColor.r, endColor.g, endColor.b, 0f);
	}
}