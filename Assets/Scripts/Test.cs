using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
 
    // Start is called before the first frame update
    void Start()
    {
        AccountManager.Login("tnxah.j@gmail.com", "123123123");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            print("F");
            ItemManager.ReadPrefabs();
            ItemManager.LoadItems();

            //PlayFabInventoryService.ConsumeItem("8B9DA07783057898", 2);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            print("R");
            ItemGenerator.instance.Init();
        }
    }

    public void Test1()
    {
        print("F");
        ItemManager.ReadPrefabs();
        ItemManager.LoadItems();

        //PlayFabInventoryService.ConsumeItem("8B9DA07783057898", 2);
    }
    public void Test2()
    {
        print("R");
        ItemGenerator.instance.Init();
    }
}
