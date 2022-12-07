using System.Linq;

public static class MessageParser
{
    public static int FindFirstMarker(string s, int markerLength)
    {
        for (int i = 0; i+markerLength < s.Length; i++)
        {
            if (s.Substring(i, markerLength).Distinct().Count() == markerLength)
            {
                return i + markerLength;
            }
        }

        return -1;
    }
}