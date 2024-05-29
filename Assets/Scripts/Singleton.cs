using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T instance;
    private static readonly object lockObj = new object();

    public static T Instance {
        get {
            lock (lockObj) {
                if (instance == null) {
                    instance = FindObjectOfType<T>();

                    if (instance == null) {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return instance;
            }
        }
    }

    protected virtual void Awake() {
        lock (lockObj) {
            if (instance == null) {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else if (instance != this) {
                Destroy(gameObject);
            }
        }
    }
}
