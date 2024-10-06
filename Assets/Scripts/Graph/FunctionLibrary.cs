using UnityEngine;

using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float _u, float _v, float _t);

    public enum FunctionName
    {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    }

    private static Function[] functions =
    {
            Wave,
            MultiWave,
            Ripple,
            Sphere,
            Torus
    };

    public static int GetFunctionCount() => functions.Length;

    public static Vector3 Morph(float _u, float _v, float _t, Function _from, Function _to, float _progress)
    {
        return Vector3.LerpUnclamped(_from(_u, _v, _t), _to(_u, _v, _t), SmoothStep(0.0f, 1.0f, _progress));
    }

    public static FunctionName GetNextFunctionName(FunctionName _name) => (int)_name < functions.Length - 1 ? _name + 1 : 0;

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName _name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);

        return choice == _name ? 0 : choice;
    }

    public static Function GetFunction(FunctionName _name) => functions[(int)_name];

    public static Vector3 Wave(float _u, float _v, float _t)
    {
        Vector3 p;
        p.x = _u;
        p.y = Sin(PI * (_u + _v + _t));
        p.z = _v;

        return p;
    }

    public static Vector3 MultiWave(float _u, float _v, float _t)
    {
        Vector3 p;
        p.x = _u;
        p.y = Sin(PI * (_u + 0.5f * _t));
        p.y += 0.5f * Sin(2.0f * PI * (_v + _t));
        p.y += Sin(PI * (_u + _v + 0.25f * _t));
        p.y *= 1.0f / 2.5f;
        p.z = _v;

        return p;
    }

    public static Vector3 Ripple(float _u, float _v, float _t)
    {
        float d = Sqrt(_u * _u + _v * _v);

        Vector3 p;
        p.x = _u;
        p.y = Sin(PI * (4.0f * d - _t));
        p.y /= 1.0f + 10.0f * d;
        p.z = _v;

        return p;
    }

    public static Vector3 Sphere(float _u, float _v, float _t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (12.0f * _u + 8.0f * _v + _t));
        float s = r * Cos(0.5f * PI * _v);
        
        Vector3 p;
        p.x = s * Sin(PI * _u);
        p.y = r * Sin(PI * 0.5f * _v);
        p.z = s * Cos(PI * _u);

        return p;
    }

    public static Vector3 Torus(float _u, float _v, float _t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (8.0f * _u + 0.5f * _t));
        float r2 = 0.15f + 0.05f * Sin(PI * (16.0f * _u + 8.0f * _v + 3.0f * _t));
        float s = r1 + r2 * Cos(PI * _v);
        
        Vector3 p;
        p.x = s * Sin(PI * _u);
        p.y = r2 * Sin(PI * _v);
        p.z = s * Cos(PI * _u);

        return p;
    }
}