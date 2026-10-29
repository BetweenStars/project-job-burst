# GPU 유닛 이동 테스트

1. `GPU Test Scene`에서 빈 GameObject를 만들고 위치를 `(0, 0, 0)`으로 설정합니다.
2. `GpuUnitTest` 컴포넌트를 추가합니다. 두 셰이더는 추가 시 자동 연결됩니다.
   - 기존 컴포넌트라면 `Unit Compute Shader`에 `GpuUnitMovement.compute`, `Unit Render Shader`에 `GpuUnitRender.shader`를 연결합니다.
3. Play를 누르면 1,000개의 초록색 사각형이 XY 평면에서 이동하며 영역 끝에서 튕깁니다.

카메라는 `(0, 0, -10)`, 회전 `(0, 0, 0)`, Orthographic Size `5`를 기준으로 합니다.
`Unit Count`, `Area Size`, `Unit Radius` 변경은 컴포넌트를 껐다 켜면 적용됩니다.
`Move Speed`, `Unit Color`, 오브젝트 위치는 실행 중에도 변경할 수 있습니다.
오브젝트의 회전과 스케일은 사용하지 않습니다. 유닛 간 충돌과 기존 게임 유닛 연동은 포함하지 않습니다.

## 파일과 흐름

- `GpuUnitTest.cs`: 초기 데이터를 GPU에 전달하고 매 프레임 계산과 그리기를 요청합니다. 비활성화 시 GPU 리소스를 해제합니다.
- `GpuUnitData.hlsl`: 두 셰이더가 공유하는 유닛 구조체입니다. C# 구조체와 동일한 32바이트 레이아웃입니다.
- `GpuUnitMovement.compute`: 스레드 하나가 유닛 하나의 위치와 경계 반사를 계산합니다. 마지막 그룹의 범위 밖 스레드는 종료합니다.
- `GpuUnitRender.shader`: GPU 버퍼를 직접 읽어 유닛마다 삼각형 2개를 그립니다. URP용 단색 셰이더입니다.

위치를 CPU로 읽어오는 `GetData`나 유닛별 GameObject 생성은 사용하지 않습니다.
그리기는 Unity의 [Graphics.RenderPrimitives](https://docs.unity.cn/ScriptReference/Graphics.RenderPrimitives.html)를 사용합니다.

## 검증

Unity 6000.4.0f1의 컴파일러와 엔진 참조로 `GpuUnitTest.cs`의 에디터 포함 C# 컴파일을 확인했습니다.
셰이더 임포트 및 GPU/Play 모드 실행은 별도로 확인해야 합니다.
