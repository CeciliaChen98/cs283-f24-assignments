using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks; 

public class GameText : MonoBehaviour
{
    public TextMeshProUGUI output;

    // Start is called before the first frame update
    async void Start()
    {
        await Task.Delay(1000);
        output.text = "Welcome to Lingering Garden (Liu Yuan)!"; 
        await Task.Delay(3000);
        output.text = "To escape from here, you need to lighten 4 lanterns around the garden...";
        await Task.Delay(4000);
        output.text = "GOOD LUCK...";
        await Task.Delay(2000);
        output.text = "Have Fun...";
        await Task.Delay(2000);
        output.text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
