using UnityEngine;
using System.Collections;

public class ProceduralNumberGenerator {
	public static int currentPosition = 0;
	//public const string key = "123424123342421432233144441212334432121223344";

	public static int GetNextNumber() {
        // To make the same Lab each time following a sequence (key), could be a date or whatever we want (works just like hashing)

		//string currentNum = key.Substring(currentPosition++ % key.Length, 1);
		//return int.Parse (currentNum);

        return Random.Range(1,5);
	}
}
