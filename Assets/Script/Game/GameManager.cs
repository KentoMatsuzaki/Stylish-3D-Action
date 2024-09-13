using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>ƒCƒ“ƒQ[ƒ€‚ÌŠÇ—</summary>
public class GameManager : MonoBehaviour 
{
    private int _comboCount = 0;

    private float _timer = 0;

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (_timer > 0) _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _comboCount = 0;
            UIManager.Instance.DisableComboText();
        }
    }

    public void AddComboCount()
    {
        _comboCount++;
        _timer = 15f;
        Player.Instance.GainFloatEnergy(_comboCount / 100);
        UIManager.Instance.EnableComboText();
    }

    public int GetComboCount()
    {
        return _comboCount;
    }
}
