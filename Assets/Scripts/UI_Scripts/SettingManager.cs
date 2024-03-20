using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MySingleton<SettingManager>
{
    public Image SettingPanel;
    public Image Bg_Panel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("ESC ����");
            OnClickSetting_On_Or_Close();
        }
        BgCtrl();
    }
    void BgCtrl()
    {
        Bg_Panel.gameObject.SetActive(SettingPanel.gameObject.activeSelf);
    }
    public void OnClickSetting_On_Or_Close() //���� ��ư �ݱ� ��ư Ŭ���� or Esc ��ư �����ÿ� ����ȭ���� �����ų� ������.
    {
        if (SettingPanel.gameObject.activeSelf == true)
        {
            Time.timeScale = 1f;

        }
        else
        {
            Time.timeScale = 0f;
        }
        SettingPanel.gameObject.SetActive(!SettingPanel.gameObject.activeSelf); // �г��� Ȱ��ȭ ���¸� ����
    }

}
