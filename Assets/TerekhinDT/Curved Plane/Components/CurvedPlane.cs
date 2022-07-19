using UnityEngine;

namespace TerekhinDT.Curved_Plane.Components
{
    public class CurvedPlane : MonoBehaviour
    {
        public float diameter = 1;
        public float width = 1;
        public float height = 1f;
        public int edgesCount = 360;
        private int _pointsCount;

        private GameObject [] _verticesGo;
        private Vector3[] _vertices;
        private int[] _triangles;

        private MeshFilter _meshFilter;
        private Mesh _mesh;

        private GameObject _testObj;

        public bool isTesting;
        public bool isVisualization;
        
        private void Awake()
        {
            _pointsCount = (edgesCount + 1) * 2;
            _vertices = new Vector3[_pointsCount];
            
            _mesh = new Mesh();
            _meshFilter = GetComponent<MeshFilter>();
            
            if (isVisualization) InitVizObjects();
            if (isTesting) InitTesting();
        }

        private void InitVizObjects()
        {
            _verticesGo = new GameObject[_pointsCount];
            
            for (var i = 0; i < _pointsCount; i++)
            {
                _verticesGo[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                _verticesGo[i].transform.localScale = Vector3.one * 0.1f;
            }
        }

        private void InitTesting()
        {
            _testObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _testObj.transform.localScale = Vector3.one * 0.3f;
            _testObj.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        private void Testing()
        {
            var r = diameter / 2;
            var center = transform.position;
            
            var pos = new Vector3
            {
                x = Mathf.Cos(width/r*-1)*r,
                y = height/2,
                z = Mathf.Sin(width/r)*r
            };

            _testObj.transform.position = pos;
        }

        private void Start()
        {
            Create();
        }

        private void Update()
        {
            if (isTesting) Testing();
        }

        private void Create()
        {
            // drawing vertices
            var r = diameter / 2;
            var center = transform.position;
            var delta = (width / r)/edgesCount;
            
            for (var i = 0; i < _pointsCount; i++)
            {

                var angle = i % 2 == 0 ? delta * i / 2 : delta * (i-1) / 2;
                
                var pos = new Vector3
                {
                    x = Mathf.Cos(angle)*r,
                    y = i % 2 == 0 ? center.y - height / 2 : center.y + height / 2,
                    z = Mathf.Sin(angle)*r
                };

                _vertices[i] = pos;

                if (isVisualization)
                {
                    _verticesGo[i].transform.position = pos;
                }
            }
            
            //drawing triangles
            _triangles = new int[edgesCount*2*3];

            var j = 0;
            
            for (var i = 0; i < edgesCount * 2 * 3; i += 6, j += 2)
            {
                _triangles[i] = j;
                _triangles[i+1] = j+1;
                _triangles[i+2] = j+3;
                
                _triangles[i+3] = j+3;
                _triangles[i+4] = j+2;
                _triangles[i+5] = j;
                
            }
            
            // UV settings
            var uvs = new Vector2[_vertices.Length];
            for (var i = 0; i < uvs.Length; i++)
            {
                uvs[i] = _vertices[i];   
            }
            
            // Set values in MeshFilter
            _mesh.Clear();
            
            _mesh.vertices = _vertices;
            _mesh.triangles =  _triangles;
            _mesh.uv = uvs;
            
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
            
            _mesh.name = "Curved plane";
            _meshFilter.mesh = _mesh;
        }
    }
}
