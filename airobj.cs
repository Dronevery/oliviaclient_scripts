using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;
using LitJson;
using System;
using System.Text;
using System.Threading;

public class airobj 
{
    // C# 3.0 auto-implemented properties
	public float lon {get; set;}
	public float lat {get; set;}
	public float height {get; set;}
	public float yaw {get;set;}
	public float pitch {get;set;}
	public float roll {get;set;}
	public float[] loc ;
	public GameObject go;
	public string name;
	public TerrainManager terrainmanager;

	private bool isInited = false;

	public void update(JsonData jd)
	{
		lon = (float) ( (double) jd["lon"]);
		lat = (float) ( (double) jd["lat"]);
		height = (float) ( (double) jd["height"]);
        yaw = (float) ( (double) jd["yaw"]);
		pitch = (float) ( (double) jd["pitch"]);
		roll = (float) ( (double) jd["roll"]);

		if (!isInited){
			isInited = true;
			int[] center = map.lnglatToXY (lon, lat, publicvar.zoom);
			publicvar.basei = center [0];
			publicvar.basej = center [1];
		}

		loc = map.getUnityPosfromLatlng (lon, lat, publicvar.zoom);

		go.transform.position = new Vector3(loc[0], height,loc[2]);
		go.transform.Rotate(new Vector3 (0, 0, 0));
//		GUIText statusbar =GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<GUIText>();
		GameObject statusbar = GameObject.Find("StatusBar");
		if (statusbar != null) {
//			statusbar.GetComponent<GUIText>().text = "lat: " + this.lat.ToString() +"\nlon: " + this.lon.ToString();
			
			
			}
//		Debug.Log ("yaw: " + yaw);
		
//        go.gameObject.SetActive(true);
	}
	public airobj(string name ,GameObject originairplane ,JsonData jd)
	{
		// create a plane on the stage
    	go = (GameObject)MonoBehaviour.Instantiate( originairplane);
		go.SetActive (true);
		go.name = name;

		// inactivate the camera
		Camera camera = go.GetComponentInChildren<Camera> ();
		camera.gameObject.SetActive(false);

		// register to the camera manager
		CameraManager cmngr = GameObject.Find ("Connecter").GetComponent<CameraManager> ();
		cmngr.cameras [name] = camera;

//		GameObject script = GameObject.Find("Script");
//		script.GetComponent<cameraControl> ().camera2 = go.GetComponentInChildren<Camera>().gameObject;
		update(jd);
		this.go.AddComponent("TerrainManager");
		TerrainManager terrainmanager = this.go.GetComponent<TerrainManager>();
		terrainmanager.plane = go;
		terrainmanager.StartUpdate ();
	}
//	public airobj(GameObject air){
//		go = air;
//
//		go.SetActive (true);
//		string name = air.name;
//		// inactivate the camera
//		Camera camera = go.GetComponentInChildren<Camera> ();
////		camera.gameObject.SetActive (false);
//
//		// register to the camera manager
//		try{
//		CameraManager cmngr = GameObject.Find ("Connecter").GetComponent<CameraManager> ();
//
//		cmngr.cameras [name] = camera;
//		}catch	{
//		}
//			
//
//		this.go.AddComponent("TerrainManager");
//		TerrainManager terrainmanager = this.go.GetComponent<TerrainManager>();
//		terrainmanager.plane = go;
//		terrainmanager.StartUpdate ();
//
//	}

	public void destroy(){
		GameObject.Destroy(go);

	}
}
