using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    Level level;

    GameSession gameStatus;

    [SerializeField] int timeHit;

    private void Start()
    {
        CountBreakableBlocks();

        gameStatus = FindObjectOfType<GameSession>();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Breakable")
            level.CountBreakableBlocks();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHit();
        }

    }

    private void HandleHit()
    {
        timeHit++;

        int maxHits = hitSprites.Length + 1;

        if (timeHit >= maxHits)
            DestroyBlock();
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timeHit-1;

        if (hitSprites[spriteIndex] != null)
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        else
        {
            Debug.LogError("Block sprite is missing from array: "+ gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayBlockDestroy();

        Destroy(gameObject);

        level.BlockDestoryed();

        TriggerSparklesVFX();
    }

    private void PlayBlockDestroy()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);

        gameStatus.AddToScore();
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);

        Destroy(sparkles, 1f);
    }
}
