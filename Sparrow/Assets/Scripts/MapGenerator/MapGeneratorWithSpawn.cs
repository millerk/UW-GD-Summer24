using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneratorWithSpawn : MonoBehaviour
{

    //tile
    Dictionary<int, GameObject> tileSet; //gives access to prefabs
    Dictionary<int, GameObject> tileGroups;
    public GameObject prefabDeepWater;
    public GameObject prefabWater;
    public GameObject prefabSand;
    public GameObject prefabGrass;

    //Spawner
    public GameObject player;
    public GameObject enemyPrefab; // The enemy prefab
    public int numberOfEnemies = 5;
    public float enemySpawnRadius = 5f;
    public float minDistanceFromPlayer = 10f;

    //Perlin Noise
    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 30f;
    public int octaves = 3;
    public float persistence = 0.1f;
    public float lacunarity = 10f;
    public int seed = 0;

    public AnimationCurve heightCurve;

    private float[,] noiseMap;

    //Grid
    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<GameObject>> tileGrid = new List<List<GameObject>>();

    //Collider Boarder
    public Vector2 terrainMin; // Minimum bounds of your terrain keep min set to 0, set max to mapWidth - 1
    public Vector2 terrainMax; // Maximum bounds of your terrain keep min set to 0, set max to mapHeight - 1

    //Tile Collider
    public GameObject[] gameObjectAddCollider;

    //Enemy Spawning Positions
    private List<Vector2Int> enemySpawnPoints = new List<Vector2Int>();

    //Ref to CameraController
    public CameraController cameraController;

    //Activations on Start
    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateNoiseMap();
        GenerateMap();
        CreateBorderColliders();      
        SetTileColliders();

        Vector3 playerSpawnPosition = AddCharacterSpawnerToRandomTileOfType(prefabDeepWater);

        FindEnemySpawnPoints();
        SpawnEnemies();

        Vector3 mapCenter = new Vector3(mapWidth / 2, mapHeight / 2, 0);

        if (cameraController != null)
        {
            cameraController.SetCameraTarget(playerSpawnPosition, mapCenter, mapWidth, mapHeight);
        }
        else
        {
            Debug.LogError("CameraController reference is not set.");
        }

    }

    void GenerateNoiseMap()
    {
        noiseMap = new float[mapWidth, mapHeight];

        // Generate random seed if seed is zero
        if (seed == 0)
            seed = Random.Range(0, 100000);

        // Create a random number generator with the seed
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        // Calculate max possible height (for normalizing)
        for (int i = 0; i < octaves; i++)
        {
            maxPossibleHeight += amplitude;
            amplitude *= persistence;
        }

        // Calculate offsets for each octave
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000);
            float offsetY = prng.Next(-100000, 100000);
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Generate Perlin noise map
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / noiseScale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / noiseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                // Normalize noise height to range between 0 and 1
                noiseMap[x, y] = noiseHeight / maxPossibleHeight;
                // Keep track of min and max noise height
                if (noiseMap[x, y] > maxNoiseHeight)
                    maxNoiseHeight = noiseMap[x, y];
                if (noiseMap[x, y] < minNoiseHeight)
                    minNoiseHeight = noiseMap[x, y];
            }
        }

        // Apply falloff map
        float[,] falloffMap = GenerateFalloffMap(mapWidth, mapHeight);
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
            }
        }
    }

    // Generate falloff map
    float[,] GenerateFalloffMap(int width, int height)
    {
        float[,] map = new float[width, height];

        float maxDist = Mathf.Sqrt(width * width / 4f + height * height / 4f);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float distX = x - width / 2f;
                float distY = y - height / 2f;
                float value = Mathf.Sqrt(distX * distX + distY * distY) / maxDist;
                map[x, y] = heightCurve.Evaluate(value);
            }
        }
        return map;
    }

    void CreateTileset()
    {
        /** Collect and assign ID codes to tile prefabs, for ease of access.
            Best ordered to match land elevation. **/
        tileSet = new Dictionary<int, GameObject>();
        tileSet.Add(0, prefabDeepWater);
        tileSet.Add(1, prefabWater);
        tileSet.Add(2, prefabSand);
        tileSet.Add(3, prefabGrass);

    }

    void CreateTileGroups()
    {
        /**Create empty gameobjects for grouping tiles of the same type, ie forest tiles **/
        tileGroups = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> prefabPair in tileSet)
        {
            GameObject tileGroup = new GameObject(prefabPair.Value.name);
            tileGroup.transform.parent = gameObject.transform;
            tileGroup.transform.localPosition = new Vector3(0, 0, 0);
            tileGroups.Add(prefabPair.Key, tileGroup);
        }
    }

    void GenerateMap()
    {
        /** Generate a 2D grid using Perlin noise function, storing it as both rad ID values
            and tile gameobjects. **/
        for (int x = 0; x < mapWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            tileGrid.Add(new List<GameObject>());

            for (int y = 0; y < mapHeight; y++)
            {
                int tileID = GetIdUsingPerlin(x, y);
                noiseGrid[x].Add(tileID);
                createTile(tileID, x, y);

            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        // Using a grid coordinate input, generate a Perlin noise value to be converted into a
        //    tile ID code. Rescale the normalised Perlin value to the number of tiles available.
        float rawPerlin = noiseMap[x, y];

        float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileSet.Count;
        if (scalePerlin == 4)
        {
            scalePerlin = 3;
        }
        return Mathf.FloorToInt(scalePerlin);

    }

    void createTile(int tileID, int x, int y)
    {
        /** Creates a new tile using the type id code, group it with common tile, sets it's
            position and store the gameobject. **/
        GameObject tilePrefab = tileSet[tileID];
        GameObject tileGroup = tileGroups[tileID];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = string.Format("tile_x{0}_y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tileGrid[x].Add(tile);
    }


    void CreateBorderColliders()
    {
        // Create edge colliders for each side of the terrain

        // Bottom edge
        AddEdgeCollider2D(new Vector2(terrainMin.x, terrainMin.y), new Vector2(terrainMax.x, terrainMin.y));

        // Top edge
        AddEdgeCollider2D(new Vector2(terrainMin.x, terrainMax.y), new Vector2(terrainMax.x, terrainMax.y));

        // Left edge
        AddEdgeCollider2D(new Vector2(terrainMin.x, terrainMin.y), new Vector2(terrainMin.x, terrainMax.y));

        // Right edge
        AddEdgeCollider2D(new Vector2(terrainMax.x, terrainMin.y), new Vector2(terrainMax.x, terrainMax.y));
    }

    void AddEdgeCollider2D(Vector2 pointA, Vector2 pointB)
    {
        GameObject colliderObject = new GameObject("EdgeCollider");
        colliderObject.transform.parent = transform; // Attach to your border container

        EdgeCollider2D collider = colliderObject.AddComponent<EdgeCollider2D>();
        collider.points = new Vector2[] { pointA, pointB };
    }

    void SetTileColliders() {
        //Sets Collieder to Specified GameObject Tiles
        gameObjectAddCollider = new GameObject[] { prefabSand, prefabGrass };
        foreach (GameObject obj in gameObjectAddCollider)
        {
            // Add BoxCollider2D component if not already present
            if (!obj.GetComponent<BoxCollider2D>())
            {
                obj.AddComponent<BoxCollider2D>();
            }
        }
    }
        

    Vector3 AddCharacterSpawnerToRandomTileOfType(GameObject tileType)
        {
        // Create a list to store tiles of the specified type
        List<GameObject> tilesOfType = new List<GameObject>();

        // Iterate over tileGrid to find tiles of the specified type
        foreach (var row in tileGrid)
        {
            foreach (var tile in row)
            {
                if (tile != null && tile.CompareTag(tileType.tag)) // Use tag or another identifier
                {
                    tilesOfType.Add(tile);
                }
            }
        }

        // Ensure there are tiles of the specified type
        if (tilesOfType.Count == 0)
        {
            Debug.LogWarning($"No tiles of the type {tileType.name} found.");
            return Vector3.zero;
        }

        // Pick a random tile from the list
        GameObject randomTile = tilesOfType[Random.Range(0, tilesOfType.Count)];

        // Add a CharacterSpawner component to the chosen tile
        AddCharacterSpawner(randomTile);

        // Return the spawn position
        return randomTile.transform.position;
    }

    void AddCharacterSpawner(GameObject tile)
    {
        if (tile != null)
        {
            CharacterSpawner spawner = tile.AddComponent<CharacterSpawner>();
            spawner.characterPrefab = player;

            // Optionally, immediately spawn a character
            spawner.SpawnCharacter();
        }
        else
        {
            Debug.LogWarning("Selected tile is null.");
        }
    }

    void FindEnemySpawnPoints()
    {
        int spawnTileID = 0;//first prefab in in tile ID set (ex. prefabDeepWater)

        enemySpawnPoints.Clear();
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (noiseGrid[x][y] == spawnTileID)
                {
                    enemySpawnPoints.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    void SpawnEnemies()
    {
        if (enemySpawnPoints.Count == 0)
        {
            Debug.LogWarning("No valid enemy spawn points found.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("Player object is not assigned.");
            return;
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2Int spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)];
            Vector3 spawnPosition = new Vector3(spawnPoint.x, spawnPoint.y, 0);

            // Ensure enemies are spawned at least 'minDistanceFromCharacterSpawner' away from the character
            float distanceFromPlayer = Vector3.Distance(spawnPosition, player.transform.position);
            if (distanceFromPlayer < minDistanceFromPlayer)
                {
                    i--;
                    continue;
                }

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }            
        }
    }