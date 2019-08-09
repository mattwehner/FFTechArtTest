using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquishyBehaviour : MonoBehaviour
{
	[SerializeField]
	private GameObject _minePrefab;

	//Mouse position tracking
	private Vector3 _mousePositionStart = Vector3.zero;
	private bool _mouseButtonHeld = false;
	
	//Movement
	private Rigidbody _rigidbody;
	private float _jumpForce = 0f;
	
	void Start ()
	{
		_jumpForce = GameRules.Instance.SquishyJumpForce;
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	void Update ()
	{
		CheckInputs();
	}

	private void CheckInputs()
	{
		if (Input.GetMouseButtonDown(0))
		{
			_mousePositionStart = Input.mousePosition;
		}

		//Get mouse up and make sure that the game hasn't ended yet
		if (Input.GetMouseButtonUp(0) && !GameRules.Instance.HasGameEnded)
		{
			//Get mouse drag delta from screenspace (x, y), and convert the vector to match our game alignment (x, z)
			Vector3 mouseDelta = Input.mousePosition - _mousePositionStart;
			mouseDelta = new Vector3(mouseDelta.x, 0f, mouseDelta.y);

			//Spawn a mine at squishy's current location
			Instantiate(_minePrefab, transform.position, Quaternion.identity, GameRules.Instance.MineParent);

			PushRigidbody(mouseDelta.normalized * _jumpForce);
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
			//Squishy has hit or been hit by a missile or explosion
			GameRules.Instance.GameOver();
		}
	}
}
