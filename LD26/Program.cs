using System;

namespace Spiridios.LD26
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LD26 game = new LD26())
            {
                game.Run();
            }
        }
    }
#endif
}

