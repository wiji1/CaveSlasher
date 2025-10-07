public class Font
{
    private const string FolderPath = "Fonts/";
    
    public static readonly Font Cartoon = new("cartoon");
    public static readonly Font KiwiSoda = new("KiwiSoda");


        
    private readonly string _path;
        
    private Font(string path)
    {
        _path = path;
    }
        
    public string GetPath()
    {
        return FolderPath + _path;
    }
}