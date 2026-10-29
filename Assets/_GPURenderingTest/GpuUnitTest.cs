using System.Runtime.InteropServices;
using UnityEngine;

public class GpuUnitTest : MonoBehaviour
{
    [SerializeField] private ComputeShader unitComputeShader;
    [SerializeField] private Shader unitRenderShader;
    [SerializeField, Min(1)] private int unitCount = 1000;
    [SerializeField, Min(0f)] private float moveSpeed = 2f;
    [SerializeField] private Vector2 areaSize = new Vector2(16f, 8f);
    [SerializeField, Min(0.01f)] private float unitRadius = 0.08f;
    [SerializeField] private Color unitColor = new Color(0.3f, 0.9f, 0.7f, 1f);

    private GraphicsBuffer unitBuffer;
    private ComputeShader runtimeComputeShader;
    private Material unitMaterial;
    private RenderParams renderParams;
    private int moveKernel;
    private int dispatchCount;
    private int activeUnitCount;
    private Vector2 activeAreaSize;

    // HLSL의 UnitData와 순서/크기를 맞추고 16바이트 배수로 정렬한다.
    [StructLayout(LayoutKind.Sequential)]
    private struct UnitData
    {
        public Vector2 position;
        public Vector2 direction;
        public Vector2 size;
        public Vector2 padding;
    }

#if UNITY_EDITOR
    private void Reset()
    {
        unitComputeShader = UnityEditor.AssetDatabase.LoadAssetAtPath<ComputeShader>(
            "Assets/_GPURenderingTest/GpuUnitMovement.compute");
        unitRenderShader = UnityEditor.AssetDatabase.LoadAssetAtPath<Shader>(
            "Assets/_GPURenderingTest/GpuUnitRender.shader");
    }
#endif

    private void OnEnable()
    {
        if (!SystemInfo.supportsComputeShaders || !SystemInfo.supportsInstancing)
        {
            Debug.LogError("GPU 유닛 테스트는 Compute Shader와 GPU Instancing 지원이 필요합니다.", this);
            enabled = false;
            return;
        }

        if (unitComputeShader == null || unitRenderShader == null || !unitRenderShader.isSupported)
        {
            Debug.LogError("GpuUnitMovement와 지원되는 GpuUnitRender 셰이더를 연결해 주세요.", this);
            enabled = false;
            return;
        }

        activeUnitCount = Mathf.Clamp(unitCount, 1, 1000000);
        float radius = Mathf.Max(0.01f, unitRadius);
        activeAreaSize = new Vector2(
            Mathf.Max(areaSize.x, radius * 2f + 0.01f),
            Mathf.Max(areaSize.y, radius * 2f + 0.01f));
        Vector2 spawnExtent = activeAreaSize * 0.5f - Vector2.one * radius;
        UnitData[] units = new UnitData[activeUnitCount];

        for (int i = 0; i < units.Length; i++)
        {
            float angle = Random.value * Mathf.PI * 2f;
            units[i] = new UnitData
            {
                position = new Vector2(Random.Range(-spawnExtent.x, spawnExtent.x),
                    Random.Range(-spawnExtent.y, spawnExtent.y)),
                direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)),
                size = Vector2.one * radius
            };
        }

        // 여러 테스트 오브젝트가 서로의 버퍼 설정을 덮어쓰지 않도록 복제한다.
        runtimeComputeShader = Instantiate(unitComputeShader);
        moveKernel = runtimeComputeShader.FindKernel("MoveUnits");
        runtimeComputeShader.GetKernelThreadGroupSizes(moveKernel, out uint threadsX, out _, out _);
        dispatchCount = Mathf.CeilToInt(activeUnitCount / (float)threadsX);

        unitBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured,
            activeUnitCount, Marshal.SizeOf<UnitData>());
        unitBuffer.SetData(units);
        runtimeComputeShader.SetBuffer(moveKernel, "_Units", unitBuffer);
        runtimeComputeShader.SetInt("_UnitCount", activeUnitCount);
        runtimeComputeShader.SetVector("_HalfArea", activeAreaSize * 0.5f);

        unitMaterial = new Material(unitRenderShader) { enableInstancing = true };
        unitMaterial.SetBuffer("_Units", unitBuffer);
        renderParams = new RenderParams(unitMaterial) { layer = gameObject.layer };
    }

    private void Update()
    {
        if (unitBuffer == null) return;

        runtimeComputeShader.SetFloat("_DeltaTime", Time.deltaTime);
        runtimeComputeShader.SetFloat("_MoveSpeed", Mathf.Max(0f, moveSpeed));
        runtimeComputeShader.Dispatch(moveKernel, dispatchCount, 1, 1);

        // GPU 위치를 CPU로 읽지 않고 렌더 셰이더에서 그대로 사용한다.
        unitMaterial.SetVector("_Origin", transform.position);
        unitMaterial.SetColor("_Color", unitColor);
        renderParams.worldBounds = new Bounds(transform.position,
            new Vector3(activeAreaSize.x, activeAreaSize.y, 1f));
        Graphics.RenderPrimitives(renderParams, MeshTopology.Triangles, 6, activeUnitCount);
    }

    private void OnDisable()
    {
        unitBuffer?.Release();
        unitBuffer = null;
        if (unitMaterial != null) Destroy(unitMaterial);
        if (runtimeComputeShader != null) Destroy(runtimeComputeShader);
    }
}
