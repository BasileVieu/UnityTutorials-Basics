using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    [SerializeField] private ComputeShader computeShader;

    [SerializeField] private Material material;

    [SerializeField] private Mesh mesh;
    
    [SerializeField] [Range(10, maxResolution)] private int resolution = 10;

    [SerializeField] FunctionLibrary.FunctionName function;

    [SerializeField] private TransitionMode transitionMode;

    [SerializeField] [Min(0.0f)] private float functionDuration = 1.0f;
    [SerializeField] [Min(0.0f)] private float transitionDuration = 1.0f;

    public enum TransitionMode
    {
        Cycle,
        Random
    }

    private float duration;

    private const int maxResolution = 1000;

    private bool transitioning;

    private FunctionLibrary.FunctionName transitionFunction;

    private ComputeBuffer positionsBuffer;

    private static readonly int positionsId = Shader.PropertyToID("_Positions");
    private static readonly int resolutionId = Shader.PropertyToID("_Resolution");
    private static readonly int stepId = Shader.PropertyToID("_Step");
    private static readonly int timeId = Shader.PropertyToID("_Time");
    private static readonly int transitionProgressId = Shader.PropertyToID("_TransitionProgress");

    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }

    void OnDisable()
    {
        positionsBuffer.Release();

        positionsBuffer = null;
    }

    void Update()
    {
        duration += Time.deltaTime;

        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;

                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;

            transitioning = true;

            transitionFunction = function;

            PickNextFunction();
        }

        UpdateFunctionOnGPU();
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle
                           ? FunctionLibrary.GetNextFunctionName(function)
                           : FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    void UpdateFunctionOnGPU()
    {
        float step = 2.0f / resolution;

        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        if (transitioning)
        {
            computeShader.SetFloat(transitionProgressId, Mathf.SmoothStep(0.0f, 1.0f, duration / transitionDuration));
        }

        int kernelIndex = (int)function + (int)(transitioning ? transitionFunction : function) * 5;

        computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8.0f);

        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2.0f + 2.0f / resolution));

        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, resolution * resolution);
    }
}