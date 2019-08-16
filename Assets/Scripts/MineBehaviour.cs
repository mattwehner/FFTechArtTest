using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MineBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _mineCap;
        
        [SerializeField]
        private float _blinkPerSecond = 1;
        
        
        [SerializeField]
        private GameObject _explosionPrefab;

        private AudioSource _beepSound;

        private float _detonationTimer = 1f;

        private void Start()
        {
            _detonationTimer = GameRules.Instance.MineDetonationTimer;
            _beepSound = gameObject.GetComponent<AudioSource>();

            StartCoroutine(BeginDetonationCountdown());
        }

        public IEnumerator BeginDetonationCountdown()
        {
            _beepSound.Play();

            //Flash Minecap while counting down
            float currentTimer = 0f;
            Material mineCap = _mineCap.GetComponent<Renderer>().material;
            bool capIsOn = false;
            float blinkRate = _blinkPerSecond / 2;

            while(currentTimer < _detonationTimer)
            {
                currentTimer += blinkRate;
                
                if (capIsOn)
                    mineCap.DisableKeyword("_EMISSION");
                else
                    mineCap.EnableKeyword("_EMISSION");

                capIsOn = !capIsOn;

                yield return new WaitForSeconds(blinkRate);
            }

            Detonate();
        }

        private void Detonate()
        {
            _beepSound.Stop();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameRules.Instance.ExplosionParent);
            Destroy(gameObject);
        }
    }
}
