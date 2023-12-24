using CourseProjectDB.Clases;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
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

namespace CourseProjectDB.Windows
{
    /// <summary>
    /// Логика взаимодействия для SupplierPanel.xaml
    /// </summary>
    public partial class SupplierPanel : Window
    {
        MainWindow mainWindow;

        int day, month, year;
        string monthName;

        int MyId;
        int DateId;
        int product_Id;
        int randomIdStoreHouse;
        int randomIdEmployee;

        public SupplierPanel(MainWindow mainWindow, int myId)
        {
            InitializeComponent();
            MyId = myId;
            this.mainWindow = mainWindow;
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string category = Category.Text;
            string priceS = Price.Text;
            string countS = Count.Text;

            decimal price;
            int count = 0;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(priceS) ||
                string.IsNullOrEmpty(countS))
            {
                // Один из полей пуст, выполните необходимые действия, например, выведите сообщение об ошибке
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Пытаемся преобразовать значения из текстовых полей в соответствующие типы данных
            if (decimal.TryParse(priceS, out price) && int.TryParse(countS, out count))
            {
                Console.WriteLine($"Price: {price}, Count: {count}");
            }
            else
            {
                // Если преобразование не удалось, вы можете выдать сообщение об ошибке или предпринять другие действия
                MessageBox.Show("Ошибка: Введите корректные значения для Price и Count.");
                return;
            }
            if (price <= 0)
            {
                MessageBox.Show("Вы указали неккоретную цену, она не может быть <= 0 или с использованием .");
                return;
            }
            if (count <= 0 || (count % 1 != 0))
            {
                MessageBox.Show("Вы указали неккоретное кол-во товара, оно не может быть ниже 1 или дробным");
                return;
            }



            // заполнение таблиц

            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKSupplier;PASSWORD = SupplierPass;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                GetDate();

                // Создаем команду для вызова процедуры Date
                using (OracleCommand command = new OracleCommand("AddDimTimeSupplier", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Добавляем параметры процедуры
                    command.Parameters.Add("p_Day", OracleDbType.Int32).Value = day;
                    command.Parameters.Add("p_Month", OracleDbType.Int32).Value = month;
                    command.Parameters.Add("p_Year", OracleDbType.Int32).Value = year;
                    command.Parameters.Add("p_Month_Name", OracleDbType.NVarchar2).Value = monthName;

                    // Параметр для получения созданного идентификатора
                    OracleParameter DateIDresultParam = new OracleParameter("p_Result", OracleDbType.Int32);
                    DateIDresultParam.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(DateIDresultParam);

                    // Выполняем процедуру
                    command.ExecuteNonQuery();

                    // Получение ид даты
                    OracleDecimal oracleDecimalValueDateIDresultParam = (OracleDecimal)DateIDresultParam.Value;

                    DateId = (int)oracleDecimalValueDateIDresultParam.Value;

                    Console.WriteLine($"Созданное Date_Id: {DateId}");
                }

                // Добавление продукта

                using (OracleCommand command = new OracleCommand("AddDimProductSupplier", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Добавьте параметры, если необходимо
                    command.Parameters.Add("p_Product_Name", OracleDbType.NVarchar2).Value = name;
                    command.Parameters.Add("p_Product_Category", OracleDbType.NVarchar2).Value = category;
                    command.Parameters.Add("p_Product_Price", OracleDbType.Decimal).Value = price;
                    command.Parameters.Add("p_Product_Count", OracleDbType.Int32).Value = count;

                    OracleParameter product_Id_Param = new OracleParameter("p_Product_Id", OracleDbType.Decimal);
                    product_Id_Param.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(product_Id_Param);

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimalValueproduct_Id = (OracleDecimal)product_Id_Param.Value;

                    product_Id = (int)oracleDecimalValueproduct_Id.Value;

                    Console.WriteLine($"Created Product Id: {product_Id}");
                }

                // случайный склад

                using (OracleCommand command = new OracleCommand("GetRandomStoreHouseId", connection))
                {
                    
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter randomIdParam = new OracleParameter("p_RandomId", OracleDbType.Decimal);
                    randomIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(randomIdParam);

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimalValuerandomIdStoreHouse = (OracleDecimal)randomIdParam.Value;

                    randomIdStoreHouse = (int)oracleDecimalValuerandomIdStoreHouse.Value;

                    Console.WriteLine($"Random StoreHouse Id: {randomIdStoreHouse}");
                }

                // случайный работник 

                using (OracleCommand command = new OracleCommand("GetRandomEmployeeId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter randomIdParam = new OracleParameter("p_RandomId", OracleDbType.Decimal);
                    randomIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(randomIdParam);

                    command.ExecuteNonQuery();

                    OracleDecimal oracleDecimalValuerandomIdEmployeee = (OracleDecimal)randomIdParam.Value;

                    randomIdEmployee = (int)oracleDecimalValuerandomIdEmployeee.Value;

                    Console.WriteLine($"Random Employee Id: {randomIdEmployee}");
                }

                // добавление поставки

                using (OracleCommand command = new OracleCommand("AddArrival", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_Date_Id", OracleDbType.Int32).Value = DateId;
                    command.Parameters.Add("p_Employee_Id", OracleDbType.Int32).Value = randomIdEmployee;
                    command.Parameters.Add("p_StoreHouse_Id", OracleDbType.Int32).Value = randomIdStoreHouse;
                    command.Parameters.Add("p_Product_Id", OracleDbType.Int32).Value = product_Id;
                    command.Parameters.Add("p_Supplier_Id", OracleDbType.Int32).Value = MyId;

                    command.ExecuteNonQuery();

                    Jsoner.InputSupCusUsers(MyId, DateId, product_Id, randomIdStoreHouse, randomIdEmployee);
                    MessageBox.Show($"Поставка успешно доставленна на {randomIdStoreHouse} склад и разгружена {randomIdEmployee} работником!");
                }
            }
        }

        private void GetDate()
        {
            // Получаем текущую дату
            DateTime currentDate = DateTime.Now;

            // Извлекаем значения дня, месяца и года
            day = currentDate.Day;
            month = currentDate.Month;
            year = currentDate.Year;

            // Получаем название месяца
            monthName = currentDate.ToString("MMMM");

            // Выводим полученные значения
            Console.WriteLine($"Day: {day}");
            Console.WriteLine($"Month: {month}");
            Console.WriteLine($"Year: {year}");
            Console.WriteLine($"Month Name: {monthName}");
        }
    }
}
