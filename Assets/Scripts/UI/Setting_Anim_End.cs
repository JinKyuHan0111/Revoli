using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Anim_End : MonoBehaviour
{
    Animator SettingPanel_Anim;
    private void Start()
    {
        SettingPanel_Anim = GetComponent<Animator>();
    }
    public void Setting_Animation_End()
    {
        gameObject.SetActive(true);
    }
}
