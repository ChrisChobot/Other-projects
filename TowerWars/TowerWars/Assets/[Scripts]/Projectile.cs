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
    public Tower parent;
    public float timer=0f;
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tower>() != null)
        {   
            if (parent != null) { parent.KilledSomeone(); }
            if (other.GetComponent<Tower>().isYellow == true)
            {
                TowerManager.Instance.DeleteTower(other.GetComponent<Tower>());
                TowerManager.Instance.MakeNewYellow();
            }
            else { TowerManager.Instance.DeleteTower(other.GetComponent<Tower>()); }
        }
        GameManager.projectilesCount--;
        Destroy(this.gameObject);
      //  if (other != null) { Destroy(other.gameObject); }
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > 20f)
        {
            GameManager.projectilesCount--;
            Destroy(this.gameObject);
        }
        else if( timer > 0.5f)
        {
            GetComponent<BoxCollider>().isTrigger = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
