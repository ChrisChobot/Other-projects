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

public class Tower : MonoBehaviour {
    private float timer = 0f;
   // public float lifeTimer = 0f;
    public bool isYellow = false;

    private Color GetRandomColor()
    {
        Color result;
        switch (Random.Range(0, 6))
        {
            case 0: result = new Color(0, 0, 0, 1); break;
            case 1: result = new Color(0, 0, 1, 1); break;
            case 2: result = new Color(0, 1, 1, 1); break;
            case 3: result = new Color(0, 1, 0, 1); break;
            case 4: result = new Color(1, 0, 1, 1); break;
            case 5: result = new Color(0.2F, 0.3F, 0.4F, 0.5F); break;
            default: result = new Color(); break;
        }


        return result;
    }

    public void KilledSomeone()
    {
        if (!isYellow) { GetComponent<MeshRenderer>().material.color = GetRandomColor(); }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
     //   lifeTimer += Time.deltaTime;
        if (timer < 0)
        {
            if (Random.Range(0, 2) == 0) { transform.Rotate(Vector3.up, Random.Range(15, 61)); }
            else { transform.Rotate(Vector3.down, Random.Range(15, 61)); }
            timer = Random.Range(4f, 8f);
            GameObject projectile = Instantiate(PrefabHolder.Instance.projectile, transform.position, transform.rotation);
            projectile.transform.Translate(transform.right * 30, Space.World);
            GameManager.projectilesCount++;
            projectile.GetComponent<Rigidbody>().AddForce(transform.right * 1300f);
            projectile.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;
            projectile.GetComponent<Projectile>().parent = this;
        }
    }
}
