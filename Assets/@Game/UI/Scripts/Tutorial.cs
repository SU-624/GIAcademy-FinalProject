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

    // 0 : ��� ��ũ��Ʈ1, 1 : ��� ��ũ��Ʈ2_1����, 2 : �����ũ��Ʈ2_�հݷ� ���� ���, 3: �����ũ��Ʈ2_�⺻
    public List<List<string>> RecommendResult = new List<List<string>>();


    void Awake()
    {
        StartTutorial.Add("��, ���̱���. ����� �������̽���? ���� �б�ο� û���̶�� �ҹ��� �����ߴ�ϴ�.");
        StartTutorial.Add("���� ������ ������� ���� �п��� �濵�� �Ѱ� �Ŵ��� �ѾƸ��̶���ؿ�. �ߺ�Ź�����!");
        StartTutorial.Add("��..���� �̸��� �����ؼ� �׷��� ����� ������ ��� �ǽ���?");
        StartTutorial.Add("��! ����� ��ﳵ���. ���� ������ ���������ϳ׿�. �ƹ��ɷ� �ߺ�Ź�帳�ϴ�.");
        StartTutorial.Add("�˰��ð����� ����� �������縦 �����ϴ� ��������̿���.");
        StartTutorial.Add("Ȥ�� ������ ��� ����������� �˰��ó���?");
        StartTutorial.Add("������ ��ȹ");
        StartTutorial.Add("��Ʈ");
        StartTutorial.Add("���α׷��� �� ��Ʈ�� �����Ͽ� ���������.");
        StartTutorial.Add("����� �� ��Ʈ�� �ְ� �� �޳������� �����ϰ��ִ�ϴ�.");
        StartTutorial.Add("������ ���� GI������ ��Ȳ�� �׷��� �������� �ʾƿ�..");
        StartTutorial.Add("������ ���̳ʽ� ������ �л����� �����Ż���� ���ܳ�������.");
        StartTutorial.Add("�̰� �ֱ� 10�Ⱓ�� ���� ��������. ");
        StartTutorial.Add("�ų� ���ӱ������ ���� 1���� ��ġ�� �ʴ� GI������ ��� �̷��� �Ǿ�����..");
        StartTutorial.Add("��.��.��! ĥ���ȱ�! �� �ѾƸ��� [��������_�����]������� �Բ� �� ���� �ٽ� ����Ű�� �˴ϴ�!");
        StartTutorial.Add("�츮�� ��ǥ�� 1�� Żȯ!!");
        StartTutorial.Add("����, [��������_�����]�����, ����� ���������� �̹���ȸ�� GI�����̶�� �̸��� �ٲٴ°� ����?");
        StartTutorial.Add("[��������_��ī���̸�]��ī����..�۸����� �����̴� �п����̳׿�! �ʹ� ������ ����.");
        StartTutorial.Add("�켱 �⺻���� ���� �ý��ۺ��� �˷��帱�Կ�.");
        StartTutorial.Add("���� ��ī���̴� 1~2������ ����, 3~4������ �л������н��� �����ϰ� �־��.");
        StartTutorial.Add("�ſ� ù°�ְ��Ǹ� �л����� 1,2���� ������ �������־���մϴ�.");
        StartTutorial.Add("������ ��� ���� �л����� �����н��� �ϴµ���, ");
        StartTutorial.Add("���! �� ������ ��! ��ī���� ���θ� �����帮�� �ʾұ���?");
        StartTutorial.Add("�ϳ��� ���캼���?");
        StartTutorial.Add("�̰��� �����̿���. �л����� ������ ��� ��������.");
        StartTutorial.Add("����� �帣��! �̰����� �帣���ɵ��� Ű���ϴ�.");
        StartTutorial.Add("�л����� ��������� �δ�ü��� �̿��ϸ� ü���� ȸ���ؿ�.");
        StartTutorial.Add("����, �Ǹź���� ��ī���� ������ �Ǵ°���.");
        StartTutorial.Add("��! �� ���� ������ ��ī���̸� ��ϸ� �˷��帱�Կ�.");
        StartTutorial.Add("��, �ð��� ���� �̷���? �ų� 3�����̵Ǹ� ���Ի��� ���´�ϴ�.");
        StartTutorial.Add("���Ի� ���� �غ� �غ����?");


        // PD : 0~1 -> ����� -> PD : 2~5 -> �п��� ->  PD : 6~8
        NameTutorial.Add("�ȳ��ϼ���! �̹��� ���ο� ������̽ñ���!");
        NameTutorial.Add("�� �� ��ī������ ������ �ð��ִ� ����ī����մϴ�. ������� ������ ��Եǽ���?");
        NameTutorial.Add("[��������_�����]����� �̽ñ���. �ݰ����ϴ�!");
        NameTutorial.Add("�濵���ۿ� �ռ�, �� ������� �ǽ��� ������ ���翡�� �п��̸��� �ʹ� �����ٴ� ���� ���ҽ��ϴ�.");
        NameTutorial.Add("�ٵ��� GI������ �����ϼ̴µ�.. ���� ������ϴ� �������� �ٲ����� �ؿ�.");
        NameTutorial.Add("������� �����Ͻô� �п����� �����ΰ���?");
        NameTutorial.Add("��! �� ���� ������ �� ��µ���?");
        NameTutorial.Add("������ [��������_��ī���̸�] ��(��) �ٽ� 1���� ������ ã�� �� �ֵ��� �ּ��� ���� ���ڽ��ϴ�.");
        NameTutorial.Add("�� ��Ź�帳�ϴ�!");

        // PD : 0~3 -> 4 -> 5 -> 6(���� Ŭ��) -> 7 -> 8(���� Ŭ��) -> 9(�Ϸ� Ŭ��) -> 10 -> ���� ���� -> PD : 11~13
        ClassTutorial.Add("�켱 �̹� �� Ŀ��ŧ���� ��������.");
        ClassTutorial.Add("1,2������ ���� �� ������ ���縦 �������ּ���.");
        ClassTutorial.Add("��, ��ȭ�� �����ϸ� ������ ������ �� ����ϴ�. �� ��, �������ּ���.");
        ClassTutorial.Add("���� �������� �а��� �����Դϴ�.");
        ClassTutorial.Add("�������� ��ȭ��Ȳ�Դϴ�.");
        ClassTutorial.Add("������ ������ ���縦 �� �� �ֽ��ϴ�.");
        ClassTutorial.Add("������ ������ �������� ���� ���ʽ� ������ ��´�ϴ�.");
        ClassTutorial.Add("�̰����� ������ ������ �� �� �ֽ��ϴ�.");
        ClassTutorial.Add("�������� ����ϴ� ���������� ���� �ٸ��� ���࿡ �ʿ��� ��ȭ���� ���̰� �ֽ��ϴ�.");
        ClassTutorial.Add("�� �а��� �� ���� ���������� �Ϸ��ϸ� �����а��� ������ �������־���մϴ�.");
        ClassTutorial.Add("2�������� ��� Ŀ��ŧ�������� �Ϸ��غ�����.");
        ClassTutorial.Add("�����ϴ�! �̹� �� Ŀ��ŧ���� �̷��� ¥�ѰԿ�.");
        ClassTutorial.Add("������ �����Ҽ��� ���� �پ��� �������� ������ �� �ִ�ϴ�.");
        ClassTutorial.Add("�ߺ������� �����ϴ� �ø��� ���� ������ �ִٸ� ���ϼ����� �� �� �����ϴ°͵� ������ ���� �����̿���.");
        
        GameJamTutorial.Add("�����! ���� ù �������� ���ֵǾ��ٴ� �ҽ��Դϴ�. ������ ������ �л����� �Ƿ��� �˳� �ð��̿���!");
        GameJamTutorial.Add("�л����� �����뿡 �������� ������ ���� �� �ֽ��ϴ�.");
        GameJamTutorial.Add("�������� Ư��, ������ ����Ͽ� ������ �л����� �������� ������.");
        GameJamTutorial.Add("���� ������ ���۱��� �л����� �����ϱ� ������ �츮 �п��� ���ϴ� ����������!	");
        GameJamTutorial.Add("���࿹���� ������ ����� Ȯ���� �� �ֽ��ϴ�.");
        GameJamTutorial.Add("�ش� �������� ������ �� �ʿ��� ��, ��������� ��� �� �� ���������� Ȯ���� �� �ֽ��ϴ�.");
        GameJamTutorial.Add("�����뿡 ������ �л��� ������ �� �ֽ��ϴ�. ");
        GameJamTutorial.Add("�л��� �� �޿� 1���� ������ �� �ֽ��ϴ�.");
        GameJamTutorial.Add("�����뿡�� ����ϴ� �帣�� ���������Դϴ�.");
        GameJamTutorial.Add("������ ������ ������ ���ҵǴ� ��ȭ�Դϴ�.");
        GameJamTutorial.Add("������ �л���� ������ ���� �ϼ� Ȯ���� �����帣�Դϴ�.");
        GameJamTutorial.Add("�����뿡�� �䱸�ϴ� �ּ� �ɷ�ġ�Դϴ�.");
        GameJamTutorial.Add("�ְ��� �������鿡�� �������� �ʶ��� �л����̴� �и� ���� ����� �����̴ϴ�.");
        GameJamTutorial.Add("�������� 3�ϰ� ����Ǵ� ���ݸ� ��ٷ�����.");

        GameShowTutorial.Add("�����! ���� ù ���Ӽ ���ֵǾ��ٴ� �ҽ��Դϴ�. ���� ���Ӽ �츮�� ������ �����ϰ���?");
        GameShowTutorial.Add("�����뿡�� ���� ���� ������ �����غ��ô�.");
        GameShowTutorial.Add("��, ���Ӽ ���� �л����� ���� ���ӿ� ���� ���ߵ��� ������ ���캼 �� �ֽ��ϴ�.");
        GameShowTutorial.Add("���࿹���� ���Ӽ� ����Դϴ�.");
        GameShowTutorial.Add("���Ӽ��̸��� ���̵��Դϴ�.");
        GameShowTutorial.Add("���̵��� �������� ��ٷ����� ����� �ε��մϴ�.");
        GameShowTutorial.Add("���Ӽ��� ���ֻ�, ��������, ȹ�溸�� ���� Ȯ���� �� �ֽ��ϴ�.");
        GameShowTutorial.Add("�ش� ���Ӽ�� �߽��ϴ� ��ҵ��Դϴ�.");
        GameShowTutorial.Add("�����뿡�� ���� ���ӵ��� ����Դϴ�.");
        GameShowTutorial.Add("�����ϰ��� �ϴ� ������ �����ϰ� ��û�� ������ ���Ӽ ������ �� �ֽ��ϴ�.");
        GameShowTutorial.Add("���� �츮�л��� ���� ������ � �򰡸� �ްԵɱ��? ������� �����׿�!");

        InJaeRecommendTutorial.Add("�����! ���� 1���� �������ϴ�.");
        InJaeRecommendTutorial.Add("�ų� 2���� �Ǹ� �п������� ������ ��� ��ģ �л����� �������� ȸ�翡 ��õ�� �� �ֽ��ϴ�.");
        InJaeRecommendTutorial.Add("�츮 �п����� �ֻ��� ������ ���� �л����� ������ ������ ������ �� �ְԲ� ������ �����ִ°���!");
        InJaeRecommendTutorial.Add("������õ�� 2�� �Ѵ� ���� ����Ǹ� ���� ȸ�翡 ��õ�ؼ� �հ��Ҽ��� �츮 �п��� ������ �ö󰣴�ϴ�.");
        InJaeRecommendTutorial.Add("��Ż���� ������ �ʵ��� ��� �л����� ��õ���ּ���.");
        InJaeRecommendTutorial.Add("��õ �� �� �ִ� ȸ�� ����Դϴ�. ���� ����� ȸ���ϼ��� ��� ���縦 ���մϴ�.");
        InJaeRecommendTutorial.Add("��ġ �� �ش� ȸ���� ���� ���� ���� �� �� �ֽ��ϴ�.");
        InJaeRecommendTutorial.Add("���� ����Դϴ�. ȸ���� �����ϴ� ������ Ȯ���� �� �ֽ��ϴ�.");
        InJaeRecommendTutorial.Add("�Ϻ� ����� Ư�� �а��� �л����� ���ϱ⵵ �մϴ�.");
        InJaeRecommendTutorial.Add("���� �����Դϴ�.");
        InJaeRecommendTutorial.Add("������ �䱸�ϴ� �а��� �ɷ��Դϴ�.");
        InJaeRecommendTutorial.Add("�ϳ��� �������� ���ϴ� �л��� ��õ�� �� �����ϴ�.");
        InJaeRecommendTutorial.Add("������ �䱸�ϴ� ���� �ּ�ġ�Դϴ�.");
        InJaeRecommendTutorial.Add("�����Ҽ��� ��õ ���� Ȯ���� �����մϴ�.");
        InJaeRecommendTutorial.Add("��õ�ϰ����ϴ� �л��� ������ �� �ֽ��ϴ�.");
        InJaeRecommendTutorial.Add("�ѹ��� �������� �л��� ������ �� �ֽ��ϴ�.");
        InJaeRecommendTutorial.Add("������ �л��� ��õ������ ������ Ȯ���Դϴ�.");
        InJaeRecommendTutorial.Add("���� �����Ӱ� �پ��� ȸ�翡 �л����� ��õ�غ�����.");

        VacationTutorial.Add("����� �л����� ������ ������ �Բ� ã�ƿԳ׿�.");
        VacationTutorial.Add("���� 3���޿� ���ο� ���Ի��� �����ٵ�, ���Ӱ� ������ �ϱ⿡ �ռ� �޽ĵ� �ʿ��ϰ���?");
        VacationTutorial.Add("[��������_��ī���̸�]�� �� �� ���бⰣ�� ���� �� �ſ���.");
        VacationTutorial.Add("�̹� ���� ��������̽��ϴ�. [����_�����] �����.");
        VacationTutorial.Add("�� �� ���� ���ð� ������!");

        UpgradeTutorial.Add("�׷� ������Ȳ�� �����̳��� �����ο������� ��ī���̸� �� �ܰ� ���׷��̵� ���Ѻ����?");
        UpgradeTutorial.Add("��ȣ! �帣���� ���� ���־������!");
        UpgradeTutorial.Add("���� �л����� �� ���� ȯ�濡�� ������ �� �ְ� �Ǿ��׿�~");

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� �� ���� �������׿�.");
                newScripts.Add("�̹� �ص� ��������̽��ϴ�. [����_�����]�����.");
                newScripts.Add("���� �ֿ� �ƾ��!");
                VacationNormal.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("[����_�����]�����, �̹� �ص� �����ϼ̽��ϴ�.");
                newScripts.Add("ǫ ���ð� �����ֿ� �˰ڽ��ϴ�!");
                VacationNormal.Add(newScripts);
            }
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("����� ������ ã�ƿԳ׿�.");
                newScripts.Add("[����_�����]�����, �̹� �ص� ��� �����̽��ϴ�.");
                VacationNormal.Add(newScripts);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�� �� ���� ���� ���̳���?");
                newScripts.Add("���� �Ⱓ ���� ������õ ����� ���Դ�ϴ�.");
                newScripts.Add("Ȯ���غ�����!");
                RecommendResult.Add(newScripts);
            }
            else if (i == 1)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("���� �л����� [����_��ī���̸�]�� ���� ���� ������ �����ڷμ� Ȱ���ϰ� �˴ϴ�.");
                newScripts.Add("�ѵ��ϸ鼭�� �� �ƽ��� ����� �����?");
                newScripts.Add("�׳����� ���� �� 3���̳׿�.\n������ ���� ���Ի����� � �л����ϱ��?");
                newScripts.Add("���Ӱ� ���۵� �� �ظ� �� �غ��غ��ô�!");
                RecommendResult.Add(newScripts);
            }
            else if (i == 2)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("������õ ����� ���׿�!\n�л����� ���� ����� �� �⻵��.");
                newScripts.Add("���� �� 3�� �̳׿�.\n������ ���� ���Ի����� � �л����ϱ��?");
                newScripts.Add("���Ӱ� ���۵� �� �ظ� �� �غ��غ��ô�.");
                RecommendResult.Add(newScripts);
            }
            else if (i == 3)
            {
                List<string> newScripts = new List<string>();
                newScripts.Add("�̷��� �� �ذ� ������ �Ǿ����ϴ�.");
                newScripts.Add("��ȸ�� ������ �� �л����� ������ �������.");
                newScripts.Add("���Ӱ� ���۵� �� ��! �̹� �ص� �������� [����_�����]�����!");
                RecommendResult.Add(newScripts);
            }
        }
    }
}
