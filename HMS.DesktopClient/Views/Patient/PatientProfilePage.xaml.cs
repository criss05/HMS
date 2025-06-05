using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace HMS.DesktopClient.Views.Patient
{
    public sealed partial class PatientProfilePage : Page
    {
        public PatientProfilePage()
        {
            this.InitializeComponent();

            // Set the NameTextBlock to show the current user's name
            if (App.CurrentUser != null)
            {
                NameTextBlock.Text = App.CurrentUser.Name;
            }
            else
            {
                NameTextBlock.Text = "Unknown User";
            }
        }
    }
}
