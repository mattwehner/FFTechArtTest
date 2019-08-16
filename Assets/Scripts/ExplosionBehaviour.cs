using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class ExplosionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ExplosionType _explosionType;
        
        private AudioSource _explosionSound;

        //Explosion Colors
        private Color32 _startingColor;
        private readonly Color32 _endingColor = Color.gray;

        private Material _material;
        private float _expansionTime = 1f;
        private float _expansionSize = 1f;

        private enum ExplosionType
        {
            Mine,
            Missile
        }

        void Start ()
        {
            if (_explosionType == ExplosionType.Mine)
            {
                _expansionTime = GameRules.Instance.MineExplosionTime;
                _expansionSize = GameRules.Instance.MineExplosionSize;
            }
            else if(_explosionType == ExplosionType.Missile)
            {
                _expansionTime = GameRules.Instance.MissileExplosionTime;
                _expansionSize = GameRules.Instance.MissileExplosionSize;
            }

            _material = gameObject.GetComponent<Renderer>().material;
            _startingColor = _material.color;
            _explosionSound = gameObject.GetComponent<AudioSource>();

            //Retimes audio so it lasts as long as explosion
            _explosionSound.pitch = _explosionSound.clip.length / _expansionTime;

            StartCoroutine(BeginExplosionExpansion());
        }


        public IEnumerator BeginExplosionExpansion()
        {
            float currentCuttoff = 0.2f;
            float currentTimer = 0f;

            _explosionSound.Play();
            while(currentTimer < _expansionTime)
            {
                currentTimer += Time.deltaTime;
                float explosionProgress = (currentTimer / _expansionTime);

                //Make Material Dissapear
                float newCuttoff = (currentCuttoff + explosionProgress) * 0.8f;
                _material.SetFloat("_Cutoff", newCuttoff);

                //Transition Color
                _material.color = Color.Lerp(_startingColor,_endingColor, explosionProgress);

                //Expand Explosion
                transform.localScale = Vector3.one * _expansionSize * explosionProgress;

                yield return null;
            }

            transform.localScale = Vector3.one * _expansionSize;

            _explosionSound.Stop();
            Destroy(gameObject);
        }

    }
}
