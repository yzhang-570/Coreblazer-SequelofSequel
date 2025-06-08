using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEditor.Tilemaps;

public class Block2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    BlockLevelManager levelManager;
    new Image renderer;

    public int id {get; set;}

    public BlockType blockType {get; set;}

    public bool isOnGrid {get; set;}
    
    Vector3 resetPosition;

    public Sprite bigSquare;
    public Sprite bigTriangle;
    public Sprite bigTriangle2;
    public Sprite bigTriangle3;
    public Sprite bigTriangle4;
    // public Sprite bigCircle;
    // public Sprite quarterCircle;
    // public Sprite quarterCircle2;
    // public Sprite quarterCircle3;
    // public Sprite quarterCircle4;

    bool isEnabled = true;
    bool selected = false;

    private Vector3 screenPoint;
    private Vector3 offset;

    private RectTransform rectTransform;
    private Canvas canvas;

    private bool isDragging = false;

    public void initBlock(int id, BlockType type, BlockLevelManager levelManager, Canvas canvas)
    {
        // GetComponent<FlashingAnim>().SetAnimated(false);
        rectTransform = GetComponent<RectTransform>();
        this.canvas = canvas;

        this.id = id;
        isOnGrid = false;
        blockType = type;
        this.levelManager = levelManager;

        renderer = GetComponent<Image>();
        renderer.sprite = spriteByName(type.name);

        rectTransform.sizeDelta = new Vector2(type.width, type.height) * BlockLevelManager.pixelsPerUnit;

        rectTransform.localScale = new Vector3(
            rectTransform.localScale.x * (type.hflipped ? -1 : 1), rectTransform.localScale.y * (type.vflipped ? -1 : 1),
            rectTransform.localScale.z);

        renderer.alphaHitTestMinimumThreshold = 0.1f; // Only respond to pixels with alpha > 0.1
    }

    public void initResetPosition()
    {
        resetPosition = rectTransform.anchoredPosition3D;
    }

    public Sprite spriteByName(string name)
    {
        switch (name)
        {
            case "bigSquare":
                return bigSquare;
            case "smallSquare":
                return bigSquare;
            case "bigTriangle":
                return bigTriangle;
            case "bigTriangle2":
                return bigTriangle2;
            case "bigTriangle3":
                return bigTriangle3;
            case "bigTriangle4":
                return bigTriangle4;
            case "smallTriangle":
                return bigTriangle;
            case "smallTriangle2":
                return bigTriangle2;
            case "smallTriangle3":
                return bigTriangle3;
            case "smallTriangle4":
                return bigTriangle4;
            // case "bigCircle":
            //     return bigCircle;
            // case "quarterCircle":
            //     return quarterCircle;
            // case "quarterCircle2":
            //     return quarterCircle2;
            // case "quarterCircle3":
            //     return quarterCircle3;
            // case "quarterCircle4":
            //     return quarterCircle4;
            default:
                return bigSquare;
        }
    }

    public void setEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    private bool IsPointerOnVisiblePixel(PointerEventData eventData)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera);
        // Note: alphaHitTestMinimumThreshold is automatically respected by Unity's EventSystem
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isEnabled || !IsPointerOnVisiblePixel(eventData))
        {
            return;
        }

        isDragging = true;

        if (isOnGrid)
        {
            removeFromGrid();
        }

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        transform.SetAsLastSibling(); // bring to front
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isEnabled || !isDragging)
        {
            return;
        }
        // Move the UI element
        rectTransform.anchoredPosition3D += new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEnabled || !isDragging)
        {
            return;
        }

        isDragging = false;

        Vector3 pos = getSpriteTopLeft();

        if (levelManager.checkBlockPosition(pos, blockType))
        {
            Vector3Int snap = levelManager.snapToGrid(pos, blockType) + BlockLevelManager.blockOffset;

            if (isOnGrid)
            {
                levelManager.updateBlock(id, blockType, snap);
            }
            else
            {
                isOnGrid = true;
                levelManager.addBlock(blockType, id, snap);
            }
            placeBlockAt(snap);
        }
        else
        {
            rectTransform.anchoredPosition3D = resetPosition;
        }

        if (levelManager.metRequirements())
        {
            Debug.Log("Correct solution! Yippee!");
            levelManager.endLevel();
        }
    }

    public Vector3 getSpriteTopLeft()
    {
        Vector3 pos = rectTransform.anchoredPosition3D;
        return new Vector3(pos.x - blockType.width * BlockLevelManager.pixelsPerUnit / 2,
                            pos.y - blockType.height * BlockLevelManager.pixelsPerUnit / 2, 0);
    }

    void removeFromGrid() {
        isOnGrid = false;
        levelManager.removeBlock(blockType, id);
    }

    public void placeBlockAt(Vector3 position)
    {
        // AudioSFXManager.Instance.PlayAudio("thump");
        rectTransform.anchoredPosition3D = new Vector3(position.x + blockType.width * BlockLevelManager.pixelsPerUnit / 2,
                                                        position.y - blockType.height * BlockLevelManager.pixelsPerUnit / 2, 0);
    }
}