using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Data.SqlClient;

namespace Store.Views
{
    public partial class RegistrationWindow : Window
    {
        public enum RegistrationCheck { MinUserName = 3, MinLogin = 8, MinPassword = 8, MinEmail = 12 };
        private Window logInWindow;
        private bool isClosingLogInWindow;

        public RegistrationWindow(Window logInWindow)
        {
            InitializeComponent();
            this.logInWindow = logInWindow;
            isClosingLogInWindow = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (isClosingLogInWindow) logInWindow.Close();
        }


        private void CheckData(object sender)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Tag.ToString() == "Username")
                {
                    if (userName.Text.Length < (int)RegistrationCheck.MinUserName) userName.BorderBrush = Brushes.Red;
                    else userName.BorderBrush = Brushes.Gray;
                }
                else if (textBox.Tag.ToString() == "Login")
                {
                    if (login.Text.Length < (int)RegistrationCheck.MinLogin) login.BorderBrush = Brushes.Red;
                    else login.BorderBrush = Brushes.Gray;
                }
                else if (textBox.Tag.ToString() == "Password")
                {
                    if (textBoxShowPassword.Text.Length < (int)RegistrationCheck.MinPassword) textBoxShowPassword.BorderBrush = Brushes.Red;
                    else textBoxShowPassword.BorderBrush = Brushes.Gray;
                }
                else
                {
                    if (email.Text.Length < (int)RegistrationCheck.MinEmail) email.BorderBrush = Brushes.Red;
                    else email.BorderBrush = Brushes.Gray;
                }
            }
            else if (sender is PasswordBox)
            {
                if (password.Password.Length < (int)RegistrationCheck.MinPassword) password.BorderBrush = Brushes.Red;
                else password.BorderBrush = Brushes.Gray;
            }
        }

        private bool CheckSendData()
        {
            return userName.Text.Length >= (int)RegistrationCheck.MinUserName &&
                   login.Text.Length >= (int)RegistrationCheck.MinLogin &&
                   password.Password.Length >= (int)RegistrationCheck.MinPassword &&
                   email.Text.Length >= (int)RegistrationCheck.MinEmail;
        }

        private void ShowLogInWindow()
        {
            isClosingLogInWindow = false;
            Close();
            logInWindow.Show();
        }


        private void InsertUserInDB()
        {
            using SqlCommand command = new() { Connection = App.Connection };
            command.CommandText = @"INSERT INTO [User](name, login, password, email)
                                    VALUES (@Name, @Login, @Password, @Email)";
            command.Parameters.AddWithValue("@Name", userName.Text);
            command.Parameters.AddWithValue("@Login", login.Text);
            command.Parameters.AddWithValue("@Password", password.Password);
            command.Parameters.AddWithValue("@Email", email.Text);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private bool CheckUniqueDataInDB()
        {
            string str = "";
            using SqlCommand command = new() { Connection = App.Connection };

            command.CommandText = @"SELECT Count(*) FROM [User] WHERE login = @login";
            command.Parameters.AddWithValue("@login", login.Text);
            try
            {
                int count = (int)command.ExecuteScalar();
                if (count == 1) str += "login, ";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            command.CommandText = @"SELECT Count(*) FROM [User] WHERE password = @password";
            command.Parameters.AddWithValue("@password", password.Password);
            try
            {
                int count = (int)command.ExecuteScalar();
                if (count == 1) str += "password, ";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            command.CommandText = @"SELECT Count(*) FROM [User] WHERE email = @email";
            command.Parameters.AddWithValue("@email", email.Text);
            try
            {
                int count = (int)command.ExecuteScalar();
                if (count == 1) str += "email, ";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            if (!string.IsNullOrEmpty(str))
            {
                MessageBox.Show($"{str} already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else return true;
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckData(sender);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckData(sender);
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.Text == "Log In")
                {
                    ShowLogInWindow();
                }
            }
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

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSendData())
            {
                if (CheckUniqueDataInDB())
                {
                    InsertUserInDB();
                    MessageBox.Show($"You have successfully sign up, {userName.Text}!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowLogInWindow();
                }
            }
            else { MessageBox.Show("Not all fields are filled", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
