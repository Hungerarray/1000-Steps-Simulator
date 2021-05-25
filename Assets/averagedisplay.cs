using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class averagedisplay : MonoBehaviour
{
    public Text display;
   
    private SpawnEntity mainObj;
    private string val;

    private void Start()
    {
        display.text = "";
        mainObj = GameObject.Find("SpawnEntity").GetComponent<SpawnEntity>();
        mainObj.RunStart += ResetDisplay;
        mainObj.RunCompleted += SetDisplay;
    }

    private void Update()
    {
        display.text = val;
    }

    private void ResetDisplay()
    {
        val = "";
    }

    private void SetDisplay()
    {
        val = $"Average Distance travelled: {mainObj.GetComponent<SpawnEntity>().Distances.Average():G}";
    }

}
