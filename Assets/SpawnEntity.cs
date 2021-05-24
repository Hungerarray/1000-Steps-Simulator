using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpawnEntity : MonoBehaviour
{
    // public GameObject entity;
    //[Range(500, 10_000)]
    public int totalEntity;
    public int limit = 100;

    private List<GameObject> entities;
    private int steps = 1000;
    private Task<List<int>> sequence;
    private int entityCompleted = 0;


    // Start is called before the first frame update
    async Task Start()
    {
        entities = new List<GameObject>(totalEntity);
        sequence = GenerateSequence();

        for (int i = 0; i < totalEntity; ++i)
        {
            var entity = await CreateEntity(i);
            entity.GetComponent<circleScript>().Completed += runCompleted;
            entities.Add(entity);
        }
    }

    private void runCompleted()
    {
        ++entityCompleted;
        if (entityCompleted == totalEntity)
        {
            SortList();
        }
    }

    private void SortList()
    {
        entities = entities.OrderByDescending(gObj => gObj.GetComponent<circleScript>().distance).ToList() ;
    }

    async Task<GameObject> CreateEntity(int i)
    {
        var entity = new GameObject($"Circle_{i}", typeof(SpriteRenderer), typeof(LineRenderer), typeof(circleScript));
        entity.GetComponent<SpriteRenderer>().color = UnityEngine.Random.ColorHSV(0, 1, .3f, 1, .8f, 1);

        var cScr = entity.GetComponent<circleScript>();
        cScr.sequence = (await sequence).GetRange(i * steps, steps);

        return entity;
    }

    async Task<List<int>> GenerateSequence()
    {
        System.Random rand = new System.Random();
        List<Task<List<int>>> tot = new List<Task<List<int>>>();
        List<int> ret = new List<int>(totalEntity * steps);
        for (int i = 0; i < totalEntity; ++i)
        {
            tot.Add(Steps(rand));
        }

        foreach(var tsk in tot)
        {
            var ls = await tsk;
            ret.AddRange(ls);
        }

        return ret;
    }

    private async Task<List<int>> Steps(System.Random rand)
    {
        var ls = new List<int>(steps);

        for (int i = 0; i < steps; ++i)
        {
            int res;
            // if multiple threads call on random them it will return 0 
            while ((res = rand.Next(1, 5)) == 0) ;
            ls.Add(res);
        }
        return ls;
    }

    // Update is called once per frame
    void Update()
    {
        if(entityCompleted == totalEntity)
        {
            DisableGameObj(limit);
        }
    }

    private void DisableGameObj(int limit)
    {
        for (int i = limit; i < totalEntity; ++i)
        {
            entities[i].SetActive(false);
        }
    }
}
