using UnityEngine;

/// <summary>攻撃エフェクトの基底クラス</summary>
public abstract class AttackEffectBase : MonoBehaviour
{
    protected virtual void Start()
    {
        // プレイヤーの正面方向へ回転させる
        transform.rotation = Quaternion.LookRotation(Player.Instance.transform.forward);
    }

    /// <summary>派生クラスでオーバーライドする抽象メソッド</summary>
    protected abstract void UniqueAction();
}
