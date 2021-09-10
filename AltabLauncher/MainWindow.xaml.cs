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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace AltabLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Had_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }



        //Initiate auth
        private void ButtonAutorization_Click(object sender, RoutedEventArgs e)
        {
            string inputLogin = LoginAutorization.Text;
            string inputPassword = PasswordBoxLogin.Password;
            string connectionString = @"Data Source=DESKTOP-EHUN6GL\SQLEXPRESS;Initial Catalog=DB_UserInfo;Integrated Security=True";
            string dbLogin;
            string dbPassword;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection  tion = conn;
                    cmd.CommandText = "SELECT * FROM TableMainCredentials";

                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        dbLogin = dr["login"].ToString().Trim();
                        dbPassword = dr["password"].ToString().Trim();


                        if (dbLogin.Equals(inputLogin) && dbPassword.Equals(inputPassword))
                        {
                            MessageAuthSuccessful msg = new MessageAuthSuccessful();
                            msg.ShowDialog();
                            ExpAutorization.Text = "Authorization Successful!";
                            ExpAutorization.Visibility = Visibility.Visible;
                            break;
                        }       
                    }
                }
            }
        }

        //Toggle sign up window from login window
        private void LinkRegistration_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Menu.Visibility = Visibility.Collapsed;
            signUpMenu.Visibility = Visibility.Visible;
        }

        //_____________________________________________________________Registration__________________________________________________________




        //Toggle sign in window from sign up window
        private void Button_SignUpMenu_OpenSignIn_Click(object sender, RoutedEventArgs e)
        {
            signUpMenu.Visibility = Visibility.Collapsed;
            Menu.Visibility = Visibility.Visible;
        }



        //method for testing whether a char is allowed
        private bool testCharacterValidity(char ch)
        {
            bool charIsValid = false;
            char[] validChars = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                                 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                                 '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '!', '@', '#', '$', '%', '^', '&', '*', '_', '-', '+', '=',
                                 '?', '`', '~', ';', ':', '[', ']', '{', '}', '(', ')'};                        //array of valid characters

            foreach (char arrCh in validChars) {
                {
                    if (ch.Equals(arrCh))
                    {
                        charIsValid = true;
                        break;
                    }
                }
            }
            
            return charIsValid;
        }



        //recursive method testing each string character for validity
        private bool testCharValRec(string input)
        {
            if (input.Length <= 0)
                return true;

            return (testCharacterValidity(input[0]) && testCharValRec(input.Substring(1)));
        }



        //Wrapper method for testing character validity of inputted credentials
        private bool testCredentialValidity(string input)
        {
            return testCharValRec(input);
        }



        //Method for testing username availability
        bool testUsernameAvailability(string username)
        {
            string connectionString = @"Data Source=DESKTOP-EHUN6GL\SQLEXPRESS;Initial Catalog=DB_UserInfo;Integrated Security=True";
            bool usernameAvailable = true;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM TableMainCredentials";

                    SqlDataReader dr = cmd.ExecuteReader();
                    string login;

                    while (dr.Read())
                    {
                        login = dr["login"].ToString().Trim();
                        if (username.Equals(login))
                        {
                            usernameAvailable = false;
                            break;
                        }
                    }
                    dr.Close();
                }

            }

            return usernameAvailable;
        }

        bool invalidLogin = true;
        //Test username validity upon clicking off the login input box
        private void TextBox_SignUpUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_SignUpUsername.Text.Length > 18) {      //username too long

                invalidLogin = true;
                TextBlock_UsernameError.Visibility = Visibility.Visible;
                TextBlock_UsernameError.Text = "Username cannot exceed 18 characters";

            } else if (!testCredentialValidity(TextBox_SignUpUsername.Text)) {      //username contains invalid characters

                invalidLogin = true;
                TextBlock_UsernameError.Visibility = Visibility.Visible;
                TextBlock_UsernameError.Text = "Username can only conntain letters a-Z, \n numbers 0-9 and symbols !@#$%^&*()_-+=?`~:;[]{}";

            } else if (!testUsernameAvailability(TextBox_SignUpUsername.Text)) {      //username is taken

                invalidLogin = true;
                TextBlock_UsernameError.Visibility = Visibility.Visible;
                TextBlock_UsernameError.Text = "Username already taken.";

            } else {
                invalidLogin = false;
                TextBlock_UsernameError.Visibility = Visibility.Collapsed;
                //show some indication that everything is alright
            }


        }


        bool invalidPassword = true;
        //test password validity upon clicking off the password input box
        private void PasswordBox_SignUpPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox_SignUpPassword.Password.Length > 24)
            {

                invalidPassword = true;
                TextBlock_PasswordError.Visibility = Visibility.Visible;
                TextBlock_PasswordError.Text = "Password cannot exceed 24 characters.";

            }
            else if (!testCredentialValidity(PasswordBox_SignUpPassword.Password))
            {

                invalidPassword = true;
                TextBlock_PasswordError.Visibility = Visibility.Visible;
                TextBlock_PasswordError.Text = "Password can only conntain letters a-Z, \n numbers 0-9 and symbols !@#$%^&*()_-+=?`~:;[]{}";

            }
            else
            {

                invalidPassword = false;
                TextBlock_PasswordError.Visibility = Visibility.Collapsed;
                //show some indication that everything is alright

            }
        }


        bool mismatchingPassword = true;
        //test password matching
        private void PasswordBox_ConfirmPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!PasswordBox_SignUpPassword.Password.Equals(PasswordBox_ConfirmPassword.Password)) {

                mismatchingPassword = true;
                TextBlock_ConfirmPasswordError.Visibility = Visibility.Visible;
                TextBlock_ConfirmPasswordError.Text = "Passwords do not match.";

            } else {

                mismatchingPassword = false;
                TextBlock_ConfirmPasswordError.Visibility = Visibility.Collapsed;
                //show some indication that everything is alright

            }

        }

        private void Button_SignUp_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-EHUN6GL\SQLEXPRESS;Initial Catalog=DB_UserInfo;Integrated Security=True";
            string inputLogin = TextBox_SignUpUsername.Text;
            string inputPassword = PasswordBox_SignUpPassword.Password;



            if (!invalidLogin && !invalidPassword & !mismatchingPassword && testUsernameAvailability(inputLogin))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO TableMainCredentials (login, password) VALUES ('" + inputLogin + "', '" + inputPassword + "')";

                        SqlDataReader reader = cmd.ExecuteReader();

                        string dblogin, dbpassword;

                        while (reader.Read())
                        {
                            dblogin = reader["login"].ToString();
                            dbpassword = reader["password"].ToString();

                            if (dblogin.Equals(inputLogin) && dbpassword.Equals(inputPassword))
                            {
                                TextBlock_ConfirmPasswordError.Visibility = Visibility.Visible;
                                TextBlock_ConfirmPasswordError.Text = "registration successful!";
                            }
                        }
                    }
                }

                MessageAuthSuccessful msg = new MessageAuthSuccessful();
                msg.MessageAuthSuccessful_TextBlock.Text = "Registration Successful!";
                msg.ShowDialog();

                LoginAutorization.Text = inputLogin;
                PasswordBoxLogin.Password = inputPassword;

                TextBox_SignUpUsername.Clear();
                PasswordBox_SignUpPassword.Clear();
                PasswordBox_ConfirmPassword.Clear();

            }
        }

        private void ButtonClose_Log_Reg_Window_Click(object sender, RoutedEventArgs e)
        {
            ButtonClose_Log_Reg_Window.Visibility = Visibility.Collapsed;
            ButtonLogin.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            ButtonLogin.IsEnabled = false;
            ButtonClose_Log_Reg_Window.Visibility = Visibility.Visible;
        }
        private void ButtonClose_News_Window_Click(object sender, RoutedEventArgs e)
        {
            ButtonClose_News_Window.Visibility = Visibility.Collapsed;
            ButtonNews.IsEnabled = true;
        }
        private void ButtonNews_Click(object sender, RoutedEventArgs e)
        {
            ButtonClose_News_Window.Visibility = Visibility.Visible;
            ButtonNews.IsEnabled = false;
        }
    }
}
