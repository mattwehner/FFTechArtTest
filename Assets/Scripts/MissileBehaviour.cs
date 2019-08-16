using UnityEngine;

namespace Assets.Scripts
{
    public class MissileBehaviour : MonoBehaviour, IExplodable
    {
        [SerializeField]
        private GameObject _explosionPrefab;
    
        private float _missileThrust = 0f;
        private float _missileAccuracy = 0f;
        private Rigidbody _rigidbody;
        private GameObject _squishyInstance;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _missileThrust = GameRules.Instance.MissileThrust;
            _missileAccuracy = GameRules.Instance.MissileAccuracy;
            _squishyInstance = FindObjectOfType<SquishyBehaviour>().gameObject;
        }

        void FixedUpdate()
        {
            PushRigidbody(Vector3.forward * _missileThrust);
            TrackTarget(_squishyInstance);
        }

        private void PushRigidbody(Vector3 force)
        {
            _rigidbody.AddRelativeForce(force);
        }

        private void TrackTarget (GameObject target)
        {
            Vector3 dir = _squishyInstance.transform.position - transform.position;
            dir.y = 0;
            Quaternion rotation = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _missileAccuracy * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            //Destroy if collision wasn't with squishy
            SquishyBehaviour squishy = collision.collider.GetComponent<SquishyBehaviour>();
            if (squishy == null) Explode();
        }

        public void Explode()
        {
            GameRules.Instance.MissileDestroyed();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameRules.Instance.ExplosionParent);
            Destroy(gameObject);
        }
    }
}