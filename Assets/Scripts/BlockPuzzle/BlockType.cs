using System.Diagnostics;
using System.Text.RegularExpressions;

public class BlockType
{
    public string name { get; }
    public int width { get; }
    public int height { get; }

    public bool hflipped { set; get; }
    public bool vflipped { set; get; }

    public BlockType(string n, int w, int h)
    {
        name = n;
        width = w;
        height = h;
        hflipped = false;
    }

    public string fullName()
    {
        return name + (hflipped ? "T" : "F") + (vflipped ? "T" : "F");
    }

    public static BlockType stringToBlock(string name)
    {
        switch (name)
        {
            case "bigSquare":
                return new BlockType("bigSquare", 2, 2);
            case "smallSquare":
                return new BlockType("smallSquare", 1, 1);
            case "bigTriangle":
                return new BlockType("bigTriangle", 2, 2);
            case "bigTriangle2":
                return new BlockType("bigTriangle2", 2, 2);
            case "bigTriangle3":
                return new BlockType("bigTriangle3", 2, 2);
            case "bigTriangle4":
                return new BlockType("bigTriangle4", 2, 2);
            case "smallTriangle":
                return new BlockType("smallTriangle", 1, 1);
            case "smallTriangle2":
                return new BlockType("smallTriangle2", 1, 1);
            case "smallTriangle3":
                return new BlockType("smallTriangle3", 1, 1);
            case "smallTriangle4":
                return new BlockType("smallTriangle4", 1, 1);
            // case "bigCircle":
            //     return new BlockType("bigCircle", 2, 2);
            // case "quarterCircle":
            //     return new BlockType("quarterCircle", 1, 1);
            // case "quarterCircle2":
            //     return new BlockType("quarterCircle2", 1, 1);
            // case "quarterCircle3":
            //     return new BlockType("quarterCircle3", 1, 1);
            // case "quarterCircle4":
            //     return new BlockType("quarterCircle4", 1, 1);
            default:
                throw new System.Exception("Wrong block type bruh: " + name);
        }
    }
}