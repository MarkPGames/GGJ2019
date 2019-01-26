using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private PlayerResources _playerResource;

    [SerializeField]
    private Slider[] playerSliders;
    

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _playerResource.Resources.Length; i++)
        {
            playerSliders[i].value = _playerResource.Resources[i].Progress;
          //  playerSliders[_playerResource]
          //  _playerResource.Resources[i];
        }
    }
}
