namespace Script.Packages.FW_Core.Src
{
    public interface IMonohaviour
    {
        void Awake();
        void Start();
        void Update();
        void LateUpdate();
        void FixedUpdate();
    }
}