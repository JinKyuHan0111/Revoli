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
            //Debug.Log("ESC 누름");
            OnClickSetting_On_Or_Close();
        }
        BgCtrl();
    }
    void BgCtrl()
    {
        Bg_Panel.gameObject.SetActive(SettingPanel.gameObject.activeSelf);
    }
    public void OnClickSetting_On_Or_Close() //셋팅 버튼 닫기 버튼 클릭시 or Esc 버튼 누를시에 설정화면이 켜지거나 꺼진다.
    {
        if (SettingPanel.gameObject.activeSelf == true)
        {
            Time.timeScale = 1f;

        }
        else
        {
            Time.timeScale = 0f;
        }
        SettingPanel.gameObject.SetActive(!SettingPanel.gameObject.activeSelf); // 패널의 활성화 상태를 반전
    }

}
