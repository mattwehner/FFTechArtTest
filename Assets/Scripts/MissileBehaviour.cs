using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour
{
	[SerializeField]
	private GameObject _explosionPrefab;

	private float _missileThrust = 0f;
	private Rigidbody _rigidbody;

	private void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_missileThrust = GameRules.Instance.MissileThrust;
	}

	void FixedUpdate()
	{
		PushRigidbody(Vector3.forward * _missileThrust);
	}

	private void PushRigidbody(Vector3 force)
	{
		_rigidbody.AddRelativeForce(force);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//Destroy the missiles whenever they collide with anything
		GameRules.Instance.MissileDestroyed();
		Instantiate(_explosionPrefab, transform.position, Quaternion.identity, GameRules.Instance.ExplosionParent);
		Destroy(gameObject);
	}
}
