public class MeshModel
{
    protected MeshModel[] children;
    protected float width;
    protected float heigth;
    protected float depth;
    protected int material;

    public MeshModel()
    {
    }

    public MeshModel(int childrenCount)
    {
        children = new MeshModel[childrenCount];
    }

    public MeshModel(MeshModel[] children)
    {
        this.children = children;
    }

    public MeshModel(MeshModel[] children, int material) : this(children)
    {
        this.material = material;
    }

    public MeshModel(float width, float heigth, float depth, int material)
    {
        this.width = width;
        this.heigth = heigth;
        this.depth = depth;
        this.material = material;
    }

    public MeshModel(MeshModel[] children, float width, float heigth, float depth)
    {
        this.children = children;
        this.width = width;
        this.heigth = heigth;
        this.depth = depth;
    }

    public MeshModel(MeshModel[] children, float width, float heigth, float depth, int material) : this(children, width, heigth, depth)
    {
        this.material = material;
    }

    public float Width { get; set; }
    public float Height { get; set; }
    public float Depth { get; set; }
    public int Material { get; set; }
    public MeshModel[] Children { get; set; }
}
