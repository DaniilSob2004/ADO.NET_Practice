using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Store.Views
{
    public partial class LogInWindow : Window
    {
        public DAL.Entity.User User { get; set; } = new() { Login = "Login", Password = "Password" };
        private DAL.DAO.UserDao userDao;

        public LogInWindow()
        {
            InitializeComponent();
            userDao = new DAL.DAO.UserDao(App.GetConnection());
            DataContext = User;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.CloseConnectionToDb();  // закрываем подключение с БД
        }


        private void ClearTextBox()
        {
            // сбрасываем значение полей по умолчанию
            login.Text = login.Tag.ToString();
            login.Foreground = Brushes.Gray;
            textBoxShowPassword.Text = textBoxShowPassword.Tag.ToString();
            textBoxShowPassword.Foreground = Brushes.Gray;
            password.Foreground = Brushes.Gray;
            textBoxShowPassword.Visibility = Visibility.Hidden;  // скрывавем показ пароля
        }

        private void ShowMainWindow()
        {
            ClearTextBox();
            Hide();
            new MainWindow().ShowDialog();  // TODO: можно передать User в конструктор MainWindow
            Show();
        }


        private void ShowPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GuiBaseManipulation.TextBoxShowPassword(textBoxShowPassword);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            GuiBaseManipulation.TextBoxGotFocus(sender);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            GuiBaseManipulation.TextBoxLostFocus(sender);
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (password.Password != textBoxShowPassword.Text)
                GuiBaseManipulation.SetPasswordBox(textBoxShowPassword, password);  // чтобы значения двух полей для пароля совпадали
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (password.Password != textBoxShowPassword.Text)
                GuiBaseManipulation.SetTextBoxPassword(textBoxShowPassword, password);  // чтобы значения двух полей для пароля совпадали
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.Text == "Sign Up")
                {
                    ClearTextBox();
                    Hide();
                    new RegistrationWindow(this).ShowDialog();  // переключаемся на окно регистрации
                }
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userDao.CheckUser(User))  // если User есть в БД
                {
                    MessageBox.Show($"Welcome!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMainWindow();
                }
                else MessageBox.Show("Invalid login and/or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception) { MessageBox.Show("Something is wrong with the database. Try a little later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
