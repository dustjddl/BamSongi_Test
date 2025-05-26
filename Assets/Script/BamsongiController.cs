// 마우스 클릭하면 밤송이가 과녁으로 날아가는 동작 제어

using UnityEngine;

public class BamsongiController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 디바이스 성능에 따른 실행결과 차이 없애기
        Application.targetFrameRate = 60;

        /*
         *  밤송이가 화면 안쪽으로 날아가도록 z축 방향의 벡터를 매개변수로 전달하고
           f_TargetShoot 메서드를 호출. 
         * y축 방향으로 힘을 200 가하는 이유는 밤송이가 과녁에 닿기 전에 중력의 영향을 받아 지면으로 낙하하는 것을 막기 위함
         * Start 메서드를 호출하는 시작과 동시에 밤송이가 과녁으로 날아감
         */
        //f_TargetShoot(new Vector3(0, 200, 2000));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 매개변수 방향으로 밤송이에게 힘을 가하는 메서드
    public void f_TargetShoot(Vector3 argDir)
    {
        // 매개변수로 전달된 Vector 값으로 힘을 가한다.
        GetComponent<Rigidbody>().AddForce(argDir);
    }

    // physics를 사용하므로 과녁과 밤송이가 충돌하면 onCollisionEnter 메서드가 호출되어 실행됨
    private void OnCollisionEnter(Collision collision)
    {
        // 밤송이가 과녁에 닿는 순간 밤송이 움직임이 멈추므로, Rigidbody 컴포넌트의 iskinematic 메서드를 true로 설정
        // isKinematic 메서드를 true로 설정 하면, 오브젝트에 작용하는 힘을 무시하고 밤송이를 정지시킴
        // iskinematic 메서드 : 외부에서 가해지는 물리적 힘에 반응하지 않는 오브젝트라는 의미. 중력과 충돌에 반응하지 않도록 함
        GetComponent<Rigidbody>().isKinematic = true;
        if(collision.gameObject.CompareTag("Target"))
        {
            // 밤송이를 과녁의 자식으로 설정하여 밤송이가 과녁에 명중했을 때 함께 움직이도록 설정
            transform.SetParent(collision.transform);

            // 밤송이가 과녁에 명중하면 스코어 업데이트
            UiManager.Instance.UpdateScore();
            // 카메라 줌 효과 시작
            CameraController.Instance.StartZoomEffect();

        }
        GetComponent<ParticleSystem>().Play();             
    }

}
