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
        // 매칭 중이 아니면.
        if (!matchingInProgress)
        {
            Match_Btn.GetComponent<Image>().color = Color.cyan;
            Match_Btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "매치 취소";

            // 매치 신청 관련 코드

            //
        }
        else
        {
            Match_Btn.GetComponent<Image>().color = new Color(0.25f,0.5f,1f,1f);
            Match_Btn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "매치 참가";

            // 매치 신청 취소 관련 코드

            //
        }
        matchingInProgress = !matchingInProgress;
    }    

    public void load_UserInfo()
    {
        // 로비 캐릭터 정보 불러오는 함수.
        // 캐릭터 이름 스트링 값 대입. 캐릭터 랭킹 인트 값 대입.
        //UserName_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = 
        //UserRank_UI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = int.Parse("");
    }

    public void load_UserRankInfo()
    {
        // 랭크 정보 물러오는 함수.

        Transform content = Rank_UI.transform.Find("Scroll View").Find("Viewport").Find("Content").transform;
        
        //for(int i = 0; i < "데이터 갯수"; i++)
        //{
        //    content.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // 랭킹.
        //    content.GetChild(i).transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // 아이디
        //    content.GetChild(i).transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = ""; // 레이팅.
        //}        
    }
}
