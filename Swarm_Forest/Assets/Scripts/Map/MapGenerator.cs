using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ReadMap();
        Generate();
    }
    
    readonly string mapFilePath = "Map.txt";
    string[] map;
    int N, Gap;
    const int TerrainSize = 1000;

    void ReadMap(){
        map = File.ReadAllLines(mapFilePath);
        N = int.Parse(map[0]);
        Gap = TerrainSize / N;

        map = map.TakeLast(N).ToArray();
    }

    class TileInfo{
        public static readonly char None = '0';
        public static readonly char Tree = '1';
        public static readonly char Stone = '2';
        public static readonly char Elite = '3';
        public static readonly char Elite2 = '4';
    }

    void Generate(){
        var treePrefab = Resources.Load<GameObject>("Polytope Studio/Lowpoly_Environments/Prefabs/Trees/PT_Fruit_Tree_01_plums");
        var stonePrefab = Resources.Load<GameObject>("Polytope Studio/Lowpoly_Environments/Prefabs/Rocks/PT_Generic_Rock_01");

        var GapHalf = Gap / 2;

        for(int z = 0; z < N; ++z){
            for(int x = 0; x < N; ++x){
                var currentTile = map[z][x];
                Debug.Log($"z: {z}, x: {x}, val: {currentTile}");

                if(currentTile == TileInfo.None) continue;

                GameObject toInstantiate = null;
                int posX = x * Gap + GapHalf;
                int posZ = z * Gap + GapHalf;

                if(currentTile == TileInfo.Tree)
                    toInstantiate = treePrefab;
                if(currentTile == TileInfo.Stone)    
                    toInstantiate = stonePrefab;

                if(toInstantiate == null) continue;

                Instantiate(toInstantiate, new Vector3(posX, 0f, posZ), Quaternion.identity, gameObject.transform);
            }
        }
    }
}
