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
    [SerializeField] float pushTime;
    int currentDistance;
    float pushTimer;

    [SerializeField] Transform brickParent;
    [SerializeField] BrickBase[] brickPrefabs;
    List<BrickBase> bricks = new List<BrickBase>();

    void Start()
    {
        GenerateArenas();

        GlobalOberserver.AddListener<GlobalEvent_BrickBreak>(Event_OnBrickBreak);
    }
    void Update()
    {
        PushArenaWithTime();
        PushAreanWithDistance();

        //Testing
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     for (int i = bricks.Count - 1; i >= 0; i--)
        //     {
        //         BrickBase brick = bricks[i];
        //         brick.Damage(100);
        //     }
        // }
    }
    void PushArenaWithTime()
    {
        pushTimer += Time.deltaTime;
        if (pushTimer > pushTime)
        {
            pushTimer = 0;
            GenerateArena(true);
            GlobalOberserver.TriggerEvent(this, new GlobalEvent_ArenaPush());
        }
    }
    void PushAreanWithDistance()
    {
        int distance = GetBrickDistance();
        if (distance < pushDistance)
        {
            GenerateArena(true);
            GlobalOberserver.TriggerEvent(this, new GlobalEvent_ArenaPush());
        }
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
        arena.transform.parent = arenaParent;
        arena.name = "Arena " + currentDistance;

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

            if (UnityEngine.Random.value > 0.5f)
            {
                BrickBase brick = Instantiate(brickPrefabs[index], brickPosition, Quaternion.identity);
                brick.transform.parent = brickParent;

                bricks.Add(brick);
            }
        }
    }

    int GetBrickDistance()
    {
        int closed = int.MaxValue;
        foreach (var brick in bricks)
        {
            closed = Mathf.Min(closed, (int)brick.transform.position.z);
        }

        return currentDistance - closed;
    }

    void Event_OnBrickBreak(object sender, EventArgs e)
    {
        GlobalEvent_BrickBreak arg = e as GlobalEvent_BrickBreak;

        bricks.Remove(arg.brick);
    }

}
