using UnityEngine;
using UnityEngine.Assertions;
using System;

public class CreateMeshFromChildren : MonoBehaviour {

    // pre-calculated constants
    private int density;
    private MeshFilter meshFilter;
    public int shown;
    [Range(0, 0.15f)]
    public float lineWidth = 0.025f;
    [Range(1.0f, 2.0f)]
    public float gridOffset = 1.0f;


    public Transform child;

    /// <summary>
    /// Children starting top left going left-to-right, top-to-bottom
    /// Indexed first by Y (row), then X (column)
    /// </summary>
    private Transform[,] orderedChildren;

    public int size = 10;
    void Awake()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = -size/2; x < size/2; x++)
            {
                Transform cube = (Transform)Instantiate(child, new Vector3(x * gridOffset, y * gridOffset, 0), Quaternion.identity);
                cube.transform.parent = transform;
            }
        }

        Transform min = transform.GetChild(0);
        Transform max = transform.GetChild(transform.childCount - 1);
        Bounds bounds = new Bounds();
        bounds.SetMinMax(min.position, max.position);
        Game.World.Instance.Map.Bounds = bounds;
    }

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

        {
            int x = 0;
            int y = 0;
            foreach (Transform child in transform)
            {
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

                vertices[column + row + 0] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, -0.5f, 0) * lineWidth;
                vertices[column + row + 1] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, 0.5f, 0) * lineWidth;
                vertices[column + row + 2] = orderedChildren[x, y].localPosition + new Vector3(0.5f, 0.5f, 0) * lineWidth;
                vertices[column + row + 3] = orderedChildren[x, y].localPosition + new Vector3(0.5f, -0.5f, 0) * lineWidth;

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
                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 1;
                        tri[triIndex++] = (column - 0) + (row - 0);
                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 2;

                        tri[triIndex++] = (column - 0) + (row - rowOffset) + 2;
                        tri[triIndex++] = (column - 0) + (row - 0);
                        tri[triIndex++] = (column - 0) + (row - 0) + 3;
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
                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 3;
                    tri[triIndex++] = (column + 0) + (row + 0) + 2;
                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 0;

                    tri[triIndex++] = (column + 0) + (row + rowOffset) + 0;
                    tri[triIndex++] = (column + 0) + (row + 0) + 2;
                    tri[triIndex++] = (column + 0) + (row + 0) + 1;

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

                vertices[column + row + 0] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, -0.5f, 0) * lineWidth;
                vertices[column + row + 1] = orderedChildren[x, y].localPosition + new Vector3(-0.5f, 0.5f, 0) * lineWidth;
                vertices[column + row + 2] = orderedChildren[x, y].localPosition + new Vector3(0.5f, 0.5f, 0) * lineWidth;
                vertices[column + row + 3] = orderedChildren[x, y].localPosition + new Vector3(0.5f, -0.5f, 0) * lineWidth; 
            }
        }
        meshFilter.mesh.vertices = vertices;
    }

    void Update()
    {
        RenderSquare();
    }
}
