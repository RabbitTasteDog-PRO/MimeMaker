using UnityEngine;
using System.Collections;

	public abstract class Ray_Singleton<T> : MonoBehaviour where T : Ray_Singleton<T>{

	// protected static T _instance = null;
    // public static T Instance
	// {
	// 	get{
	// 		/// Instance requiered for the first time, lock for it
	// 		if (_instance == null){
	// 			_instance = GameObject.FindObjectOfType(typeof(T)) as T;
	// 		}
	// 		return _instance;
	// 	}
	// }
	// // If no other monobehaviour request the instance in an awake function
	// // executing before this one, no need to search the object.
	// private void Awake(){  
	// 	if ( _instance == null){
	// 		_instance = this as T;
	// 	}else{
	// 		if ( _instance != this as T ){
	// 			Destroy(gameObject);
	// 			return;
	// 		}
	// 	}
	// 	Init ();
	// }
	
	// public virtual void Init (){}
	private static T _instance = null;
	
	/// <summary>
	/// gets the instance of this Singleton
	/// use this for all instance calls:
	/// MyClass.Instance.MyMethod();
	/// or make your public methods static
	/// and have them use Instance
	/// </summary>
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = (T)FindObjectOfType (typeof(T));
				if (_instance == null) {
					string goName = typeof(T).ToString ();          
					GameObject go = GameObject.Find (goName);
					if (go == null) {
						go = new GameObject ();
						go.name = goName;
					}
					_instance = go.AddComponent<T> ();                 
				}
			}
			return _instance;
		}
	}
	
	/// <summary>
	/// for garbage collection
	/// </summary>
	public virtual void OnApplicationQuit ()
	{
		// release reference on exit
		_instance = null;
	}
	
	// in your child class you can implement Awake()
	// and add any initialization code you want such as
	// DontDestroyOnLoad(go);
	// if you want this to persist across loads
	// or if you want to set a parent object with SetParent()
	
	/// <summary>
	/// parent this to another gameobject by string
	/// call from Awake if you so desire
	/// </summary>
	protected void SetParent (string parentGOName)
	{
		if (parentGOName != null) {
			GameObject parentGO = GameObject.Find (parentGOName);
			if (parentGO == null) {
				parentGO = new GameObject ();
				parentGO.name = parentGOName;
			}
			this.transform.parent = parentGO.transform;
		}
	}

}

