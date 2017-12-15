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

        Vector3[] vertices = new Vector3[density * density];
        Vector3[] normals = new Vector3[density * density];
        Vector2[] uv = new Vector2[density * density];
        int[] tri = new int[6 * ((density - 1) * (density - 1))];

        int triIndex = 0;
        for (int y = 0; y < orderedChildren.GetLength(0); y++)
        {
            int row = y * density;
            for (int x = 0; x < orderedChildren.GetLength(1); x++)
            {
                int column = x;

                vertices[column + row] = orderedChildren[x, y].position;
                normals[column + row] = -Vector3.forward;
                uv[column + row] = new Vector2((float)x / (density - 1), (float)y / (density - 1));
                Debug.Log(String.Format("UX for {0}: {1}", orderedChildren[x, y].gameObject.name, uv[column + row]));

                // for anything that's not the first row+column we set up the triangles
                if (column == 0 || row == 0)
                {
                    continue;
                }

                // Handles.Label(orderedChildren[x - 1, y - 1].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 1) + (row - density);

                // Handles.Label(orderedChildren[x - 0, y - 0].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 0) + (row - 0);

                // Handles.Label(orderedChildren[x - 1, y - 0].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 1) + (row - 0);


                // Handles.Label(orderedChildren[x - 0, y - 0].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 0) + (row - 0);

                // Handles.Label(orderedChildren[x - 1, y - 1].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 1) + (row - density);

                // Handles.Label(orderedChildren[x - 0, y - 1].position, (triIndex == shown) ? String.Format("{0}", triIndex) : "");
                tri[triIndex++] = (column - 0) + (row - density);
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
            int row = y * density;
            for (int x = 0; x < orderedChildren.GetLength(1); x++)
            {
                int column = x;

                vertices[column + row] = orderedChildren[x, y].localPosition;
            }
        }
        meshFilter.mesh.vertices = vertices;
    }

    void Update()
    {
        RenderSquare();
    }
}
