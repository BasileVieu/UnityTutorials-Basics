using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private Transform pointPrefab;

    [SerializeField] [Range(10, 200)] private int resolution = 10;

    [SerializeField] FunctionLibrary.FunctionName function;

    [SerializeField] private TransitionMode transitionMode;

    [SerializeField] [Min(0.0f)] private float functionDuration = 1.0f;
    [SerializeField] [Min(0.0f)] private float transitionDuration = 1.0f;

    public enum TransitionMode
    {
        Cycle,
        Random
    }

    private Transform[] points;

    private float duration;

    private bool transitioning;

    private FunctionLibrary.FunctionName transitionFunction;

    void Awake()
    {
        float step = 2.0f / resolution;

        Vector3 scale = Vector3.one * step;

        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab, transform, false);
            point.localScale = scale;

            points[i] = point;
        }
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

        if (transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateFunction();
        }
    }

    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle
                           ? FunctionLibrary.GetNextFunctionName(function)
                           : FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

        float time = Time.time;

        float step = 2.0f / resolution;

        float v = 0.5f * step - 1.0f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1.0f;
            }

            float u = (x + 0.5f) * step - 1.0f;

            points[i].localPosition = f(u, v, time);
        }
    }

    void UpdateFunctionTransition()
    {
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);

        float progress = duration / transitionDuration;

        float time = Time.time;

        float step = 2.0f / resolution;

        float v = 0.5f * step - 1.0f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
                v = (z + 0.5f) * step - 1.0f;
            }

            float u = (x + 0.5f) * step - 1.0f;

            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}