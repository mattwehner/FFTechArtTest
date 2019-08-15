﻿using Assets.Scripts.Extensions;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SquishyBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private GameObject _minePrefab;

        //Mouse position tracking
        private Vector3 _mousePositionStart = Vector3.zero;
	
        //Movement
        private Rigidbody _rigidbody;
        private float _jumpForce = 0f;
	
        //Animation
        private Animator _animator;
        private string _lurchAnimation = "SquishyLurch";

        //Material
        [SerializeField]
        private Material _material;

        void Start ()
        {
            _jumpForce = GameRules.Instance.SquishyJumpForce;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();

            //Start Squishy Smiling
            _material.mainTextureOffset = new Vector2(0, 0);
        }
	
        void Update ()
        {
            if (GameRules.Instance.HasGameStarted)
            {
                CheckInputs();
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

                //Rotate and move squishy
                transform.LookAt(mouseDelta);
                _animator.Play(_lurchAnimation);
                PushRigidbody(transform.forward * _jumpForce);

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
                //Switch Squishy to frown
                _material.mainTextureOffset = new Vector2(0.5f, 0);

                //Squishy has hit or been hit by a missile or explosion
                GameRules.Instance.GameOver(collision);
            }
        }
    }
}
