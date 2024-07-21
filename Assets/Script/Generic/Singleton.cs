using UnityEngine;

/// <summary>MonoBehaviourを継承した、ジェネリックなシングルトン</summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>継承先のクラスのインスタンス</summary>
    private static T _instance;

    /// <summary>インスタンスのプロパティ</summary>
    public static T Instance
    {
        get
        {
            // インスタンスが存在しない場合
            if (_instance == null)
            {
                // シーン内からインスタンスを検索
                _instance = FindAnyObjectByType<T>();

                // シーン内にもインスタンスが存在しない場合
                if(_instance == null)
                {
                    // 新しいゲームオブジェクトを作成してインスタンスに設定する
                    var singletonObject = new GameObject();
                    singletonObject.name = $"{typeof(T).Name} (Singleton)";
                    _instance = singletonObject.AddComponent<T>();

                    // シーン遷移時にインスタンスを破棄しないように設定する
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
