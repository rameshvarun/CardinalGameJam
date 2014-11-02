using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utils {
	public static void Shuffle<T>(IList<T> list) {
		int n = list.Count;
		System.Random rnd = new System.Random();

		while (n > 1) {
			int k = (rnd.Next(0, n) % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}


	public static char GetLetter() {
		int num = Random.Range(0, 26);
		return (char)('a' + num);
	}
}
