using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fantasy3D
{
    public enum EnemyType
    {
        Monster,
        Zombie
    }
    [System.Serializable]
    public struct EnemyPrefabPair
    {
        public EnemyType EnemyType;
        public GameObject prefab;
    }
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyPrefabPair[] _enemyPrefabPairs;
        [SerializeField] float _spawnInterval = 1f;
        [SerializeField] int _maxSpawnCount = 5;
        Dictionary<EnemyType, GameObject> _enemyPrefabMap;

        int _currentSpawnCount = 0;
        bool _canSpawn = true;

        Vector3 _spawnAreaMin = new Vector3(-25f, 0f, -25f);
        Vector3 _spawnAreaMax = new Vector3(25f, 0f, 25f);

        private void Awake()
        {
            _spawnAreaMin = (Vector3)transform.position + _spawnAreaMin;
            _spawnAreaMax = (Vector3)transform.position + _spawnAreaMax;
        }

        private void Start()
        {
            _enemyPrefabMap = new Dictionary<EnemyType, GameObject>();
            foreach(var pair in _enemyPrefabPairs)
            {
                _enemyPrefabMap.Add(pair.EnemyType, pair.prefab);
            }
            StartCoroutine(SpawnEnemy());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position;

            Vector3 size = new Vector3(
                _spawnAreaMax.x - _spawnAreaMin.x, 
                0,
                _spawnAreaMax.z - _spawnAreaMin.z);

            Gizmos.DrawWireCube(center, size);
        }
         
        IEnumerator SpawnEnemy()
        {
            while (_canSpawn)
            {
                if(_currentSpawnCount >= _maxSpawnCount)
                {
                    yield return null;
                    continue;
                }

                EnemyPrefabPair app = _enemyPrefabPairs[Random.Range(0, _enemyPrefabPairs.Length)];
                EnemyType randomType = app.EnemyType;

                if (!_enemyPrefabMap.TryGetValue(randomType, out GameObject prefab))
                {
                    Debug.LogError($"prefab not found for {randomType}");
                    yield return null;
                }
                Vector3 spawnPosition = new Vector3(
                    Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
                    0.1f,
                    Random.Range(_spawnAreaMin.z, _spawnAreaMax.z)
                    );
                Instantiate(prefab, spawnPosition, Quaternion.identity, this.transform);
                _currentSpawnCount++;
                yield return new WaitForSeconds(_spawnInterval);
            }
        }


    }
}
