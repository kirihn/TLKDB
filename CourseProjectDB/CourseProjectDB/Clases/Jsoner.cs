using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CourseProjectDB.Clases
{
    public static class Jsoner
    {

        static public void CreateLogAndPassJsonFile(string login, string password)
        {
            var credentials = new
            {
                Login = login,
                Password = password
            };

            string jsonString = System.Text.Json.JsonSerializer.Serialize(credentials);

            try
            {
                // Создаем файл с именем логина и записываем в него JSON-строку
                File.WriteAllText($"../../UsersPassword/{login}.json", jsonString);
                Console.WriteLine($"File {login}.json created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания файла пароля пользователя: {ex.Message}");
            }
        }

        static public void GeneratyEmployeesJsonFile()
        {
            JArray jsonArray = new JArray();

            Random random = new Random();

            for (int i = 1; i <= 100000; i++)
            {
                JObject jsonItem = new JObject
                {
                { "Employee_Name", "Работник-" + i },
                { "Employee_Role", "Комплектовщик " + random.Next(1, 4) + " разряда"},
                { "Employee_Contact", "+37544" + (1000000 + i).ToString() } };

                jsonArray.Add(jsonItem);

            }

            // Сохранение в файл
            using (StreamWriter file = new StreamWriter("../../UsersPassword/EmployeesJson.json", false, Encoding.UTF8))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jsonArray.WriteTo(writer);
                }
            }
        }

        static public void InpuoEmployeesToTable()
        {
            string jsonFilePath = "../../UsersPassword/EmployeesJson.json";

            // Чтение JSON из файла
            string jsonData = File.ReadAllText(jsonFilePath);

            // Замените вашу строку подключения и запрос
            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;";
            string procedureName = "LoadDataFromJsonEmployee";

            // Вызов процедуры Oracle с использованием ODP.NET
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(procedureName, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_JsonData", OracleDbType.Clob).Value = jsonData;

                        command.ExecuteNonQuery();

                        MessageBox.Show("Процедура успешно выполнена.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка выполнения процедуры: " + ex.Message);
                    }
                }
            }
        }

        public static void InputAnalyzToJson(DataTable dataTable)
        {
            if (dataTable != null)
            {
                string jsonString = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
                DateTime time = DateTime.Now;
                try
                {
                    // Создаем файл с именем логина и записываем в него JSON-строку
                    File.WriteAllText($"../../Analyzes/Analyz_{time.Year}-{time.Month}-{time.Day}-{time.Hour}-{time.Minute}-{time.Millisecond}.json", jsonString);
                    MessageBox.Show($"Файл с данными создан, его имя - Analyz_{time.Year}-{time.Month}-{time.Day}-{time.Hour}-{time.Minute}-{time.Millisecond}.json");
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Ошибка создания файла данных анализа");
                }
                return;
            }
            MessageBox.Show($"Ошибка создания файла данных анализа: некорректный запрос");
        }

        public static void InputSupCusUsers(int MyId, int DateId, int product_Id, int randomIdStoreHouse, int randomIdEmployee)
        {
            var credentials = new
            {
                myId = MyId,
                dateId = DateId,
                Product_Id = product_Id,
                RandomIdStoreHouse = randomIdStoreHouse,
                RandomIdEmployee = randomIdEmployee
            };

            string jsonString = System.Text.Json.JsonSerializer.Serialize(credentials);

            try
            {
                DateTime time = DateTime.Now;
                // Создаем файл с именем логина и записываем в него JSON-строку
                File.WriteAllText($"../../SupCus/SupCus_{time.Year}-{time.Month}-{time.Day}-{time.Hour}-{time.Minute}-{time.Millisecond}.json", jsonString);
                MessageBox.Show($"Файл с данными создан, его имя - SupCus_{time.Year}-{time.Month}-{time.Day}-{time.Hour}-{time.Minute}-{time.Millisecond}.json");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
