public class Texture
{
    private const string FolderPath = "Textures/";
    
    public static readonly Texture HeartFull = new("HealthHeart/heart");
    public static readonly Texture HeartBackground = new("HealthHeart/background");
    public static readonly Texture HeartBorder = new("HealthHeart/border");

        
    private readonly string _path;
        
    private Texture(string path)
    {
        _path = path;
    }
        
    public string GetPath()
    {
        return FolderPath + _path;
    }
}