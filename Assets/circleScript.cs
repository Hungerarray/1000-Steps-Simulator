using System;
using System.Collections.Generic;
using UnityEngine;


public class circleScript : MonoBehaviour
{
    public List<int> sequence;
    public float distance;
    public event Action Completed;

    private int currStep = 0;
    private int noOfSteps = 1000;
    private LineRenderer line;
    private SpriteRenderer sptRnd;

    // Start is called before the first frame update
    void Start()
    {
        sptRnd = GetComponent<SpriteRenderer>();
        LineInitializer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currStep < noOfSteps)
            SimulStep();
        else if (currStep == noOfSteps)
        {
            ++currStep;
            distance = Vector3.Distance(Vector3.zero, line.GetPosition(noOfSteps - 1));
            Completed();
        }
    }

    void SimulStep()
    {
        var pos = EvalPos(sequence[currStep]);
        transform.position += pos;
        line.SetPosition(currStep, transform.position);
        ++currStep;
    }

    private void LineInitializer()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = noOfSteps;
        line.material = sptRnd.material;

        var clr = sptRnd.color;
        line.startColor = clr;
        line.endColor = clr;

        line.startWidth = 0.45f;
        line.endWidth = 0.5f;

    }

    private Vector3 EvalPos(int num)
    {
        return num switch
        {
            1 => Vector3.up,
            2 => Vector3.right,
            3 => Vector3.down,
            4 => Vector3.left,
            _ => Vector3.zero
        };
    }
}
