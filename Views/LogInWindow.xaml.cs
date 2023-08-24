using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Data.SqlClient;

namespace Store.Views
{
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.CloseConnectionToDb();
        }


        private int GetUserByLoginPasswordInBD()
        {
            using SqlCommand command = new() { Connection = App.Connection };
            command.CommandText = @"SELECT COUNT(*) FROM [User]
                                    WHERE login = @login AND password = @password";
            command.Parameters.AddWithValue("@login", login.Text);
            command.Parameters.AddWithValue("@password", password.Password);
            try
            {
                return (int)command.ExecuteScalar();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
            return -1;
        }


        private void ClearTextBox()
        {
            login.Text = login.Tag.ToString();
            login.Foreground = Brushes.Gray;
            password.Password = password.Tag.ToString();
            password.Foreground = Brushes.Gray;
            textBoxShowPassword.Text = textBoxShowPassword.Tag.ToString();
            textBoxShowPassword.Foreground = Brushes.Gray;
        }

        private void ShowMainWindow()
        {
            ClearTextBox();
            Hide();
            new MainWindow().ShowDialog();
            Show();
        }


        private void ShowPasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxShowPassword.Visibility == Visibility.Hidden)
            {
                textBoxShowPassword.Text = password.Password;
                textBoxShowPassword.Visibility = Visibility.Visible;
            }
            else
            {
                password.Password = textBoxShowPassword.Text;
                textBoxShowPassword.Visibility = Visibility.Hidden;
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (GetUserByLoginPasswordInBD() == 1)
            {
                MessageBox.Show($"Welcome!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMainWindow();
            }
            else MessageBox.Show("Invalid login and/or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.Text == "Sign Up")
                {
                    Hide();
                    new RegistrationWindow(this).ShowDialog();
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == textBox.Tag.ToString())
                {
                    textBox.Clear();
                    textBox.Foreground = Brushes.Black;
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Password == passwordBox.Tag.ToString())
                {
                    passwordBox.Clear();
                    passwordBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = textBox.Tag.ToString();
                    textBox.BorderBrush = Brushes.Gray;
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (String.IsNullOrEmpty(passwordBox.Password))
                {
                    passwordBox.Foreground = Brushes.Gray;
                    passwordBox.Password = passwordBox.Tag.ToString();
                    passwordBox.BorderBrush = Brushes.Gray;
                }
            }
        }
    }
}
