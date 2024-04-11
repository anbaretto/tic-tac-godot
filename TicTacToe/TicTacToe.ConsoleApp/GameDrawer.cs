using System.Text;

namespace TicTacToe.ConsoleApp;

public class GameDrawer
{
    private readonly StringBuilder _stringBuilder = new();

    public string GenerateMatchView(Match match)
    {
        _stringBuilder.Clear();

        var tl = GetLocationView(match, Location.TopLeft);
        var tc = GetLocationView(match, Location.TopCenter);
        var tr = GetLocationView(match, Location.TopRight);
        var ml = GetLocationView(match, Location.MiddleLeft);
        var mc = GetLocationView(match, Location.MiddleCenter);
        var mr = GetLocationView(match, Location.MiddleRight);
        var bl = GetLocationView(match, Location.BottomLeft);
        var bc = GetLocationView(match, Location.BottomCenter);
        var br = GetLocationView(match, Location.BottomRight);
        
        _stringBuilder.AppendLine($"  {tl}  |  {tc}  |  {tr}  ");
        _stringBuilder.AppendLine("-----|-----|-----");
        _stringBuilder.AppendLine($"  {ml}  |  {mc}  |  {mr}  ");
        _stringBuilder.AppendLine("-----|-----|-----");
        _stringBuilder.AppendLine($"  {bl}  |  {bc}  |  {br}  ");

        return _stringBuilder.ToString();
    }

    private static string GetLocationView(Match match, Location location)
    {
        var player = match.ValueAt(location);
        var isFirstTurn = match.JustBegun();
        return player == Player.None 
            ? GetPositionNumberOrEmpty(location, isFirstTurn)
            : player.ToString();
    }

    private static string GetPositionNumberOrEmpty(Location location, bool isFirstTurn)
    {
        return isFirstTurn ? ((int)location+1).ToString() : " ";
    }
}
