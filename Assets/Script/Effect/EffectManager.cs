using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>攻撃エフェクトのプレハブのリスト</summary>
    [SerializeField, Header("攻撃エフェクト")] private List<GameObject> _attackEffectList = new List<GameObject>();

    /// <summary>攻撃エフェクトの情報と対応するインデックスのマップ</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _attackEffectIndexMap;

    protected override void Awake()
    {
        // シングルトンの設定
        base.Awake();

        // マップの初期化
        _attackEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
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
            {(AttackEffectType.Wind, AttackEffectCategory.Ult), 12},
            {(AttackEffectType.Lightning, AttackEffectCategory.Ult), 13},
            {(AttackEffectType.White, AttackEffectCategory.Ult), 14}
        };
    }

    /// <summary>攻撃エフェクトを表示する</summary>
    /// <param name="pos">攻撃エフェクトの生成位置</param>
    public void DisplayAttackEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    { 
        if (!_attackEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // index の値が有効かどうかをチェック
        if (index >= 0 && index < _attackEffectList.Count)
        {
            Instantiate(_attackEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }
}
