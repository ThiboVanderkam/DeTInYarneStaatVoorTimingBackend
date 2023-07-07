using System.Text.RegularExpressions;

namespace DeTInYarneStaatVoorTiming
{
    public class Methods
    {
        public bool FilterStringInput(string inputString)
        {
            return inputString.Length <= 300;
        }
    }
}
