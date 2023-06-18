using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsManager : MonoBehaviour
{
    private static ScriptsManager instance = null;

    public static ScriptsManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    // 학생
    public List<List<string>> GenreRoomScripts = new List<List<string>>();      // 장르방 8종류
    public List<string> RepairScripts = new List<string>();                     // 장르방 수리 스크립트
    public List<List<string>> FacilityScripts = new List<List<string>>();       // 시설 4종류
    public List<string> StudyRoom2M = new List<string>();                       // 시설 4종류 중 자습실만 2월에 달라짐
    public List<List<string>> ObjectScripts = new List<List<string>>();         // 오브젝트 5종류
    public List<List<string>> ClassScripts = new List<List<string>>();          // 학과수업 3종류, 2월 3종류
    public List<List<string>> InteractionScripts = new List<List<string>>();    // 학생간 친밀도별 3종류
    public List<string> FreeWalkScripts = new List<string>();                   // 자유이동시
    public List<string> FreeWalkScripts2 = new List<string>();                  // 2월 자유이동시
    public List<List<string>> VacationScripts = new List<List<string>>();       // 방학 인사, 방학 이동

    // 강사
    public List<List<string>> ProfScripts = new List<List<string>>();           // 학과수업 3종류, 2월 3종류
    public List<List<string>> StuProScripts = new List<List<string>>();         // 학생-강사 친밀도별 3종류
    public List<List<string>> ProStuScripts = new List<List<string>>();         // 강사-학생 친밀도별 3종류
    public List<List<string>> ProProScripts = new List<List<string>>();         // 강사-강사 기본, 2월

    // 인재추천 기간 대화 스크립트들
    public List<List<string>> CStuCStuScripts = new List<List<string>>();       // 인재추천 완 학생 - 인재추천 완 학생 친밀도별 3종류
    public List<List<string>> CStuIStuScripts = new List<List<string>>();       // 인재추천 완 학생 - 인재추천 미완 학생 친밀도별 3종류
    public List<List<string>> IStuCStuScripts = new List<List<string>>();       // 인재추천 미완 학생 - 인재추천 완 학생 친밀도별 3종류
    public List<List<string>> IStuIStuScripts = new List<List<string>>();       // 인재추천 미완 학생 - 인재추천 미완 학생 친밀도별 3종류

    public List<List<string>> CStuProScripts = new List<List<string>>();        // 인재추천 완 학생 - 강사 친밀도별 3종류
    public List<List<string>> IStuProScripts = new List<List<string>>();        // 인재추천 미완 학생 - 강사 친밀도별 3종류
    public List<List<string>> ProCStuScripts = new List<List<string>>();        // 강사 - 인재추천 완 학생 친밀도별 3종류
    public List<List<string>> ProIStuScripts = new List<List<string>>();        // 강사 - 인재추천 미완 학생 친밀도별 3종류

    //public List<List<string>> ProfFacilityScripts = new List<List<string>>();

    public Sprite[] Emoticons;
    public Sprite[] GenreSprites;
    public Sprite[] StatSprites;
    public Sprite[] OtherSprites; // 친밀도, 체력, 열정

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 장르방
        for (int i = 0; i < 8; i++)
        {
            // 퍼즐
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("풀었다!");
                newScripts.Add("꿀잼~");
                newScripts.Add("똑똑해진다!");
                newScripts.Add("생각해,생각");
                newScripts.Add("머리아파");
                GenreRoomScripts.Add(newScripts);
            }
            // 시뮬레이션
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("돈을벌자$");
                newScripts.Add("양털깎기~");
                newScripts.Add("재화운용!");
                newScripts.Add("평화롭구만");
                newScripts.Add("재밌네");
                newScripts.Add("얼마에팔지?");
                newScripts.Add("노잼");
                GenreRoomScripts.Add(newScripts);
            }
            // 리듬
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("둠칫둠칫♬");
                newScripts.Add("이리듬은..");
                newScripts.Add("쏘울이느껴져!");
                newScripts.Add("씐단다~");
                newScripts.Add("오예♬");
                newScripts.Add("날따라해봐요~");
                newScripts.Add("노래가별로..");
                GenreRoomScripts.Add(newScripts);
            }
            // 어드벤쳐
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("보물은 어디에?");
                newScripts.Add("모험을 떠나요~");
                newScripts.Add("귀여워!");
                newScripts.Add("안녕!요정님~");
                newScripts.Add("신나는모험!");
                newScripts.Add("재미없어...");
                GenreRoomScripts.Add(newScripts);
            }
            // RPG
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("와아★");
                newScripts.Add("재밌어!");
                newScripts.Add("오우야");
                newScripts.Add("RPG짱!");
                newScripts.Add("하암..");
                newScripts.Add("힘내 용가리!");
                newScripts.Add("용사화이팅!");
                newScripts.Add("재미없어...");
                GenreRoomScripts.Add(newScripts);
            }
            // 스포츠
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("뜨거운코트를~");
                newScripts.Add("박진감넘쳐!");
                newScripts.Add("휘잇!");
                newScripts.Add("멋진경기");
                newScripts.Add("영감이떠오른다");
                newScripts.Add("손에땀난다");
                newScripts.Add("나랑안맞아");
                GenreRoomScripts.Add(newScripts);
            }
            // 액션
            else if (i == 6)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("얍얍");
                newScripts.Add("잘한다");
                newScripts.Add("멋져!");
                newScripts.Add("예에~");
                newScripts.Add("액션쾌감!");
                newScripts.Add("폭력적이야");
                GenreRoomScripts.Add(newScripts);
            }
            // 슈팅
            else if (i == 7)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("에임실화?");
                newScripts.Add("날아라비행기!");
                newScripts.Add("피융피융~");
                newScripts.Add("탕탕!");
                newScripts.Add("피해!");
                newScripts.Add("별로야");
                GenreRoomScripts.Add(newScripts);
            }
        }

        // 장르방 수리
        RepairScripts.Add("수리해줘!!!");
        RepairScripts.Add("고장났네");
        RepairScripts.Add("공부도 못하게하네");
        RepairScripts.Add("수리해주세요");
        RepairScripts.Add("들어가고싶어");
        RepairScripts.Add("도대체 몇번째야!!!");
        RepairScripts.Add("다른곳 가야지");
        RepairScripts.Add("언제 고쳐?");
        RepairScripts.Add("고쳐줘");

        // 시설
        for (int i = 0; i < 5; i++)
        {
            // 매점
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("폭행몬 빵!");
                newScripts.Add("이거살까?");
                newScripts.Add("흠");
                newScripts.Add("배고프다");
                newScripts.Add("이거주세요");
                newScripts.Add("너무비싸요");
                FacilityScripts.Add(newScripts);
            }
            // 서점
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("멋진 구절이야");
                newScripts.Add("심오하네");
                newScripts.Add("신작이다!");
                newScripts.Add("마음의 양식");
                newScripts.Add("엄청두껍네");
                newScripts.Add("이거주세요");
                newScripts.Add("이걸로할게요");
                FacilityScripts.Add(newScripts);
            }
            // 자습실
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("zZz");
                newScripts.Add("출튀마렵다...");
                newScripts.Add("하기싫어");
                newScripts.Add("깨달았다!");
                newScripts.Add("쉽다쉬워");
                FacilityScripts.Add(newScripts);
                
                StudyRoom2M.Add("zZz");
                StudyRoom2M.Add("와 어려워");
                StudyRoom2M.Add("할 수..없다!");
                StudyRoom2M.Add("와 이거였네");
                StudyRoom2M.Add("오오옷!");
            }
            // 라운지 1
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("아으좋다~");
                newScripts.Add("꿀잼!");
                newScripts.Add("휴식시간♬");
                newScripts.Add("신난다!");
                newScripts.Add("휴~");
                FacilityScripts.Add(newScripts);
            }
            // 라운지 2
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("아으좋다~");
                newScripts.Add("꿀잼!");
                newScripts.Add("휴식시간♬");
                newScripts.Add("신난다!");
                newScripts.Add("휴~");
                FacilityScripts.Add(newScripts);
            }
        }

        // 오브젝트
        for (int i = 0; i < 5; i++)
        {
            // 자판기
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("시원해");
                newScripts.Add("벌컥벌컥");
                ObjectScripts.Add(newScripts);
            }
            // 화분
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("향기좋다");
                newScripts.Add("쑥쑥커라~");
                newScripts.Add("벌레다!");
                newScripts.Add("돈이다!");
                ObjectScripts.Add(newScripts);
            }
            // 게임기
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("재밌당!");
                newScripts.Add("즐거워");
                newScripts.Add("앗 죽었다..");
                ObjectScripts.Add(newScripts);
            }
            // 정수기
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("살것같아");
                newScripts.Add("후~");
                newScripts.Add("시원해");
                ObjectScripts.Add(newScripts);
            }
            // 게시판
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("이것은 게시판이여");
                newScripts.Add("게시판에 아무것도 없어");
                ObjectScripts.Add(newScripts);
            }
        }

        // 교실
        for (int i = 0; i < 6; i++)
        {
            // 기획
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("좋은 아이디어가 떠올랐어!");
                newScripts.Add("나는 아무 생각이 없다...");
                newScripts.Add("수업이 지루해요");
                ClassScripts.Add(newScripts);
            }
            // 아트
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("와! 내가 봐도 개잘그림");
                newScripts.Add("손목이 아파...");
                newScripts.Add("모델링 꿀잼~");
                ClassScripts.Add(newScripts);
            }
            // 프로그래밍
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("01101");                 
                newScripts.Add("0111");         
                newScripts.Add("01111100"); 
                ClassScripts.Add(newScripts);
            }
            // 2월
            // 기획
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("디..자인");
                newScripts.Add("어렵다");
                newScripts.Add("기획의도는?");
                newScripts.Add("소통이 중요해");
                newScripts.Add("손목이 아파");
                ClassScripts.Add(newScripts);
            }
            // 아트
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("아..트");
                newScripts.Add("덩어리감..");
                newScripts.Add("AI어떡하지");
                newScripts.Add("디테일 살리자");
                newScripts.Add("허리 아프다");
                ClassScripts.Add(newScripts);
            }
            // 프로그래밍
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("프로그래밍..");
                newScripts.Add("할 수 있나?");
                newScripts.Add("고통스러워");
                newScripts.Add("1010101");
                newScripts.Add("눈이 아프네");
                ClassScripts.Add(newScripts);
            }
        }

        // 학생끼리
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕?/안녕ㅎ");
                newScripts.Add("좋은날~/너두~");
                newScripts.Add("우리친구하자/좋아");
                newScripts.Add("처음보네/누구세요?");
                newScripts.Add("하이/하이");
                newScripts.Add("이름이?/가던길가쇼");
                newScripts.Add("똑똑/이상한애네");
                newScripts.Add("너 멋지다/고마워");
                newScripts.Add("나랑친구할래?/물론이지");
                newScripts.Add("나랑사귀자/꺼져");
                newScripts.Add("배고프다/나두");
                newScripts.Add("16/11");
                newScripts.Add("안녕?/16");
                InteractionScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("히사시부리/십덕아");
                newScripts.Add("안녕~/안녕!");
                newScripts.Add("요즘어때?/힘들어ㅠㅠ");
                newScripts.Add("잘지내?/웅웅!");
                newScripts.Add("좋아보이네/여친생겼어");
                newScripts.Add("좋은일있어?/남친생겼어");
                newScripts.Add("날씨좋다/밖에나갈래?");
                newScripts.Add("너무힘들어/같이힘내자!");
                newScripts.Add("나랑사귀자/생각좀해보고");
                newScripts.Add("밥은먹고다녀?/내 배를봐");
                newScripts.Add("오랜만이야!/16");
                newScripts.Add("16/16");
                newScripts.Add("반가워!/반가워!");
                InteractionScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("히사시부리!/여~");
                newScripts.Add("안녕하살법!/받아치기!");
                newScripts.Add("안녕!/안녕!");
                newScripts.Add("잘지내지?/그럼!");
                newScripts.Add("나랑사귀자/내가아깝지");
                newScripts.Add("배고파/시켜먹을까?");
                newScripts.Add("주말에뭐해?/수작ㄴ");
                newScripts.Add("주말에바빠?/나랑놀자!");
                newScripts.Add("주말에놀자!/벌써신나!");
                newScripts.Add("14/14");
                newScripts.Add("13/14");
                newScripts.Add("머리바꿨네?/14");
                InteractionScripts.Add(newScripts);
            }
        }

        // 복도
        FreeWalkScripts.Add("심심해..");
        FreeWalkScripts.Add("뭐하지?");
        FreeWalkScripts.Add("흠");
        FreeWalkScripts.Add("하암");
        FreeWalkScripts.Add("어딜갈까?");

        FreeWalkScripts2.Add("게임만들고싶다");
        FreeWalkScripts2.Add("하암..");
        FreeWalkScripts2.Add("취업하고싶다");
        FreeWalkScripts2.Add("놀고싶어");
        FreeWalkScripts2.Add("탈아카데미원츄");
        FreeWalkScripts2.Add("면접연습해야지");

        // 강사 교실 스크립트
        for (int i = 0; i < 3; i++)
        {
            // 기획
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("조용조용!");
                newScripts.Add("집중하세요");
                newScripts.Add("시스템은..");
                newScripts.Add("기획이란..");
                newScripts.Add("과제내줄게");
                newScripts.Add("흐음");
                newScripts.Add("하암..");
                newScripts.Add("쉽지않군.");
                ProfScripts.Add(newScripts);
            }
            // 아트
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("참 쉽죠?");
                newScripts.Add("이걸 왜 못해");
                newScripts.Add("비율이 중요해");
                newScripts.Add("덩어리감!");
                newScripts.Add("조용조용!");
                newScripts.Add("흐음");
                newScripts.Add("하암..");
                newScripts.Add("쉽지않군.");
                ProfScripts.Add(newScripts);
            }
            // 프로그래밍
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("여기잘봐봐");
                newScripts.Add("왜 안되지?");
                newScripts.Add("조용조용!");
                newScripts.Add("PBR이란..");
                newScripts.Add("JAVA란..");
                newScripts.Add("흐음");
                newScripts.Add("하암..");
                newScripts.Add("쉽지않군.");
                ProfScripts.Add(newScripts);
            }
            // 2월
            // 기획
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("요즘 트렌드는..");
                newScripts.Add("면접 일정이..");
                newScripts.Add("추천할 곳이..");
                newScripts.Add("곧 끝이군");
                newScripts.Add("흐음");
                newScripts.Add("쉽지 않아.");
                newScripts.Add("합격률이..");
                newScripts.Add("가독성 떨어져");
                ProfScripts.Add(newScripts);
            }
            // 아트
            else if (i == 4)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("부족해");
                newScripts.Add("왜 못하지?");
                newScripts.Add("쉽지 않군.");
                newScripts.Add("잘했네");
                newScripts.Add("이정도면..");
                newScripts.Add("무리야");
                newScripts.Add("추천할 곳이..");
                newScripts.Add("환상적이야");
                ProfScripts.Add(newScripts);
            }
            // 프로그래밍
            else if (i == 5)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("구현이 덜됐어");
                newScripts.Add("얘다!");
                newScripts.Add("서버는..");
                newScripts.Add("자체제작?");
                newScripts.Add("합격률이..");
                newScripts.Add("누굴 추천하지");
                newScripts.Add("코딩정리가..");
                newScripts.Add("멋진 코드야!");
                ProfScripts.Add(newScripts);
            }
        }

        // 학생-강사 대화
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕하세요/안녕~");
                newScripts.Add("쌤멋져요/고마워");
                newScripts.Add("있잖아요../저런..");
                newScripts.Add("16/11");
                newScripts.Add("이봐선생/예의지켜");
                StuProScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕하세요/안녕!");
                newScripts.Add("쌤!/16");
                StuProScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("놀러가요!/그럴까?!");
                newScripts.Add("저 지쳐요/힘내!!");
                newScripts.Add("재밌어요!/14");
                StuProScripts.Add(newScripts);
            }
        }

        // 강사-학생 대화
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("머리이쁘네./감사합니다.");
                newScripts.Add("밥잘먹고다녀/11");
                newScripts.Add("어렵진않니?/할만해요.");
                newScripts.Add("안녕/안녕하세요");
                newScripts.Add("힘들지?/괜찮아요");
                newScripts.Add("과제다했니?/10");
                ProStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("좋은아침!/쌤두요!");
                newScripts.Add("안녕?/안녕하세요!");
                newScripts.Add("살빠진것같다/진짜요?");
                newScripts.Add("열심히해/네!");
                newScripts.Add("응원한다/감사합니다!");
                ProStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕!/안녕하세요!");
                newScripts.Add("재밌지?/네!");
                newScripts.Add("힘들지?/아뇨!");
                newScripts.Add("항상 응원해/고마워요");
                newScripts.Add("과제했니?/18");
                ProStuScripts.Add(newScripts);
            }
        }

        // 강사-강사 대화
        for (int i = 0; i < 2; i++)
        {
            // 3월 ~ 1월
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("좋은날입니다./그렇네요.");
                newScripts.Add("학생들 어때요?/잘따라와요.");
                newScripts.Add("화이팅!/20");
                newScripts.Add("힘드네요./화이팅");
                newScripts.Add("안녕하세요/안녕하세요");
                ProProScripts.Add(newScripts);
            }
            // 2월
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("고지가 보여요/그러게말입니다");
                newScripts.Add("이별이 오네요/슬프네요");
                newScripts.Add("잘 지내시죠?/네 물론이죠");
                newScripts.Add("이번 해도/고생하셨습니다");
                ProProScripts.Add(newScripts);
            }
        }

        // 인재추천 기간
        // 방학 인사, 이동
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("즐거웠어");
                newScripts.Add("다음에 봐!");
                newScripts.Add("좋은 결과 있길");
                newScripts.Add("아쉽다");
                newScripts.Add("회사에서 보자!");
                newScripts.Add("힘들었다");
                newScripts.Add("보고싶을거야");
                newScripts.Add("이별이네");
                newScripts.Add("또 보자");
                newScripts.Add("방학에 놀자");
                VacationScripts.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("벌써다시가고파");
                newScripts.Add("아싸게임해야지");
                newScripts.Add("이히히");
                newScripts.Add("게임 뭐하지");
                newScripts.Add("뭐하고 놀지");
                newScripts.Add("장르방가고파");
                newScripts.Add("너무 좋았어");
                newScripts.Add("두근두근");
                newScripts.Add("아쉬워...");
                newScripts.Add("너무 좋아");
                VacationScripts.Add(newScripts);
            }
        }

        // 완 학생 - 완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("잘했냐/몰라");
                newScripts.Add("합격하고싶어/나도");
                newScripts.Add("끝나면뭐할거야/놀거야");
                newScripts.Add("우리다시만날../...?");
                newScripts.Add("회사진짜좋더라/와진짜?어디");
                CStuCStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("될 것 같냐/안될듯");
                newScripts.Add("될 것 같아?/당연한거아님?");
                newScripts.Add("나 합격인듯/바로 소고기");
                newScripts.Add("지원동기뭐야/몰라 망했어");
                newScripts.Add("고생했어/고마워");
                newScripts.Add("면접 어땠어/완벽했지");
                CStuCStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("잘하고 왔어?/뿌앵");
                newScripts.Add("잘하고 왔어?/나 좀 잘한듯");
                newScripts.Add("나 합격인듯/아니 바로탈락");
                newScripts.Add("너 합격인듯/넌 탈락일듯ㅎ");
                newScripts.Add("고생많았어/고생많았다");
                newScripts.Add("이제집가고싶어/집가서놀자");
                newScripts.Add("밤샘게임고?/고~!!!");
                CStuCStuScripts.Add(newScripts);
            }
        }

        // 완 학생 - 미완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("나 지원서냈어/고생요");
                newScripts.Add("끝나고 뭐해?/잘거야");
                newScripts.Add("나 다녀왔음/오냐");
                newScripts.Add("나이제 끝났다/부럽네ㅋ");
                CStuIStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("준비 잘해/ㅠㅠ고마워");
                newScripts.Add("면접보고왔어/홀리쓋");
                newScripts.Add("나 다녀왔어/고생했다");
                newScripts.Add("요즘 어때/힘들어...");
                newScripts.Add("잘 되어가니/그래 보이냐");
                newScripts.Add("될 까/부럽다");
                CStuIStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("나 지원서냈어/보여줘봐");
                newScripts.Add("준비하고있어?/다음주 면접봐");
                newScripts.Add("끝나고 놀자/너무좋아!");
                newScripts.Add("면접보고왔어/어땠어?!");
                newScripts.Add("걱정된다/될거야");
                CStuIStuScripts.Add(newScripts);
            }
        }

        // 미완 학생 - 완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("꿀팁 알려줘/내가 왜?");
                newScripts.Add("면접 잘보는법/은 내가 모르지");
                newScripts.Add("너무 어려워/화이팅 해");
                newScripts.Add("떨려/힘내");
                IStuCStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("꿀팀좀 줘봐/돈 줘");
                newScripts.Add("면접 꿀팁 줘/주세요 해^.^");
                newScripts.Add("진짜 어렵다/뭐가 어려워?");
                newScripts.Add("도와줘/알려줄게!");
                newScripts.Add("떨려!!!/두근두근!");
                IStuCStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("허니팁!허니팁/안 사요");
                newScripts.Add("면접 어떻게해/알려줄게!");
                newScripts.Add("너무 어려워../도와줄게!");
                newScripts.Add("어떻게 했어?/이건 이렇게..");
                newScripts.Add("살려줘/음료수 사줄게");
                newScripts.Add("후.../하...");
                IStuCStuScripts.Add(newScripts);
            }
        }

        // 미완 학생 - 미완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("같이 공부하자/난 혼자가좋아");
                newScripts.Add("같이 할래?/노땡큐");
                newScripts.Add("잘 돼가?/그럭저럭");
                newScripts.Add("동기 뭐야/몰라 묻지마");
                newScripts.Add("넌 합격할 듯/고맙다");
                IStuIStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("매점 가자/콜");
                newScripts.Add("면접준비할래?/좋아 같이하자");
                newScripts.Add("잘 하고있어?/11");
                newScripts.Add("두근 두근/심장소리 들려");
                newScripts.Add("될까?/걱정된다");
                newScripts.Add("게임하고싶어/방학때 하자");
                IStuIStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("잠깐 쉬러가자/좋은 생각이야");
                newScripts.Add("편의점 가자/가자 가자");
                newScripts.Add("오늘집 언제가/집은 이곳이야");
                newScripts.Add("면접 준비하자/언제 할래");
                newScripts.Add("12/19");
                newScripts.Add("떨려/후...");
                newScripts.Add("두근두근/듀근듀근");
                IStuIStuScripts.Add(newScripts);
            }
        }

        // 완 학생 - 강사
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("쌤!/20");
                newScripts.Add("저 잘될까요?/잘 될거야");
                newScripts.Add("걱정돼요/걱정마");
                newScripts.Add("다녀왔어요/그래");
                newScripts.Add("안녕하세요/안녕");
                CStuProScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("감사해요 쌤/고생많았다");
                newScripts.Add("안녕하세요/잘 다녀왔어?");
                newScripts.Add("저희 놀러가요/합격한거아니다");
                newScripts.Add("날이 좋네요/그러게");
                CStuProScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("쌤 덕분이에요/고럼고럼");
                newScripts.Add("후련해요/장하다");
                newScripts.Add("히히/16");
                newScripts.Add("망한것 같아요/무슨 일이야");
                newScripts.Add("날이 좋아요/너무 좋아");
                newScripts.Add("신나요!/나도 신나네");
                CStuProScripts.Add(newScripts);
            }
        }

        // 미완 학생 - 강사
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("쌤!/20");
                newScripts.Add("어려워요/알려줄게");
                newScripts.Add("힘들어요/힘내자!");
                IStuProScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕하세요/안녕~");
                newScripts.Add("미래가 깜깜../불을 켜주마");
                newScripts.Add("놀러가요/끝나면 가자!");
                newScripts.Add("어떻게 해요?/도와줄게");
                IStuProScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("힘들어요/무슨 일이야");
                newScripts.Add("꿀팁 주세요/이 회사는..");
                newScripts.Add("추천해주세요/흠");
                newScripts.Add("안녕하세요/힘든건 없니?");
                newScripts.Add("밥 사주세요/뭐먹을래?");
                IStuProScripts.Add(newScripts);
            }
        }

        // 강사 - 완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕/안녕하세요!");
                newScripts.Add("좋은 아침/13");
                newScripts.Add("결과는 어때?/괜찮아요");
                newScripts.Add("고생했다/감사해요");
                ProCStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕!/안녕하세요!");
                newScripts.Add("좋은 아침이야/히히 그러게요");
                newScripts.Add("잘 보고왔니?/최선을 다했어요");
                newScripts.Add("고생 많았어/감사합니다");
                newScripts.Add("이제 놀겠네!/너무 행복해요");
                newScripts.Add("결과는 어때?/아직이에요");
                ProCStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("안녕~/안녕하세요!");
                newScripts.Add("밥은 먹었어?/네!!!");
                newScripts.Add("밥은 먹었니?/밥 사주세요");
                newScripts.Add("잘 다녀왔니?/괜찮은것같아요");
                newScripts.Add("고생 했어/감사해요 쌤");
                newScripts.Add("이제 놀겠네!/이제 시작이죠!");
                ProCStuScripts.Add(newScripts);
            }
        }

        // 강사 - 미완 학생
        for (int i = 0; i < 3; i++)
        {
            // 아는 사이
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("괜찮아?/살려주세요");
                newScripts.Add("준비해야해/알겠습니다");
                ProIStuScripts.Add(newScripts);
            }
            // 친한 사이
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("힘들지?/10");
                newScripts.Add("밥은 먹고다니니/네 아마도..");
                ProIStuScripts.Add(newScripts);
            }
            // 베프
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("좀 어떠니?/어려워요");
                newScripts.Add("준비는 다했니?/네! 다했어요");
                newScripts.Add("지원해보자/생각해볼게요");
                ProIStuScripts.Add(newScripts);
            }
        }
    }
}
