using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{


    public Text moneyText;

    public float money = 20;
    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$ " + money;

    }
}
