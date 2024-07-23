using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>斬撃エフェクトのリスト</summary>
    [SerializeField, Header("斬撃エフェクト")] 
        private List<GameObject> _slashEffectList = new List<GameObject>();

    /// <summary>斬撃エフェクトの属性とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _slashEffectIndexMap;

    /// <summary>必殺技エフェクトのリスト</summary>
    [SerializeField, Header("必殺技エフェクト")] 
        private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>必殺技エフェクトの情報とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _ultEffectIndexMap;

    /// <summary>斬撃エフェクトの回転量を示す定数。X軸方向の回転角度（0度）。</summary>
    private const float SLASH_EFFECT_X_ANGLE = 0f;

    /// <summary>斬撃エフェクトの回転量を示す定数。Y軸方向の回転角度（180度）。</summary>
    private const float SLASH_EFFECT_Y_ANGLE = 180f;

    /// <summary>斬撃エフェクトの回転量を示す定数。右方向のZ軸回転角度（180度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT = 180f;

    /// <summary>斬撃エフェクトの回転量を示す定数。右斜め方向のZ軸回転角度（-135度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL = -135f;

    /// <summary>斬撃エフェクトの回転量を示す定数。左方向のZ軸回転角度（0度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT = 0f;

    /// <summary>斬撃エフェクトの回転量を示す定数。左斜め方向のZ軸回転角度（-45度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL = -45f;


    protected override void Awake()
    {
        // シングルトンの設定
        base.Awake();

        // 斬撃エフェクトのマップの初期化
        _slashEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };

        // 必殺技エフェクトのマップの初期化
        _ultEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Wind , 0},
            {AttackEffectType.Lightning, 1},
            {AttackEffectType.White, 2}
        };
    }

    /// <summary>斬撃エフェクトを生成・表示する</summary>
    /// <param name="type">斬撃の属性</param>
    /// <param name="pos">生成位置</param>
    /// <param name="player">プレイヤーの位置</param>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手、2が両手）</param>
    public void PlaySlashEffect(AttackEffectType type, Vector3 pos, Transform player, int handIndex)
    {
        // マップに存在しない場合は、インデックスを-1に設定する（0は既に割り当てられているため）
        if (!_slashEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // indexの値が正常である場合
        if (index >= 0 && index < _slashEffectList.Count)
        {
            switch (handIndex)
            {
                // 右手
                case 0:
                    // 右向きのエフェクトを生成し、正しい方向に回転させる
                    var rightEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    rightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    rightEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_RIGHT, Space.Self);
                    break;

                // 左手
                case 1:
                    // 左向きのエフェクトを生成し、正しい方向に回転させる
                    var leftEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    leftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    leftEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_LEFT, Space.Self);
                    break;

                // 両手
                case 2:
                    // 斜め右向きのエフェクトを生成し、正しい方向に回転させる
                    var diagonalRightEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    diagonalRightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalRightEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL, Space.Self);

                    // 斜め左向きのエフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL, Space.Self);

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
    /// <param name="type">必殺技エフェクトの属性</param>
    /// <param name="pos">必殺技エフェクトの生成位置</param>
    public void DisplayUltEffect(AttackEffectType type, Vector3 pos, Transform player)
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
            effect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            effect.transform.Rotate(0, -180, 0, Space.Self);
        }

        // 不正なindexである場合
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }
}
