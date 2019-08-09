using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
	[SerializeField]
	private float _startDelay = 1f;

	[SerializeField]
	private float _fadeTime = 1f;

	[SerializeField]
	private CanvasGroup _canvasGroup;

	void Start ()
	{
		if (_canvasGroup != null)
		{
			StartCoroutine(FadeUI());
		}
	}
	
	public IEnumerator FadeUI()
	{
		yield return new WaitForSeconds(_startDelay);

		float timer = 0f;
		while(timer < _fadeTime)
		{
			timer += Time.deltaTime;
			_canvasGroup.alpha = 1f - (timer / _fadeTime);
			yield return null;
		}

		_canvasGroup.alpha = 0f;
		_canvasGroup.gameObject.SetActive(false);
	}
}
