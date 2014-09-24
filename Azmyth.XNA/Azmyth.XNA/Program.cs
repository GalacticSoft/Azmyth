using System;

namespace Azmyth.XNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Azmyth game = new Azmyth())
            {
                game.Run();
            }
        }
    }
#endif
}

