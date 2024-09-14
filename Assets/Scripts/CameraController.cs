using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;  // 시네머신 카메라 참조
    [SerializeField] float dragSpeed = 2f;

    private Vector2 dragOrigin;       // 드래그 시작 시점에서의 마우스 위치
    private Vector3 cameraOrigin;     // 드래그 시작 시점에서의 카메라 위치
    private Vector3 cameraTargetPosition; // LateUpdate에서 적용할 카메라 위치

    private Transform cameraFollowTarget; // 카메라가 따라갈 빈 오브젝트

    void Start()
    {
        // 카메라가 따라갈 빈 오브젝트를 생성하여 시네머신 카메라에 할당
        GameObject followObject = new GameObject("CameraFollowTarget");
        cameraFollowTarget = followObject.transform;
        virtualCamera.Follow = cameraFollowTarget;
        cameraTargetPosition = cameraFollowTarget.position; // 초기값 설정
    }

    void Update()
    {
        // 마우스 좌클릭 중일 때 드래그 시작
        if (Mouse.current.leftButton.isPressed)
        {
            // 드래그 시작 시점에서 마우스 위치와 카메라 위치 기록
            if (dragOrigin == Vector2.zero)
            {
                dragOrigin = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                cameraOrigin = cameraFollowTarget.position;
            }

            // 현재 마우스 위치
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // 마우스 드래그 시작 위치와 현재 위치의 차이를 계산
            Vector3 difference = dragOrigin - currentMousePos;

            // 드래그 시작 시점의 카메라 위치에서 차이값을 적용하여 목표 위치 설정
            cameraTargetPosition = cameraOrigin + (Vector3)difference * dragSpeed;
        }
        else
        {
            // 드래그가 끝나면 시작 좌표 초기화
            dragOrigin = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        // LateUpdate에서 카메라의 위치 이동 적용
        cameraFollowTarget.position = cameraTargetPosition;
    }
}
