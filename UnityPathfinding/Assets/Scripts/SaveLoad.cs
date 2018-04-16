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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[System.Serializable]
public class SaveFieldContainer
{
    public int n;
    public List<int> savedObstacles;
    public int start;
    public int finish;
    public SaveFieldContainer(int n, List<int> savedObstacles, int start, int finish)
    {
        this.n = n;
        this.savedObstacles = savedObstacles ;
        this.start = start;
        this.finish = finish;
    }
}




public class SaveLoad : MonoBehaviour {

   public static void Save()
    {
        SaveFieldContainer objectForSave = FieldManager.Instance.SaveCurrentMap();


        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("save.bin",
                                 FileMode.Create,
                                 FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, objectForSave);
        stream.Close();
    }

   public static void Load()
    {
        //TODO if save dont exist
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream("save.bin",
                                  FileMode.Open,
                                  FileAccess.Read,
                                  FileShare.Read);
        SaveFieldContainer loadedObject = (SaveFieldContainer)formatter.Deserialize(stream);
        stream.Close();

        GameManager.Instance.ChangeSize(loadedObject.n);
        FieldManager.Instance.LoadInit(loadedObject.n, loadedObject.savedObstacles, loadedObject.start, loadedObject.finish);
        //loadedObject = null;
    }
}
