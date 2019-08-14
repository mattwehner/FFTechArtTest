using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UIFader : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        [SerializeField]
        private string _startingMessage = "Ready?";

        [SerializeField]
        private float _countdownDelay = 3f;

        [SerializeField]
        private float _countdown = 3f;

        [SerializeField]
        private Text _canvasText;

        private void Start()
        {
            if (_canvasGroup != null && _canvasText != null)
            {
                StartCoroutine(Countdown());
            }
            else
            {
                Debug.LogError("Unassigned Variable in the Inspector Window");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        public IEnumerator Countdown()
        {
            _canvasText.text = _startingMessage;

            yield return new WaitForSeconds(_countdownDelay);

            float timer = 0f;
            while (timer < _countdown)
            {
                timer += Time.deltaTime;
                
                var countdownText = Math.Ceiling(_countdown - timer)
                    .ToString(CultureInfo.InvariantCulture);
                _canvasText.text = countdownText;

                yield return null;
            }

            yield return new WaitForSeconds(1f);
            _canvasGroup.gameObject.SetActive(false);
            GameRules.Instance.BeginLevel();
        }
    }
}