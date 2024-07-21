using UnityEngine;

/// <summary>MonoBehaviour���p�������A�W�F�l���b�N�ȃV���O���g��</summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>�p����̃N���X�̃C���X�^���X</summary>
    private static T _instance;

    /// <summary>�C���X�^���X�̃v���p�e�B</summary>
    public static T Instance
    {
        get
        {
            // �C���X�^���X�����݂��Ȃ��ꍇ
            if (_instance == null)
            {
                // �V�[��������C���X�^���X������
                _instance = FindAnyObjectByType<T>();

                // �V�[�����ɂ��C���X�^���X�����݂��Ȃ��ꍇ
                if(_instance == null)
                {
                    // �V�����Q�[���I�u�W�F�N�g���쐬���ăC���X�^���X�ɐݒ肷��
                    var singletonObject = new GameObject();
                    singletonObject.name = $"{typeof(T).Name} (Singleton)";
                    _instance = singletonObject.AddComponent<T>();

                    // �V�[���J�ڎ��ɃC���X�^���X��j�����Ȃ��悤�ɐݒ肷��
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this) 
        {
            Destroy(gameObject);
        }
    }
}
