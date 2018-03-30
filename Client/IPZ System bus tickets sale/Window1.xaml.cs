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
using System.Net.Sockets;

namespace IPZ_System_bus_tickets_sale
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            MainWindow f0 = new MainWindow();
            f0.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (passwordBox1.Password != passwordBox2.Password)
                MessageBox.Show("Пароль не збігається");
            else
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
                {
                    MessageBox.Show("Не заповнені обов'язкові поля");
                    
                }
                else
                {
                    string message = textBox1.Text + "#" + passwordBox1.Password + "#" + textBox2.Text + "#" + textBox3.Text + "#" + textBox5.Text + "#" + textBox4.Text + "#"; //login + password + name + secondname + phone + email   
                    Connect("192.168.141.1", message);

                    this.Close();

                    MainWindow f1 = new MainWindow();
                    f1.Show();
                }
        }

        public void Connect(String server, String message)
        {
            // index = 1  -   надсилання запита для перевірки аунтифікації
            // index = 2  -   надсилання запита для реєстрації користувача
            // index = 3  -   надсилання запита для пошуку маршруту
            // index = 4  -   надсилання запита для оформлення замовлення


            int index = 2;

            ////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////
            //
            //index = 2  -   надсилання запита для реєстрації користувача
            //
            ////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////
            try
            {
                // створюємо TcpClient.
                // Для е TcpListener 
                // настроюємо на IP нашего сервера і той самий порт.
                String responseData = String.Empty;
                Int32 port = 9595;
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(server, port);
                // Переводимо наше повідомлення в ЮТФ8, а потім в масив Byte.
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(index.ToString() + message);
                // Получаем потік для читання і запису даних.
                NetworkStream stream = client.GetStream();
                //відправяємо повідомлення  серверу.
                stream.Write(data, 0, data.Length);
                data = new Byte[512];  // Масив для зберігання отриманих даних.
                // Читаемо перший пакет відповіді сервера.      

                int krt = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, krt);

                String cod = String.Empty;
                cod += responseData[0];
                //Convert.ToInt32(cod) == 9 помилка
                //Convert.ToInt32(cod) == 1 приймання 1 рядка даних

                if (Convert.ToInt32(cod) != 9)
                {
                    MessageBox.Show("Реєстрація завершена успішно:) ");
                    this.Close();
                }
                else
                    MessageBox.Show(" Даний логін уже зайнятий :( ");
                stream.Close();
                client.Close();

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
