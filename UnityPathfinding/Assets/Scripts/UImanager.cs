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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour {
    private static UImanager instance;
    public static UImanager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<UImanager>();
            return instance;
        }
    }
    public InputField sizeOfMap;
    public GameObject messageBox;
    public Canvas canvas;
    public GameObject checkImage;

    public void ShowColorCheck()
    {
        if (checkImage.active == true) { checkImage.SetActive(false); }
        else { checkImage.SetActive(true); }
    }

    public void ShowInfo(string text)
    {
        GameObject tempDisplay = Instantiate(messageBox, canvas.transform);
        tempDisplay.GetComponent<Text>().text = text;
        Destroy(tempDisplay, 5f);
    }

    public void GenerateMap()
    {
        if(sizeOfMap.text.Length > 0)
        { 
            int typedValue = int.Parse(sizeOfMap.text);
            if (typedValue < 10)
            {
                ShowInfo("Wielkość musi wynosić conajmniej 10!");
            }
            else if(typedValue > 150)
            {
                ShowInfo("Wpisz mniejszą wartość!");
            }
            else
            {
                GameManager.Instance.ChangeSize(typedValue);
                FieldManager.Instance.GenerateInit(GameManager.Instance.n, GameManager.Instance.m);
            }
        }
        else
        {
            ShowInfo("Wpisz wielkość mapy!");
        }
    }
}
