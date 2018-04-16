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


public class Field : MonoBehaviour {
    public int id;
    public List<int> neightbours = new List<int>();
    public bool isObstacle = false;


    public void DetectNeightbours()
    {
        int nSize = GameManager.Instance.n;

        if (id > nSize - 1)  { neightbours.Add(id - nSize); } //top
        if (id < ((nSize * nSize) - nSize))  { neightbours.Add(id + nSize); } //down
        if (id % (nSize) != nSize - 1)  { neightbours.Add(id + 1); } //left
        if (id % (nSize) != 0)  { neightbours.Add(id - 1); } //right
    }

}
