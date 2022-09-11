﻿using CmlLib.Core.Auth.Microsoft.UI.Wpf;
using CmlLib.Core.Auth;
using System;
using System.Windows;
using static OCM_Installer_V2.Util;
using System.IO;

namespace OCM_Installer_V2
{
    public partial class Cuenta
    {
        readonly string usernameFile = Globals.AppDirectory + @"\Username.txt";

        public Cuenta()
        {
            InitializeComponent();
            
            if (!File.Exists(usernameFile)) { var f = File.Create(usernameFile); f.Close(); } else CustomUsername.Text = File.ReadAllText(usernameFile);
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MicrosoftLoginWindow loginWindow = new()
                {
                    LoadingText = "Cargando...\nEspera un momento plis :D",
                    Title = "Iniciar sesión con tu cuenta de Microsoft | ¿Para qué? Para abrir el juego usando tu cuenta :)"
                };
                MSession session = await loginWindow.ShowLoginDialog();
                loginWindow.Close();
                ShowMessageBox("Ya tienes la sesión iniciada", "Hola " + session.Username);
            }
            catch (Exception err)
            {
                if (!err.Message.Contains("The user has denied access") && !err.Message.Contains("User cancelled login"))
                {
                    new Reporter().ReportError(err.ToString());
                }
                return;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MicrosoftLoginWindow logoutWindow = new()
                {
                    LoadingText = "Cerrando sesión...\nEspera un momento plis :D",
                    Title = "Cerrar sesión de tu cuenta de Microsoft"
                };
                logoutWindow.ShowLogoutDialog();
                logoutWindow.Close();
            }
            catch (Exception err)
            {
                new Reporter().ReportError(err.ToString());
                return;
            }
        }

        private void CustomUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!File.Exists(usernameFile)) { var f = File.Create(usernameFile); f.Close(); }
            File.WriteAllText(usernameFile, CustomUsername.Text);
        }
    }
}
