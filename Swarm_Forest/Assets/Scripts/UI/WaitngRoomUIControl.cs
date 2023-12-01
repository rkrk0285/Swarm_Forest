using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaitngRoomUIControl : MonoBehaviour
{
    [SerializeField]
    private GameObject Match_Btn;
    [SerializeField]
    private GameObject Rank_UI;
    [SerializeField]
    private GameObject UserName_UI;
    [SerializeField]
    private GameObject UserRank_UI;
    
    private bool matchingInProgress = false;

    private void Start()
    {
        load_UserInfo();
        load_UserRankInfo();
    }
    public void click_Match_Btn()
    {
        // ��Ī ���� �ƴϸ�.
        if (!matchingInProgress)
        {
            Match_Btn.GetComponent<Image>().color = Color.cyan;
            Match_Btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "��ġ ���";

            // ��ġ ��û ���� �ڵ�

            //
        }
        else
        {
            Match_Btn.GetComponent<Image>().color = new Color(0.25f,0.5f,1f,1f);
            Match_Btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "��ġ ����";

            // ��ġ ��û ��� ���� �ڵ�

            //
        }
        matchingInProgress = !matchingInProgress;
    }    

    public void load_UserInfo()
    {
        // �κ� ĳ���� ���� �ҷ����� �Լ�.
        // ĳ���� �̸� ��Ʈ�� �� ����. ĳ���� ��ŷ ��Ʈ �� ����.
        //UserName_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
        //UserRank_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = int.Parse("");
    }

    public void load_UserRankInfo()
    {
        // ��ũ ���� �������� �Լ�.

        Transform content = Rank_UI.transform.Find("Scroll View").Find("Viewport").Find("Content").transform;
        
        //for(int i = 0; i < "������ ����"; i++)
        //{
        //    content.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // ��ŷ.
        //    content.GetChild(i).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // ���̵�
        //    content.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // ������.
        //}        
    }
}
