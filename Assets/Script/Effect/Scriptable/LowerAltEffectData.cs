using UnityEngine;

/// <summary>����G�t�F�N�g(�a��グ)�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "LowerAltEffectData", menuName = "ScriptableObjects/LowerAltEffectData")]

public class LowerAltEffectData : ScriptableObject
{
    /// <summary>�a���̕t������</summary>
    public SlashEnchantment _enchantment;

    /// <summary>����G�t�F�N�g(�E)</summary>
    public GameObject _rightAltEffect;

    /// <summary>����G�t�F�N�g(��)</summary>
    public GameObject _leftAltEffect;
}
