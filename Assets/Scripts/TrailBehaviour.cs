using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class TrailBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float _fadeDuration = 5f;

        void Start ()
        {
            //TODO: Only if shader supports alpha
            StartCoroutine(BeginFading());
        }

        public IEnumerator BeginFading()
        {
            float currentTimer = 0f;
            Material material = gameObject.GetComponent<Renderer>().material;
            Color blendFrom = material.color;

            while(currentTimer < _fadeDuration)
            {
                currentTimer += Time.deltaTime;
                float progress = currentTimer / _fadeDuration;
                material.color = Color.Lerp(blendFrom, Color.clear, progress);

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
