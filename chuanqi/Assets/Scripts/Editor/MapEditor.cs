using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGrid))]
public class MapEditor : Editor {
	//是否处于编辑模式;
	public bool editMode;
	public MapGrid mapGrid;
	public int datavalue;
	void OnEnable()
	{
		mapGrid = (MapGrid)target;
	}
	public void OnSceneGUI()
	{
		if(editMode){
			//取消编辑器的选择功能;
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
			mapGrid.isshow = true;
			Event e = Event.current;

			//如果是鼠标左键;
			if (e.isMouse && e.button == 0 && e.clickCount == 1) {
				//获取鼠标产生的射线;
				Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
				RaycastHit hitinfo;
				if (Physics.Raycast(ray, out hitinfo, 20)){
					int tx = (int)(hitinfo.point.x / mapGrid.gridw);
					int ty = (int)(hitinfo.point.y / mapGrid.gridw);
					mapGrid.setData (tx, ty, datavalue);
				}
			}
			//如果shift1键被按住;
			if(e.shift){
				Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
				RaycastHit hitinfo;
				if (Physics.Raycast(ray, out hitinfo, 20)){
					int tx = (int)(hitinfo.point.x / mapGrid.gridw);
					int ty = (int)(hitinfo.point.y / mapGrid.gridw);
					mapGrid.setData (tx, ty, datavalue);
				}
			}
		} else {
			//恢复编辑器的选择功能;
			HandleUtility.Repaint();
			mapGrid.isshow = false;
		}
	}

	public override void OnInspectorGUI()
	{
		editMode = EditorGUILayout.Toggle ("Edit", editMode);
		datavalue = EditorGUILayout.IntSlider ("DataV", datavalue, 0, 2);
	}
}
