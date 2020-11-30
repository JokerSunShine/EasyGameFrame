public abstract class AbstractManager
{
    #region 属性
    private AbstractManager instance;
    public AbstractManager Instance {
        get {
            if (instance == null)
                instance = this;
            return instance;
        }
    }
    #endregion

    public virtual void Awake() { }

    public virtual void Start() { }

    public virtual void Update() { }

    public virtual void Destroy() { }
}
