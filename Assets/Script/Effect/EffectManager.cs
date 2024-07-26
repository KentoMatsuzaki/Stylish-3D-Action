using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>斬撃エフェクトのリスト</summary>
    [SerializeField, Header("斬撃エフェクト")] private List<GameObject> _slashEffectList = new List<GameObject>();

    /// <summary>斬撃エフェクトの属性とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _slashEffectIndexMap;

    /// <summary>必殺技エフェクトのリスト</summary>
    [SerializeField, Header("必殺技エフェクト")] private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>必殺技エフェクトの情報とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _ultEffectIndexMap;

    /// <summary>特殊攻撃エフェクトのリスト</summary>
    [SerializeField, Header("特殊攻撃のエフェクト")] private List<GameObject> _altEffectList = new List<GameObject>();

    private Dictionary<AttackEffectType, int> _altEffectIndexMap;

    /// <summary>斬撃エフェクトの回転量を示す定数。X軸方向の回転角度（0度）。</summary>
    private const float SLASH_EFFECT_X_ANGLE = 0f;

    /// <summary>斬撃エフェクトの回転量を示す定数。Y軸方向の回転角度（180度）。</summary>
    private const float SLASH_EFFECT_Y_ANGLE = 180f;

    /// <summary>右向き斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（180度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT = 180f;

    /// <summary>右斜め下の斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（-135度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL_LOWER = -135f;

    /// <summary>右斜め上の斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（-135度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL_UPPER= 45f;

    /// <summary>左向き斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（0度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT = 0f;

    /// <summary>左斜め下の斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（-45度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL_LOWER = -45f;

    /// <summary>左斜め上の斬撃エフェクトの回転量を示す定数。Z軸方向の回転角度（-45度）。</summary>
    private const float SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL_UPPER = 135f;

    /// <summary>特殊攻撃エフェクトを停止させるまでの遅延時間</summary>
    private const float ALT_ATTACK_EFFECT_STOP_DELAY = 0.25f;

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

        // 特殊攻撃エフェクトのマップの初期化
        _altEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };
    }

    /// <summary>斬撃エフェクトを生成・表示する</summary>
    /// <param name="type">エフェクトの属性</param>
    /// <param name="pos">エフェクトの生成位置</param>
    /// <param name="player">エフェクトの位置</param>
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
                    diagonalRightEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL_LOWER, Space.Self);

                    // 斜め左向きのエフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_slashEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(SLASH_EFFECT_X_ANGLE, SLASH_EFFECT_Y_ANGLE, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL_LOWER, Space.Self);

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
    public void PlayUltEffect(AttackEffectType type, Vector3 pos, Transform player)
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

    /// <summary>特殊攻撃エフェクト（上方）を生成・表示する</summary>
    /// <param name="type">エフェクトの属性</param>
    /// <param name="pos">エフェクトの生成位置</param>
    /// <param name="player">プレイヤーの位置</param>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手）</param>
    public void PlayUpperAltEffect(AttackEffectType type, Vector3 pos, Transform player, int handIndex)
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
                    diagonalRightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalRightEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL_UPPER, Space.Self);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var rightEffectParticle = diagonalRightEffect.GetComponent<ParticleSystem>();
                    StartCoroutine(PlayAndStopAltEffect(rightEffectParticle));

                    // エフェクトを前方に移動させ続ける
                    MoveForwardIndefinitely(diagonalRightEffect);
                    break;

                // 左手
                case 1:
                    // 斜め左向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL_UPPER, Space.Self);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var leftEffectParticle = diagonalLeftEffect.GetComponent<ParticleSystem>();
                    StartCoroutine(PlayAndStopAltEffect(leftEffectParticle));

                    // エフェクトを前方に移動させ続ける
                    MoveForwardIndefinitely(diagonalLeftEffect);
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
    public void PlayLowerAltEffect(AttackEffectType type, Vector3 pos, Transform player, int handIndex)
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
                    diagonalRightEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalRightEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_RIGHT_DIAGONAL_LOWER, Space.Self);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var rightEffectParticle = diagonalRightEffect.GetComponent<ParticleSystem>();
                    StartCoroutine(PlayAndStopAltEffect(rightEffectParticle));

                    // エフェクトを前方に移動させ続ける
                    MoveForwardIndefinitely(diagonalRightEffect);
                    break;

                // 左手
                case 1:
                    // 斜め左向きの特殊攻撃エフェクトを生成し、正しい方向に回転させる
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);
                    diagonalLeftEffect.transform.rotation = Quaternion.LookRotation(player.transform.forward);
                    diagonalLeftEffect.transform.Rotate(0, -180, SLASH_EFFECT_Z_ANGLE_LEFT_DIAGONAL_LOWER, Space.Self);

                    // エフェクトのパーティクルを再生し、適切なタイミングで停止させる
                    var leftEffectParticle = diagonalLeftEffect.GetComponent<ParticleSystem>();
                    StartCoroutine(PlayAndStopAltEffect(leftEffectParticle));

                    // エフェクトを前方に移動させ続ける
                    MoveForwardIndefinitely(diagonalLeftEffect);
                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }

    IEnumerator PlayAndStopAltEffect(ParticleSystem particle)
    {
        // パーティクルシステムを再生
        particle.Play();

        // 一定時間待機
        yield return new WaitForSeconds(ALT_ATTACK_EFFECT_STOP_DELAY);

        // パーティクルシステムを停止
        particle.Pause();
    }

    private void MoveForwardIndefinitely(GameObject particle)
    {
        particle.transform.DOBlendableMoveBy(transform.forward * 10f, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() => MoveForwardIndefinitely(particle));
    }
}
