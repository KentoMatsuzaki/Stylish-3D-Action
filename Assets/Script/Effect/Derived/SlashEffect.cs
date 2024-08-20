using UnityEngine;

/// <summary>�a���G�t�F�N�g</summary>
public class SlashEffect : AttackEffectBase
{
    /// <summary>�E�����̃G�t�F�N�g���ǂ���</summary>
    [SerializeField] bool _isRightEffect = true;

    /// <summary>�a���G�t�F�N�g�ŗL�̏���</summary>
    protected override void UniqueAction()
    {
        if (_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180f);
        }
    }
}
