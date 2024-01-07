using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppInit : CoreInit
{
    public override void OnInit()
    {

        Loom.QueueOnMainThreadIfAsync(()=> { });

        LanguageText.LoadLanguage("en_US");
        UIManager.Instance.ShowWindow<MainMenu>();
    }

    private void OnApplicationQuit()
    {
        GlobalObj.Instance.port.Close();
    }

}
