﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CallMeMaybeClient.Models;
using CallMeMaybeClient.ViewsModels;

namespace CallMeMaybeClient.Views
{
    /// <summary>
    /// Logique d'interaction pour AddSalarieWindow.xaml
    /// </summary>
    public partial class AddSalarieWindow : Window
    {

        public AddSalarieWindow()
        {
            InitializeComponent();
            this.DataContext = new AddSalarieViewModel();



        }
        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            // Instancier la nouvelle fenêtre
            var nouvelleFenetre = new AddSalarieWindow();

            // Ouvrir la fenêtre
            nouvelleFenetre.ShowDialog();
            

        }

        private void AnnulerButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AjouterSalarieButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }



    }
}
