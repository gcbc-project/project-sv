using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;    // 시네머신 카메라 참조
    [SerializeField] float dragSpeed = 2f;
    [SerializeField] float zoomSpeed = 2f;                      // 줌 인/아웃 속도
    [SerializeField] float minZoom = 5f;                        // 최소 줌 값
    [SerializeField] float maxZoom = 15f;                       // 최대 줌 값
    [SerializeField] float confinerPadding = 0.5f;              // 패딩 값

    private Vector2 dragOrigin;                                 // 드래그 시작 시점의 마우스 위치
    private Vector3 cameraOrigin;                               // 드래그 시작 시점의 카메라 위치
    private Vector3 cameraTargetPosition;                       // LateUpdate에서 적용할 카메라 위치

    private Transform cameraFollowTarget;                       // 카메라가 따라갈 빈 오브젝트
    private CinemachineConfiner2D confiner;                     // Confiner2D 참조

    void Start()
    {
        // 카메라가 따라갈 빈 오브젝트 생성 및 설정
        GameObject followObject = new GameObject("CameraFollowTarget");
        cameraFollowTarget = followObject.transform;
        virtualCamera.Follow = cameraFollowTarget;
        cameraTargetPosition = cameraFollowTarget.position;     // 초기값 설정

        // Confiner2D 컴포넌트 가져오기
        confiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
    }

    void Update()
    {
        bool isDragging = Mouse.current.rightButton.isPressed;

        // 마우스 드래그 처리
        if (isDragging)
        {
            // 현재 마우스 위치 가져오기
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // 드래그 시작 시점 설정
            if (dragOrigin == Vector2.zero)
            {
                dragOrigin = currentMousePos;
                cameraOrigin = cameraTargetPosition;
            }

            // 마우스 이동 차이 계산
            Vector3 difference = dragOrigin - currentMousePos;

            // 원하는 카메라 위치 계산
            Vector3 desiredPosition = cameraOrigin + difference * dragSpeed;

            // 패딩을 적용한 위치 제한
            Vector3 confinedPosition = GetConfinedPosition(desiredPosition);

            // 실제 이동한 거리 계산
            Vector3 actualMovement = confinedPosition - cameraTargetPosition;

            // 카메라가 이동했다면 드래그 기준점 업데이트
            if (actualMovement != Vector3.zero)
            {
                dragOrigin = currentMousePos;
                cameraOrigin = cameraTargetPosition;
            }

            // 카메라 목표 위치 업데이트
            cameraTargetPosition = confinedPosition;
        }
        else
        {
            // 드래그가 끝나면 시작 좌표 초기화
            dragOrigin = Vector2.zero;
        }

        // 줌 인/아웃 처리
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        if (scrollValue != 0)
        {
            var lens = virtualCamera.m_Lens;
            lens.OrthographicSize -= scrollValue * zoomSpeed * Time.deltaTime;
            lens.OrthographicSize = Mathf.Clamp(lens.OrthographicSize, minZoom, maxZoom);
            virtualCamera.m_Lens = lens;

            // Confiner2D 캐시 무효화
            if (confiner != null)
            {
                confiner.InvalidateCache();
            }
        }
    }

    void LateUpdate()
    {
        // 카메라 위치 적용
        cameraFollowTarget.position = cameraTargetPosition;
    }

    // 패딩을 적용한 위치 제한 함수
    private Vector3 GetConfinedPosition(Vector3 desiredPosition)
    {
        if (confiner != null && confiner.m_BoundingShape2D != null)
        {
            // 카메라의 뷰포트 크기 계산
            float orthographicSize = virtualCamera.m_Lens.OrthographicSize;
            float aspectRatio = Screen.width / (float)Screen.height;

            float cameraHeight = orthographicSize * 2;
            float cameraWidth = cameraHeight * aspectRatio;

            // 패딩 적용
            float paddingX = cameraWidth / 2f - confinerPadding;
            float paddingY = cameraHeight / 2f - confinerPadding;

            // Confiner의 경계 가져오기
            PolygonCollider2D confinerCollider = confiner.m_BoundingShape2D as PolygonCollider2D;
            if (confinerCollider != null)
            {
                Bounds confinerBounds = confinerCollider.bounds;

                // 이동 가능한 최소/최대 위치 계산
                float minX = confinerBounds.min.x + paddingX;
                float maxX = confinerBounds.max.x - paddingX;
                float minY = confinerBounds.min.y + paddingY;
                float maxY = confinerBounds.max.y - paddingY;

                // 위치 제한 적용
                float x = Mathf.Clamp(desiredPosition.x, minX, maxX);
                float y = Mathf.Clamp(desiredPosition.y, minY, maxY);

                return new Vector3(x, y, desiredPosition.z);
            }
            else
            {
                // PolygonCollider2D가 아닌 경우 ClosestPoint 사용
                Vector2 confined2D = confiner.m_BoundingShape2D.ClosestPoint(desiredPosition);
                return new Vector3(confined2D.x, confined2D.y, desiredPosition.z);
            }
        }
        else
        {
            // Confiner가 없으면 원하는 위치 반환
            return desiredPosition;
        }
    }
}
