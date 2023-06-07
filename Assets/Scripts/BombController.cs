using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{    
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombFuseTime = 3f;
    public int bombAmount = 3;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;


    private void OnEnable()
    {
        bombsRemaining = bombAmount;        
    }

    private void Update()
    {
        if (bombsRemaining > 0 && Input.GetKeyDown(inputKey)){
            StartCoroutine(PlaceBomb());            
        }
    }

    private IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        Destroy(explosion.gameObject, explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);


        Destroy(bomb);
        bombsRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int lenght)
    {
        if (lenght <= 0) {return;}

        position += direction;

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(lenght > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);
        // Destroy(explosion.gameObject, explosionDuration);

        Explode(position, direction, lenght -1);

    }

  

}
