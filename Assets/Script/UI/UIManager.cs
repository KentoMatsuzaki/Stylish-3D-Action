using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text _comboText;

    [SerializeField] Scrollbar _bar;

    private static UIManager instance;
    public static UIManager Instance => instance;

    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        _comboText.text = $"{GameManager.Instance.GetComboCount().ToString()} Combo";

        _bar.size = Player.Instance._floatEnergy;
    }

    public void EnableComboText()
    {
        _comboText.enabled = true ;
    }

    public void DisableComboText()
    {
        _comboText.enabled = false ;
    }
}
