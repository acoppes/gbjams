using System;
using System.Linq;
using GBJAM.Commons;
using GBJAM.Commons.Menus;
using UnityEngine;

public class TestMenuSceneController : MonoBehaviour
{
    public OptionsMenu optionsMenu;

    public string title;
    public string[] options;

    public GameboyButtonKeyMapAsset keyMapAndState;
    
    // Start is called before the first frame update
    void Start()
    {
        optionsMenu.title = title;
        var optionsList = options.Select(o => new Option
        {
            name = o
        }).ToList();
        
        optionsMenu.Show(optionsList, (i, option) =>
        {
            Debug.Log(option);
            return true;
        }, () =>
        {
            
        });
    }

    private void Update()
    {
        keyMapAndState.UpdateControlState();
    }
}
