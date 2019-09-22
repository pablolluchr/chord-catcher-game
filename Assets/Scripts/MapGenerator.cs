using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Transform tilePrefab;
    public GameObject treePrefab;
    public Vector3 treeOffset;
    public Vector2Int mapSize = new Vector2Int(10, 20); //should be even
    public Transform character;
    private Vector2Int lastRoundedCharacterPosition; //not updated character position floored.
    private Vector2Int mapOffset;
    private string holderName = "Generate Map";
    public Color grassColorA = new Color(173f / 255f, 224f / 255f, 78f / 255f, 1f);
    public Color grassColorB = new Color(173f / 255f, 224f / 255f, 78f / 255f, 1f);
    //reimplement color map by dictionary of position keys mapped to colors
    public Vector2Int[] colorMap = { new Vector2Int(0, 0), new Vector2Int(30, 0) };
    [Range(0,1)]
    public float treeDensity;


    // Start is called before the first frame update
    void Start()
    {
        generateBaseMap();
        lastRoundedCharacterPosition = new Vector2Int(Mathf.FloorToInt(character.position.x), Mathf.FloorToInt(character.position.z));
        mapOffset = new Vector2Int(0, 0);
    }

    private void FixedUpdate()
    {
        //check position of character to autogenerate more tiles
        Vector2Int roundedCharacterPosition = new Vector2Int(Mathf.FloorToInt(character.position.x), Mathf.FloorToInt(character.position.z));


        //check if player moves in order to generate new rows/columns in the map

        if (roundedCharacterPosition.x > lastRoundedCharacterPosition.x)
        {
            generateLine("right");
            lastRoundedCharacterPosition.x = roundedCharacterPosition.x;
            mapOffset.x = roundedCharacterPosition.x;

        }

        else if (roundedCharacterPosition.x < lastRoundedCharacterPosition.x)
        {
            generateLine("left");
            lastRoundedCharacterPosition.x = roundedCharacterPosition.x;
            mapOffset.x = roundedCharacterPosition.x;

        }
        else if (roundedCharacterPosition.y > lastRoundedCharacterPosition.y)
        {
            generateLine("up");
            lastRoundedCharacterPosition.y = roundedCharacterPosition.y;
            mapOffset.y = roundedCharacterPosition.y;

        }
        else if (roundedCharacterPosition.y < lastRoundedCharacterPosition.y)
        {
            generateLine("down");
            lastRoundedCharacterPosition.y = roundedCharacterPosition.y;
            mapOffset.y = roundedCharacterPosition.y;

        }

        DeleteTiles();

    }
    public void generateLine(string isLeftRightUpDown)
    {
        Transform mapHolder = GameObject.Find(holderName).transform;
        Vector3 tilePosition;
        int loopMax = 0;
        int xColor = 0; int yColor = 0; //variables needed for coloring


        if (string.Equals(isLeftRightUpDown, "left") || string.Equals(isLeftRightUpDown, "right"))
            loopMax = mapSize.y;
        else
            loopMax = mapSize.x;

        //generate tiles
        for (int i = 0; i < loopMax; i++)
        {
            //set position of starting tile
            if (string.Equals(isLeftRightUpDown, "left"))
                tilePosition = new Vector3(-mapSize.x / 2 - 0.5f + mapOffset.x, 0f, -mapSize.y / 2 + 0.5f + mapOffset.y + i); //position of new upmost tile
            else if (string.Equals(isLeftRightUpDown, "right"))
                tilePosition = new Vector3(mapSize.x / 2 + 0.5f + mapOffset.x, 0f, -mapSize.y / 2 + 0.5f + mapOffset.y + i); //position of new upmost tile
            else if (string.Equals(isLeftRightUpDown, "up"))
                tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + mapOffset.x + i, 0f, mapSize.y / 2 + 0.5f + mapOffset.y); //position of new upmost tile
            else //down
                tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + mapOffset.x + i, 0f, -mapSize.y / 2 - 0.5f + mapOffset.y); //position of new upmost tile

            //create new tiles
            Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
            newTile.parent = mapHolder;

            //set up color parameters

            if (string.Equals(isLeftRightUpDown, "left"))
            {
                xColor = Mathf.RoundToInt(-mapSize.x / 2 + mapOffset.x); 
                yColor = Mathf.RoundToInt(i + mapOffset.y);
            }
            else if (string.Equals(isLeftRightUpDown, "right"))
            {
                xColor = Mathf.RoundToInt(mapSize.x / 2 + mapOffset.x);
                yColor = Mathf.RoundToInt(i + mapOffset.y);
            }
            else if (string.Equals(isLeftRightUpDown, "top"))
            {
                xColor = Mathf.RoundToInt(i + mapOffset.x);
                yColor = Mathf.RoundToInt(mapSize.y/2 + mapOffset.y);
            }
            else //down
            {
                xColor = Mathf.RoundToInt(i + mapOffset.x);
                yColor = Mathf.RoundToInt(-mapSize.y / 2 + mapOffset.y);
            }

            ColorTile(newTile, xColor, yColor);

            GenerateRandomShit(newTile);
        }

    }


    private void GenerateRandomShit(Transform tilePosition) //TODO: FIX IT SO THAT ONCE TREES ARE FOUND IN AN AREA THEY DONT KEEP ON SPAWNING
    {
        int densityReducer = 30;

        bool generateTree = Random.value > (1 - treeDensity/densityReducer);
        if (generateTree)
        {
            Instantiate(treePrefab, tilePosition.position+ treeOffset, Quaternion.identity);
            //newTile.parent = mapHolder;
        }
    }

    //color tile based on its position
    private void ColorTile(Transform tile, int x, int y) 
    {
        //interpolate between colors in map using linear combination based on proximity. This should be extended to support n number of colors
        float proximityA = Vector2.Distance(new Vector2(tile.position.x, tile.position.z), colorMap[0]);
        float proximityB = Vector2.Distance(new Vector2(tile.position.x, tile.position.z), colorMap[1]);

        //normalize distances 
        float normalizedProximityA = 1 - proximityA / (proximityA + proximityB);
        float normalizedProximityB = 1- proximityB / (proximityA + proximityB);

        Color grassColor = new Color(grassColorA.r*normalizedProximityA+grassColorB.r*normalizedProximityB,
                                        grassColorA.g * normalizedProximityA + grassColorB.g * normalizedProximityB,
                                        grassColorA.b * normalizedProximityA + grassColorB.b * normalizedProximityB);

        float darker = 1 - Random.value * 0.03f; //sets the color darker
        float lighter = 1 + Random.value * 0.03f; //sets the base color lighter
        float yellower = 1 + (Random.value * 0.05f - 0.0025f);

        if (Mathf.Abs(x - y) % 2 == 0) //dark tile
            tile.GetComponent<Renderer>().material.color = new Color(grassColor.r * darker * yellower, grassColor.g * darker, grassColor.b * darker);
        else
            tile.GetComponent<Renderer>().material.color = new Color(grassColor.r * lighter * yellower, grassColor.g * lighter, grassColor.b * lighter);
    }
    //delete unseen tiles
   private void DeleteTiles()
    {
        Transform mapHolder = GameObject.Find(holderName).transform;

        //loop through tiles and delete those off the camera view
        foreach (Transform child in mapHolder)
        {
            //check to delete unwanted tiles
            if (child.position.x >=mapSize.x/2 + mapOffset.x+0.5f  || child.position.x <= -mapSize.x / 2 + mapOffset.x - 0.5f ||
                child.position.z >= mapSize.y / 2 + mapOffset.y + 0.5f || child.position.z <= -mapSize.y / 2 + mapOffset.y - 0.5f)
            {
                Destroy(child.gameObject);
            }
        }
    }
    //check equality between floats (up to epsilon)
    private bool IsEqual(float a, float b)
    {
        if (a >= b - Mathf.Epsilon && a <= b + Mathf.Epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void generateBaseMap() 
    {

        if (transform.Find(holderName))
            DestroyImmediate(transform.Find(holderName).gameObject);


        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0f, -mapSize.y / 2 + 0.5f + y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.parent = mapHolder;

                // set color
                ColorTile(newTile, x, y);


            }
        }
    }




}
