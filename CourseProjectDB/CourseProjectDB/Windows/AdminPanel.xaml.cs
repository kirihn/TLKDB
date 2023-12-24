using CourseProjectDB.Windows;
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
using System.Windows.Shapes;

namespace CourseProjectDB
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        MainWindow MainWindow { get; set; }
        public AdminPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e) // back
        {
            this.Close();
            MainWindow.Show();
        }

        private void Button_Click_Employee(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddEmployee addEmployee = new AddEmployee(this);
            addEmployee.Show();

        }

        private void Button_Click_Supplier(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddSupplier addSupplier = new AddSupplier(this);
            addSupplier.Show();
        }

        private void Button_Click_Customer(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AddCustomer addCustomer = new AddCustomer(this);
            addCustomer.Show();
        }

        private void Button_Click_StoreHouse(object sender, RoutedEventArgs e)
        {
            this.Hide();
            StoreHause storeHause = new StoreHause(this);
            storeHause.Show();
        }

        private void Button_Click_DelInfo(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DelFacts delFacts = new DelFacts(this);
            delFacts.Show();
        }
    }
}
