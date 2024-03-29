﻿using System;
using System.Collections.Generic;

namespace SyntaxError.V2.Modell.Utility
{
    /// <summary>Class containing extra implementation</summary>
    public static class ExtraImplementation
    {
        private static Random rng = new Random();

        /// <summary>Shuffles the specified list.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        public static void Shuffle<T>(this IList<T> list)  
        {
            #pragma warning disable CA1062 // Validate arguments of public methods
            int n = list.Count;
            #pragma warning restore CA1062 // Validate arguments of public methods
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}
