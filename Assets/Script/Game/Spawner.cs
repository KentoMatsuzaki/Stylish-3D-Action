using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�G�̃X�|�[�����Ǘ�����N���X</summary>
public class Spawner : MonoBehaviour
{
    /// <summary>�G�̃X�|�[���n�_�̃��X�g</summary>
    [SerializeField] private List<GameObject> _spawnPoints = new List<GameObject>();

    /// <summary>�n��̓G�̃��X�g</summary>
    [SerializeField] private List<GameObject> _groundEnemyList = new List<GameObject>();

    /// <summary>�󒆂̓G�̃��X�g</summary>
    [SerializeField] private List<GameObject> _airEnemyList = new List<GameObject>();

    /// <summary>�S�Ă̓G�̃��X�g</summary>
    [SerializeField] private List<GameObject> _allEnemyList = new List<GameObject>();

    /// <summary>�V�[����ɑ��݂���G�̐�</summary>
    private int _enemyCount = 0;

    /// <summary>�E�F�[�u�̊����t���O</summary>
    private bool _isWaveCompleted = false;

    /// <summary>���݂̃E�F�[�u��</summary>
    private int _currentWaveCount = 0;

    private static Spawner _instance;
    public static Spawner Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {
        // �G���S�ł����ꍇ
        if (_enemyCount == 0 && !_isWaveCompleted)
        {
            _isWaveCompleted = true;

            StartCoroutine(StartNewWave());
        }
    }

    /// <summary>�E�F�[�u�̊J�n����</summary>
    private IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(3f);

        // �E�F�[�u���𑝉�������
        _currentWaveCount++;

        if (_currentWaveCount > _spawnPoints.Count) yield break;

        // �X�|�[���n�_�̃��X�g���R�s�[
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

    /// <summary>�G�̃X�|�[������</summary>
    private void SpawnEnemy(Transform spawnPoint)
    {
        // �E�F�[�u����1�`3
        if (_currentWaveCount <= 3)
        {
            CreateEnemy(_groundEnemyList, spawnPoint);
        }
        // �E�F�[�u����4�`6
        else if (_currentWaveCount <= 6)
        {
            CreateEnemy(_airEnemyList, spawnPoint);
        }
        // �E�F�[�u����7�`
        else
        {
            CreateEnemy(_allEnemyList, spawnPoint);
        }
    }

    /// <summary>�G�̐�������</summary>
    /// <param name="enemyList">�����������G���܂܂�郊�X�g</param>
    private void CreateEnemy(List<GameObject> enemyList, Transform spawnPoint)
    {
        Instantiate(enemyList[Random.Range(0, enemyList.Count)], spawnPoint.position,
            Quaternion.identity, transform);
    }

    /// <summary>�G�̐�������������</summary>
    public void DecreaseEnemyCount()
    {
        _enemyCount--;
    }
}
