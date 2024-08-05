using UnityEngine;

/// <summary>����G�t�F�N�g�̃f�[�^���܂Ƃ߂��N���X</summary>
[CreateAssetMenu(fileName = "AltEffectData", menuName = "ScriptableObjects/AltEffectData")]

public class AltEffectData : ScriptableObject
{
    /// <summary>�a���̕t������</summary>
    public SlashEnchantment _enchantment;

    /// <summary>����G�t�F�N�g(�E)</summary>
    public GameObject _rightAltEffect;

    /// <summary>����G�t�F�N�g(��)</summary>
    public GameObject _leftAltEffect;

}
