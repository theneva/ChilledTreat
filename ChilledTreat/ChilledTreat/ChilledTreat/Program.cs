// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="X&A Team">
//   Copyright (c) X&A Team. All rights reserved
// </copyright>
// <summary>
//   The entry-point of the application
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ChilledTreat
{
    /// <summary>
    /// The entry-point of the application
    /// </summary>
#if WINDOWS || XBOX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Have to be there</param>
        public static void Main(string[] args)
        {
            Game1 game;
            using (game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}