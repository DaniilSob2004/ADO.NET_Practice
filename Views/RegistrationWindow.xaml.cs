using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Store.Views
{
    public partial class RegistrationWindow : Window
    {
        private Window logInWindow;
        private bool isClosingLogInWindow;
        public DAL.Entity.User User { get; set; } = new() { Name = "Username", Login = "Login", Password = "Password", Email = "Email" };  // user который регистрируется
        private DAL.DAO.UserDao userDao;

        public RegistrationWindow(Window logInWindow)
        {
            InitializeComponent();
            DataContext = User;
            this.logInWindow = logInWindow;
            isClosingLogInWindow = true;
            userDao = new DAL.DAO.UserDao(App.GetConnection());
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (isClosingLogInWindow) logInWindow.Close();  // если можно закрыть окно, то закрываем
        }


        private void CheckCorrectData(object sender)
        {
            if (sender is TextBox textBox)
            {
                // узнаём какое это поле
                if (textBox.Tag.ToString() == "Username")
                {
                    // если в поле строка по умолчанию или данные неверно введены, то красим периметр красным цветом
                    if (User.Name != "Username" && !User.CheckUserName()) textBox.BorderBrush = Brushes.Red;
                    else textBox.BorderBrush = Brushes.Gray;
                }
                else if (textBox.Tag.ToString() == "Login")
                {
                    if (User.Login != "Login" && !User.CheckLogin()) textBox.BorderBrush = Brushes.Red;
                    else textBox.BorderBrush = Brushes.Gray;
                }
                else if (textBox.Tag.ToString() == "Password")
                {
                    if (User.Password != "Password" && !User.CheckPassword()) textBox.BorderBrush = Brushes.Red;
                    else textBox.BorderBrush = Brushes.Gray;
                }
                else
                {
                    if (User.Email != "Email" && !User.CheckEmail()) textBox.BorderBrush = Brushes.Red;
                    else textBox.BorderBrush = Brushes.Gray;
                }
            }
            else if (sender is PasswordBox passwordBox)  // если это поле для ввода пароля
            {
                if (passwordBox.Password != "Password" && !User.CheckPassword()) passwordBox.BorderBrush = Brushes.Red;
                else passwordBox.BorderBrush = Brushes.Gray;
            }
        }

        private void ShowLogInWindow()
        {
            isClosingLogInWindow = false;
            Close();
            logInWindow.Show();
        }


        private bool CheckUniqueDataInDB()
        {
            string notUniqueFields = "";
            try
            {
                if (!userDao.CheckUniqueByLogin(User.Login)) notUniqueFields += "login, ";
            }
            catch (Exception) { MessageBox.Show("Something is wrong with the database. Try a little later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            try
            {
                if (!userDao.CheckUniqueByPassword(User.Password)) notUniqueFields += "password, ";
            }
            catch (Exception) { MessageBox.Show("Something is wrong with the database. Try a little later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            try
            {
                if (!userDao.CheckUniqueByEmail(User.Email)) notUniqueFields += "email, ";
            }
            catch (Exception) { MessageBox.Show("Something is wrong with the database. Try a little later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

            if (!String.IsNullOrEmpty(notUniqueFields))
            {
                MessageBox.Show($"{notUniqueFields}already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else return true;
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
            CheckCorrectData(sender);

            if (password.Password != textBoxShowPassword.Text)
                GuiBaseManipulation.SetPasswordBox(textBoxShowPassword, password);  // чтобы значения двух полей для пароля совпадали
            if (passwordCheck.Password != textBoxShowPasswordCheck.Text)
                GuiBaseManipulation.SetPasswordBox(textBoxShowPasswordCheck, passwordCheck);  // чтобы значения двух полей для доп.ввода пароля совпадали
        }

        private void ShowPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image)
            {
                if (image.Tag.ToString()!.EndsWith("Check"))
                    GuiBaseManipulation.TextBoxShowPassword(textBoxShowPasswordCheck);
                else
                    GuiBaseManipulation.TextBoxShowPassword(textBoxShowPassword);
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckCorrectData(sender);

            if (password.Password != textBoxShowPassword.Text)
                GuiBaseManipulation.SetTextBoxPassword(textBoxShowPassword, password);  // чтобы значения двух полей для пароля совпадали
            if (passwordCheck.Password != textBoxShowPasswordCheck.Text)
                GuiBaseManipulation.SetTextBoxPassword(textBoxShowPasswordCheck, passwordCheck);  // чтобы значения двух полей для доп.ввода пароля совпадали
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock)
            {
                if (textBlock.Text == "Log In")
                {
                    ShowLogInWindow();  // переключаемся на окно для авторизации
                }
            }
        }

        private void SignUpBtn_Click(object sender, RoutedEventArgs e)
        {
            if (User.CheckAllData())  // данные корректны
            {
                if (CheckUniqueDataInDB())  // данные уникальны
                {
                    if (User.CheckPasswordByString(passwordCheck.Password))  // пароль и пароль подтверждения совпадают
                    {
                        try
                        {
                            userDao.Add(User);  // запись user-а в БД
                            MessageBox.Show($"You have successfully sign up, {User.Name}!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            ShowLogInWindow();  // переключаемся на окно для авторизации
                        }
                        catch (Exception) { MessageBox.Show("Something is wrong with the database. Try a little later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                    }
                    else MessageBox.Show("Passwords are different!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else MessageBox.Show("Not all fields are filled", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
