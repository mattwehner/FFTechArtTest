using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts
{
    public class SquishyBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _minePrefab;

        [SerializeField]
        private GameObject _trailPrefab;

        //Mouse position tracking
        private Vector3 _mousePositionStart = Vector3.zero;
	
        //Movement
        private Rigidbody _rigidbody;
        private float _jumpForce = 0f;

        //Slime Trail
        private GameObject _currentTrail;
        private Vector3 _trailStartPosition;
	
        //Animation
        private Animator _animator;
        private string _lurchAnimation = "SquishyLunge";

        //Material
        [SerializeField]
        private Material _material;

        private enum Expressions
        {
            Smile,
            Frown
        }

        void Start ()
        {
            _jumpForce = GameRules.Instance.SquishyJumpForce;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            ChangeExpression(Expressions.Smile);
        }

        void Update ()
        {
            if (GameRules.Instance.HasGameStarted)
            {
                CheckInputs();
            }

            if (_currentTrail !=null && _rigidbody.velocity.magnitude > 0)
            {
                Vector3 currentPosition = transform.position;
                float distance = Vector3.Distance(_trailStartPosition, currentPosition);
                _currentTrail.transform.localScale = new Vector3(1, 1, distance * 10);
            }
        }

        private void CheckInputs()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mousePositionStart = Input.mousePosition.ToWorldPosition(_camera);
            }

            //Get mouse up and make sure that the game hasn't ended yet
            if (Input.GetMouseButtonUp(0) && !GameRules.Instance.HasGameEnded)
            {
                Vector3 mousePosition = Input.mousePosition.ToWorldPosition(_camera);

                //Convert the vector to match our game alignment (x, z)
                Vector3 mouseDelta = mousePosition - _mousePositionStart;
                mouseDelta = new Vector3(mouseDelta.x, 0f, mouseDelta.y);

                //Rotate squishy
                transform.LookAt(mouseDelta);
                _animator.Play(_lurchAnimation);
                PushRigidbody(transform.forward * _jumpForce);

                _trailStartPosition = transform.position;
                _currentTrail = Instantiate(_trailPrefab, _trailStartPosition, transform.rotation, GameRules.Instance.TrailParent).gameObject;

                //Spawn a mine at squishy's current location
                Instantiate(_minePrefab, transform.position, Quaternion.identity, GameRules.Instance.MineParent);
            }
        }

        private void PushRigidbody(Vector3 force)
        {
            _rigidbody.AddForce(force);
        }
	
        private void OnCollisionEnter(Collision collision)
        {
            //Check the objects we've collided with for specific components
            MissileBehaviour missile = collision.collider.GetComponent<MissileBehaviour>();
            ExplosionBehaviour explosion = collision.collider.GetComponent<ExplosionBehaviour>();

            if ((missile != null || explosion != null) && !GameRules.Instance.HasGameEnded)
            {
                ChangeExpression(Expressions.Frown);
                GameRules.Instance.GameOver(collision);
            }
        }
        
        private void ChangeExpression(Expressions expression)
        {
            Vector2 offset = new Vector2();
            switch (expression)
            {
                case Expressions.Smile:
                    offset.x = 0f;
                    break;
                case Expressions.Frown:
                    offset.x = 0.5f;
                    break;
            }

            _material.mainTextureOffset = offset;
        }
    }
}
