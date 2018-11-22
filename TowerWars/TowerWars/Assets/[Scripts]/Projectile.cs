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
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Tower Parent;
    public float Timer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tower>() != null)
        {   
            if (Parent != null)
            {
                Parent.KilledSomeone();
            }

            if (other.GetComponent<Tower>().IsYellow == true)
            {
                TowerManager.Instance.DeleteTower(other.GetComponent<Tower>());
                TowerManager.Instance.MakeNewYellow();
            }
            else
            {
                TowerManager.Instance.DeleteTower(other.GetComponent<Tower>());
            }
        }

        GameManager.ProjectilesCount--;
        Destroy(gameObject);
    }

    private void Update()
    {
        Timer += Time.deltaTime;

        if (Timer > 20f)
        {
            GameManager.ProjectilesCount--;
            Destroy(gameObject);
        }
        else if( Timer > 0.5f)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
