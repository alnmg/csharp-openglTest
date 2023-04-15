using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openglTest
{
    internal class main
    {
        public static void Main()
        {
            using (Window window = new Window(800, 600, "test"))
            {
                window.Run();
            }
        }
    }
}
