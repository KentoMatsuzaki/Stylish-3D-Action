using UnityEngine;

/// <summary>�a���G�t�F�N�g�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "SlashEffectData", menuName = "ScriptableObjects/SlashEffectData")]

public class SlashEffectData : ScriptableObject
{
    /// <summary>�a���G�t�F�N�g(�����E�E)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�����E�E)")] public GameObject _horizontalRightSlashEffect;

    /// <summary>�a���G�t�F�N�g(�����E��)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�����E��)")] public GameObject _horizontalLeftSlashEffect;

    /// <summary>�a���G�t�F�N�g(�a��グ�E�E)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�a��グ�E�E)")] public GameObject _lowerRightSlashEffect;

    /// <summary>�a���G�t�F�N�g(�a��グ�E��)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�a��グ�E��)")] public GameObject _lowerLeftSlashEffect;

    /// <summary>�a���G�t�F�N�g(�a�艺���E�E)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�a�艺���E�E)")] public GameObject _upperRightSlashEffect;

    /// <summary>�a���G�t�F�N�g(�a�艺���E��)</summary>
    [SerializeField, Header("�a���G�t�F�N�g(�a�艺���E��)")] public GameObject _upperLeftSlashEffect;
}