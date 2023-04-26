using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour
{
    public void Generate()
    {
        InitializeObjs();
        InitializeModelPoints();
        InitializeModelTriangles();
        InitializeModelFaces();
        InitializeModelAdjacents();
        InitializeUVProfiles();

        FillModelPointsArr();
        FillModelTrianglesArr();
        FillModelFacesArr();
        FillModelAdjacentsArr();
        FillUVProfilesArr();
    }

    public List<Obj> AllObjs;

    // Model Points
    public int ModelPointsMaxSize = 128;
    public List<List<Vector3>> ModelPoints;
    public Vector3[] ModelPointsArr;

    // Model Triangles
    public int ModelTrianglesMaxSize = 128;
    public List<List<int>> ModelTriangles;
    public int[] ModelTrianglesArr;

    // Model Faces
    public int ModelFacesMaxSize = 14;
    public List<List<int>> ModelFaces;
    public int[] ModelFacesArr;

    // Model Adjacents
    public int ModelAdjacentsMaxSize = 48;
    public List<List<int>> ModelAdjacents;
    public int[] ModelAdjacentsArr;

    // UV Profiles
    public int UVProfilesMaxSize = 128;
    public List<List<Vector2>> UVProfiles;
    public Vector2[] UVProfilesArr;

    public struct Obj
    {
        public int ObjNum;
        public int ModelNum;
        public int UVProfileNum;
        public int TexArrNum;
        public Vector2 TexAtlasPos;
    };
    
    public void InitializeObjs()
    {
        // Grass
        Obj Grass = new Obj();
        Grass.ObjNum = 0;
        Grass.ModelNum = 0;
        Grass.UVProfileNum = 0;
        Grass.TexArrNum = 0;
        Grass.TexAtlasPos = new Vector2(0,10);
        
        // Brick
        Obj Brick = new Obj();
        Brick.ObjNum = 1;
        Brick.ModelNum = 0;
        Brick.UVProfileNum = 0;
        Brick.TexArrNum = 0;
        Brick.TexAtlasPos = new Vector2(7,12);

        // StoneStairs
        Obj StoneStairs = new Obj();
        StoneStairs.ObjNum = 2;
        StoneStairs.ModelNum = 1;
        StoneStairs.UVProfileNum = 1;
        StoneStairs.TexArrNum = 0;
        StoneStairs.TexAtlasPos = new Vector2(5,10);

        // Cliff
        Obj Cliff = new Obj();
        Cliff.ObjNum = 3;
        Cliff.ModelNum = 0;
        Cliff.UVProfileNum = 0;
        Cliff.TexArrNum = 0;
        Cliff.TexAtlasPos = new Vector2(0,11);

        AllObjs = new List<Obj>()
        {
            Grass,
            Brick,
            StoneStairs,
            Cliff,
        };
    }
    
    public void InitializeModelPoints()
    {
        List<Vector3> VoxelPoints = new List<Vector3>()
        {
            new Vector3(-.5f, -.5f,  .5f),  // p0
            new Vector3(-.5f, -.5f, -.5f),  // p1
            new Vector3( .5f, -.5f,  .5f),  // p2
            new Vector3( .5f, -.5f, -.5f),  // p3
            new Vector3(-.5f,  .5f,  .5f),  // p4
            new Vector3(-.5f,  .5f, -.5f),  // p5
            new Vector3( .5f,  .5f,  .5f),  // p6
            new Vector3( .5f,  .5f, -.5f),  // p7
        };

        List<Vector3> StairPoints = new List<Vector3>()
        {
            new Vector3(-.5f,  -.5f,   .5f), // p0
            new Vector3(-.5f,  -.5f,  -.5f), // p1
            new Vector3( .5f,  -.5f,   .5f), // p2
            new Vector3( .5f,  -.5f,  -.5f), // p3
            new Vector3(-.5f, -.25f, -.25f), // p4
            new Vector3(-.5f, -.25f,  -.5f), // p5
            new Vector3( .5f, -.25f, -.25f), // p6
            new Vector3( .5f, -.25f,  -.5f), // p7
            new Vector3(-.5f,    0f,    0f), // p8
            new Vector3(-.5f,    0f, -.25f), // p9
            new Vector3( .5f,    0f,    0f), // p10
            new Vector3( .5f,    0f, -.25f), // p11
            new Vector3(-.5f,  .25f,  .25f), // p12
            new Vector3(-.5f,  .25f,    0f), // p13
            new Vector3( .5f,  .25f,  .25f), // p14
            new Vector3( .5f,  .25f,    0f), // p15
            new Vector3(-.5f,   .5f,   .5f), // p16
            new Vector3(-.5f,   .5f,  .25f), // p17
            new Vector3( .5f,   .5f,   .5f), // p18
            new Vector3( .5f,   .5f,  .25f), // p19
        };

        ModelPoints = new List<List<Vector3>>()
        {
            VoxelPoints,
            StairPoints,
        };
    }

    public void InitializeModelTriangles()
    {
        List<int> VoxelTriangles = new List<int>()
        {
            // x -
            0, 4, 1,
            1, 4, 5,

            // x +
            3, 7, 2,
            2, 7, 6,

            // y -
            2, 0, 3,
            3, 0, 1,

            // y +
            4, 6, 5,
            5, 6, 7,

            // z -
            1, 5, 3,
            3, 5, 7,

            // z +
            2, 6, 0,
            0, 6, 4,
        };

        List<int> StairTriangles = new List<int>()
        {
            // x -
            0, 16, 1,
            16, 17, 12,
            12, 13, 8,
            8, 9, 4,
            4, 5, 1,

            // x +
            3, 18, 2,
            3, 7, 6,
            6, 11, 10,
            10, 15, 14,
            14, 19, 18,

            // y -
            3, 2, 1,
            2, 0, 1,

            // z +
            2, 18, 16,
            2, 16, 0,

            // ALWAYS
            16, 18, 17,
            17, 18, 19,
            12, 14, 13,
            13, 14, 15,
            8, 10, 9,
            9, 10, 11,
            4, 6, 5,
            5, 6, 7,
            1, 5, 7,
            1, 7, 3,
            4, 9, 11,
            4, 11, 6,
            8, 13, 15,
            8, 15, 10,
            12, 17, 19,
            12, 19, 14,
        };

        ModelTriangles = new List<List<int>>()
        {
            VoxelTriangles,
            StairTriangles,
        };
    }

    public void InitializeModelFaces()
    {
        List<int> VoxelFaces = new List<int>()
        {
            // x -
            0, 2,

            // x +
            2, 2,

            // y -
            4, 2,

            // y +
            6, 2,

            // z -
            8, 2,

            // z +
            10, 2,

            // ALWAYS
            -1, -1
        };

        List<int> StairFaces = new List<int>()
        {
            // x -
            0, 5,

            // x +
            5, 5,

            // y -
            10, 2,

            // y +
            -1, -1,

            // z -
            -1, -1,

            // z +
            12, 2,

            // ALWAYS
            14, 16,
        };



        ModelFaces = new List<List<int>>()
        {
            VoxelFaces,
            StairFaces,
        };
    }

    // Each direction tells what adjacent obj types signal you don't have to render that face
    public void InitializeModelAdjacents()
    {
        List<int> VoxelAdjacents = new List<int>()
        {
            // x -
            0, -1, -1, -1, -1, -1, -1, -1, 

            // x +
            0, -1, -1, -1, -1, -1, -1, -1, 

            // y -
            0, -1, -1, -1, -1, -1, -1, -1, 

            // y +
            0, 1, -1, -1, -1, -1, -1, -1, 

            // z -
            0, 1, -1, -1, -1, -1, -1, -1, 

            // z +
            0, -1, -1, -1, -1, -1, -1, -1, 
        };

        List<int> StairAdjacents = new List<int>()
        {
            // x -
            1, -1, -1, -1, -1, -1, -1, -1, 

            // x +
            1, -1, -1, -1, -1, -1, -1, -1, 

            // y -
            0, -1, -1, -1, -1, -1, -1, -1, 

            // y +
            -1, -1, -1, -1, -1, -1, -1, -1, 

            // z -
            -1, -1, -1, -1, -1, -1, -1, -1, 

            // z +
            0, -1, -1, -1, -1, -1, -1, -1, 
        };

        ModelAdjacents = new List<List<int>>()
        {
            VoxelAdjacents,
            StairAdjacents,
        };
    }

    public void InitializeUVProfiles()
    {
        List<Vector2> VoxelUVs = new List<Vector2>()
        {
            new Vector2(0,  16), //p0
            new Vector2(32,  0), //p1
            new Vector2(0,   0), //p2
            new Vector2(64, 16), //p3
            new Vector2(0,  48), //p4
            new Vector2(32, 32), //p5
            new Vector2(32, 64), //p6
            new Vector2(64, 48), //p7
        };

        List<Vector2> StairUVs = new List<Vector2>()
        {
            new Vector2(0,  16), //p0
            new Vector2(32, 0), //p1
            new Vector2(0,0), //p2
            new Vector2(64, 16), //p3
            new Vector2(25, 12), //p4
            new Vector2(32, 8), //p5
            new Vector2(56, 28), //p6
            new Vector2(64, 25), //p7
            new Vector2(17, 24), //p8
            new Vector2(24, 21), //p9
            new Vector2(48, 40), //p10
            new Vector2(55, 37), //p11
            new Vector2(9, 36), //p12
            new Vector2(16, 33), //p13
            new Vector2(39, 52), //p14
            new Vector2(47, 49), //p15
            new Vector2(0, 49), //p16
            new Vector2(8, 45), //p17
            new Vector2(29, 64), //p18
            new Vector2(38, 60), //p19
        };


        UVProfiles = new List<List<Vector2>>()
        {
            VoxelUVs,
            StairUVs,
        };
    }

    public void FillModelPointsArr()
    {
        int ModelCount = ModelPoints.Count;
        ModelPointsArr = new Vector3[ModelCount * ModelPointsMaxSize];
        
        // Iterates once after each loop
        int ListIndex = 0;

        // Loop through all lists in ModelPoints
        foreach (List<Vector3> CurList in ModelPoints)
        {
            int ArrIndex = ListIndex * ModelPointsMaxSize;

            // Loop through each point in the CurList
            foreach (Vector3 Point in CurList)
            {
                ModelPointsArr[ArrIndex] = Point;
                ArrIndex++;
            }
            ListIndex++;
        }
    }
    
    public void FillModelTrianglesArr()
    {
        int ModelCount = ModelTriangles.Count;
        ModelTrianglesArr = new int[ModelCount * ModelTrianglesMaxSize];
        
        // Iterates once after each loop
        int ListIndex = 0;

        // Loop through all lists in ModelTriangles
        foreach (List<int> CurList in ModelTriangles)
        {
            int ArrIndex = ListIndex * ModelTrianglesMaxSize;

            // Loop through each int in the CurList
            foreach (int Int in CurList)
            {
                ModelTrianglesArr[ArrIndex] = Int;
                ArrIndex++;
            }
            ListIndex++;
        }
    }

    public void FillModelFacesArr()
    {
        int ModelCount = ModelFaces.Count;
        ModelFacesArr = new int[ModelCount * ModelFacesMaxSize];
        
        // Iterates once after each loop
        int ListIndex = 0;

        // Loop through all lists in ModelFaces
        foreach (List<int> CurList in ModelFaces)
        {
            int ArrIndex = ListIndex * ModelFacesMaxSize;

            // Loop through each integer in the CurList
            foreach (int Int in CurList)
            {
                ModelFacesArr[ArrIndex] = Int;
                ArrIndex++;
            }
            ListIndex++;
        }
    }

    public void FillModelAdjacentsArr()
    {
        int ModelCount = ModelAdjacents.Count;
        ModelAdjacentsArr = new int[ModelCount * ModelAdjacentsMaxSize];
        
        // Iterates once after each loop
        int ListIndex = 0;

        // Loop through all lists in ModelAdjacents
        foreach (List<int> CurList in ModelAdjacents)
        {
            int ArrIndex = ListIndex * ModelAdjacentsMaxSize;

            // Loop through each integer in the CurList
            foreach (int Int in CurList)
            {
                ModelAdjacentsArr[ArrIndex] = Int;
                ArrIndex++;
            }
            ListIndex++;
        }
    }

    public void FillUVProfilesArr()
    {
        int UVProfileCount = UVProfiles.Count;
        UVProfilesArr = new Vector2[UVProfileCount * UVProfilesMaxSize];
        
        // Iterates once after each loop
        int ListIndex = 0;

        // Loop through all lists in UVProfiles
        foreach (List<Vector2> CurList in UVProfiles)
        {
            int ArrIndex = ListIndex * UVProfilesMaxSize;

            // Loop through each int in the CurList
            foreach (Vector2 Point in CurList)
            {
                UVProfilesArr[ArrIndex] = Point;
                ArrIndex++;
            }
            ListIndex++;
        }
    }
}
