﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace JourneeEditteurRecos
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FullForm());
            //Application.Run(new SearchForm());
            Application.Run(new PrescriptionForm());
            //Application.Run(new StarterForm());
        }
    }
}
