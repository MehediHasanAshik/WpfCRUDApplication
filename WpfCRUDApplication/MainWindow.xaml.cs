using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace WpfCRUDApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Employee;Integrated Security=True");

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
      
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        public void clearData()
        {
            name.Clear();
            email.Clear();
            department.Clear();
            salary.Clear();
            search_txt.Clear();
        }

        public void LoadGrid()
        {
            SqlCommand cmd = new SqlCommand("Select * from EmployeeTable", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader sqlReader = cmd.ExecuteReader();
            dt.Load(sqlReader);
            connection.Close();
            dataGrid.ItemsSource = dt.DefaultView;
        }

        public bool isValid()
        {
            if(name.Text == string.Empty)
            {
                MessageBox.Show("Name is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (email.Text == string.Empty)
            {
                MessageBox.Show("Email is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (department.Text == string.Empty)
            {
                MessageBox.Show("Department is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (salary.Text == string.Empty)
            {
                MessageBox.Show("Salary is Required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    SqlCommand command = new SqlCommand("Insert into EmployeeTable Values (@Name, @Email, @Department, @Salary)", connection);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Name", name.Text);
                    command.Parameters.AddWithValue("@Email", email.Text);
                    command.Parameters.AddWithValue("@Department", department.Text);
                    command.Parameters.AddWithValue("@Salary", salary.Text);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    LoadGrid();
                    MessageBox.Show("Successfully Registered", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("delete from EmployeeTable where Id = " + search_txt.Text + " ", connection);

            try
            {
                command.ExecuteNonQuery ();
                MessageBox.Show("Reacord has been deleted", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                connection.Close();
                clearData();
                LoadGrid();

            }catch(SqlException ex)
            {
                MessageBox.Show ("Not Deleted\n" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isValid())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(
                    "update EmployeeTable set Name = '" + name.Text + "', Email = '" + email.Text + "', Department = '" + department.Text + "', Salary = '" + salary.Text + "' where Id = '" + search_txt.Text + "'", connection);

                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show("Record has been updated successfully", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                    clearData();
                    LoadGrid();
                }
            }
        }
    }
}
