using UnityEngine;

/// <summary>�a���G�t�F�N�g�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "SlashEffectData", menuName = "ScriptableObjects/SlashEffectData")]
public class SlashEffectData : ScriptableObject
{
    /// <summary>�a���̕t������</summary>
    public SlashEnchantment _enchantment;

    /// <summary>�a���G�t�F�N�g(�E)</summary>
    public GameObject _rightSlashEffect;

    /// <summary>�a���G�t�F�N�g(��)</summary>
    public GameObject _leftSlashEffect;
}
