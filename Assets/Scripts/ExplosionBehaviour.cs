using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ExplosionBehaviour : MonoBehaviour
    {
        [SerializeField]
        private ExplosionType _explosionType;

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
            StartCoroutine(BeginExplosionExpansion());
        }

        public IEnumerator BeginExplosionExpansion()
        {
            float currentTimer = 0f;
            while(currentTimer < _expansionTime)
            {
                currentTimer += Time.deltaTime;
                transform.localScale = Vector3.one * _expansionSize * (currentTimer / _expansionTime);
                yield return null;
            }

            transform.localScale = Vector3.one * _expansionSize;

            yield return null;

            Destroy(gameObject);
        }

    }
}
