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
    public List<float> Distances;
    public event Action RunStart;
    public event Action RunCompleted;

    private List<GameObject> entities;
    private int steps = 1000;
    private Task<List<int>> sequence;
    private int entityCompleted = 0;

    private bool run = false;

    // Update is called once per frame
    async Task Update()
    {
        if (run)
        {
            RunStart();
            clearEntities();
            await GenerateEntities();
            run = false;
        }

        if (entityCompleted == totalEntity)
        {
            DisableGameObj(limit);
            RunCompleted();
        }
    }

    public void StartSimul()
    {
        run = true;
    }

    public void SetTotalEnity(string text)
    {
        if(!run)
        {
            totalEntity = int.Parse(text);
        }
    }

    public void SetLimit(string text)
    {
        if(!run)
        {
            limit = int.Parse(text);
        }
    }
    private void clearEntities()
    {
        Distances = null;
        if(entities != null)
        {
            foreach (var entity in entities)
                Destroy(entity);
            entities.Clear();
        }
    }

    private async Task GenerateEntities()
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
            Distances = entities.Select(obj => obj.GetComponent<circleScript>().distance).ToList();
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

  
    private void DisableGameObj(int limit)
    {
        for (int i = 0; i < limit; ++i)
        {
            entities[i].SetActive(true);
        }
        for (int i = limit; i < totalEntity; ++i)
        {
            entities[i].SetActive(false);
        }
    }
}
