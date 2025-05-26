using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BamsongiGenerator : MonoBehaviour
{
    // ������ ���赵 ������ ���� public GameObject ���� ����
    public GameObject gBamsongiPrefab = null;

    // �Ŀ���¡�� ���� ������ ����
    public Image iChargeGauge = null;
    public Image iChargeGaugeBg = null;

    private float ChargeSpeed = 1000f;  // ������ ��¡�ӵ�
    private float MaxCharge = 2000f;    // �ִ� ��¡������ ��

    private float CurrentCharge = 0f; // ���� ��¡�� �������� ��
    //private bool Click = false; // ���콺 Ŭ�� ������� 
    private bool Charging = false; // �Ŀ������� �������� �����

    // Instantiate�� ����� ������Ʈ ���� ����
    GameObject insBamsongiPrefab = null;

    // ����� ���� ��ǥ
    Vector3 vBamsongiworldDir = Vector3.zero;

    void Start()
    {
        // �Ŀ������� �ʱ⼼��
        //Click = true;
        Charging = true;
        iChargeGauge.gameObject.SetActive(false);
        iChargeGaugeBg.gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        // �� ���¸� Ȯ���ϱ� ���� �� �ȿ��� CameraController ������Ʈ�� ���� ��ü�� ã��
        CameraController zoomController = FindFirstObjectByType<CameraController>();
        // �� ���� ���� ���, ����� �߻縦 ����
        if (zoomController != null && zoomController.IsZooming)
            return; // �� ���� �߿� �߻� �Ұ�
        {
            // ����ȭ���� ���콺�� Ŭ�������� �۵��ϴ� �Լ�
            if (Input.GetMouseButtonDown(0))
            {
                CurrentCharge = 0f;     // ���� ��¡�� �� �ʱ�ȭ
                iChargeGauge.gameObject.SetActive(true);
                iChargeGaugeBg.gameObject.SetActive(true);
            }

            // ����ȭ���� ���콺�� ������ ������ �Ŀ������� �۵�
            if (Input.GetMouseButton(0))
            {
                /* Charging ������ true�϶� CurrentCharge ���� �����ϴٰ�  MaxCharge���� �����ϸ�
                   Charging ���� flase�� ��ȯ�ϰ� CurrentCharge ���� �����ϴ� 0�� �Ǹ� �ٽ� Charging ���� 
                   true�� ��ȯ�ϰ� �� ������ �ݺ�
                */
                if (Charging == true)
                {
                    CurrentCharge += ChargeSpeed * Time.deltaTime;
                    if (CurrentCharge >= MaxCharge)
                    {
                        CurrentCharge = MaxCharge;
                        Charging = false;
                    }
                }
                else
                {
                    CurrentCharge -= ChargeSpeed * Time.deltaTime;
                    if (CurrentCharge <= 0f)
                    {
                        CurrentCharge = 0f;
                        Charging = true;
                    }
                }
                // CurrentCharge �������� fillAmount�� ��ȭ
                iChargeGauge.fillAmount = CurrentCharge / MaxCharge;
            }

            // ���콺 ��ư�� ������ �۵��ϴ� �Լ�
            if (Input.GetMouseButtonUp(0))
            {
                UiManager.Instance.UpdateGameCount();               // ����̰� �߻�Ǹ� ����ī��Ʈ 1 �����ϴ� �Լ�
                iChargeGauge.gameObject.SetActive(false);           // �Ŀ������� ��Ȱ��ȭ
                iChargeGaugeBg.gameObject.SetActive(false);         // �Ŀ������� ��� ��Ȱ��ȭ
                // ������ �����ϴ� ���߿� ����� ������Ʈ�� ����
                insBamsongiPrefab = Instantiate(gBamsongiPrefab);

                /*
                 * Ray Ŭ����
                 *  Ray(����)�� �̸� �״�� �����̸�, ������ ��ǥ(Origin)�� ������ ����(direction)�� ��� ������ ����
                 *  Ray�� �ݶ��̴��� ����� ������Ʈ�� �浹�� �����ϴ� Ư¡�� ����
                 *  ScreenPointToRay �޼����� ��ȯ������ ���� �� �ִ� Ray�� Origin�� Main Camera�� ��ǥ��.
                 *  direction �������� ����̸� ������ ������ direction ���Ͱ� ���� normalized ������ ����� ���̰� 1�� ���͸� ���� ��
                 *  ���� 2000 ���Ѵ�. �ϴ� ���̸� 1 ���ͷ� �ؼ� direction ���� ũ�⿡ ������� ����̿� ������ ���� ���� �� ����
                 */

                Ray ScreenPointToRayBamsongi = Camera.main.ScreenPointToRay(Input.mousePosition);

                vBamsongiworldDir = ScreenPointToRayBamsongi.direction;

                insBamsongiPrefab.GetComponent<BamsongiController>().f_TargetShoot(vBamsongiworldDir.normalized * CurrentCharge);

                
            }
        }
        
    }
}
