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
    public GameObject ground;
    public GameObject fieldPrefab;
    public GameObject obstaclePrefab;
    public Material startMaterial;
    public Material finishMaterial;
    public Material roadMaterial;
    public Material dijkstraLookedMaterial;
    public Material aStarLookedMaterial;
    private bool _doColorRoad = true;
    public bool doColorRoad
    {
        get { return _doColorRoad; }
        private set { _doColorRoad = value; }
    }

    private List<Field> allFields = new List<Field>();
    private List<GameObject> allObstacles = new List<GameObject>();

    private Field start;
    private Field finish;
    public void DoColorChange()
    {
        if (doColorRoad) { doColorRoad = false; }
        else { doColorRoad = true; }

        UImanager.Instance.ShowColorCheck();
    }

    private void ClearMap()
    { 
        GameObject[] fields = GameObject.FindGameObjectsWithTag("pole");
    
        foreach (GameObject field in fields)
        {
            Destroy(field);
        }   

        allFields.Clear();
        allObstacles.Clear();
    }


    public void GenerateInit(int n, int m)
    {
        ClearMap(); //ensure that we will not generate map again, until it is destroyed

        Vector3 groundPosition = ground.transform.position;
        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        var z = 0f;
        var x = 0f; 

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                position.Set(groundPosition.x + x, groundPosition.y + 1, groundPosition.z + z);
                GameObject temp = Instantiate(fieldPrefab, position, rotation);
                temp.GetComponent<Field>().id = ((i * n) + j);
                allFields.Add(temp.GetComponent<Field>());
                z += 1.025f;
            }
            z = 0;
            x += 1.025f;
        }

        int currentN;
        for (int i = 0; i < m; i++)
        {
            //currentN = which row + which column
            currentN = ((i / (n / 3)) * 3 * n) + (i % (n / 3)) * 3;

            int obstacleType = Random.Range(0, 4);
            AddObstacle(obstacleType, currentN, n,i);
        }
        FindStartAndFinish(n*n);
        GameManager.Wait(2f); //prevent "racing"
        foreach (Field field in allFields)
        {
            field.DetectNeightbours();
            for (int i = field.neightbours.Count - 1; i >= 0; i--)
            {
                if (allFields[field.neightbours[i]].isObstacle)
                {
                    field.neightbours.RemoveAll(k => k == field.neightbours[i]);
                }
            }
        }

    }

    public void LoadInit(int n, List<int> savedObstacles, int start, int finish)
    {
        ClearMap();

        Vector3 groundPosition = ground.transform.position;
        Vector3 position = new Vector3(0, 0, 0);
        Quaternion rotation = new Quaternion(0, 0, 0, 0);
        var z = 0f;
        var x = 0f;

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n; j++)
            {
                position.Set(groundPosition.x + x, groundPosition.y + 1, groundPosition.z + z);
                GameObject temp = Instantiate(fieldPrefab, position, rotation);
                temp.GetComponent<Field>().id = ((i * n) + j);
                allFields.Add(temp.GetComponent<Field>());
                z += 1.025f;
            }
            z = 0;
            x += 1.025f;
        }


        foreach(int obstacle in savedObstacles)
        {
            InstntiateObstacle(obstacle);
        }

        this.start = allFields[start].GetComponent<Field>();
        this.finish = allFields[finish].GetComponent<Field>();
        this.start.GetComponent<MeshRenderer>().material = startMaterial;
        this.finish.GetComponent<MeshRenderer>().material = finishMaterial;
        GameManager.Wait(2f); //prevent "racing"
        foreach (Field field in allFields)
        {
            field.DetectNeightbours();
            for (int i = field.neightbours.Count-1; i >= 0; i--)
            {
                if (allFields[field.neightbours[i]].isObstacle)
                {
                    field.neightbours.RemoveAll(k => k == field.neightbours[i]);
                }
            }
        }
    }

    public void StartDijkstra()
    {
        Pathfinding.Instance.StartDijkstra(start, finish, allFields);
    }
    public void StartAstar()
    {
        Pathfinding.Instance.StartAstar(start, finish, allFields);
    }
    private void AddObstacle(int obstacleType, int currentN, int n, int i)
    {
        int randomPosition;
        int calculatedPosition; 
        switch (obstacleType)
        {
            case 0: //1*1
                randomPosition = Random.Range(0, 9);
                calculatedPosition = currentN + randomPosition % 3 + (randomPosition / 3) * n;

                InstntiateObstacle(calculatedPosition);
                break;
            case 1: //2*1
                randomPosition = Random.Range(0, 6); // only 6 possibilities
                calculatedPosition = currentN + randomPosition % 2 + (randomPosition / 2) * n;

                InstntiateObstacle(calculatedPosition);
                InstntiateObstacle(calculatedPosition+1);
                break;
            case 2: //1*2
                randomPosition = Random.Range(0, 6); // only 6 possibilities
                calculatedPosition = currentN + randomPosition % 3 + (randomPosition / 3) * n;

                InstntiateObstacle(calculatedPosition);
                InstntiateObstacle(calculatedPosition + n);
                break;
            case 3: //2*2
                randomPosition = Random.Range(0, 4); // only 4 possibilities
                calculatedPosition = currentN + randomPosition % 2 + (randomPosition / 2) * n;

                InstntiateObstacle(calculatedPosition);
                InstntiateObstacle(calculatedPosition + n);
                InstntiateObstacle(calculatedPosition + 1);
                InstntiateObstacle(calculatedPosition + 1 + n);
                break;
            default:
                Debug.LogError("incorrect random range!");
                break;
        }
    }

    private void InstntiateObstacle(int calculatedPosition)
    {
        Vector3 parentPosition = new Vector3();
        allFields[calculatedPosition].GetComponent<Field>().isObstacle = true;

        parentPosition.Set(allFields[calculatedPosition ].transform.position.x, allFields[calculatedPosition ].transform.position.y + 1, allFields[calculatedPosition ].transform.position.z);
        GameObject obstacle = Instantiate(obstaclePrefab, parentPosition, new Quaternion(0, 0, 0, 0));
        obstacle.GetComponent<Field>().id = allFields[calculatedPosition ].GetComponent<Field>().id;
        obstacle.GetComponent<Field>().isObstacle = true;
        allObstacles.Add(obstacle);
    }

    private void FindStartAndFinish(int n, int startField = 0, int finishField = 0)
    {
        if (startField == 0) { startField = Random.Range(0, n); }
        if (finishField == 0) { finishField = Random.Range(0, n); }

        if (startField == finishField || ( allFields[startField].GetComponent<Field>().isObstacle==true && allFields[finishField].GetComponent<Field>().isObstacle == true))
        {
            FindStartAndFinish(n, 0, 0);
        }
        else if (allFields[startField].GetComponent<Field>().isObstacle == true && allFields[finishField].GetComponent<Field>().isObstacle == false)
        {
            FindStartAndFinish(n, 0, finishField);
        }
        else if (allFields[startField].GetComponent<Field>().isObstacle == false && allFields[finishField].GetComponent<Field>().isObstacle == true)
        {
            FindStartAndFinish(n, startField, 0);
        }
        else
        {
            start = allFields[startField].GetComponent<Field>();
            start.GetComponent<MeshRenderer>().material = startMaterial;
            finish = allFields[finishField].GetComponent<Field>();
            finish.GetComponent<MeshRenderer>().material = finishMaterial;
        }
    }



    public SaveFieldContainer SaveCurrentMap()
    {
        List<int> savedObstacles = new List<int>();
        foreach(GameObject obstacle in allObstacles)
        {
            savedObstacles.Add(obstacle.GetComponent<Field>().id );
        }
        SaveFieldContainer result = new SaveFieldContainer(GameManager.Instance.n, savedObstacles, start.id , finish.id );
        return result;
    }
}
