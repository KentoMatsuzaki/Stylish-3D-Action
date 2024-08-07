using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>斬撃エフェクトを生成する際の高さのオフセット</summary>
    [SerializeField] private float _slashEffectHeightOffset = 1.25f;

    /// <summary>斬撃エフェクトデータのリスト</summary>
    [SerializeField, Header("斬撃エフェクトデータのリスト")]
        private List<SlashEffectData> _slashEffectDataList = new List<SlashEffectData>();

    //-------------------------------------------------------------------------------
    // 斬撃エフェクトを生成する処理
    //-------------------------------------------------------------------------------

    /// <summary>水平の斬撃エフェクトを生成する共通メソッド</summary>
    public void CreateHorizontalSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Horizontal, hand);

        effects.ForEach(effect => CreateEffect(effect, GetAttackEffectPosition()));
    }

    /// <summary>斬り上げの斬撃エフェクトを生成する共通メソッド</summary>
    public void CreateLowerSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Lower, hand);

        effects.ForEach(effect => CreateEffect(effect, GetAttackEffectPosition()));
    }

    /// <summary>斬り下げの斬撃エフェクトを生成する共通メソッド</summary>
    public void CreateUpperSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Upper, hand);

        effects.ForEach(effect => CreateEffect(effect, GetAttackEffectPosition()));
    }

    //-------------------------------------------------------------------------------
    // 斬撃エフェクトの生成に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>エフェクトを生成する共通メソッド</summary>
    /// <param name="effectPrefab">エフェクトのプレハブ</param>
    /// <param name="position">生成位置</param>
    private void CreateEffect(GameObject effectPrefab, Vector3 position)
    {
        Instantiate(effectPrefab, position, Quaternion.identity, transform);
    }

    /// <summary>攻撃エフェクトを生成する位置を求める</summary>
    private Vector3 GetAttackEffectPosition()
    {
        // プレイヤーの位置にオフセットを加えた位置を求めて返す
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 effectPos = new Vector3(playerPos.x, playerPos.y + _slashEffectHeightOffset, playerPos.z);
        return effectPos;
    }

    /// <summary>斬撃エフェクトを取得する</summary>
    /// <returns>斬撃エフェクトのプレハブのリスト</returns>
    private List<GameObject> GetSlashEffect(SlashType type, AttackHand hand)
    {
        // プレイヤーがもつ斬撃の属性を基に、斬撃エフェクトのデータを取得する
        var data = _slashEffectDataList[(int)Player.Instance.Enchantment];

        // 斬撃エフェクトを格納するリストを作成する
        var effects = new List<GameObject>();

        switch(type)
        {
            case SlashType.Horizontal:

                if(IsRightHandSlash(hand)) effects.Add(data._horizontalRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._horizontalLeftSlashEffect); break;

            case SlashType.Lower:

                if(IsRightHandSlash(hand)) effects.Add(data._lowerRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._lowerLeftSlashEffect); break;

            case SlashType.Upper:

                if(IsRightHandSlash(hand)) effects.Add(data._upperRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._upperLeftSlashEffect); break;   
        }

        return effects;
    }

    /// <summary>右手による斬撃かどうか</summary>
    /// <returns>true = 右手・両手，false = 左手</returns>
    private bool IsRightHandSlash(AttackHand hand)
    {
        if(hand == AttackHand.Right ||  hand == AttackHand.Both) return true;
        else return false;
    }

    /// <summary>左手による斬撃かどうか</summary>
    /// <returns>true = 左手・両手, false = 右手</returns>
    private bool IsLeftHandSlash(AttackHand hand)
    {
        if (hand == AttackHand.Left || hand == AttackHand.Both) return true;
        else return false;
    }
}