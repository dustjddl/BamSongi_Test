// ���콺 Ŭ���ϸ� ����̰� �������� ���ư��� ���� ����

using UnityEngine;

public class BamsongiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ����̽� ���ɿ� ���� ������ ���� ���ֱ�
        Application.targetFrameRate = 60;

        /*
         *  ����̰� ȭ�� �������� ���ư����� z�� ������ ���͸� �Ű������� �����ϰ�
           f_TargetShoot �޼��带 ȣ��. 
         * y�� �������� ���� 200 ���ϴ� ������ ����̰� ���ῡ ��� ���� �߷��� ������ �޾� �������� �����ϴ� ���� ���� ����
         * Start �޼��带 ȣ���ϴ� ���۰� ���ÿ� ����̰� �������� ���ư�
         */
        //f_TargetShoot(new Vector3(0, 200, 2000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �Ű����� �������� ����̿��� ���� ���ϴ� �޼���
    public void f_TargetShoot(Vector3 argDir)
    {
        // �Ű������� ���޵� Vector ������ ���� ���Ѵ�.
        GetComponent<Rigidbody>().AddForce(argDir);
    }

    // physics�� ����ϹǷ� ����� ����̰� �浹�ϸ� onCollisionEnter �޼��尡 ȣ��Ǿ� �����
    private void OnCollisionEnter(Collision collision)
    {
        // ����̰� ���ῡ ��� ���� ����� �������� ���߹Ƿ�, Rigidbody ������Ʈ�� iskinematic �޼��带 true�� ����
        // isKinematic �޼��带 true�� ���� �ϸ�, ������Ʈ�� �ۿ��ϴ� ���� �����ϰ� ����̸� ������Ŵ
        // iskinematic �޼��� : �ܺο��� �������� ������ ���� �������� �ʴ� ������Ʈ��� �ǹ�. �߷°� �浹�� �������� �ʵ��� ��
        GetComponent<Rigidbody>().isKinematic = true;
        if(collision.gameObject.CompareTag("Target"))
        {
            // ����̸� ������ �ڽ����� �����Ͽ� ����̰� ���ῡ �������� �� �Բ� �����̵��� ����
            transform.SetParent(collision.transform);

            // ����̰� ���ῡ �����ϸ� ���ھ� ������Ʈ
            UiManager.Instance.UpdateScore();
            // ī�޶� �� ȿ�� ����
            CameraController.Instance.StartZoomEffect();

        }
        GetComponent<ParticleSystem>().Play();             
    }

}
