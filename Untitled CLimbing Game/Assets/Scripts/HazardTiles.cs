using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HazardTiles : MonoBehaviour
{
    public GridLayout gridLayout;
    public Tilemap hazardsTilemap;
    private TileBase tileBase;
    private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        hazardsTilemap = GetComponent<Tilemap>();
        gridLayout = hazardsTilemap.GetComponentInParent<GridLayout>();
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //GetContact(0) gets the first contact point between colliders
            //point gets the world space coordinates of the contact point
            Vector2 collisionLocation = collision.GetContact(0).point;

            //convert that world point to cell coordinates and then return the tile at that position
            Vector3Int cellLocation = gridLayout.WorldToCell(collisionLocation);
            tileBase = hazardsTilemap.GetTile(cellLocation);

            ManageHazard(tileBase);
        }
    }

    private void ManageHazard(TileBase hazardTile)
    {
        if(hazardTile != null)
        {
            if (hazardTile.name == "SpikeTile" || hazardTile.name == "SpikeTile_Down")
            {
                playerHealth.DamagePlayer(100, true);
            }
            else if (hazardTile.name == "SmallSpikes")
            {
                playerHealth.DamagePlayer(25);
            }
        }
    }
}
