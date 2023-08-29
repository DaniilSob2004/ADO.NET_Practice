using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Store
{
    // общие операции над интерфейсом, которые дублировались в окнах
    public static class GuiBaseManipulation
    {
        public static void TextBoxGotFocus(object sender)  // при получении фокуса для TextBox и PasswordBox
        {
            string tag;
            if (sender is TextBox textBox)
            {
                tag = textBox.Tag.ToString()!;
                if (textBox.Text == tag || (tag.EndsWith("Check") && textBox.Text == "Password"))  // если тэг и текст textBox-а совпадают
                {
                    textBox.Clear();
                    textBox.Foreground = Brushes.Black;
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                tag = passwordBox.Tag.ToString()!;
                if (passwordBox.Password == tag || (tag.EndsWith("Check") && passwordBox.Password == "Password"))   // если тэг и текст passwordBox-а совпадают
                {
                    passwordBox.Clear();
                    passwordBox.Foreground = Brushes.Black;
                }
            }
        }

        public static void TextBoxLostFocus(object sender)  // при потери фокуса для TextBox и PasswordBox
        {
            if (sender is TextBox textBox)
            {
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.BorderBrush = Brushes.Gray;
                    if (textBox.Tag.ToString()!.EndsWith("Check"))  // если это textBox для ввода пароля и оно для доп. ввода пароля
                        textBox.Text = "Password";
                    else
                        textBox.Text = textBox.Tag.ToString();  // если обычный textBox
                }
            }
            else if (sender is PasswordBox passwordBox)
            {
                if (String.IsNullOrEmpty(passwordBox.Password))
                {
                    passwordBox.Foreground = Brushes.Gray;
                    passwordBox.BorderBrush = Brushes.Gray;
                    if (passwordBox.Tag.ToString()!.EndsWith("Check"))  // если это passwordBox для доп. ввода пароля
                        passwordBox.Password = "Password";
                    else
                        passwordBox.Password = passwordBox.Tag.ToString();  // если обычный passwordBox
                }
            }
        }

        public static void TextBoxShowPassword(TextBox textBoxShowPassword)  // показать/скрыть TextBox
        {
            if (textBoxShowPassword.Visibility == Visibility.Hidden) textBoxShowPassword.Visibility = Visibility.Visible;
            else textBoxShowPassword.Visibility = Visibility.Hidden;
        }

        public static void SetPasswordBox(TextBox textBoxShowPassword, PasswordBox password)
        {
            password.Password = textBoxShowPassword.Text;
        }

        public static void SetTextBoxPassword(TextBox textBoxShowPassword, PasswordBox password)
        {
            textBoxShowPassword.Text = password.Password;
        }
    }
}
