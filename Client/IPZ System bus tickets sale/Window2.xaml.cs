using IPZ_System_bus_tickets_sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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

namespace IPZ_System_bus_tickets_sale
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }
        public class ListtView
        {
            public string Звідки { get; set; }
            public string  Куди { get; set; }
            public string Проміжні { get; set; }
            public string Відправлення { get; set; }
            public string Прибуття { get; set; }
            public string Вільних { get; set; }
            public string ID { get; set; }

        }
        String id = String.Empty;
        String free = String.Empty;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listView1.Items.Clear();

            if (comboBox1.Text == "" || comboBox2.Text == "")
                MessageBox.Show("Виберіть назви міст з випадаючого списку");
            else
            {
                string message = comboBox1.Text + "#" + comboBox2.Text + "#"; //from  + to   
                Connect("192.168.141.1", message, 3);
            }
        }

        public void Connect(String server, String message, int index)
        {
            // index = 1  -   надсилання запиту для перевірки аунтифікації
            // index = 2  -   надсилання запиту для реєстрації користувача
            // index = 3  -   надсилання запиту для пошуку маршруту
            // index = 4  -   надсилання запиту для пошуку місць
            // index = 5  -   надсилання запиту для оформлення замовлення

            ////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////
            //
            //index = 3  -   надсилання запиту для пошуку маршрута
            //
            ////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////
            if (index == 3)
            {
                try
                {
                    // створюєм TcpClient.
                    // Для е TcpListener 
                    // настроюємо на IP нашего сервера і той самий порт.
                    String responseData = String.Empty;

                    Int32 port = 9595;
                    TcpClient client = new TcpClient(server, port);

                    // Переводимо наше повідомлення в ЮТФ8, а потім в масив Byte.
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(index.ToString() + message);

                    // Получаем потік для чтання і запису даних.
                    NetworkStream stream = client.GetStream();

                    //відправяємо повідомлення  серверу. 

                    stream.Write(data, 0, data.Length);

                    // Масив для зберігання отриманих даних.
                    data = new Byte[512];

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
                        if (Convert.ToInt32(cod) != 1)
                        {
                            String[] from = new String[Convert.ToInt32(cod)];
                            String[] to = new String[Convert.ToInt32(cod)];
                            String[] ic = new String[Convert.ToInt32(cod)];
                            String[] d_time = new String[Convert.ToInt32(cod)];
                            String[] time_o_a = new String[Convert.ToInt32(cod)];
                            String[] free_t = new String[Convert.ToInt32(cod)];
                            String[] Id_bus = new String[Convert.ToInt32(cod)];

                            //розбір вхідного пакета даних на окремі змінні 
                            for (int i = 2, k = 0, y = 0; i < responseData.Length; i++)
                            {
                                if (responseData[i] == '#') k++;
                                else
                                {
                                    if ((responseData[i] != '#') && (k == 0)) from[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 1)) to[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 2)) ic[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 3)) d_time[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 4)) time_o_a[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 5)) free_t[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 6)) Id_bus[y] += responseData[i];
                                    if ((responseData[i] != '#') && (k == 7))
                                    {
                                        if (y == Convert.ToInt32(cod) - 1) break;
                                        else { y++; k = 0; i--; }
                                    }
                                }
                            }

                            // добавлення рядків з даними в listView1  
                            for (int i = 0; i < Convert.ToInt32(cod); i++)
                            {
                                add(from[i], to[i], ic[i], d_time[i], time_o_a[i], free_t[i], Id_bus[i], i);
                            }

                        }

                        else
                        {
                            String from = String.Empty;
                            String to = String.Empty;
                            String ic = String.Empty;
                            String d_time = String.Empty;
                            String time_o_a = String.Empty;
                            String free_t = String.Empty;
                            String Id_bus = String.Empty;

                            //розбір вхідного пакета даних на окремі змінні 
                            for (int i = 2, k = 0; i < responseData.Length - 1; i++)
                            {
                                if (responseData[i] == '#') k++;
                                else
                                    if ((responseData[i] != '#') && (k == 0)) from += responseData[i];
                                if ((responseData[i] != '#') && (k == 1)) to += responseData[i];
                                if ((responseData[i] != '#') && (k == 2)) ic += responseData[i];
                                if ((responseData[i] != '#') && (k == 3)) d_time += responseData[i];
                                if ((responseData[i] != '#') && (k == 4)) time_o_a += responseData[i];
                                if ((responseData[i] != '#') && (k == 5)) free_t += responseData[i];
                                if ((responseData[i] != '#') && (k == 6)) Id_bus += responseData[i];

                            }
                            // добавлення рядка з даними в listView1
                            add(from, to, ic, d_time, time_o_a, free_t, Id_bus, 0);

                        }
                    }
                    else
                    {
                        MessageBox.Show("\tПошук не дав результатів .\n Можливо ви неправильно ввели назви міст,або даного маршруту не існує. ");
                    }
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

        public void add(string from, string to, string ic, string d_time, string time_o_a, string free_t, string ID_bus, int y)
        {
            List<ListtView> item = new List<ListtView>();
            item.Add(new ListtView { Звідки = from, Куди = to, Проміжні = ic, Відправлення = d_time, Прибуття = time_o_a, Вільних = free_t, ID = ID_bus });

            this.listView1.ItemsSource = null;
            this.listView1.ItemsSource = item;
            id = ID_bus;
            free = free_t;
        }

        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Window3 f3 = new Window3(id, free);
            f3.Show();

            this.Close();
        }

    }
}
