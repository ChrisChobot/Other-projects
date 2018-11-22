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

public class Tower : MonoBehaviour
{
    public bool IsYellow;

    private const float _maxTimeToShoot = 8f;
    private const float _minTimeToShoot = 4f;
    private const float _minRotateAngle = 15f;
    private const float _maxRotateAngle = 60f;
    private const float _projectileForce = 1300f;
    private const float _projectileSpawnLocationModifier = 30;
    
    private float _timer;

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                transform.Rotate(Vector3.up, Random.Range(_minRotateAngle, _maxRotateAngle));
            }
            else
            {
                transform.Rotate(Vector3.down, Random.Range(_minRotateAngle, _maxRotateAngle));
            }

            _timer = Random.Range(_minTimeToShoot, _maxTimeToShoot);

            GameObject projectile = Instantiate(PrefabHolder.Instance.Projectile, transform.position, transform.rotation);
            projectile.transform.Translate(transform.right * _projectileSpawnLocationModifier, Space.World);
            GameManager.ProjectilesCount++;

            projectile.GetComponent<Rigidbody>().AddForce(transform.right * _projectileForce);
            projectile.GetComponent<MeshRenderer>().material.color = GetComponent<MeshRenderer>().material.color;
            projectile.GetComponent<Projectile>().Parent = this;
        }
    }

    public void KilledSomeone()
    {
        if (!IsYellow)
        {
            GetComponent<MeshRenderer>().material.color = GetRandomColor();
        }
    }

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
}
