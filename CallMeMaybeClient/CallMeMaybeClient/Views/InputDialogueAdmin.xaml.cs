using System;
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
using CallMeMaybeClient.Services;
using CallMeMaybeClient.Views;
using static CallMeMaybeClient.Services.RoleManager;



namespace CallMeMaybeClient.Views
{
    /// <summary>
    /// Logique d'interaction pour InputDialogueAdmin.xaml
    /// </summary>
    public partial class InputDialogueAdmin : Window
    {
        public InputDialogueAdmin()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string password = "admin";

            if (PasswordBox.Password == password) 
            {
                RoleManager.SetRole(Role.Admin); // Définit le rôle en tant qu'administrateur
                MessageBox.Show("Vous êtes maintenant administrateur.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
