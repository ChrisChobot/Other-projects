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
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour {
    private static FieldManager instance;
    public static FieldManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<FieldManager>();
            return instance;
        }
    }
    public struct LocationForTower
    {
        public int x;
        public int z;
        public bool isOcuppied;
        public LocationForTower(int x, int z,bool isOcuppied=false)
        {
            this.x = x;
            this.z = z;
            this.isOcuppied = isOcuppied;
        }
    }
    List<List<bool>> map = new List<List<bool>>();
    //public List<Field> map = new List<Field>();
    public List<LocationForTower> locationForTower = new List<LocationForTower>();
    public void GenerateMap()
    {
        //generacja "ziemi" 700x700
        int x = 0;
        int z = 0;
        GameObject groundPrefab = PrefabHolder.Instance.ground;
        Vector3 position = new Vector3(0f, 0f, 0f);
        Quaternion quaternion = new Quaternion(0f, 0f, 0f, 0f);
        for (int i = 0; i < 700; i++, x++, z = 0)
        {
            map.Add(new List<bool>());
            for (int j = 0; j < 700; j++, z++)
            {
                map[i].Add(new bool());
                //map[i].Add(Instantiate(groundPrefab, position, quaternion).GetComponent<Field>());
                //  position.Set(x, 0f, z);
            }
        }

        //szukanie miejsca na wieze
        int placeForTowers = 0;
        while (placeForTowers < GameManager.towerMax * 1.3)
        {           
            x = Random.Range(20, 680);
            z = Random.Range(20, 680);

            if (map[x][z]) { continue; }
            if (map[x - 20][z - 20]) { continue; }
            if (map[x + 20][z + 20]) { continue; }
            if (map[x - 20][z + 20]) { continue; }
            if (map[x + 20][z - 20]) { continue; }

            placeForTowers++;
            locationForTower.Add(new LocationForTower(x, z));
            position.Set(x, 0f, z);
            GameObject placeForTower = Instantiate(groundPrefab, position, quaternion);
            placeForTower.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
            placeForTower.transform.localScale = new Vector3(40f,0.1f,40f);
            x -= 20;
            z -= 20;

            

            for (int i = 0; i < 40; i++)
            {
                for (int j = 0; j < 40; j++)
                {
                    map[x + i][z + j] = true;
                   // map[x + i][z + j].GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 1);
                }
            }
        }


        map.Clear();
        position.Set(350f, 0f, 350f);
        GameObject ground = Instantiate(groundPrefab, position, quaternion);
        ground.GetComponent<MeshRenderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        ground.transform.localScale = new Vector3(700, 0.01f, 700f);
    }

    public void Start()
    {
        GenerateMap();
    }
}
