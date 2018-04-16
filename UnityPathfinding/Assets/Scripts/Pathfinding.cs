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
using System.Linq;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    private static Pathfinding instance;
    public static Pathfinding Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<Pathfinding>();
            return instance;
        }
    }

    public void StartAstar(Field start, Field finish, List<Field> allFields)
    {
        int[] previous = new int[GameManager.Instance.n * GameManager.Instance.n];
        int cost = Astar(GameManager.Instance.n, start.id , finish.id , ref previous, allFields);
        

        if (cost == 999999) { UImanager.Instance.ShowInfo("Nie można znaleźć drogi!"); }
        else if (FieldManager.Instance.doColorRoad) { ColorRoad(start.id, finish.id, previous,allFields, out cost); }
        Debug.Log("Koszt = " + cost);
        System.Array.Clear(previous, 0, GameManager.Instance.n * GameManager.Instance.n);
    }

    private int heuristic(int start, int finish, List<Field> allFields)
    {
        int dx = (int)Mathf.Abs(allFields[start].transform.position.x - allFields[finish].transform.position.x);
        int dz = (int)Mathf.Abs(allFields[start].transform.position.z - allFields[finish].transform.position.z);

        return 2 * (dx + dz);
    }

    private int Astar(int n, int start, int finish,ref int[] previous, List<Field> allFields)
    {
        int count = 0;
        List<int> open = new List<int>();
        List<int> close = new List<int>();
        int[] gPrice = new int[n * n];
        int[] fPrice = new int[n * n];
        for (int i=0; i< n * n; i++)
        {
            gPrice[i] = 999999;
            fPrice[i] = 999999;
        }
        gPrice[start] = 0;
        fPrice[start] = heuristic(start, finish, allFields);
        open.Add(start);
        Field current;

        while (open.Count > 0)
        {
            int lowestPriceId = fPrice[open[0]];
            foreach (int field in open)
            {
                if (fPrice[field]< fPrice[lowestPriceId]) { lowestPriceId = field; }
            }
            current = allFields[lowestPriceId];
            if(current.id  == finish)
            {
                allFields[start].GetComponent<MeshRenderer>().material = FieldManager.Instance.startMaterial;
                allFields[finish].GetComponent<MeshRenderer>().material = FieldManager.Instance.finishMaterial;
                Debug.Log("l krokow:" + count);
                return 1;
            }
            current.GetComponent<MeshRenderer>().material = FieldManager.Instance.aStarLookedMaterial; //showing what we have processed
            count++;
            open.RemoveAll(x => x == lowestPriceId);
            close.Add(lowestPriceId);

            foreach (int neightbour in current.neightbours)
            {
                int tempId = neightbour ;
                if (close.Contains(tempId)) { continue; }
                if (!open.Contains(tempId)) { open.Add(tempId); }

                int tentative_gPrice = gPrice[current.id ] + heuristic(current.id , tempId, allFields);
                if (tentative_gPrice >= gPrice[tempId]) { continue; }

                previous[tempId] = current.id ;
                gPrice[tempId] = tentative_gPrice;
                fPrice[tempId] = gPrice[tempId] + heuristic(tempId, finish, allFields);
            }
        }
        return 999999;
    }

    public void StartDijkstra(Field start, Field finish, List<Field> allFields)
    {
        int[] previous = new int[GameManager.Instance.n * GameManager.Instance.n];
        int cost = Dijkstra(GameManager.Instance.n, start.id, finish.id, ref previous, allFields);
        Debug.Log("Koszt = " + cost);

        if (cost == 999999) { UImanager.Instance.ShowInfo("Nie można znaleźć drogi!"); }
        else if (FieldManager.Instance.doColorRoad) { ColorRoad(start.id, finish.id, previous, allFields); }

        System.Array.Clear(previous, 0, GameManager.Instance.n * GameManager.Instance.n);
    }

    private void ColorRoad(int start, int finish, int[] previous, List<Field> allFields) // for dijkstra
    {
        int current = previous[finish];
        while (current != start)
        {
            allFields[current].GetComponent<MeshRenderer>().material = FieldManager.Instance.roadMaterial;
            current = previous[current];
        }
        allFields[start].GetComponent<MeshRenderer>().material = FieldManager.Instance.startMaterial;
        allFields[finish].GetComponent<MeshRenderer>().material = FieldManager.Instance.finishMaterial;
    }
    private void ColorRoad(int start, int finish, int[] previous, List<Field> allFields, out int cost) // for Astar
    {
        cost = 1;
        int current = previous[finish];    
        while (current != start)
        {
            allFields[current].GetComponent<MeshRenderer>().material = FieldManager.Instance.roadMaterial;
            current = previous[current];
            cost++;
        }
        allFields[start].GetComponent<MeshRenderer>().material = FieldManager.Instance.startMaterial;
        allFields[finish].GetComponent<MeshRenderer>().material = FieldManager.Instance.finishMaterial;
    }

    private void DijkstraInit(int[] price, bool[] done, int numberOfFields, int start)
    {
        for (int i = 0; i < numberOfFields; i++)
        {
            price[i] = 999999;
            done[i] = false;
        }
        price[start] = 0;
    }

    private int Dijkstra(int n, int start, int finish, ref int[] previous, List<Field> allFields)
    {
        int numberOfFields = n * n;
        int[] price = new int[numberOfFields];
        bool[] done = new bool[numberOfFields];
        int i = 0;
        int count = 0;
        DijkstraInit(price, done, numberOfFields, start);
        Field current;
        int temp;

        while (count < numberOfFields - GameManager.Instance.m) //count < numberOfFields - numberOfallObstacles
        {
            count++;
            for (int j = 0; j < numberOfFields; j++)
            {
                if (!done[j] && price[i] > price[j])
                {
                    i = j;
                }
            }
            if (i == finish) { break; }
            done[i] = true;
            current = allFields[i];
            current.GetComponent<MeshRenderer>().material = FieldManager.Instance.dijkstraLookedMaterial; //showing what we have processed
            foreach (int neightbour in current.neightbours)
            {
                temp = neightbour ;
                if (!done[temp])
                {
                    if (price[i] + 1 < price[temp])
                    {
                        price[temp] = price[i] + 1;
                        previous[temp] = i;
                    }
                }
            }
            i = finish;
        }
        Debug.Log("l krokow:" + count);
        done = null;
        allFields[start].GetComponent<MeshRenderer>().material = FieldManager.Instance.startMaterial;
        allFields[finish].GetComponent<MeshRenderer>().material = FieldManager.Instance.finishMaterial;
        return price[finish];
    }
}
