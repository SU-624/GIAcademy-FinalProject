using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<string> StartTutorial = new List<string>();
    public List<string> NameTutorial = new List<string>();
    public List<string> ClassTutorial = new List<string>();
    public List<string> GameJamTutorial = new List<string>();
    public List<string> GameShowTutorial = new List<string>();
    public List<string> InJaeRecommendTutorial = new List<string>();
    public List<string> VacationTutorial = new List<string>();
    public List<string> UpgradeTutorial = new List<string>();

    public List<List<string>> VacationNormal = new List<List<string>>();

    // 0 : 결과 스크립트1, 1 : 결과 스크립트2_1년차, 2 : 결과스크립트2_합격률 높을 경우, 3: 결과스크립트2_기본
    public List<List<string>> RecommendResult = new List<List<string>>();


    void Awake()
    {
        StartTutorial.Add("아, 오셨군요. 원장님 자제분이시죠? 젊고 패기로운 청년이라고 소문이 자자했답니다.");
        StartTutorial.Add("저는 앞으로 원장님을 도와 학원을 경영할 총괄 매니저 한아름이라고해요. 잘부탁드려요!");
        StartTutorial.Add("저..제가 이름을 깜빡해서 그런데 원장님 성함이 어떻게 되시죠?");
        StartTutorial.Add("아! 듣고나니 기억났어요. 요즘 정신이 오락가락하네요. 아무쪼록 잘부탁드립니다.");
        StartTutorial.Add("알고계시겠지만 저희는 게임인재를 육성하는 교육기관이에요.");
        StartTutorial.Add("혹시 게임이 어떻게 만들어지는지 알고계시나요?");
        StartTutorial.Add("게임은 기획");
        StartTutorial.Add("아트");
        StartTutorial.Add("프로그래밍 세 파트가 협업하여 만들어져요.");
        StartTutorial.Add("저희는 각 파트의 최고가 될 꿈나무들을 육성하고있답니다.");
        StartTutorial.Add("하지만 지금 GI서당의 상황이 그렇게 좋지만은 않아요..");
        StartTutorial.Add("연이은 마이너스 실적에 학생들의 대거이탈까지 생겨나고있죠.");
        StartTutorial.Add("이건 최근 10년간의 실적 보고서에요. ");
        StartTutorial.Add("매년 게임교육기관 전국 1위를 놓치지 않던 GI서당이 어떻게 이렇게 되었는지..");
        StartTutorial.Add("그.러.나! 칠전팔기! 저 한아름과 [유저지정_원장명]원장님이 함께 이 곳을 다시 일으키면 됩니다!");
        StartTutorial.Add("우리의 목표는 1위 탈환!!");
        StartTutorial.Add("아참, [유저지정_원장명]원장님, 새출발 새마음으로 이번기회에 GI서당이라는 이름도 바꾸는게 어떨까요?");
        StartTutorial.Add("[유저지정_아카데미명]아카데미..작명센스가 돋보이는 학원명이네요! 너무 마음에 들어요.");
        StartTutorial.Add("우선 기본적인 교육 시스템부터 알려드릴게요.");
        StartTutorial.Add("저희 아카데미는 1~2주차에 수업, 3~4주차에 학생자율학습을 지향하고 있어요.");
        StartTutorial.Add("매월 첫째주가되면 학생들의 1,2주차 수업을 지정해주어야합니다.");
        StartTutorial.Add("수업을 모두 들은 학생들은 자율학습을 하는데요, ");
        StartTutorial.Add("어머! 내 정신좀 봐! 아카데미 내부를 보여드리지 않았군요?");
        StartTutorial.Add("하나씩 살펴볼까요?");
        StartTutorial.Add("이곳이 교실이에요. 학생들이 수업을 듣는 공간이죠.");
        StartTutorial.Add("여기는 장르룸! 이곳에서 장르관심도를 키운답니다.");
        StartTutorial.Add("학생들은 쉬고싶을때 부대시설을 이용하며 체력을 회복해요.");
        StartTutorial.Add("물론, 판매비용은 아카데미 수익이 되는거죠.");
        StartTutorial.Add("휴! 더 많은 정보는 아카데미를 운영하며 알려드릴게요.");
        StartTutorial.Add("앗, 시간이 벌써 이렇게? 매년 3월달이되면 신입생이 들어온답니다.");
        StartTutorial.Add("신입생 맞을 준비를 해볼까요?");


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
        ClassTutorial.Add("우선 이번 달 커리큘럼을 설정하죠.");
        ClassTutorial.Add("1,2주차에 진행 할 수업과 강사를 지정해주세요.");
        ClassTutorial.Add("참, 재화가 부족하면 수업을 진행할 수 없답니다. 이 점, 유의해주세요.");
        ClassTutorial.Add("현재 지정중인 학과와 주차입니다.");
        ClassTutorial.Add("보유중인 재화현황입니다.");
        ClassTutorial.Add("수업을 진행할 강사를 고를 수 있습니다.");
        ClassTutorial.Add("강사의 레벨이 높을수록 많은 보너스 스탯을 얻는답니다.");
        ClassTutorial.Add("이곳에서 진행할 수업을 고를 수 있습니다.");
        ClassTutorial.Add("수업마다 상승하는 스탯종류와 양이 다르며 진행에 필요한 재화에도 차이가 있습니다.");
        ClassTutorial.Add("한 학과의 한 주차 수업지정을 완료하면 다음학과의 수업을 지정해주어야합니다.");
        ClassTutorial.Add("2주차까지 모든 커리큘럼지정을 완료해보세요.");
        ClassTutorial.Add("좋습니다! 이번 달 커리큘럼은 이렇게 짜둘게요.");
        ClassTutorial.Add("게임을 진행할수록 더욱 다양한 수업들을 진행할 수 있답니다.");
        ClassTutorial.Add("중복수업도 가능하니 올리고 싶은 스탯이 있다면 동일수업을 두 번 실행하는것도 나쁘지 않은 선택이에요.");
        
        GameJamTutorial.Add("원장님! 올해 첫 게임잼이 개최되었다는 소식입니다. 열심히 공부한 학생들의 실력을 뽐낼 시간이에요!");
        GameJamTutorial.Add("학생들을 게임잼에 참가시켜 게임을 만들 수 있습니다.");
        GameJamTutorial.Add("게임잼의 특성, 성격을 고려하여 적절한 학생들을 참여시켜 보세요.");
        GameJamTutorial.Add("만든 게임의 저작권은 학생들이 졸업하기 전까지 우리 학원에 속하니 걱정마세요!	");
        GameJamTutorial.Add("진행예정인 게임잼 목록을 확인할 수 있습니다.");
        GameJamTutorial.Add("해당 게임잼을 참여할 때 필요한 것, 참여결과로 얻는 것 등 세부정보를 확인할 수 있습니다.");
        GameJamTutorial.Add("게임잼에 참가할 학생을 선택할 수 있습니다. ");
        GameJamTutorial.Add("학생당 한 달에 1번만 참여할 수 있습니다.");
        GameJamTutorial.Add("게임잼에서 우대하는 장르와 참여보상입니다.");
        GameJamTutorial.Add("게임잼 일정과 참여시 감소되는 재화입니다.");
        GameJamTutorial.Add("선택한 학생들로 참여시 게임 완성 확률과 예상장르입니다.");
        GameJamTutorial.Add("게임잼에서 요구하는 최소 능력치입니다.");
        GameJamTutorial.Add("최고의 강사진들에게 교육받은 똘똘한 학생들이니 분명 멋진 결과가 있을겁니다.");
        GameJamTutorial.Add("게임잼은 3일간 진행되니 조금만 기다려보죠.");

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
        GameShowTutorial.Add("과연 우리학생이 만든 게임은 어떤 평가를 받게될까요? 벌써부터 떨리네요!");

        InJaeRecommendTutorial.Add("원장님! 벌써 1년이 지났습니다.");
        InJaeRecommendTutorial.Add("매년 2월이 되면 학원에서의 수학을 모두 마친 학생들을 제휴중인 회사에 추천할 수 있습니다.");
        InJaeRecommendTutorial.Add("우리 학원에서 최상의 교육을 받은 학생들을 무사히 현업에 정착할 수 있게끔 끝까지 도와주는거죠!");
        InJaeRecommendTutorial.Add("인재추천은 2월 한달 내내 진행되며 좋은 회사에 추천해서 합격할수록 우리 학원의 점수도 올라간답니다.");
        InJaeRecommendTutorial.Add("박탈감을 느끼지 않도록 모든 학생들을 추천해주세요.");
        InJaeRecommendTutorial.Add("추천 할 수 있는 회사 목록입니다. 높은 등급의 회사일수록 고급 인재를 원합니다.");
        InJaeRecommendTutorial.Add("터치 시 해당 회사의 현재 모집 공고를 볼 수 있습니다.");
        InJaeRecommendTutorial.Add("공고 목록입니다. 회사명과 모집하는 직군을 확인할 수 있습니다.");
        InJaeRecommendTutorial.Add("일부 공고는 특정 학과의 학생만을 원하기도 합니다.");
        InJaeRecommendTutorial.Add("공고 개요입니다.");
        InJaeRecommendTutorial.Add("공고에서 요구하는 학과와 능력입니다.");
        InJaeRecommendTutorial.Add("하나라도 만족하지 못하는 학생은 추천할 수 없습니다.");
        InJaeRecommendTutorial.Add("공고에서 요구하는 스탯 최소치입니다.");
        InJaeRecommendTutorial.Add("부족할수록 추천 성공 확률이 감소합니다.");
        InJaeRecommendTutorial.Add("추천하고자하는 학생을 선택할 수 있습니다.");
        InJaeRecommendTutorial.Add("한번에 여러명의 학생을 선택할 수 있습니다.");
        InJaeRecommendTutorial.Add("선택한 학생을 추천했을때 성공할 확률입니다.");
        InJaeRecommendTutorial.Add("이제 자유롭게 다양한 회사에 학생들을 추천해보세요.");

        VacationTutorial.Add("어느덧 학생들의 졸업과 방학이 함께 찾아왔네요.");
        VacationTutorial.Add("이제 3월달엔 새로운 신입생이 들어올텐데, 새롭게 시작을 하기에 앞서 휴식도 필요하겠죠?");
        VacationTutorial.Add("[유저지정_아카데미명]은 한 주 방학기간을 갖게 될 거에요.");
        VacationTutorial.Add("이번 한해 고생많으셨습니다. [유저_원장명] 원장님.");
        VacationTutorial.Add("한 주 편히 쉬시고 오세요!");

        UpgradeTutorial.Add("그럼 재정상황이 조금이나마 여유로워졌으니 아카데미를 한 단계 업그레이드 시켜볼까요?");
        UpgradeTutorial.Add("야호! 장르방이 한층 멋있어졌어요!");
        UpgradeTutorial.Add("이제 학생들이 더 멋진 환경에서 공부할 수 있게 되었네요~");

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("벌써 일 년이 지나갔네요.");
                newScripts.Add("이번 해도 고생많으셨습니다. [유저_원장명]원장님.");
                newScripts.Add("다음 주에 뵈어요!");
                VacationNormal.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("[유저_원장명]원장님, 이번 해도 수고하셨습니다.");
                newScripts.Add("푹 쉬시고 다음주에 뵙겠습니다!");
                VacationNormal.Add(newScripts);
            }
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("어느덧 방학이 찾아왔네요.");
                newScripts.Add("[유저_원장명]원장님, 이번 해도 고생 많으셨습니다.");
                VacationNormal.Add(newScripts);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("한 주 편히 쉬고 오셨나요?");
                newScripts.Add("방학 기간 동안 인재추천 결과가 나왔답니다.");
                newScripts.Add("확인해보시죠!");
                RecommendResult.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("드디어 학생들이 [유저_아카데미명]을 떠나 게임 업계의 개발자로서 활동하게 됩니다.");
                newScripts.Add("뿌듯하면서도 이 아쉬운 기분은 뭘까요?");
                newScripts.Add("그나저나 이제 곧 3월이네요.\n앞으로 들어올 신입생들은 어떤 학생들일까요?");
                newScripts.Add("새롭게 시작될 한 해를 잘 준비해봅시다!");
                RecommendResult.Add(newScripts);
            }
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("인재추천 결과가 좋네요!\n학생들이 좋은 결과를 얻어서 기뻐요.");
                newScripts.Add("이제 곧 3월 이네요.\n앞으로 들어올 신입생들은 어떤 학생들일까요?");
                newScripts.Add("새롭게 시작될 한 해를 잘 준비해봅시다.");
                RecommendResult.Add(newScripts);
            }
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("이렇게 한 해가 마무리 되었습니다.");
                newScripts.Add("사회에 나가게 될 학생들의 무운을 빌어봐요.");
                newScripts.Add("새롭게 시작될 한 해! 이번 해도 힘내보죠 [유저_원장명]원장님!");
                RecommendResult.Add(newScripts);
            }
        }
    }
}
