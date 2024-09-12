using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>敵のスポーンを管理するクラス</summary>
public class Spawner : MonoBehaviour
{
    /// <summary>敵のスポーン地点のリスト</summary>
    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();

    /// <summary>地上の敵のリスト</summary>
    [SerializeField] private List<GameObject> _groundEnemyList = new List<GameObject>();

    /// <summary>空中の敵のリスト</summary>
    [SerializeField] private List<GameObject> _airEnemyList = new List<GameObject>();

    /// <summary>全ての敵のリスト</summary>
    [SerializeField] private List<GameObject> _allEnemyList = new List<GameObject>();

    /// <summary>シーン上に存在する敵の数</summary>
    private int _enemyCount = 0;

    /// <summary>ウェーブの完了フラグ</summary>
    private bool _isWaveCompleted = false;

    /// <summary>現在のウェーブ数</summary>
    private int _currentWaveCount = 0;

    private static Spawner _instance;
    public static Spawner Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        // 敵が全滅した場合
        if (_enemyCount == 0 && !_isWaveCompleted)
        {
            _isWaveCompleted = true;

            StartCoroutine(StartNewWave());
        }
    }

    /// <summary>ウェーブの開始処理</summary>
    private IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(3f);

        // ウェーブ数を増加させる
        _currentWaveCount++;

        if (_currentWaveCount > _spawnPoints.Count) yield break;

        // スポーン地点のリストをコピー
        var spawnList = new List<GameObject>(_spawnPoints);

        for(int i = 0; i < _currentWaveCount; i++)
        {
            int index = Random.Range(0, spawnList.Count);
            var spawnPoint = spawnList[index];
            spawnList.RemoveAt(index);
            spawnPoint.GetComponent<SpawnPoint>().EnableSpawnEffectTemporarily();
            SpawnEnemy(spawnPoint.transform);
            _enemyCount++;
        }

        _isWaveCompleted = false;
    }

    /// <summary>敵のスポーン処理</summary>
    private void SpawnEnemy(Transform spawnPoint)
    {
        // ウェーブ数が1～3
        if (_currentWaveCount <= 3)
        {
            CreateEnemy(_groundEnemyList, spawnPoint);
        }
        // ウェーブ数が4～6
        else if (_currentWaveCount <= 6)
        {
            CreateEnemy(_airEnemyList, spawnPoint);
        }
        // ウェーブ数が7～
        else
        {
            CreateEnemy(_allEnemyList, spawnPoint);
        }
    }

    /// <summary>敵の生成処理</summary>
    /// <param name="enemyList">生成したい敵が含まれるリスト</param>
    private void CreateEnemy(List<GameObject> enemyList, Transform spawnPoint)
    {
        Instantiate(enemyList[Random.Range(0, enemyList.Count)], spawnPoint.position,
            Quaternion.identity, transform);
    }

    /// <summary>敵の数を減少させる</summary>
    public void DecreaseEnemyCount()
    {
        _enemyCount--;
    }
}
