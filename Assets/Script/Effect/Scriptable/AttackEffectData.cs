using UnityEngine;

/// <summary>�U���G�t�F�N�g�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "AttackEffectData", menuName = "ScriptableObjects/AttackEffectData")]

public class AttackEffectData : ScriptableObject
{
    /// <summary>�a���G�t�F�N�g(�E)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�E)")] public GameObject _rightSlashEffect;

    /// <summary>�a���G�t�F�N�g(��)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(��)")] public GameObject _leftSlashEffect;

    /// <summary>����G�t�F�N�g(�a��グ�E�E)</summary>
    [SerializeField, Header("����G�t�F�N�g(�a��グ�E�E)")] public GameObject _lowerRightAltEffect;

    /// <summary>����G�t�F�N�g(�a��グ�E��)</summary>
    [SerializeField, Header("����G�t�F�N�g(�a��グ�E��)")] public GameObject _lowerLeftAltEffect;

    /// <summary>����G�t�F�N�g(�a�艺���E�E)</summary>
    [SerializeField, Header("����G�t�F�N�g(�a�艺���E�E)")] public GameObject _upperRightAltEffect;

    /// <summary>����G�t�F�N�g(�a�艺���E��)</summary>
    [SerializeField, Header("����G�t�F�N�g(�a�艺���E��)")] public GameObject _upperLeftAltEffect;
}