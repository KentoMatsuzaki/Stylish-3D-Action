using UnityEngine;

/// <summary>����G�t�F�N�g(�a�艺��)�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "UpperAltEffectData", menuName = "ScriptableObjects/UpperAltEffectData")]

public class UpperAltEffectData : ScriptableObject
{
    /// <summary>�a���̕t������</summary>
    public SlashEnchantment _enchantment;

    /// <summary>����G�t�F�N�g(�E)</summary>
    public GameObject _rightAltEffect;

    /// <summary>����G�t�F�N�g(��)</summary>
    public GameObject _leftAltEffect;
}
