using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 

public class FPSDisplay : MonoBehaviour {

    float timeA; 

    public int fps;

    public int lastFPS;

    public GUIStyle textStyle;

    // Use this for initialization

    public void Start () 
	{
        timeA = Time.timeSinceLevelLoad;
		Application.targetFrameRate = 300;
        //DontDestroyOnLoad (this);
//		InvokeRepeating("CheckMem", 0, 1);
    }

    

    // Update is called once per frame

    void Update () {

        //Debug.Log(Time.timeSinceLevelLoad+" "+timeA);

        if(Time.timeSinceLevelLoad  - timeA <= 1)

        {

            fps++;

        }

        else

        {

            lastFPS = fps + 1;

            timeA = Time.timeSinceLevelLoad;

            fps = 0;

        }

		if(Record)
		{
			frame.Add(1 / Time.deltaTime);
	//		memory.Add(NativeCaller.MemoryUsage()/1024f/1024f);
		}


    }

	void CheckAllMem(){

	}
	
	bool log = false;
	void CheckMem(){
		if (log){
			float sum = 0;

			sum += CheckTexture();
			sum += Check(typeof(Animation));
			sum += Check(typeof(AnimationClip));
			sum += Check(typeof(Mesh));
			sum += Check(typeof(Shader));
			sum += Check(typeof(AssetBundle));
			sum += Check(typeof(AudioClip));

			StringBuilder sb = new StringBuilder();
			sb.Append("#Profiler.GetMonoHeapSize:" + UnityEngine.Profiling.Profiler.GetMonoHeapSize()/(1024f * 1024f) + "\n");
			sb.Append("#Profiler.GetMonoUsedSize:" + UnityEngine.Profiling.Profiler.GetMonoUsedSize()/(1024f * 1024f) + "\n");

			sb.Append("#Profiler.GetTotalAllocatedMemory:" + UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory()/(1024f * 1024f) + "\n");
			sb.Append("#Profiler.GetTotalReservedMemory:" + UnityEngine.Profiling.Profiler.GetTotalReservedMemory()/(1024f * 1024f) + "\n");
			sb.Append("#Profiler.GetTotalUnusedReservedMemory:" + UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemory()/(1024f * 1024f) + "\n");

			sum += UnityEngine.Profiling.Profiler.GetMonoHeapSize()/(1024f * 1024f);
			sum += UnityEngine.Profiling.Profiler.GetTotalReservedMemory()/(1024f * 1024f);

			sb.Append("#Sum : " + sum);

			//Debug.Log(sb);
		}
		log = false;
	}
	
	float CheckTexture(){
		var textures = Resources.FindObjectsOfTypeAll(typeof(Texture2D));
		float sum = 0;
		StringBuilder sb = new StringBuilder();
		foreach(Texture t in textures){
			float use = UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(t) / 1024f / 1024f;
			sb.Append(use + ":" + t.width + ":" + t.height + ":" + t.name + "\n");
			sum += use;
		}
		sb.Append("#TotalTexture:" + sum);
		//Debug.Log(sb);

		return sum;
	}
	
	float Check(System.Type type){
		var items = Resources.FindObjectsOfTypeAll(type);
		float sum = 0;
		StringBuilder sb = new StringBuilder();
		foreach (var item in items){
			float use = UnityEngine.Profiling.Profiler.GetRuntimeMemorySize(item) / 1024f / 1024f;
			sb.Append(use + ":" + item.name + "\n");
			sum += use;
		}
		sb.Append("#" + type.ToString() + ":" + sum);
		//Debug.Log(sb);

		return sum;
	}
	GameObject go;
	GameObject ui;
	Texture2D  tex;
    void OnGUI()
    {
		string MemoryUsage = (NativeCaller.MemoryUsage()/1024f/1024f).ToString("0.00");
		string debugInfo = string.Format("FPS:{0}\nMemoryUsage:{1}\n", lastFPS, MemoryUsage);
		//Debug.Log(debugInfo);
		GUI.Label(new Rect( 400,300, 250,200), debugInfo,textStyle);

        //		if (GUI.Button(new Rect(100, 500, 150, 50), "Log Texture Use")){
        //			log = true;
        //		}
#if ASSETS
        if (GUI.Button(new Rect(100, 200, 150, 50), "Log Bundle memory")){
			string snapShot = 	AssetsMgr.Instance.GetBundleMemSnapshot();
			//Debug.Log("======GetBundleMemSnapshot: " + snapShot);
		}
#endif
//
////		if (GUI.Button(new Rect(250, 500, 150, 50), "Unload res")){
////			Resources.UnloadUnusedAssets();
////		}
//
		if (GUI.Button(new Rect(250, 400, 150, 50), "Test file")){
			TestFileHandle();
			Resources.UnloadUnusedAssets();
		}
//
//		if (GUI.Button(new Rect(400, 500, 150, 50), "LoadPrefab")){
//			if(go != null)
//				Destroy(go);
////			AssetsMgr.Instance.LoadAsset<Texture2D>("t_UI_Atlas1", (obj, result)=>{
//////				go = GameObject.Instantiate<GameObject>( obj as GameObject);
////
////			}, this);
//			tex = Resources.Load<Texture2D>("t_UI_Atlas1");
//		}
//
//		if (GUI.Button(new Rect(550, 500, 150, 50), "UnLoadPrefab")){
//			if(go != null)
//				Destroy(go);
////			AssetsMgr.Instance.UnloadAsset("t_UI_Atlas1");
//			Resources.UnloadAsset(tex);
//			tex = null;
//			Resources.UnloadUnusedAssets();
//			GC.Collect();
//		}
		//		if (GUI.Button(new Rect(550, 500, 150, 50), "LoadMaterial")){
//		
//			AssetsMgr.Instance.LoadAsset<GameObject>("tm1", (obj, result)=>{
//			}, this);
//		}

//		if (GUI.Button(new Rect(550, 100, 150, 50), "UnLoadMaterial")){
//		//	AssetsMgr.Instance.UnloadAsset("tm1");
//		}

//		if (GUI.Button(new Rect(250, 100, 150, 50), "LoadUI")){
//			if(ui != null)
//				Destroy(ui);
//			AssetsMgr.Instance.LoadAsset<GameObject>("UI_Resource", (obj, result)=>{
////				AssetsMgr.Instance.UnloadAsset("UI_Technology");
//				ui = GameObject.Instantiate<GameObject>( obj as GameObject);
//
//			}, this);
//		}
//
//		if (GUI.Button(new Rect(400, 100, 150, 50), "UnLoadUI")){
//			if(ui != null)
//				Destroy(ui);
////			DestroyImmediate(obj, true);
//			AssetsMgr.Instance.UnloadAsset("UI_Resource");
//		}


		/*		GUI.Label( new Rect( 100,300, 100,100), "quality: "+ QualitySettings.GetQualityLevel());
		GUI.Label( new Rect( 100,400, 100,100), "AA: "+ QualitySettings.antiAliasing);
	
		if( GUI.Button(new Rect(100, 500, 100, 100), "+") )
		{
			QualitySettings.SetQualityLevel( QualitySettings.GetQualityLevel() + 1 );
		}
		
		if( GUI.Button(new Rect(200, 500, 100, 100), "-") )
		{
			QualitySettings.SetQualityLevel( QualitySettings.GetQualityLevel() - 1 );
		}*/	
//		
//		if( GUI.Button(new Rect(200, 500, 100, 100), "Switch ui") )
//		{
//			MainContainer.Instance.gameObject.SetActive(!MainContainer.Instance.gameObject.activeSelf);
//		}
    }

//	void OnGUI()
//	{
//		{
//			// Resources.UnloadUnusedAssets();
//			// GC.Collect();
//			/*
//			GIGRoot.Instance.Init();
//			FSM<GIGRoot> fsm = GIGRoot.Instance.Fsm;
//			fsm.ChangeState(GIGRoot.DragonKeepState, new object[]{GIGDragonKeep.DragonKeepViewVO.GreatDragonKeep()});
//			*/
//		}
//	}

