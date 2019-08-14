using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject _missileSpawnPrefab;
	[SerializeField]
	private Transform _missileSpawnPoint;
    [SerializeField]
	private GameObject _missileParent;

    public static MissileSpawner Instance;

	//Store these variables internally
	private float _missileSpawnTimer = 2f;
	private int _missileSpawnCount = 10;

	void Start ()
    {
        Instance = this;
		_missileSpawnTimer = GameRules.Instance.MissileSpawnTimer;
		_missileSpawnCount = GameRules.Instance.MissileSpawnCount;
    }

	public IEnumerator SpawnMissiles()
	{
		int missileCounter = 0;
		while(missileCounter < _missileSpawnCount)
		{
			yield return new WaitForSeconds(_missileSpawnTimer);
			Instantiate(_missileSpawnPrefab, _missileSpawnPoint.position, Quaternion.identity, GameRules.Instance.MissileParent);
			missileCounter++;
		}
	}

    public void Cleanup()
    {
        StopSpawning();
        _missileParent.SetActive(false);
        foreach (Transform missile in _missileParent.transform)
        {
            missile.GetComponent<MissileBehaviour>().Destroy();
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnMissiles());
    }
    
    public void StopSpawning()
    {
        StopCoroutine(SpawnMissiles());
    } 
}
