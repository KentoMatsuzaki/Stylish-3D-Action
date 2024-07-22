using UnityEngine;
using System.Collections.Generic;

/// <summary>各種エフェクトを管理するクラス</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>斬撃エフェクト（右）のリスト</summary>
    [SerializeField, Header("斬撃エフェクト（右）")] 
        private List<GameObject> _rightSlashEffectList = new List<GameObject>();

    /// <summary>斬撃エフェクト（右）の属性とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _rightSlashEffectIndexMap;

    /// <summary>斬撃エフェクト（左）のリスト</summary>
    [SerializeField, Header("斬撃エフェクト（左）")]
        private List<GameObject> _leftSlashEffectList = new List<GameObject>();

    /// <summary>斬撃エフェクト（左）の属性とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _leftSlashEffectIndexMap;

    /// <summary>必殺技エフェクトのリスト</summary>
    [SerializeField, Header("必殺技エフェクト")] 
        private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>必殺技エフェクトの情報とインデックスのマップ</summary>
    private Dictionary<AttackEffectType, int> _ultEffectIndexMap;

    protected override void Awake()
    {
        // シングルトンの設定
        base.Awake();

        // 斬撃エフェクト（右）のマップの初期化
        _rightSlashEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };

        // 斬撃エフェクト（左）のマップの初期化
        _leftSlashEffectIndexMap = new Dictionary<AttackEffectType, int>
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

    /// <summary>斬撃エフェクト（右）を生成・表示する</summary>
    /// <param name="type">斬撃エフェクトの属性</param>
    /// <param name="pos">斬撃エフェクトの生成位置</param>
    public void PlayRightSlashEffect(AttackEffectType type, Vector3 pos)
    { 
        if (!_rightSlashEffectIndexMap.TryGetValue((type), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // index の値が有効かどうかをチェック
        if (index >= 0 && index < _rightSlashEffectList.Count)
        {
            Instantiate(_rightSlashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>斬撃エフェクト（左）を生成・表示する</summary>
    /// <param name="type">斬撃エフェクトの属性</param>
    /// <param name="pos">斬撃エフェクトの生成位置</param>
    public void PlayLeftSlashEffect(AttackEffectType type, Vector3 pos)
    {
        if (!_leftSlashEffectIndexMap.TryGetValue((type), out int index))
        {
            // マップに存在しない場合は、インデックスを-1に設定する（0は割り当てられているため）
            index = -1;
        }

        // index の値が有効かどうかをチェック
        if (index >= 0 && index < _leftSlashEffectList.Count)
        {
            Instantiate(_leftSlashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>必殺技エフェクトを生成・表示する</summary>
    /// <param name="type">必殺技エフェクトの属性</param>
    /// <param name="pos">必殺技エフェクトの生成位置</param>
    public void DisplayUltEffect(AttackEffectType type, Vector3 pos)
    {
        if (!_ultEffectIndexMap.TryGetValue((type), out int index))
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
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }
}
