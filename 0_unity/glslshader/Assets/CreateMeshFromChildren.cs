using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class CreateMeshFromChildren : MonoBehaviour {

    // pre-calculated constants
    private int density;
    private MeshFilter meshFilter;
    [Range(0, 24)]
    public int shown;
    public float width = 0.2f;

    /// <summary>
    /// Children starting top left going left-to-right, top-to-bottom
    /// Indexed first by Y (row), then X (column)
    /// </summary>
    private Transform[,] orderedChildren;

    void Start()
    {
        InitMesh();
    }

    void InitMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();

        float countSqrt = Mathf.Sqrt(transform.childCount);
        Assert.IsTrue(transform.childCount > 4, "Object containing this Script has less than 4 children!");
        Assert.IsTrue(countSqrt == Mathf.Floor(countSqrt), String.Format("Cannot have a non-square grid! {0} children are used, but only {1} or {2} are valid!", transform.childCount, Mathf.Floor(countSqrt), Mathf.Ceil(countSqrt)));

        density = (int)countSqrt;
        orderedChildren = new Transform[density, density];
        Debug.Log(String.Format("Found {0}x{0} vertices.", countSqrt));

        {
            int x = 0;
            int y = 0;
            foreach (Transform child in transform)
            {
                Debug.Log(String.Format("Transform {0},{1}: Name '{2}'", x, y, child.gameObject.name));
                orderedChildren[x, y] = child;
                x++;
                if (x == density)
                {
                    y++;
                    x = 0;
                }
            }
        }

        Vector3[] vertices = new Vector3[density * density * 4];
        Vector3[] normals = new Vector3[density * density * 4];
        Vector2[] uv = new Vector2[density * density * 4];
        int[] tri = new int[(12 * (density - 1) * (density - 1)) + 12 * (density - 1)];

        int triIndex = 0;
        int rowOffset = density * 4;
        int columnOffset = 4;

        for (int y = 0; y < orderedChildren.GetLength(0); y++)
        {
            int row = y * density * 4;
            for (int x = 0; x < orderedChildren.GetLength(1); x++)
            {
                int column = x * 4;

                vertices[column + row + 0] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, -0.5f, 0) * width;
                vertices[column + row + 1] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, 0.5f, 0) * width;
                vertices[column + row + 2] = orderedChildren[x, y].localPosition + new Vector3(0.5f, 0.5f, 0) * width;
                vertices[column + row + 3] = orderedChildren[x, y].localPosition + new Vector3(0.5f, -0.5f, 0) * width;

                normals[column + row + 0] = -Vector3.forward;
                normals[column + row + 1] = -Vector3.forward;
                normals[column + row + 2] = -Vector3.forward;
                normals[column + row + 3] = -Vector3.forward;
                uv[column + row + 0] = Vector2.zero;
                uv[column + row + 1] = Vector2.zero;
                uv[column + row + 2] = Vector2.zero;
                uv[column + row + 3] = Vector2.zero;

                if (x == (orderedChildren.GetLength(0) - 1) || y == (orderedChildren.GetLength(1)-1))
                {
                    if (x == (orderedChildren.GetLength(0) - 1) && y != 0)
                    {
                        // connect current dot to dot above
                        tri[triIndex++] = (column - 0) + (row - 0);
                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 1;
                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 2;

                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 2;
                        tri[triIndex++] = (column - 0) + (row - 0) + 3;
                        tri[triIndex++] = (column - 0) + (row - 0);
                    }

                    if (y == (orderedChildren.GetLength(1) - 1) && x != 0)
                    {
                        // connect current dot to dot to the left
                        tri[triIndex++] = (column - columnOffset) + (row) + 3;
                        tri[triIndex++] = (column - columnOffset) + (row) + 2;
                        tri[triIndex++] = (column - 0) + (row - 0) + 1;

                        tri[triIndex++] = (column - 0) + (row - 0) + 1;
                        tri[triIndex++] = (column - 0) + (row - 0);
                        tri[triIndex++] = (column - columnOffset) + (row) + 3;
                    }
                }
                else
                {
                    // connect current dot to dot below
                    tri[triIndex++] = (column + 0) + (row + 0) + 2;
                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 3;
                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 0;

                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 0;
                    tri[triIndex++] = (column + 0) + (row + 0) + 1;
                    tri[triIndex++] = (column + 0) + (row + 0) + 2;

                    // connect current dot to dot to the right
                    tri[triIndex++] = (column + columnOffset) + (row) + 1;
                    tri[triIndex++] = (column + columnOffset) + (row) + 0;
                    tri[triIndex++] = (column + 0) + (row + 0) + 3;

                    tri[triIndex++] = (column + 0) + (row + 0) + 3;
                    tri[triIndex++] = (column + 0) + (row + 0) + 2;
                    tri[triIndex++] = (column + columnOffset) + (row) + 1;
                }
            }
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.triangles = tri;
        meshFilter.mesh.normals = normals;
        meshFilter.mesh.uv = uv;
    }

    void RenderSquare()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int y = 0; y < orderedChildren.GetLength(0); y++)
        {
            int row = y * density * 4;
            for (int x = 0; x < orderedChildren.GetLength(1); x++)
            {
                int column = x * 4;

                vertices[column + row + 0] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, -0.5f, 0) * width;
                vertices[column + row + 1] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, 0.5f, 0) * width;
                vertices[column + row + 2] = orderedChildren[x, y].localPosition + new Vector3(0.5f, 0.5f, 0) * width;
                vertices[column + row + 3] = orderedChildren[x, y].localPosition + new Vector3(0.5f, -0.5f, 0) * width;
            }
        }
        meshFilter.mesh.vertices = vertices;

        for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
        {
            Handles.Label(meshFilter.mesh.vertices[i], (i == shown) ? String.Format("{0}", i) : "");
        }
    }

    void OnDrawGizmos()
    {
        RenderSquare();
    }
}
