using System;
using System.Collections;
using System.Collections.Generic;
using AngusChanToolkit.Unity;
using UnityEngine;

public class GlobalEvent_ArenaPush : EventArgs_Global
{

}
public class ArenaManager : MonoBehaviour
{
    [SerializeField] Transform arenaParent;
    [SerializeField] GameObject areanPrefab;
    [SerializeField] int arenaLength;
    [SerializeField] int arenaWidth;

    [Space]
    [SerializeField] int pushDistance;
    int currentDistance;

    [SerializeField] Transform brickParent;
    [SerializeField] BrickBase[] brickPrefabs;
    List<BrickBase> bricks = new List<BrickBase>();

    void Start()
    {
        GenerateArenas();

        GlobalOberserver.AddListener<GlobalEvent_BrickBreak>(Event_OnBrickBreak);
    }
    void GenerateArenas()
    {
        for (int i = 0; i < arenaLength; i++)
        {
            bool spawnBrick = i > pushDistance;
            GenerateArena(spawnBrick);
        }
    }
    void GenerateArena(bool spawnBrick)
    {
        Vector3 position = new Vector3(0, 0, currentDistance);
        GameObject arena = Instantiate(areanPrefab, position, Quaternion.identity);
        arena.transform.parent = arenaParent; ;

        if (spawnBrick)
        {
            GenerateBricks(position);
        }

        currentDistance++;
    }
    void GenerateBricks(Vector3 position)
    {
        for (int i = 0; i < arenaWidth; i++)
        {
            Vector3 brickPosition = new Vector3(i - arenaWidth / 2f + 0.5f, 0, position.z);
            int index = UnityEngine.Random.Range(0, brickPrefabs.Length);

            BrickBase brick = Instantiate(brickPrefabs[index], brickPosition, Quaternion.identity);
            brick.transform.parent = brickParent;

            bricks.Add(brick);
        }
    }

    int GetBrickDistance()
    {
        int distance = 0;
        foreach (var brick in bricks)
        {
            distance = Mathf.Min(distance, (int)brick.transform.position.z);
        }
        return distance;
    }

    void Event_OnBrickBreak(object sender, EventArgs e)
    {
        GlobalEvent_BrickBreak arg = e as GlobalEvent_BrickBreak;

        bricks.Remove(arg.brick);
    }

}
