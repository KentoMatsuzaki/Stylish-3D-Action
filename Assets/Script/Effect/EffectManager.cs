using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>斬撃エフェクトのリスト</summary>
    [SerializeField, Header("斬撃エフェクト")] private List<GameObject> _slashEffectList = new List<GameObject>();

    /// <summary>斬撃エフェクトの情報とインデックスのマップ</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _slashEffectIndexMap;

    /// <summary>必殺技エフェクトのリスト</summary>
    [SerializeField, Header("斬撃エフェクト")] private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>必殺技エフェクトの情報とインデックスのマップ</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _ultEffectIndexMap;

    protected override void Awake()
    {
        // シングルトンの設定
        base.Awake();

        // 斬撃エフェクトのマップの初期化
        _slashEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
        {
            {(AttackEffectType.Ink, AttackEffectCategory.Right), 0},
            {(AttackEffectType.Ink, AttackEffectCategory.Left), 1},
            {(AttackEffectType.BlueFlame, AttackEffectCategory.Right), 2},
            {(AttackEffectType.BlueFlame, AttackEffectCategory.Left), 3},
            {(AttackEffectType.RedFlame, AttackEffectCategory.Right), 4},
            {(AttackEffectType.RedFlame, AttackEffectCategory.Left), 5},
            {(AttackEffectType.Nebula, AttackEffectCategory.Right), 6},
            {(AttackEffectType.Nebula, AttackEffectCategory.Left), 7},
            {(AttackEffectType.Blood, AttackEffectCategory.Right), 8},
            {(AttackEffectType.Blood, AttackEffectCategory.Left), 9},
            {(AttackEffectType.Water, AttackEffectCategory.Right), 10},
            {(AttackEffectType.Water, AttackEffectCategory.Left), 11},
        };

        // 必殺技エフェクトのマップの初期化
        _ultEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
        {
            {(AttackEffectType.Wind, AttackEffectCategory.Ult), 12},
            {(AttackEffectType.Lightning, AttackEffectCategory.Ult), 13},
            {(AttackEffectType.White, AttackEffectCategory.Ult), 14}
        };
    }

    /// <summary>斬撃エフェクトを生成・表示する</summary>
    /// <param name="type">斬撃エフェクトの属性</param>
    /// <param name="category">斬撃エフェクトの分類</param>
    /// <param name="pos">斬撃エフェクトの生成位置</param>
    public void DisplaySlashEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    { 
        if (!_slashEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // index の値が有効かどうかをチェック
        if (index >= 0 && index < _slashEffectList.Count)
        {
            Instantiate(_slashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }

    /// <summary>必殺技エフェクトを生成・表示する</summary>
    /// <param name="type">必殺技エフェクトの属性</param>
    /// <param name="category">必殺技エフェクトの分類</param>
    /// <param name="pos">必殺技エフェクトの生成位置</param>
    public void DisplayUltEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    {
        if (!_ultEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // index の値が有効かどうかをチェック
        if (index >= 0 && index < _ultEffectList.Count)
        {
            Instantiate(_ultEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }
}
