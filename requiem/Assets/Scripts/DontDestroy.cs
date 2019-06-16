﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
    	GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
    	if (objs.Length > 1)	Destroy(gameObject);
    	DontDestroyOnLoad(gameObject);
    }
}