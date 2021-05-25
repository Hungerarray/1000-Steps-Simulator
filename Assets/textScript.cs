using UnityEngine;
using UnityEngine.UI;

public class textScript : MonoBehaviour
{
    public Text display;

    private SpawnEntity mainObj;
    private string val;

    private void Start()
    {
        mainObj = GameObject.Find("SpawnEntity").GetComponent<SpawnEntity>();
        mainObj.RunStart += ResetDisplay;
        mainObj.RunCompleted += SetDisplay;
        display.text = "";
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
        val = $"Greatest distance travelled: {mainObj.Distances[0]:G}";
    }

}
