using UnityEngine;

/// <summary>�a���G�t�F�N�g</summary>
public class SlashEffect : AttackEffectBase
{
    [SerializeField] bool _isRightEffect = true;
    protected override void Start()
    {
        // ���N���X��Start���\�b�h���Ăяo��
        base.Start();

        UniqueAction();
    }

    /// <summary>�a���G�t�F�N�g�ŗL�̏���</summary>
    protected override void UniqueAction()
    {
        if (_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180f);
        }
    }
}
