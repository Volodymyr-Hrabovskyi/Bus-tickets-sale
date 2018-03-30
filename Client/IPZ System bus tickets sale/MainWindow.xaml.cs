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
using System.Net.Sockets;

namespace IPZ_System_bus_tickets_sale
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 f1 = new Window1();
            f1.Show();

            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text != "" || passwordBox1.Password != "")
            {
                string message = textBox1.Text + "#" + passwordBox1.Password + "#"; // message = login + password
                Connect("192.168.141.1", message);
            }
            else
            {
                MessageBox.Show("Поля не заповнені");
            }
        }

        public void Connect(String server, String message)
        {
            // index = 1  -   надсилання запита для перевірки аунтифікації
            int index = 1;
            try
            {
                // створюємо TcpClient.
                // Для е TcpListener 
                // настроюємо на IP нашого сервера і на той самий порт.
                String responseData = String.Empty;
                Int32 port = 9595;
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(server, port);
                // Переводимо наше повідомлення в ЮТФ8, а потім в масив Byte.
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(index.ToString() + message);
                // Получаемо поток для читання і запису даних.
                NetworkStream stream = client.GetStream();
                //відправяємо повідомлення  серверу.
                stream.Write(data, 0, data.Length);
                data = new Byte[512];  // Масив для збереження отриманих даних.
                // Читаемо перший пакет відповіді сервера.      

                int krt = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, krt);

                String cod = String.Empty;
                cod += responseData[0];
                //Convert.ToInt32(cod) == 0 помилка
                //Convert.ToInt32(cod) == 1 приймання 1 рядка даних
                //Convert.ToInt32(cod) == 2...n приймання від 2 до n рядків даних
                if (Convert.ToInt32(cod) != 9)
                {
                    if (Convert.ToInt32(cod) == 1)
                    {
                        String user_name = "";
                        String user_second_name = "";
                        String user_login = "";
                        String user_password = "";
                        String phone_number = "";
                        String email = "";
                        //розбір вхідного пакета даних на окремі змінні 
                        for (int i = 2, k = 0; i < responseData.Length; i++)
                        {
                            if (responseData[i] == '#') k++;
                            else
                            {
                                if ((responseData[i] != '#') && (k == 0)) user_name += responseData[i];
                                if ((responseData[i] != '#') && (k == 1)) user_second_name += responseData[i];
                                if ((responseData[i] != '#') && (k == 2)) user_login += responseData[i];
                                if ((responseData[i] != '#') && (k == 3)) user_password += responseData[i];
                                if ((responseData[i] != '#') && (k == 4)) phone_number += responseData[i];
                                if ((responseData[i] != '#') && (k == 5)) email += responseData[i];
                                if ((responseData[i] != '#') && (k == 6))
                                {
                                    break;
                                }
                            }
                        }

                        try
                        {
                            String s = textBox1.Text.ToString();
                            Window2 f2 = new Window2();
                            f2.Show();

                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Відсутнє з'єднання з інтернетом");
                        }


                    }
                    else
                    {
                        MessageBox.Show("\t Неправильно введено логін чи пароль . ");
                    }
                    stream.Close();
                    client.Close();
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                MessageBox.Show("Немає звязку з сервером");
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                MessageBox.Show("Немає звязку з сервером");
            }
        }
    }
}
