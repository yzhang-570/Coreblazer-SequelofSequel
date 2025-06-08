using System.Collections.Generic;
using UnityEngine;

public class BlockHint : MonoBehaviour
{
    public BlockLevelManager levelManager;

    public GameObject blockPrefab;

    public GameObject hintPopup;

    public void initLevel()
    {
        hintPopup.SetActive(false);
        DeleteHints();
    }

    void DeleteHints()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("PuzzleBlockHint");
        foreach(GameObject go in gos)
            Destroy(go);
    }

    public void showBlock(BlockType type, Vector3 position, BlockLevelManager script, Canvas canvas)
    {
        AudioManager.Instance.PlaySFX("wetding");
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, canvas.transform);

        block.GetComponent<Block2>().initBlock(100, type, script, canvas);
        block.GetComponent<Block2>().placeBlockAt(position);

        // shouldn't be draggable
        block.GetComponent<Block2>().setEnabled(false);
    }

    public void hideBlock()
    {
        DeleteHints();
    }

    public void toggleHintPopup()
    {
        hintPopup.SetActive(!hintPopup.activeSelf);
    }
}