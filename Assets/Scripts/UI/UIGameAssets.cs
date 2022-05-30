using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameAssets : MonoBehaviour
{
    #region Singleton
    //instance
    private static UIGameAssets _instance;

    public static UIGameAssets Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load("UIGameAssets") as GameObject).GetComponent<UIGameAssets>();
            }
            return _instance;
        }
    }
    #endregion

    #region References
    public GameObject UI_Pfb_ImageFileDataListObject;
    #endregion
}
