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
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手、2が両手）</param>
    public void CreateSlashEffect(Vector3 position, int handIndex)
    {
        Instantiate(_attackEffectList[(int)Player.Instance.Enchantment])

        // indexの値が正常である場合
        if (index >= 0 && index < _slashEffectList.Count)
        {
            switch (handIndex)
            {
                // 右手
                case 0:
                    // 右向きのエフェクトを生成し、正しい方向に回転させる
                    var rightEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);
                    break;

                // 左手
                case 1:
                    // 左向きのエフェクトを生成し、正しい方向に回転させる
                    var leftEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);
                    break;

                // 両手
                case 2:
                    // 斜め右向きのエフェクトを生成し、正しい方向に回転させる
                    var diagonalRightEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);

                    // 斜め左向きのエフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);

                    break;

                // 例外的な処理
                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }

        // 不正なindexである場合
        else
        {
            Debug.LogError($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>必殺技エフェクトを生成・表示する</summary>
    /// <param name="type">エフェクトの属性</param>
    /// <param name="pos">エフェクトの生成位置</param>
    /// <param name="player">プレイヤーの位置</param>
    public void PlayUltEffect(SlashEnchantment type, Vector3 pos, Transform player)
    {
        if (!_ultEffectIndexMap.TryGetValue((type), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // indexの値が正常である場合、エフェクトを生成してプレイヤーの方向に合わせる
        if (index >= 0 && index < _ultEffectList.Count)
        {
            var effect = Instantiate(_ultEffectList[index], pos, Quaternion.identity, transform);
        }

        // 不正なindexである場合
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>特殊攻撃エフェクト（上方）を生成・表示する</summary>
    /// <param name="type">エフェクトの属性</param>
    /// <param name="pos">エフェクトの生成位置</param>
    /// <param name="player">プレイヤーの位置</param>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手）</param>
    public void PlayUpperAltEffect(SlashEnchantment type, Vector3 pos, Transform player, int handIndex)
    {
        // マップに存在しない場合は、インデックスを-1に設定する（0は既に割り当てられているため）
        if (!_altEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // indexの値が正常である場合
        if (index >= 0 && index < _altEffectList.Count)
        {
            switch (handIndex)
            {
                // 右手
                case 0:
                    // 斜め右向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalRightEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var rightEffectParticle = diagonalRightEffect.GetComponent<ParticleSystem>();

                    break;

                // 左手
                case 1:
                    // 斜め左向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var leftEffectParticle = diagonalLeftEffect.GetComponent<ParticleSystem>();

                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }

    /// <summary>特殊攻撃エフェクト（下方）を生成・表示する</summary>
    /// <param name="type">エフェクトの属性</param>
    /// <param name="pos">エフェクトの生成位置</param>
    /// <param name="player">プレイヤーの位置</param>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手）</param>
    public void PlayLowerAltEffect(SlashEnchantment type, Vector3 pos, Transform player, int handIndex)
    {
        // マップに存在しない場合は、インデックスを-1に設定する（0は既に割り当てられているため）
        if (!_altEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // indexの値が正常である場合
        if (index >= 0 && index < _altEffectList.Count)
        {
            switch (handIndex)
            {
                // 右手
                case 0:
                    // 斜め右向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalRightEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    break;

                // 左手
                case 1:
                    // 斜め左向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }
}
