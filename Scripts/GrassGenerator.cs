using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{

    public ComputeShader ComputeShader;
    ComputeBuffer TrianglesBuffer;
    ComputeBuffer ArgsBuffer;
    public Material Material;
    
    public float GrassHeight = .25f;
    public float GrassWidth = .25f;
    public float Test = 1.0f;

    public int TrianglesCount = 1;
    public Texture Noise;

    void Start()
    {
        TrianglesBuffer = new ComputeBuffer((int)Mathf.Pow((float)TrianglesCount, 2.0f) * 1024 * 2, sizeof(float) * 3 * 4 + sizeof(float) * 2 * 3, ComputeBufferType.Append);
        TrianglesBuffer.SetCounterValue(0);
        ComputeShader.SetBuffer(0, "Triangles", TrianglesBuffer);
        Material.SetBuffer("Triangles", TrianglesBuffer);

        ArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);
        ComputeShader.SetBuffer(0, "Args", ArgsBuffer);
        int[] args = new int[4] {0, 1, 0, 0};
        ArgsBuffer.SetData(args);
        ComputeShader.SetTexture(0, "Noise", Noise);
    }

    void LateUpdate()
    {
        Material.SetFloat("_Timer", Time.timeSinceLevelLoad);
    }

    void Update()
    {
        ComputeShader.SetFloat("GrassHeight", GrassHeight);
        ComputeShader.SetFloat("GrassWidth", GrassWidth);
        ComputeShader.SetFloat("Test", Test);
        
        int[] args = new int[4] {0, 1, 0, 0};
        ArgsBuffer.SetData(args);
        TrianglesBuffer.SetCounterValue(0);
        ComputeShader.Dispatch(0, TrianglesCount, 128, TrianglesCount);

        Bounds b = new Bounds(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
        Graphics.DrawProceduralIndirect(Material, b, MeshTopology.Triangles, ArgsBuffer);
    }


    private void OnDestroy() 
    {
        TrianglesBuffer.Release();
        ArgsBuffer.Release();
    }
}
