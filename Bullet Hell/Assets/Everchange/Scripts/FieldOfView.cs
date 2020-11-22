using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] public LayerMask obstacleMask, enemyMask;
    public Mesh mesh;
    public float fov;
    public Vector3 origin;
    public float startingAngle;
    //public List<Transform> visibleEnemies = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 360f;
        //StartCoroutine("findEnemiesWithDelay", .2f);
    }

    void LateUpdate()
    {
        int rayCount = 500;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 15f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, getVectorFromAngle(angle), viewDistance, obstacleMask);
            if (raycastHit2D.collider == null)
            {
                //No Hit
                vertex = origin + getVectorFromAngle(angle) * viewDistance;
            } else {
                //Hit object
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {

                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;                
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public static Vector3 getVectorFromAngle(float angle)
    {
        // angle  = 0 -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float getAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0)
        {
            n += 360;
        }
        return n;
    }

    public void setOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    // if we want to change fov
    public void setAimDirection(Vector3 aimDirection)
    {
        startingAngle = getAngleFromVectorFloat(aimDirection) - fov / 2f;
    }

    /*IEnumerator findEnemiesWithDelay(float delay)
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            findVisibleEnemies();
        }
    }

    public void findVisibleEnemies()
    {
        visibleEnemies.Clear();
        Collider[] enemiesInViewRadius = Physics.OverlapSphere(transform.position, fov, enemyMask);

        for(int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 dirToEnemy = (enemy.position - transform.position).normalized;
            float dstToEnemy = Vector3.Distance(transform.position, enemy.position);
            if(!Physics.Raycast(transform.position, dirToEnemy, dstToEnemy, obstacleMask)) {
                visibleEnemies.Add(enemy);
            }
        }
    }*/
}
