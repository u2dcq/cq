using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Pathfinding.Serialization.JsonFx;

public class MapCreator : EditorWindow {

	MapGrid mapgrid;
	public string output_filename = "map01";
	public string input_filename = "map01";
	public float m_gridw = 0.5f;

	[MenuItem("Tools/MapCreator")]
	static void Create(){
		EditorWindow.GetWindow (typeof(MapCreator));
	}

	void OnGUI(){
		//格子边长;
		m_gridw = float.Parse(EditorGUILayout.TextField("格子边长", m_gridw.ToString()));
		//按钮;
		if (GUILayout.Button ("创建")) {
			CreateMap ();
		}
		//导出;
		output_filename = EditorGUILayout.TextField("导出的文件名", output_filename);
		if (GUILayout.Button ("导出")) {
			OutputFile (output_filename);
		}
		//导入;
		input_filename = EditorGUILayout.TextField("导入的文件名", input_filename);
		if (GUILayout.Button ("导入")) {
			InputFile (input_filename);
		}
		//清理;
		if (GUILayout.Button ("清理")) {
			ClearSceneObject ();
		}
	}

	public void CreateMap()
	{
		//map父对象;
		GameObject map = new GameObject("map");
		mapgrid = map.AddComponent<MapGrid> ();
		BoxCollider bc = map.AddComponent<BoxCollider> ();
		bc.center = new Vector3 (100f,100f,0);
		bc.size = new Vector4 (400f, 400f, 0.2f);

		//平铺;
		int xjpgnum = 10;
		int yjpgnum = 6;
		float jpgscenew = 5.12f;
		for (int i = 0; i < 60; i++) {
			string filename = "00_" + (i + 1).ToString ("D2");//i1=01i10=10;
			Object asset = AssetDatabase.LoadMainAssetAtPath("Assets/Resources/map/"+filename+".jpg");
			if (asset != null) {
				Texture2D t2d = Texture2D.Instantiate (asset) as Texture2D;
				GameObject obj = new GameObject (filename);
				obj.transform.SetParent (map.transform);
				SpriteRenderer sr = obj.AddComponent<SpriteRenderer> ();
				sr.sprite = Sprite.Create (t2d, new Rect(0,0,512,512), Vector2.zero);
				//坐标;
				float x = (i % xjpgnum) * jpgscenew;//0-9
				float y = (yjpgnum - i / xjpgnum - 1) * jpgscenew;//0-5 6-1 5-0;
				obj.transform.position = new Vector2(x, y);
			}
		}

		mapgrid.gridw = m_gridw;
		mapgrid.mapw = xjpgnum * jpgscenew;
		mapgrid.maph = yjpgnum * jpgscenew;
		mapgrid.xgridnum = Mathf.CeilToInt(mapgrid.mapw /  mapgrid.gridw );//10.2=11;
		mapgrid.ygridnum = Mathf.CeilToInt(mapgrid.maph /  mapgrid.gridw );
		mapgrid.data= new int[mapgrid.xgridnum * mapgrid.ygridnum];
	}
	//导出;
	public void OutputFile(string filename)
	{
		string path = Application.dataPath + "/Resources/text/map/" + filename + ".txt";
		MapObj mo = new MapObj ();
		mo.gridw = mapgrid.gridw;
		mo.xgridnum = mapgrid.xgridnum;
		mo.ygridnum = mapgrid.ygridnum;
		mo.mapw = mapgrid.mapw;
		mo.maph = mapgrid.maph;
		mo.data = mapgrid.data;
		SerializeToFile<MapObj> (path, mo);
		AssetDatabase.Refresh ();
	}
	//导入;
	public void InputFile(string filename){
		string path = "Assets/Resources/text/map/" + filename + ".txt";
		MapObj mo = DeserializeFromFile<MapObj> (path);
		mapgrid.data = mo.data;
	}
	//序列化;
	public void SerializeToFile<T>(string path, T t)
	{
		if (!File.Exists (path)) {
			FileStream fs = new FileStream (path, FileMode.Create, FileAccess.ReadWrite);
			fs.Close ();
		}
		string json = JsonWriter.Serialize (t);
		File.WriteAllText (path, json);
	}
	//反序列化;
	public T DeserializeFromFile<T>(string path){
		UnityEngine.Object asset = AssetDatabase.LoadMainAssetAtPath (path);
		TextAsset ta = TextAsset.Instantiate (asset) as TextAsset;
		string json = ta.text;
		return (T)JsonReader.Deserialize (json, typeof(T));
	}
	public void ClearSceneObject(){
		GameObject map = GameObject.Find ("map");
		if (map != null) {
			GameObject.DestroyImmediate (map);
		}
	}
}
