using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    public BlockLevelManager levelManager;
    public UIInputHandler bossmansBossman;

    public Sprite Bird;
    public Sprite LittleGirl;
    public Sprite ElderlyGardener;
    public Sprite Robot;
    public Sprite Sapling1;
    public Sprite Sapling2;
    public Sprite Storyteller1;
    public Sprite Storyteller2;

    private int lvlCount = 0;
    private string level;

    public void loadLevel(string nm)
    {
        level = nm;
        levelManager.initLevel(getSolnByName(), getImgByName());
    }

    string[] getSolnByName()
    {
        switch (level)
        {
            case "Bird":
                return new string[] {
                    "smallTriangleFF,2,2",
                    "smallTriangleFF,2,3",
                    "smallTriangleFF,2,4",
                    "smallTriangleFF,4,1",
                    "bigTriangle4FF,3,3",
                    "smallTriangle3FF,4,3",
                    "bigSquareFF,3,2",
                    "bigSquareFF,3,3",
                };
            case "Little Girl":
                return new string[] {
                    "smallTriangleFF,2,2",
                    "smallTriangleTF,2,4",
                    "smallSquareFF,3,3",
                    "smallTriangleFF,4,2",
                    "smallSquareFF,4,3",
                    "smallTriangleTF,4,4",
                    "bigSquareFF,5,2",
                    "bigTriangle3FF,4,4",
                    "smallSquareFF,5,2",
                    "smallSquareFF,6,2"
                };
            case "Elderly Gardener":
                return new string[] {
                    "smallSquareFF,1,3",
                    "bigTriangle4FF,2,5",
                    "smallTriangleFF,3,1",
                    "smallSquareFF,1,4",
                    "bigSquareFF,2,2",
                    "bigSquareFF,2,4",
                    "bigSquareFF,4,3",
                    "smallTriangleFF,5,2",
                    "smallTriangle4FF,5,5"
                };
            case "Robot":
                return new string[] {
                    "smallSquareFF,5,1",
                    "smallSquareFF,5,6",
                    "smallTriangle3FF,1,4",
                    "bigSquareFF,2,3",
                    "bigSquareFF,3,2",
                    "bigSquareFF,3,4",
                    "bigSquareFF,5,3",
                    "smallTriangle4FF,6,3",
                    "smallTriangleFF,6,4"
                };
            case "Sapling":
                if (lvlCount == 0)
                {
                    return new string[] {
                        "bigSquareFF,3,2",
                        "bigTriangle4FF,3,4",
                        "bigTriangleFF,4,3",
                        "bigSquareFF,4,3",
                        "bigSquareFF,5,2",
                        "smallSquareFF,5,4"
                    };
                }
                else
                {
                    return new string[] {
                        "bigSquareFF,2,2",
                        "bigSquareFF,2,4",
                        "bigSquareFF,2,3",
                        "bigSquareFF,4,2",
                        "bigSquareFF,4,4",
                        "smallSquareFF,1,4",
                        "smallSquareFF,6,3",
                        "smallTriangleFF,1,3",
                        "bigTriangleFF,3,3",
                        "bigTriangle3FF,2,3",
                        "bigTriangle4FF,4,3"
                    };
                }
            case "Storyteller":
                if (lvlCount == 0)
                {
                    return new string[] {
                        "bigSquareFF,2,2",
                        "bigSquareFF,2,4",
                        "bigSquareFF,4,2",
                        "bigSquareFF,4,4",
                        "bigTriangleFF,3,2",
                        "bigTriangle2FF,5,1",
                        "bigTriangle3FF,4,3",
                        "bigTriangle4FF,2,4",
                        "smallSquareFF,3,4",
                        "smallSquareFF,6,1"
                    };
                }
                else
                {
                    return new string[] {
                        "bigSquareFF,3,2",
                        "bigSquareFF,6,0",
                        "bigSquareFF,5,3",
                        "bigSquareFF,4,3",
                        "smallSquareFF,2,5",
                        "smallTriangleFF,5,3",
                        "bigTriangleFF,4,2",
                        "bigTriangle2FF,3,3",
                        "bigTriangle2FF,5,3",
                        "bigTriangle3FF,5,1",
                        "bigTriangle4FF,5,2"
                    };
                }
            default:
                return new string[] { };
        }
    }

    Sprite getImgByName()
    {
        switch (level)
        {
            case "Bird":
                return Bird;
            case "Little Girl":
                return LittleGirl;
            case "Elderly Gardener":
                return ElderlyGardener;
            case "Robot":
                return Robot;
            case "Sapling":
                return (lvlCount == 0) ? Sapling1 : Sapling2;
            case "Storyteller":
                return (lvlCount == 0) ? Storyteller1 : Storyteller2;
            default:
                return null;
        }
    }

    public void endLevel()
    {
        if (lvlCount == 0 && (level == "Sapling" || level == "Storyteller"))
        {
            ++lvlCount;
            loadLevel(level);
        }
        else
        {
            lvlCount = 0;
            bossmansBossman.EndBlockPuzzle();
        }
    }
}
