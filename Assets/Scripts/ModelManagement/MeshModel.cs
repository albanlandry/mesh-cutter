public class MeshedModel
{
    protected MeshedModel[] children;
    protected float width;
    protected float heigth;
    protected float depth;
    protected int material;

    public MeshedModel()
    {
    }

    public MeshedModel(int childrenCount)
    {
        children = new MeshedModel[childrenCount];
    }

    public MeshedModel(MeshedModel[] children)
    {
        this.children = children;
    }

    public MeshedModel(MeshedModel[] children, int material) : this(children)
    {
        this.material = material;
    }

    public MeshedModel(float width, float heigth, float depth, int material)
    {
        this.width = width;
        this.heigth = heigth;
        this.depth = depth;
        this.material = material;
    }

    public MeshedModel(MeshedModel[] children, float width, float heigth, float depth)
    {
        this.children = children;
        this.width = width;
        this.heigth = heigth;
        this.depth = depth;
    }

    public MeshedModel(MeshedModel[] children, float width, float heigth, float depth, int material) : this(children, width, heigth, depth)
    {
        this.material = material;
    }

    public float Width { get; set; }
    public float Height { get; set; }
    public float Depth { get; set; }
    public int Material { get; set; }
    public MeshedModel[] Children { get; set; }
}
