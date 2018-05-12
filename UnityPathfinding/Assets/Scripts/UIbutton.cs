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
using UnityEngine;

public enum ActionButtonType
{
    GENERATE,
    SAVE,
    LOAD,
    DIJKSTRA,
    ASTAR,
    SHOW_ROAD,
    CHANGE_VIEW,
    EXIT
}

public class UIbutton : MyButton
{

    [SerializeField]
    public ActionButtonType actionType;

    private void Start()
    {
        switch (actionType)
        {
            case ActionButtonType.GENERATE:
                _button.onClick.AddListener(() => { UImanager.Instance.GenerateMap(); });
                break;
            case ActionButtonType.SAVE:
                _button.onClick.AddListener(() => { SaveLoad.Save(); });
                break;
            case ActionButtonType.LOAD:
                _button.onClick.AddListener(() => { SaveLoad.Load(); });
                break;
            case ActionButtonType.DIJKSTRA:
                _button.onClick.AddListener(() => { FieldManager.Instance.StartDijkstra(); });
                break;
            case ActionButtonType.ASTAR:
                _button.onClick.AddListener(() => { FieldManager.Instance.StartAstar(); });
                break;
            case ActionButtonType.SHOW_ROAD:
                _button.onClick.AddListener(() => { FieldManager.Instance.DoColorChange(); });
                break;
            case ActionButtonType.CHANGE_VIEW:
                _button.onClick.AddListener(() => { GameManager.Instance.ChangeView(); });
                break;
            case ActionButtonType.EXIT:
                _button.onClick.AddListener(() => { Application.Quit(); });
                break;
        }

    }
}