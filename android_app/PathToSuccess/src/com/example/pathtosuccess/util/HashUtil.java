package com.example.pathtosuccess.util;

public class HashUtil {
	
	public static int getHash(String s) {
		int hash = 0;
		for (int i = 0; i < s.length(); i++)
		{
			char c = s.charAt(i);
			hash = ((hash * 1664525) + c + 1013904223) % Integer.MAX_VALUE;
		}
		return hash > 0 ? hash : hash * (-1);
	}
}

//uint result = 0;
//foreach (char c in toHash)
//    result = ((result*1664525) + (uint) ( c ) + 1013904223) % uint.MaxValue ;
//return result;