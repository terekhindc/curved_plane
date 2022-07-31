using UnityEngine;

namespace TerekhinDT.Curved_Plane.Components
{
    /// <summary>
    /// Procedural generation of curved plane
    /// </summary>
    public class CurvedPlane : MonoBehaviour
    {
        public float diameter = 1;
        public float width = 1.57f;
        public float height = 1f;
        public int edgesCount = 30;
        private int _pointsCount;

        private Vector3[] _vertices;
        private int[] _triangles;

        private MeshFilter MeshFilter => GetComponent<MeshFilter>();
        private Mesh _mesh;

        private void Start ()
        {
            _pointsCount = (edgesCount + 1) * 2;
            _vertices = new Vector3[_pointsCount];
            
            _mesh = new Mesh();
            
            Create();
        }


        /// <summary>
        /// Creation geometry
        /// </summary>
        private void Create()
        {
            var r = diameter / 2;
            var center = transform.position;
            var delta = (width / r)/edgesCount;

            for (var i = 0; i < _pointsCount; i++)
            {

                var angle = i % 2 == 0 ? delta * i / 2 : delta * (i-1) / 2;
                
                var pos = new Vector3
                {
                    x = Mathf.Cos(angle)*r,
                    y = i % 2 == 0 ? center.y - height/2 : center.y + height/2,
                    z = Mathf.Sin(angle*-1)*r
                };

                _vertices[i] = pos;
            }
            
            //Drawing triangles
            _triangles = new int[edgesCount*2*3];

            var j = 0;
            
            for (var i = 0; i < edgesCount * 2 * 3; i += 6, j += 2)
            {
                _triangles[i] = j;
                _triangles[i+1] = j+2;
                _triangles[i+2] = j+3;
                
                _triangles[i+3] = j+3;
                _triangles[i+4] = j+1;
                _triangles[i+5] = j;
                
            }
            
            // UV settings
            var uvs = new Vector2[_vertices.Length];
            var step = (1.0f / _vertices.Length);

            for (var i = 0; i < uvs.Length; i+=2)
            {
                var x = 1 - (step * i);
                
                var pos1 = new Vector2(x, 0);
                var pos2 = new Vector2(x, 1);
                
                uvs[i] = pos1;
                uvs[i+1] = pos2;
            }
            
            // Set values in MeshFilter
            _mesh.Clear();
            
            _mesh.vertices = _vertices;
            _mesh.triangles =  _triangles;
            _mesh.uv = uvs;
            
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
            
            _mesh.name = "CurvedPlane";
            MeshFilter.mesh = _mesh;
        }
    }
}
