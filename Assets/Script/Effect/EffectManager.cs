using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>攻撃エフェクトを生成する際の高さのオフセット</summary>
    [SerializeField] private float _attackEffectHeightOffset = 1.25f;

    /// <summary>攻撃エフェクトデータのリスト</summary>
    [SerializeField, Header("攻撃エフェクトデータのリスト")]
        private List<AttackEffectData> _attackEffectList = new List<AttackEffectData>();

    /// <summary>斬撃エフェクトを生成して表示するメソッド</summary>
    /// <param name="hand">攻撃に使用する手を表す列挙型</param>
    public void CreateSlashEffect(AttackHand hand)
    {
        switch(hand)
        {
            // 右手
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 左手
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 両手の場合は、右と左のエフェクトを両方生成する
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 例外的な処理
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    /// <summary>特殊エフェクト（斬り上げ）を生成して表示するメソッド</summary>
    /// <param name="hand">攻撃に使用する手を表す列挙型</param>
    public void CreateLowerAltEffect(AttackHand hand)
    {
        switch (hand)
        {
            // 右手
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 左手
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 両手の場合は、右と左のエフェクトを両方生成する
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 例外的な処理
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    /// <summary>特殊エフェクト（斬り下げ）を生成して表示するメソッド</summary>
    /// <param name="hand">攻撃に使用する手を表す列挙型</param>
    public void CreateUpperAltEffect(AttackHand hand)
    {
        switch (hand)
        {
            // 右手
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 左手
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 両手の場合は、右と左のエフェクトを両方生成する
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // 例外的な処理
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    /// <summary>攻撃エフェクトを生成する位置を求める</summary>
    private Vector3 GetAttackEffectPosition()
    {
        // プレイヤーの位置にオフセットを加えた位置を求めて返す
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 effectPos = new Vector3(playerPos.x, playerPos.y + _attackEffectHeightOffset, playerPos.z);
        return effectPos;
    }
}
