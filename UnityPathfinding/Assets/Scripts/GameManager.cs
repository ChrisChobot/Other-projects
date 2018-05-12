/*
Copyright 2017 Krzysztof Chobot

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }
    private int _n;
    public  int n //number of fields
    {
        get { return _n; }
        private set { _n = value; }
    }
    private int _m;
    public int m //number of obstacles
    {
        get { return _m; }
        private set { _m = value; }
    }

    public Camera mainCamera;
    public Camera centralizedCamera;

    void Start () {
        n = 15;
        m = (n / 3) * (n / 3);
        FieldManager.Instance.GenerateInit(n, m);
	}
	
   public void ChangeSize(int size)
    {
        n = size;
        m = (n / 3) * (n / 3);
    }


    public static IEnumerator Wait(float time = 2f)
    {
        yield return new WaitForSeconds(time);
    }

    public void ChangeView()
    {
        if (mainCamera.enabled)
        {
            mainCamera.enabled = false;
            centralizedCamera.enabled = true;
        }
        else
        {
            mainCamera.enabled = true;
            centralizedCamera.enabled = false;
        }
    }
    public void CentralizeCamera()
    {
        float x = 490f + ((n - 10) * 0.4f);
        float y = -259f + ((n - 10) * 0.73f);
        float z = -527f + ((n - 10) * 0.36f);

        Vector3 position = new Vector3(x,y,z);
        centralizedCamera.transform.position = position;
    }
}
