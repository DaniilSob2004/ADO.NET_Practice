namespace Store.DAL.Entity
{
    public class User
    {
        public enum RegistrationCheck { MinUserName = 3, MinLogin = 8, MinPassword = 8, MinEmail = 12 };

        public string Name { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;

        public bool CheckUserName()
        {
            return Name.Length >= (int)RegistrationCheck.MinUserName;
        }

        public bool CheckLogin()
        {
            return Login.Length >= (int)RegistrationCheck.MinLogin;
        }

        public bool CheckPassword()
        {
            return Password.Length >= (int)RegistrationCheck.MinPassword;
        }

        public bool CheckEmail()
        {
            return Email.Length >= (int)RegistrationCheck.MinEmail;
        }

        public bool CheckAllData()
        {
            return CheckUserName() && CheckLogin() && CheckPassword() && CheckEmail();
        }

        public bool CheckPasswordByString(string password)
        {
            return Password == password;
        }
    }
}
