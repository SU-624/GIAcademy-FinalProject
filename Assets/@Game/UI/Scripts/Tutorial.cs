using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<string> NameTutorial = new List<string>();
    public List<string> ClassTutorial = new List<string>();
    public List<string> GameJamTutorial = new List<string>();
    public List<string> GameShowTutorial = new List<string>();

    void Awake()
    {
        // PD : 0~1 -> 원장명 -> PD : 2~5 -> 학원명 ->  PD : 6~8
        NameTutorial.Add("안녕하세요! 이번에 새로온 원장님이시군요!");
        NameTutorial.Add("전 이 아카데미의 팀장을 맡고있는 제시카라고합니다. 원장님의 성함은 어떻게되시죠?");
        NameTutorial.Add("[유저지정_원장명]원장님 이시군요. 반갑습니다!");
        NameTutorial.Add("경영시작에 앞서, 전 원장님이 실시한 만족도 조사에서 학원이름이 너무 구리다는 평이 많았습니다.");
        NameTutorial.Add("근데도 GI서당을 고집하셨는데.. 이젠 새출발하는 마음으로 바꿨으면 해요.");
        NameTutorial.Add("원장님이 생각하시는 학원명은 무엇인가요?");
        NameTutorial.Add("와! 전 정말 마음에 쏙 드는데요?");
        NameTutorial.Add("앞으로 [유저지정_아카데미명] 이(가) 다시 1위의 영광을 찾을 수 있도록 최선을 다해 돕겠습니다.");
        NameTutorial.Add("잘 부탁드립니다!");

        // PD : 0~3 -> 4 -> 5 -> 6(강사 클릭) -> 7 -> 8(수업 클릭) -> 9(완료 클릭) -> 10 -> 수업 시작 -> PD : 11~13
        ClassTutorial.Add("우선 신입생을 맞이하기위해 이번 달 커리큘럼을 설정해보죠.");
        ClassTutorial.Add("1,2주차에 진행 할 수업과 원하는 강사를 지정해주세요.");
        ClassTutorial.Add("참, 수업마다 정해진 수업료가 있어서 재화가 부족하면 수업을 진행할 수 없습니다.");
        ClassTutorial.Add("보유중인 재화에 신경쓰며 진행하세요. ※커리큘럼은 한 달마다 새롭게 짜야합니다.");
        ClassTutorial.Add("현재 지정중인 학과와 주차입니다.");
        ClassTutorial.Add("보유중인 재화현황입니다.");
        ClassTutorial.Add("수업을 진행할 강사를 고를 수 있습니다. 강사의 레벨이 높을수록 많은 보너스 스탯을 얻습니다.");
        ClassTutorial.Add("이곳에서 진행할 수업을 고를 수 있습니다.");
        ClassTutorial.Add("수업마다 상승하는 스탯종류와 양이 다르며 진행에 필요한 재화에도 차이가 있습니다.");
        ClassTutorial.Add("한 학과의 한 주차 수업지정을 완료하면 다음학과의 수업을 지정해주어야합니다.");
        ClassTutorial.Add("2주차까지 모든 커리큘럼지정을 완료해보세요.");
        ClassTutorial.Add("좋습니다! 이번 달 커리큘럼은 이렇게 짜둘게요.");
        ClassTutorial.Add("게임을 진행할수록 더욱 다양한 수업들을 진행할 수 있답니다.");
        ClassTutorial.Add("중복수업도 가능하니 올리고 싶은 스탯이 있다면 동일수업을 두 번 실행하는것도 나쁘지 않은 선택이에요.");

        // PD : 0~3 -> 슬라이드 게임잼 클릭 -> 4(게임잼 클릭) -> 
        GameJamTutorial.Add("원장님! 올해 첫 게임잼이 개최되었다는 소식입니다. 열심히 공부한 학생들의 실력을 뽐낼 시간이에요!");
        GameJamTutorial.Add("학생들을 게임잼에 참가시켜 게임을 만들 수 있습니다.");
        GameJamTutorial.Add("게임잼의 특성, 성격을 고려하여 적절한 학생들을 참여시켜 보세요.");
        GameJamTutorial.Add("만든 게임의 저작권은 학생들이 졸업하기 전까지 우리 학원에 속하니 걱정마세요!	");
        GameJamTutorial.Add("진행예정인 게임잼 목록을 확인할 수 있습니다.");
        GameJamTutorial.Add("해당 게임잼을 참여할 때 필요한 것, 참여결과로 얻는 것 등 세부정보를 확인할 수 있습니다.");
        GameJamTutorial.Add("게임잼에 참가할 학생을 선택할 수 있습니다. ");
        GameJamTutorial.Add("한 학과당 한명의 학생만 선택가능하며, 한 학생당 한 달에 1번만 참여할 수 있습니다.");
        GameJamTutorial.Add("해당 게임에서 우대하는 장르입니다.");
        GameJamTutorial.Add("예상게임 장르와 1개이상 일치할 떄, 보너스 점수를 받습니다.");
        GameJamTutorial.Add("게임잼을 통해 얻을 수 있는 보상입니다. 결과에따라 양이 달라집니다.");
        GameJamTutorial.Add("게임잼이 시작되는 날짜입니다.");
        GameJamTutorial.Add("게임잼에 참여하는 학생들의 체력과 열정이 하락합니다.");
        GameJamTutorial.Add("만들어질 게임에 대한 예상장르입니다.");
        GameJamTutorial.Add("참여하는 학생들의 장르관심도와 관련있습니다.");
        GameJamTutorial.Add("게임이 정상적으로 만들어질 확률입니다.");
        GameJamTutorial.Add("확률이 낮으면 미완성 게임이 만들어지며, 보상을 받을 수 없고 폐기됩니다.");
        GameJamTutorial.Add("게임잼에서 요구하는 스탯의 최소요구치입니다.");
        GameJamTutorial.Add("각 파트별 요구하는 스탯의 영향을 받으며 완성확률과 만들어지는 게임 질에 영향을 줍니다.");
        GameJamTutorial.Add("최고의 강사진들에게 교육받은 똘똘한 학생들이니 분명 멋진 결과가 있을겁니다.");
        GameJamTutorial.Add("게임잼은 일주일간 진행되니 조금만 기다려보죠.");

        GameShowTutorial.Add("원장님! 올해 첫 게임쇼가 개최되었다는 소식입니다. 멋진 게임쇼에 우리가 빠지면 섭섭하겠죠?");
        GameShowTutorial.Add("게임잼에서 만든 멋진 게임을 전시해봅시다.");
        GameShowTutorial.Add("참, 게임쇼에 가면 학생들이 만든 게임에 대한 대중들의 반응을 살펴볼 수 있습니다.");
        GameShowTutorial.Add("진행예정인 게임쇼 목록입니다.");
        GameShowTutorial.Add("게임쇼이름과 난이도입니다.");
        GameShowTutorial.Add("난이도가 높을수록 까다롭지만 상금이 두둑합니다.");
        GameShowTutorial.Add("게임쇼의 주최사, 진행일자, 획득보상 등을 확인할 수 있습니다.");
        GameShowTutorial.Add("해당 게임쇼에서 중시하는 요소들입니다.");
        GameShowTutorial.Add("게임잼에서 만든 게임들의 목록입니다.");
        GameShowTutorial.Add("제출하고자 하는 게임을 선택하고 신청을 누르면 게임쇼에 참가할 수 있습니다.");
    }
    
}
