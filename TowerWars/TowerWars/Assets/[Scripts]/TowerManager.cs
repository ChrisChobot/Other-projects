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

public class TowerManager : MonoBehaviour {
    private static TowerManager instance;
    public static TowerManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<TowerManager>();
            return instance;
        }
    }
    private float timer = 0f;
    private bool producedMax = false;
    private bool isYellowTower = false;
    private List<Tower> towers = new List<Tower>();

    public void MakeNewYellow()
    {
        towers[0].isYellow = true;
        towers[0].GetComponent<MeshRenderer>().material.color = new Color(1f, 0.92f, 0.016f, 1f);
    }
    public void DeleteTower(Tower tower)
    {
        FieldManager.Instance.locationForTower.RemoveAll(item => item.x == (int)tower.transform.position.x && item.z == (int)tower.transform.position.z);
        FieldManager.Instance.locationForTower.Add(new FieldManager.LocationForTower((int)tower.transform.position.x, (int)tower.transform.position.z, false));
        towers.RemoveAll(item => item == tower);
        Destroy(tower.gameObject);
        GameManager.towerCount--;

    }
    //public float YellowLifeTime()
    //{
    //   return towers[0].GetComponent<Tower>().lifeTimer;
    //}

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (producedMax) { timer = 10f; }
            else { timer = 5f; }

            if(GameManager.towerCount < GameManager.towerMax)
            {
                for (int i = 0; i < FieldManager.Instance.locationForTower.Count; i++) 
                {
                    FieldManager.LocationForTower location = FieldManager.Instance.locationForTower[i];
                    if (location.isOcuppied) { continue; }
                    
                    FieldManager.Instance.locationForTower.Add(new FieldManager.LocationForTower(location.x, location.z, true));
                    Vector3 position = new Vector3(location.x, 1f, location.z);
                    towers.Add(Instantiate(PrefabHolder.Instance.tower, position, new Quaternion(0f, 0f, 0f, 0f)).GetComponent<Tower>());
                    if (!isYellowTower)
                    {
                        towers[0].GetComponent<MeshRenderer>().material.color = new Color(1f, 0.92f, 0.016f, 1f);
                        isYellowTower = true;
                        towers[0].isYellow = true;
                    }

                    GameManager.towerCount++;
                    GameManager.towerProduced++;
                    if(GameManager.towerProduced == GameManager.towerMax) { producedMax = true; }
                    FieldManager.Instance.locationForTower.RemoveAt(i);
                    break;
                }
                
            }
            
        }
    }
}
