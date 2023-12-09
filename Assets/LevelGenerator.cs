using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform[] levelParts;
    [SerializeField] Vector3 nextPartPosition;

    [SerializeField] float distanceToSpawn;
    [SerializeField] float distanceToDelete;
    [SerializeField] Transform player;

    void Update()
    {
        GeneratePlatform();
        DeletePlatform();
    }

    private void GeneratePlatform()
    {
        if (Vector2.Distance(player.position, nextPartPosition) < distanceToSpawn)
        {
            Transform randomPart = levelParts[Random.Range(0, levelParts.Length)];

            Vector2 newPosition = new Vector2(nextPartPosition.x - randomPart.Find("StartPoint").position.x, 0);
            Transform newPart = Instantiate(randomPart, newPosition, transform.rotation, transform).transform;
            nextPartPosition = newPart.Find("EndPoint").position;
        }
    }

    private void DeletePlatform()
    {
        // 자식 게임 오브젝트에 대한 정보는 트랜스폼 컴포넌트가 갖는다
        if (transform.childCount > 0)
        {
            Transform partToDelete = transform.GetChild(0);

            if (Vector2.Distance(player.transform.position, partToDelete.position) > distanceToDelete)
            {
                // 가장 앞에 있는 플랫폼을 제거한다
                Destroy(partToDelete.gameObject);
            }
        }
    }
}
