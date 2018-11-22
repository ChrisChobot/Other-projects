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

    private float _timer;
    private bool _producedMax;
    private bool _isYellowTower;
    private List<Tower> _towers = new List<Tower>();

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            if (_producedMax) { _timer = 10f; }
            else { _timer = 5f; }

            if (GameManager.TowerCount < GameManager.TowerMaxCount)
            {
                for (int i = 0; i < FieldManager.Instance.LocationsForTower.Count; i++)
                {
                    LocationForTower location = FieldManager.Instance.LocationsForTower[i];
                    if (location.isOcuppied) { continue; }

                    FieldManager.Instance.LocationsForTower.Add(new LocationForTower(location.x, location.z, true));
                    Vector3 position = new Vector3(location.x, 1f, location.z);
                    _towers.Add(Instantiate(PrefabHolder.Instance.Tower, position, new Quaternion(0f, 0f, 0f, 0f)).GetComponent<Tower>());
                    if (!_isYellowTower)
                    {
                        _towers[0].GetComponent<MeshRenderer>().material.color = new Color(1f, 0.92f, 0.016f, 1f);
                        _isYellowTower = true;
                        _towers[0].IsYellow = true;
                    }

                    GameManager.TowerCount++;
                    GameManager.TowerProduced++;
                    if (GameManager.TowerProduced == GameManager.TowerMaxCount) { _producedMax = true; }
                    FieldManager.Instance.LocationsForTower.RemoveAt(i);
                    break;
                }

            }

        }
    }

    public void MakeNewYellow()
    {
        _towers[0].IsYellow = true;
        _towers[0].GetComponent<MeshRenderer>().material.color = new Color(1f, 0.92f, 0.016f, 1f);
    }

    public void DeleteTower(Tower tower)
    {
        FieldManager.Instance.LocationsForTower.RemoveAll(item => item.x == (int)tower.transform.position.x && item.z == (int)tower.transform.position.z);
        FieldManager.Instance.LocationsForTower.Add(new LocationForTower((int)tower.transform.position.x, (int)tower.transform.position.z, false));
        _towers.RemoveAll(item => item == tower);
        Destroy(tower.gameObject);
        GameManager.TowerCount--;
    }
}
