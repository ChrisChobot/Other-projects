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

public class FieldManager : MonoBehaviour
{
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

    public List<LocationForTower> LocationsForTower = new List<LocationForTower>();
    private List<List<bool>> _map = new List<List<bool>>();
    
    private Color _groundColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    private Color _towerPlaceColor = new Color(1, 0, 0, 1);

    public void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        Vector3 position = Vector3.zero;
        int x = 0;
        int z = 0;

        for (int i = 0; i < GameManager.MapSize; i++, x++, z = 0)
        {
            _map.Add(new List<bool>());
            for (int j = 0; j < GameManager.MapSize; j++, z++)
            {
                _map[i].Add(new bool());
            }
        }
        
        FindPlaceForTower(x, z, position);
        
        _map.Clear();
        position.Set(GameManager.MapSize / 2, 0f, GameManager.MapSize / 2);
        PrepareGround(_groundColor, GameManager.MapSize, position);
    }

    private void FindPlaceForTower(int x, int z, Vector3 position)
    {
        int placeForTowers = 0;

        while (placeForTowers < GameManager.TowerMaxCount * 1.3)
        {
            x = Random.Range(GameManager.TowerSize, GameManager.MapSize - GameManager.TowerSize);
            z = Random.Range(GameManager.TowerSize, GameManager.MapSize - GameManager.TowerSize);

            if (_map[x][z]) { continue; }
            if (_map[x - GameManager.TowerSize][z - GameManager.TowerSize]) { continue; }
            if (_map[x + GameManager.TowerSize][z + GameManager.TowerSize]) { continue; }
            if (_map[x - GameManager.TowerSize][z + GameManager.TowerSize]) { continue; }
            if (_map[x + GameManager.TowerSize][z - GameManager.TowerSize]) { continue; }

            placeForTowers++;
            LocationsForTower.Add(new LocationForTower(x, z));
            position.Set(x, 0.1f, z);
            PrepareGround(_towerPlaceColor, GameManager.PlaceForTowerSize, position);

            x -= GameManager.TowerSize;
            z -= GameManager.TowerSize;

            for (int i = 0; i < GameManager.PlaceForTowerSize; i++)
            {
                for (int j = 0; j < GameManager.PlaceForTowerSize; j++)
                {
                    _map[x + i][z + j] = true;
                }
            }
        }
    }

    private void PrepareGround(Color color, float size, Vector3 position)
    {
        GameObject ground = Instantiate(PrefabHolder.Instance.Ground, position, new Quaternion(0f, 0f, 0f, 0f));
        ground.GetComponent<MeshRenderer>().material.color = color;
        ground.transform.localScale = new Vector3(size, 0.01f, size);
    }
}