	List<float> frame = new List<float>();
	List<float> memory = new List<float>();
	bool Record = false;
	
	public float MaxFrameRate
	{
		get;
		set;
	}
	
	public float MinFrameRate
	{
		get;
		set;
	}
	
	public float FrameRateVariance
	{
		get;
		set;
	}
	
	public float MaxMemory
	{
		get;
		set;
	}
	
	public float MinMemory
	{
		get;
		set;
	}
	
	public float MemoryVariance
	{
		get;
		set;
	}
	
	public void BeginRecord()
	{
		frame.Clear();
		memory.Clear();
		MaxFrameRate = 0f;
		MinFrameRate = 0f;
		MaxMemory = 0f;
		MinMemory = 0f;
		
		Record = true;
		//Debug.Log("begin record~~~");
	}
	
	public void EndRecord()
	{
		Record = false;
		CalculateResult();
		//Debug.Log("end record~~~");
	}

	public float MemorySnapshot()
	{
		return 0f;//NativeCaller.MemoryUsage()/1024f/1024f;
	}

	void CalculateResult()
	{
		if(frame.Count > 0)
		{
			frame.Sort();
			MaxFrameRate = frame[0];
			MinFrameRate = frame[frame.Count - 1];
			FrameRateVariance = CalculateVariance(frame);
		}
		
		if(memory.Count > 0)
		{
			memory.Sort();
			MaxMemory = memory[0];
			MinMemory = memory[memory.Count - 1];
			MemoryVariance = CalculateVariance(memory);
		}
		
	}

	void TestFileHandle()
	{
//		GameUtil.FileSystem.CacheFileDir ("log");
		GameUtil.FileSystem.CleanCacheDir ( GameUtil.FileSystem.CacheFileDir("log") );


		byte[] bytes = new byte[2];
		List<FileStream> streamList = new List<FileStream> ();
		string filePath = GameUtil.FileSystem.CacheFileDir ("log");

		for (int i = 0; i< 256; i++) 
		{
			FileStream stream = null;
			try {
				stream = new FileStream (filePath + "log"+i, FileMode.Create, FileAccess.ReadWrite);
				bytes = new byte[stream.Length];
				stream.Read (bytes, 0, (int)stream.Length);
				streamList.Add(stream);
//			localVersion = ProtoBufUtil.Deserialize<ProtoVO.common.Version>(bytes);
			} catch (Exception ex) {

				break;
			} finally {
//				stream.Close ();
			}
		}
		//Debug.Log ("opened file# " + streamList.Count);
		for (int i = 0; i< streamList.Count; i++) 
		{
			streamList[i].Close();
		}
		streamList.Clear ();
	}
	
	float CalculateVariance(List<float> data)
	{
		float sum = 0f, average = 0f, variance = 0f;
		for(int i = 0; i < data.Count; i++)
		{
			sum += data[i];
		}
		
		average = sum / data.Count;
		
		float temp = 0f;
		for(int i = 0; i < data.Count; i++)
		{
			temp += (data[i] - average) * (data[i] - average);
		}
		
		variance = temp / data.Count;
		
		return variance;
	}


	void Test(){
		Application.LoadLevelAsync("scn_gtdk");
	}
}