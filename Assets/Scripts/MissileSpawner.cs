using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _missileSpawnPrefab;
	[SerializeField]
	private Transform _missileSpawnPoint;

	//Store these variables internally
	private float _missileSpawnTimer = 2f;
	private int _missileSpawnCount = 10;

	void Start ()
	{
		_missileSpawnTimer = GameRules.Instance.MissileSpawnTimer;
		_missileSpawnCount = GameRules.Instance.MissileSpawnCount;

		StartCoroutine(BeginSpawningMissiles());
	}

	public IEnumerator BeginSpawningMissiles()
	{
		int missileCounter = 0;
		while(missileCounter < _missileSpawnCount)
		{
			yield return new WaitForSeconds(_missileSpawnTimer);
			Instantiate(_missileSpawnPrefab, _missileSpawnPoint.position, Quaternion.identity, GameRules.Instance.MissileParent);
			missileCounter++;
		}
	}
}
