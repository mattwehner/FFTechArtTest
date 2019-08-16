using System.Collections;
using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts
{
    public class MissileBehaviour : MonoBehaviour, IExplodable
    {
        [SerializeField]
        private GameObject _explosionPrefab;
    
        [Header("Trail Behaviour")]
        [SerializeField]
        private Material _disabledBy;
        [SerializeField]
        private float _effectedByTrail = 1f;
    
        private float _missileThrust = 0f;
        private float _missileAccuracy = 0f;
        private float _missileDifficulty = 1f;
        private Rigidbody _rigidbody;
        private GameObject _squishyInstance;

        private bool _inSlime = false;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _missileThrust = GameRules.Instance.MissileThrust;
            _missileAccuracy = GameRules.Instance.MissileAccuracy;
            _missileDifficulty = GameRules.Instance.MissileDifficulty;
            _squishyInstance = FindObjectOfType<SquishyBehaviour>().gameObject;
        }

        void FixedUpdate()
        {
            PushRigidbody(Vector3.forward * _missileThrust);

            if (!_inSlime)
            {
                StopCoroutine(PreventTracking());
                TrackTarget(_squishyInstance);
            }
        }

        private void PushRigidbody(Vector3 force)
        {
            _rigidbody.AddRelativeForce(force);
        }

        private void TrackTarget (GameObject target)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir);

            //Increase Accuracy the more missiles have been destroyed
            float accuracy = (_missileAccuracy * Time.deltaTime) + _missileDifficulty;
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, accuracy);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Destroy if collision wasn't with squishy
            SquishyBehaviour squishy = collision.collider.GetComponent<SquishyBehaviour>();
            if (squishy == null) Explode();
        }

        void OnTriggerEnter(Collider collider)
        {
            TrailBehaviour trail = collider.GetComponent<TrailBehaviour>();
            if (trail != null)
            {
                StartCoroutine(PreventTracking());
            }
        }

        public IEnumerator PreventTracking()
        {
            _inSlime = true;

            yield return new WaitForSeconds(_effectedByTrail);

            _inSlime = false;
        }

        public void Explode()
        {
            GameRules.Instance.MissileDestroyed();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameRules.Instance.ExplosionParent);
            Destroy(gameObject);
        }
    }
}