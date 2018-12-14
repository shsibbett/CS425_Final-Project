using System.Collections.Generic;
using UnityEngine;

public class MazeMeshGenerator
{    
    // public float width;
    // public float height;

    public int width;
    public int height;

    public MazeMeshGenerator()
    {
        //width = 3.75f;
        //height = 3.5f;
        
        width = 4;
        height = 4;
    }

    public Mesh FromData(PathNode[,] data)
    {
        Mesh maze = new Mesh();

        List<Vector3> newVertices = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();

        maze.subMeshCount = 4;
        List<int> floorTiles = new List<int>();
        List<int> trapTiles = new List<int>();
        List<int> wallTiles = new List<int>();
        List<int> ceilingTiles = new List<int>();

        int maxRows = data.GetUpperBound(0);
        int maxColumns = data.GetUpperBound(1);
        float heightHalf = height * .5f;

        for (int i = 0; i <= maxRows; i++)
        {
            for (int j = 0; j <= maxColumns; j++)
            {
                if (data[i, j].data != 1)
                {
                    if (data[i, j].data == 0 || data[i, j].data == -1) { // floor tile
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, 0, i * width),
                            Quaternion.LookRotation(Vector3.up),
                            new Vector3(width, width, 1)
                        ), ref newVertices, ref newUVs, ref floorTiles);
                    } else { // trap tile
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, 0, i * width),
                            Quaternion.LookRotation(Vector3.up),
                            new Vector3(width, width, 1)
                        ), ref newVertices, ref newUVs, ref trapTiles);
                    }

                    // ceiling
                    AddQuad(Matrix4x4.TRS(
                        new Vector3(j * width, height, i * width),
                        Quaternion.LookRotation(Vector3.down),
                        new Vector3(width, width, 1)
                    ), ref newVertices, ref newUVs, ref ceilingTiles);

                    if (i - 1 < 0 || data[i-1, j].data == 1) // wall on sides of blocked cells
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, heightHalf, (i-.5f) * width),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTiles);
                    }

                    if (j + 1 > maxColumns || data[i, j+1].data == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j+.5f) * width, heightHalf, i * width),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTiles);
                    }

                    if (j - 1 < 0 || data[i, j-1].data == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3((j-.5f) * width, heightHalf, i * width),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTiles);
                    }

                    if (i + 1 > maxRows || data[i+1, j].data == 1)
                    {
                        AddQuad(Matrix4x4.TRS(
                            new Vector3(j * width, heightHalf, (i+.5f) * width),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(width, height, 1)
                        ), ref newVertices, ref newUVs, ref wallTiles);
                    }
                }
            }
        }

        maze.vertices = newVertices.ToArray();
        maze.uv = newUVs.ToArray();
        
        maze.SetTriangles(floorTiles.ToArray(), 0);
        maze.SetTriangles(wallTiles.ToArray(), 1);
        maze.SetTriangles(ceilingTiles.ToArray(), 2);
        maze.SetTriangles(trapTiles.ToArray(), 3);

        maze.RecalculateNormals();

        return maze;
    }

    private void AddQuad(Matrix4x4 matrix, ref List<Vector3> newVertices,
        ref List<Vector2> newUVs, ref List<int> newTriangles)
    {
        int index = newVertices.Count;

        // corners before transforming
        Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
        Vector3 vert2 = new Vector3(-.5f, .5f, 0);
        Vector3 vert3 = new Vector3(.5f, .5f, 0);
        Vector3 vert4 = new Vector3(.5f, -.5f, 0);

        newVertices.Add(matrix.MultiplyPoint3x4(vert1));
        newVertices.Add(matrix.MultiplyPoint3x4(vert2));
        newVertices.Add(matrix.MultiplyPoint3x4(vert3));
        newVertices.Add(matrix.MultiplyPoint3x4(vert4));

        newUVs.Add(new Vector2(1, 0));
        newUVs.Add(new Vector2(1, 1));
        newUVs.Add(new Vector2(0, 1));
        newUVs.Add(new Vector2(0, 0));

        newTriangles.Add(index+2);
        newTriangles.Add(index+1);
        newTriangles.Add(index);

        newTriangles.Add(index+3);
        newTriangles.Add(index+2);
        newTriangles.Add(index);
    }

}

