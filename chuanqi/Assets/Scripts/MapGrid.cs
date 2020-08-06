using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrid : MonoBehaviour {

	public bool isshow;
	public float gridw;
	public int xgridnum;
	public int ygridnum;
	public float mapw;
	public float maph;
	public int[] data;//一维数组；左下角原点 从下到上，从左到右;

	void OnDrawGizmos()
	{
		if (isshow == false){
			return;
		}

		//线颜色;
		Gizmos.color = Color.white;

		// 画X方向的竖线;
		for(int i=0; i<xgridnum; i++){
			float x = gridw * i;
			Gizmos.DrawLine (new Vector2(x,0), new Vector2(x,maph));
		}
		// 画Y方向的横线;
		for(int i=0; i<ygridnum; i++){
			float y = gridw * i;
			Gizmos.DrawLine (new Vector2(0,y), new Vector2(mapw,y));
		}
		//画正方体;
		for (int i = 0; i < xgridnum; i++) {
			for (int k=0; k<ygridnum; k++)
			{
				if (getData (i, k) == 0) {
					continue;
				}
				if (getData (i, k) == 1) {
					//不可走 （红 ;
					Gizmos.color = new Color(1,0,0,0.5f);
					Gizmos.DrawCube (new Vector3(i*gridw + gridw*0.5f,k*gridw + gridw*0.5f,0), new Vector3(gridw, gridw, 0.2f));
				}
				if (getData (i, k) == 2) {
					//遮挡 （蓝 ;
					Gizmos.color = new Color(0,0,1,0.5f);
					Gizmos.DrawCube (new Vector3(i*gridw + gridw*0.5f,k*gridw + gridw*0.5f,0), new Vector3(gridw, gridw, 0.2f));
				}
			}
		}
	}
	public void setData(int x, int y, int value){
		int index = x * ygridnum + y;
		data [index] = value;
	}
	public int getData(int x, int y){
		int index = x * ygridnum + y;
		return data [index];
	}
}
