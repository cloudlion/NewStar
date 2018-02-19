using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelTest : MonoBehaviour {

	public GameObject[] pets;
	public GameObject rider;
	public GameObject footman;
	public GameObject archer;


	public int startX;
	public int startY;

	private Vector3 pos = new Vector3();
	public Vector3 center;
	int[] petNum = new int[10];
	int footmanNum = 0;
	int riderNum = 0;
	int archerNum = 0;
	int total = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Add(GameObject go)
	{
		pos.x = center.x + Random.Range (-20, 20);
		pos.z = center.z + Random.Range (-20, 20);

		Instantiate (go, pos, new Quaternion ()); 
	}

	void OnGUI()
	{

		for(int i = 0; i < 10; i++)
		{
			if(GUI.Button( new Rect(startX, startY+50*i, 80, 40), "Pet" + i.ToString() + ": " + petNum[i]))
			{
				Add(pets[i]);
				petNum[i]++;
				total++;
			}
		}

		if( GUI.Button( new Rect(startX + 100, startY+50, 80, 40), "foot man: " + footmanNum) )
		{
			Add(footman);
			footmanNum++;
			total++;
		}

		if( GUI.Button( new Rect(startX + 100, startY+100, 80, 40), "archer: " + archerNum) )
		{
			Add(archer);
			archerNum++;
			total++;
		}

		if(GUI.Button( new Rect(startX + 100, startY+150, 80, 40), "rider: " + riderNum))
		{
			Add(rider);
			riderNum++;
			total++;
		}
		GUI.Label (new Rect(startX + 100, startY+200, 80, 40), "total: " + total);
	}
}
