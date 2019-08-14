using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class MineBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject _explosionPrefab;

        private float _detonationTimer = 1f;

        private void Start()
        {
            _detonationTimer = GameRules.Instance.MineDetonationTimer;
            StartCoroutine(BeginDetonationCountdown());
        }

        public IEnumerator BeginDetonationCountdown()
        {
            yield return new WaitForSeconds(_detonationTimer);
            Detonate();
        }

        private void Detonate()
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameRules.Instance.ExplosionParent);
            Destroy(gameObject);
        }
    }
}
