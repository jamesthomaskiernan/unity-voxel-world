using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using System.Threading;


public class WorldGenerator : MonoBehaviour
{
    // Rendering
    public ComputeBuffer TrianglesBuffer;
    public ComputeBuffer ArgsBuffer;
    
    // Mesh Data
    ComputeBuffer ObjBuffer;
    ComputeBuffer ModelPointsBuffer;
    ComputeBuffer ModelTrianglesBuffer;
    ComputeBuffer ModelFacesBuffer;
    ComputeBuffer ModelAdjacentsBuffer;
    ComputeBuffer UVProfilesBuffer;

    // Changes to game world
    public ComputeBuffer AddedObjectsBuffer;
    public ComputeBuffer RemovedObjectsBuffer;

    // Shaders & Materials
    public ComputeShader ComputeShader;
    public ComputeShader FixArgsShader;
    public ComputeShader ClearArgsShader;
    public Material Material;
    public Shader Shader;
    
    int WorldWidth = 12;
    int WorldHeight = 64;

    public void Start()
    {
        CreateBuffers();
        SetBuffers();
        ComputeShader.Dispatch(0, WorldWidth, WorldHeight, WorldWidth);
        FixArgs();
    }

    public void Update()
    {
        DrawMesh();
    }

    // Releases buffers when game ends
    private void OnDestroy() 
    {
        TrianglesBuffer.Release();
        ArgsBuffer.Release();
        ObjBuffer.Release();
        ModelPointsBuffer.Release();
        ModelTrianglesBuffer.Release();
        ModelFacesBuffer.Release();
        ModelAdjacentsBuffer.Release();
        UVProfilesBuffer.Release();
        AddedObjectsBuffer.Release();
    }

    // Initializes compute buffers
    public void CreateBuffers()
    {
        // Create TrianglesBuffer
        TrianglesBuffer = new ComputeBuffer(1024 * 1024 * 8, sizeof(float) * 3 * 5 + sizeof(float) * 2 * 3 + sizeof(int), ComputeBufferType.Append);
        TrianglesBuffer.SetCounterValue(0);
        
        // Create ArgsBuffer
        ArgsBuffer = new ComputeBuffer(1, sizeof(int) * 4, ComputeBufferType.IndirectArguments);

        // Generate model data
        MeshData Data = GetComponent<MeshData>();
        Data.Generate();
        int ModelCount = Data.ModelPoints.Count;
        
        // Create Obj Buffer
        ObjBuffer = new ComputeBuffer(Data.AllObjs.Count, sizeof(int) * 4 + sizeof(float) * 2);
        ObjBuffer.SetData(Data.AllObjs.ToArray());

        // Model Points
        ModelPointsBuffer = new ComputeBuffer(Data.ModelPointsMaxSize * ModelCount, sizeof(float) * 3);
        ModelPointsBuffer.SetData(Data.ModelPointsArr);
        
        // Model Triangles
        ModelTrianglesBuffer = new ComputeBuffer(Data.ModelTrianglesMaxSize * ModelCount, sizeof(int));
        ModelTrianglesBuffer.SetData(Data.ModelTrianglesArr);

        // Model Faces
        ModelFacesBuffer = new ComputeBuffer(Data.ModelFacesMaxSize * ModelCount, sizeof(int));
        ModelFacesBuffer.SetData(Data.ModelFacesArr);

        // Model Adjacents
        ModelAdjacentsBuffer = new ComputeBuffer(Data.ModelAdjacentsMaxSize * ModelCount, sizeof(int));
        ModelAdjacentsBuffer.SetData(Data.ModelAdjacentsArr);

        // Create ObjUVs Buffer
        UVProfilesBuffer = new ComputeBuffer(Data.UVProfiles.Count * Data.UVProfilesMaxSize, sizeof(float) * 2);
        UVProfilesBuffer.SetData(Data.UVProfilesArr);

        AddedObjectsBuffer = new ComputeBuffer(2048, sizeof(int) + sizeof(float) * 3);
    }

    // Connects compute buffers to the compute shaders
    public void SetBuffers()
    {
        // Triangles buffer
        Material.SetBuffer("Triangles", TrianglesBuffer);           // Shader
        ComputeShader.SetBuffer(0, "Triangles", TrianglesBuffer);   // Compute Shader
        
        // Args buffer
        ComputeShader.SetBuffer(0, "Args", ArgsBuffer);             // Compute Shader
        FixArgsShader.SetBuffer(0, "Args", ArgsBuffer);             // Fix Args Shader
        ClearArgsShader.SetBuffer(0, "Args", ArgsBuffer);           // Clear Args Shader

        // Mesh data buffers
        ComputeShader.SetBuffer(0, "Objs",           ObjBuffer);
        ComputeShader.SetBuffer(0, "ModelPoints",    ModelPointsBuffer);
        ComputeShader.SetBuffer(0, "ModelTriangles", ModelTrianglesBuffer);
        ComputeShader.SetBuffer(0, "ModelFaces",     ModelFacesBuffer);
        ComputeShader.SetBuffer(0, "ModelAdjacents", ModelAdjacentsBuffer);
        ComputeShader.SetBuffer(0, "UVProfiles",     UVProfilesBuffer);

        // World Changes
        ComputeShader.SetBuffer(0, "AddedObjects", AddedObjectsBuffer);
    }

    public void RegenerateWorld()
    {
        ClearArgs();
        TrianglesBuffer.SetCounterValue(0);
        ComputeShader.Dispatch(0, WorldWidth, WorldHeight, WorldWidth);
        FixArgs();
    }

    // Prints args
    private void PrintArgs()
    {
        int[] args2 = new int[4] {0, 0, 0, 0};
        ArgsBuffer.GetData(args2);
        
        Debug.Log("");
        Debug.Log(args2[0]);
        Debug.Log(args2[1]);
        Debug.Log(args2[2]);
        Debug.Log(args2[3]);
        Debug.Log("");
    }

    // Clears vert count in args
    public void ClearArgs()
    {
        ClearArgsShader.Dispatch(0, 1, 1, 1);
    }

    // Draws mesh with DrawProceduralIndirect()
    public void DrawMesh()
    {
        Bounds b = new Bounds(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
        Graphics.DrawProceduralIndirect(Material, b, MeshTopology.Triangles, ArgsBuffer);
    }

    // Finds vert count from TrianglesBuffer and puts it into the ArgsBuffer
    public void FixArgs()
    {
        int[] args = new int[4] {0, 1, 0, 0};
        ArgsBuffer.SetData(args);
        ComputeBuffer.CopyCount(TrianglesBuffer, ArgsBuffer, 0);
        FixArgsShader.Dispatch(0, 1, 1, 1);
    }
}
