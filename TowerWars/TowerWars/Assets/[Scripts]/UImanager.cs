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
using UnityEngine.UI;

public class UImanager : MonoBehaviour {

    public Text Towers;
    public Text Projectiles;
    public Text GameTime;
    private float _timer;

    public void Update()
    {
        _timer += Time.deltaTime;
        Towers.text = string.Format("Towers count: {0}", GameManager.TowerCount);
        Projectiles.text = string.Format("Projectiles count: {0}", GameManager.ProjectilesCount);
        GameTime.text = string.Format("Game time: {0}", (int)_timer);
    }
}
