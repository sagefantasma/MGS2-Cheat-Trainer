namespace Simplified_Memory_Manager
{
    public class SimplePattern
    {
        public SimplePattern(string pattern) //TODO: is this the best way to input a scanning pattern?
        {
            foreach (char c in pattern)
            {
                switch (c)
                {
                    case '%':
                    case '*':
                        //skip infinitely until next declared byte is found
                        break;
                    case '_':
                    case '?':
                        //skip next byte
                        break;
                    case '[':
                        //byte can be any within the list until ]
                        //! denotes the byte can be any BUT those in the list until ]
                        break;
                    default:

                        break;
                }
            }
        }
    }
}
