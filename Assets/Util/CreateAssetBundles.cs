using UnityEngine;
using System.IO;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;


public class CreateAssetBundles
{
	[MenuItem("Assets/AssetBundle/Build AssetBundles Windows")]
	static void BuildAllAssetBundleWindows(){
		BuildPipeline.BuildAssetBundles(makeDirectory("AssetBundles/Windows"), BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
//		BuildPipeline.BuildAssetBundles(makeDirectory("AssetBundles/Windows"));
	}
	
	[MenuItem("Assets/AssetBundle/Build AssetBundles Android")]
	static void BuildAllAssetBundleAndroid(){
		BuildPipeline.BuildAssetBundles(makeDirectory("AssetBundles/Android"), BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.Android);
	}
	
	[MenuItem("Assets/AssetBundle/Build AssetBundles IOS")]
	static void BuildAllAssetBundleIOS(){
		BuildPipeline.BuildAssetBundles(makeDirectory("AssetBundles/IOS"), BuildAssetBundleOptions.DeterministicAssetBundle, BuildTarget.iOS);
	}
	
	static string makeDirectory(string path){
		DirectoryInfo di = new DirectoryInfo(path);
		if (di.Exists == false)
		{
			di.Create();
		}
		return path;
	}
}
#endif