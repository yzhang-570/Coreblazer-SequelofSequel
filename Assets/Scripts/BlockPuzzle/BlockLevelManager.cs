using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
// using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    public BlockPuzzleManager bossman;
    public float hintTimer;
    float timer = 0;
    public static int pixelsPerUnit = 60;
    public BlockHint hintManager;
    public GameObject blockPrefab;
    public Canvas canvas;

    public new Camera camera;

    public Image solutionImg;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    // IMPORTANT: positions will be offset according to blockOffset
    // maps type of block (must use fullName to account for hflip/vflip) to list of all current positions on grid
    // Vector is (x position, y position, ID)
    Dictionary<string, List<Vector3Int>> blockLocations = new Dictionary<string, List<Vector3Int>>();
    // Same as blockLocations but without a z coordinate
    Dictionary<string, List<Vector2Int>> solution = new Dictionary<string, List<Vector2Int>>();

    bool popupIsOpen = false;

    public static Vector3Int blockOffset = new Vector3Int(3, -2, 0);

    string hintedBlock = null;

    public void endLevel()
    {
        bossman.endLevel();
    }

    public void initLevel(string[] solutionArray, Sprite solutionsrc)
    {
        blocks.Clear();
        blockLocations.Clear();
        hintManager.initLevel();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("PuzzleBlock");
        foreach (GameObject go in gos)
            Destroy(go);

        solutionImg.sprite = solutionsrc;

        int id = 0;
        solution.Clear();
        foreach (string blockSoln in solutionArray)
        {
            string[] components = blockSoln.Split(",");
            Vector2Int pos = new Vector2Int(
                -60 + int.Parse(components[2]) * pixelsPerUnit + blockOffset.x,
                240 - (int.Parse(components[1]) * pixelsPerUnit) + blockOffset.y
            );
            if (solution.ContainsKey(components[0]))
            {
                solution[components[0]].Add(pos);
            }
            else
            {
                List<Vector2Int> lst = new List<Vector2Int>()
                {
                    pos
                };
                solution[components[0]] = lst;
            }
            string name = components[0].Substring(0, components[0].Length - 2);

            spawnBlock(id + 1, BlockType.stringToBlock(name), id,
                charToBool(components[0][components[0].Length - 2]), charToBool(components[0][components[0].Length - 1]));
            ++id;
        }
    }

    bool charToBool(char b)
    {
        return b == 'T';
    }

    public void setPopupStatus(bool status)
    {
        if (status == popupIsOpen)
        {
            return;
        }
        popupIsOpen = status;
        foreach (var block in blocks.Values)
        {
            block.GetComponent<Block2>().setEnabled(!status);
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        if (hintedBlock != null)
        {
            hintManager.hideBlock();
            hintedBlock = null;
        }
    }

    public void showHint()
    {
        if (timer > 0)
        {
            return;
        }
        timer = hintTimer;
        KeyValuePair<string, Vector2Int> id = getFirstMismatch();
        hintedBlock = id.Key;

        BlockType hintType = BlockType.stringToBlock(id.Key.Substring(0, id.Key.Length - 2));
        hintType.hflipped = charToBool(id.Key[id.Key.Length - 2]);
        hintType.vflipped = charToBool(id.Key[id.Key.Length - 1]);

        hintManager.showBlock(
            hintType,
            new Vector3Int(
                id.Value.x, id.Value.y, 0
                // -60 + id.Value.x * pixelsPerUnit + blockOffset.x,
                // 240 - (id.Value.y * pixelsPerUnit) + blockOffset.y,
                // 0
            ),
            this, canvas
        );
    }

    private static int Vector3IntCmp(Vector3Int a, Vector3Int b)
    {
        if (a.x == b.x)
        {
            if (a.y == b.y)
            {
                return b.z - a.z;
            }
            return b.y - a.y;
        }
        return b.x - a.x;
    }

    private static int Vector2IntCmp(Vector2Int a, Vector2Int b)
    {
        if (a.x == b.x)
        {
            return b.y - a.y;
        }
        return b.x - a.x;
    }

    KeyValuePair<string, Vector2Int> getFirstMismatch()
    {
        foreach (string key in blockLocations.Keys)
        {
            blockLocations[key].Sort(Vector3IntCmp);
        }

        foreach (string key in solution.Keys)
        {
            solution[key].Sort(Vector2IntCmp);
        }

        foreach (var kv in solution.Reverse())
        {
            string key = kv.Key;
            if (!blockLocations.ContainsKey(key))
            {
                return new KeyValuePair<string, Vector2Int>(key, solution[key][0]);
            }
            int i = 0;
            int j = 0;
            while (i < blockLocations[key].Count && j < solution[key].Count)
            {
                Vector3 v1 = blockLocations[key][i];
                Vector2 v2 = solution[key][j];
                if (v1.x != v2.x || v1.y != v2.y)
                {
                    return new KeyValuePair<string, Vector2Int>(key, solution[key][j]);
                }
                ++i; ++j;
            }
            if (i < j)
            {
                return new KeyValuePair<string, Vector2Int>(key, solution[key][j]);
            }
        }
        return new KeyValuePair<string, Vector2Int>(null, Vector2Int.zero);
    }

    void spawnBlock(int id, BlockType type, int count, bool hflip, bool vflip)
    {
        int x = count % 4;
        int y = count / 4;

        Vector3 position = new Vector3(-600f + 125f * x, 250f - 170f * y, 0);
        Vector3 jitter = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0);

        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, canvas.transform);
        // Vector3 blockpos = block.GetComponent<RectTransform>().anchoredPosition3D;
        // blockpos.z = 0f;
        // block.GetComponent<RectTransform>().anchoredPosition3D = blockpos;

        type.hflipped = hflip;
        type.vflipped = vflip;
        block.GetComponent<Block2>().initBlock(id, type, this, canvas);
        blocks.Add(id, block);

        block.GetComponent<Block2>().placeBlockAt(position + jitter);

        block.GetComponent<Block2>().initResetPosition();

        addBlock(type, id, new Vector3Int(-1000000, -1000000, id));
    }

    public bool metRequirements()
    {
        return getFirstMismatch().Key == null;
    }

    // horiz range: -60 to 420
    // vert range: -360 to 240
    // cell size: 60 x 60
    public Vector3Int snapToGrid(Vector3 world, BlockType blockType)
    {
        AudioManager.Instance.PlaySFX("click");
        return new Vector3Int(Mathf.RoundToInt(world.x / pixelsPerUnit) * pixelsPerUnit,
                                Mathf.RoundToInt(world.y / pixelsPerUnit) * pixelsPerUnit + blockType.height * pixelsPerUnit);
    }

    public bool checkBlockPosition(Vector3 pos, BlockType blockType)
    {
        Vector3 gridPos = snapToGrid(pos, blockType);
        return gridPos.x >= -60 && gridPos.x + blockType.width * pixelsPerUnit <= 420 &&
                gridPos.y <= 240 && gridPos.y - blockType.height * pixelsPerUnit >= -300;
    }

    public void updateBlock(int id, BlockType type, Vector3Int newPos)
    {
        removeBlock(type, id);
        addBlock(type, id, newPos);
    }

    Vector3Int encodeBlockPos(int id, Vector3Int newPos)
    {
        return new Vector3Int(newPos.x, newPos.y, id);
    }

    public void addBlock(BlockType type, int id, Vector3Int newPos)
    {
        string key = type.fullName();
        if (blockLocations.ContainsKey(key))
        {
            for (int i = 0; i < blockLocations[key].Count; ++i)
            {
                if (blockLocations[key][i].z == id)
                {
                    blockLocations[key][i] = encodeBlockPos(id, newPos);
                    return;
                }
            }
            blockLocations[key].Add(encodeBlockPos(id, newPos));
        }
        else
        {
            List<Vector3Int> lst = new List<Vector3Int>
            {
                encodeBlockPos(id, newPos)
            };
            blockLocations.Add(key, lst);
        }
    }

    public void removeBlock(BlockType type, int id)
    {
        string key = type.fullName();
        if (blockLocations.ContainsKey(key))
        {
            for (int i = 0; i < blockLocations[key].Count; ++i)
            {
                if (blockLocations[key][i].z == id)
                {
                    AudioManager.Instance.PlaySFX("click");
                    blockLocations[key][i] = new Vector3Int(0,0,id);
                    return;
                }
            }
        }
    }
}