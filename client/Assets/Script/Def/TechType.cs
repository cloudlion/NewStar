using UnityEngine;
using System.Collections;

public enum TechType{
	Food = 1,
	Wood = 2,
	Stone = 3,
	Ore = 4,
	Cavalry = 5,
	Scout = 31,
}

public enum TechCategory{
	COMBAT = 1,
	DEVELOP = 2,
	ASSISTANT = 3
}

public class TechSequence
{
	public static TechType[] Combat = new TechType[1] {TechType.Cavalry};
	public static TechType[] Develop = new TechType[4] {TechType.Food, TechType.Wood, TechType.Stone, TechType.Ore};
	public static TechType[] Assistant = new TechType[0];// {TechType.Scout}; 
}