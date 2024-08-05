using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>攻撃エフェクトデータのリスト</summary>
    [SerializeField, Header("攻撃エフェクトデータのリスト")]
        private List<AttackEffectData> _attackEffectList = new List<AttackEffectData>();

    /// <summary>斬撃エフェクトを生成して表示するメソッド</summary>
    /// <param name="position">生成する位置</param>
    /// <param name="hand">攻撃に使用する手を表す列挙型</param>
    public void CreateSlashEffect(Vector3 position, AttackHand hand)
    {
        switch(hand)
        {
            // 右手
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    position, Quaternion.identity, transform); break;

            // 左手
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    position, Quaternion.identity, transform); break;

            // 両手の場合は、右と左のエフェクトを両方生成する
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    position, Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    position, Quaternion.identity, transform); break;

            // 例外的な処理
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    
}
