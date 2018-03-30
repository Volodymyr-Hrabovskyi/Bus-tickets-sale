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
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.Sockets;

namespace IPZ_System_bus_tickets_sale
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3(String m, String s)
        {
            InitializeComponent();

             textBox2.Text = m;
             textBox3.Text = s;

             Connect("192.168.141.1", m, 4);

        }

        private void button55_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            Window2 f2 = new Window2();
            f2.Show();
        }

        String prices = String.Empty;
        private void button56_Click(object sender, RoutedEventArgs e)
        {
            bool ticket = false;
            String message = textBox2.Text + '#' + 1 + '#' + textBox3.Text + "#";
            if (button1.Background == Brushes.Red) { message += "1#"; ticket = true; }
            if (button2.Background == Brushes.Red) { message += "2#"; ticket = true; }
            if (button3.Background == Brushes.Red) { message += "3#"; ticket = true; }
            if (button4.Background == Brushes.Red) { message += "4#"; ticket = true; }
            if (button5.Background == Brushes.Red) { message += "5#"; ticket = true; }
            if (button6.Background == Brushes.Red) { message += "6#"; ticket = true; }
            if (button7.Background == Brushes.Red) { message += "7#"; ticket = true; }
            if (button8.Background == Brushes.Red) { message += "8#"; ticket = true; }
            if (button9.Background == Brushes.Red) { message += "9#"; ticket = true; }
            if (button10.Background == Brushes.Red) { message += "10#"; ticket = true; }
            if (button11.Background == Brushes.Red) { message += "11#"; ticket = true; }
            if (button12.Background == Brushes.Red) { message += "12#"; ticket = true; }
            if (button13.Background == Brushes.Red) { message += "13#"; ticket = true; }
            if (button14.Background == Brushes.Red) { message += "14#"; ticket = true; }
            if (button15.Background == Brushes.Red) { message += "15#"; ticket = true; }
            if (button16.Background == Brushes.Red) { message += "16#"; ticket = true; }
            if (button17.Background == Brushes.Red) { message += "17#"; ticket = true; }
            if (button18.Background == Brushes.Red) { message += "18#"; ticket = true; }
            if (button19.Background == Brushes.Red) { message += "19#"; ticket = true; }
            if (button20.Background == Brushes.Red) { message += "20#"; ticket = true; }
            if (button21.Background == Brushes.Red) { message += "21#"; ticket = true; }
            if (button22.Background == Brushes.Red) { message += "22#"; ticket = true; }
            if (button23.Background == Brushes.Red) { message += "23#"; ticket = true; }
            if (button24.Background == Brushes.Red) { message += "24#"; ticket = true; }
            if (button25.Background == Brushes.Red) { message += "25#"; ticket = true; }
            if (button26.Background == Brushes.Red) { message += "26#"; ticket = true; }
            if (button27.Background == Brushes.Red) { message += "27#"; ticket = true; }
            if (button28.Background == Brushes.Red) { message += "28#"; ticket = true; }
            if (button29.Background == Brushes.Red) { message += "29#"; ticket = true; }
            if (button30.Background == Brushes.Red) { message += "30#"; ticket = true; }
            

            if (!ticket)
            {
                MessageBox.Show("Невибране місце");
            }
            else
                Connect("192.168.141.1", message, 5);
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
            //index = 4  -   надсилання запиту для пошуку місць
            //
            ////////////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////////////
            if (index == 4)
            {
                try
                {
                    // створюємо TcpClient.
                    // Для е TcpListener 
                    // настроюємо на IP нашого сервера і той самий порт.
                    String responseData = String.Empty;

                    Int32 port = 9595;
                    TcpClient client = new TcpClient(server, port);

                    // Переводимо наше повідомлення в ЮТФ8, а потім в масив Byte.
                    Byte[] data = System.Text.Encoding.UTF8.GetBytes(index.ToString() + message);

                    // Получаемо потік для читання і запису даних.
                    NetworkStream stream = client.GetStream();

                    //відправяємо повідомлення  серверу. 

                    stream.Write(data, 0, data.Length);

                    // Масив для зберігання отриманих даних.
                    data = new Byte[512];

                    // Читаемо перший пакет відповіді сервера.      
                    String[] place = new String[30];
                    String price = String.Empty;
                    String pls = String.Empty;
                    String wagon_number = String.Empty;
                    stream.Read(data, 0, 512);
                    responseData = System.Text.Encoding.UTF8.GetString(data);

                    if (responseData[0] != '9')
                    {
                        for (int i = 0, k = 0; i < responseData.Length; i++)
                        {
                            if (responseData[i] == '#') k++;
                            else
                                if ((responseData[i] != '#') && (k == 0)) pls += responseData[i];
                            if ((responseData[i] != '#') && (k == 1)) place[0] += responseData[i];
                            if ((responseData[i] != '#') && (k == 2)) place[1] += responseData[i];
                            if ((responseData[i] != '#') && (k == 3)) place[2] += responseData[i];
                            if ((responseData[i] != '#') && (k == 4)) place[3] += responseData[i];
                            if ((responseData[i] != '#') && (k == 5)) place[4] += responseData[i];
                            if ((responseData[i] != '#') && (k == 6)) place[5] += responseData[i];
                            if ((responseData[i] != '#') && (k == 7)) place[6] += responseData[i];
                            if ((responseData[i] != '#') && (k == 8)) place[7] += responseData[i];
                            if ((responseData[i] != '#') && (k == 9)) place[8] += responseData[i];
                            if ((responseData[i] != '#') && (k == 10)) place[9] += responseData[i];
                            if ((responseData[i] != '#') && (k == 11)) place[10] += responseData[i];
                            if ((responseData[i] != '#') && (k == 12)) place[11] += responseData[i];
                            if ((responseData[i] != '#') && (k == 13)) place[12] += responseData[i];
                            if ((responseData[i] != '#') && (k == 14)) place[13] += responseData[i];
                            if ((responseData[i] != '#') && (k == 15)) place[14] += responseData[i];
                            if ((responseData[i] != '#') && (k == 16)) place[15] += responseData[i];
                            if ((responseData[i] != '#') && (k == 17)) place[16] += responseData[i];
                            if ((responseData[i] != '#') && (k == 18)) place[17] += responseData[i];
                            if ((responseData[i] != '#') && (k == 19)) place[18] += responseData[i];
                            if ((responseData[i] != '#') && (k == 20)) place[19] += responseData[i];
                            if ((responseData[i] != '#') && (k == 21)) place[20] += responseData[i];
                            if ((responseData[i] != '#') && (k == 22)) place[21] += responseData[i];
                            if ((responseData[i] != '#') && (k == 23)) place[22] += responseData[i];
                            if ((responseData[i] != '#') && (k == 24)) place[23] += responseData[i];
                            if ((responseData[i] != '#') && (k == 25)) place[24] += responseData[i];
                            if ((responseData[i] != '#') && (k == 26)) place[25] += responseData[i];
                            if ((responseData[i] != '#') && (k == 27)) place[26] += responseData[i];
                            if ((responseData[i] != '#') && (k == 28)) place[27] += responseData[i];
                            if ((responseData[i] != '#') && (k == 29)) place[28] += responseData[i];
                            if ((responseData[i] != '#') && (k == 30)) place[29] += responseData[i];
                            if ((responseData[i] != '#') && (k == 31)) price += responseData[i];
                            if ((responseData[i] != '#') && (k == 32)) break;
                        }
                        prices = price;
                        
                        textBox5.Text = Convert.ToString(0.0);
                        textBox3.Text = pls;

                        if (place[0] == "Yes") button1.Background = Brushes.Green; else button1.Background = Brushes.Gray;
                        if (place[1] == "Yes") button2.Background = Brushes.Green; else button2.Background = Brushes.Gray;
                        if (place[2] == "Yes") button3.Background = Brushes.Green; else button3.Background = Brushes.Gray;
                        if (place[3] == "Yes") button4.Background = Brushes.Green; else button4.Background = Brushes.Gray;
                        if (place[4] == "Yes") button5.Background = Brushes.Green; else button5.Background = Brushes.Gray;
                        if (place[5] == "Yes") button6.Background = Brushes.Green; else button6.Background = Brushes.Gray;
                        if (place[6] == "Yes") button7.Background = Brushes.Green; else button7.Background = Brushes.Gray;
                        if (place[7] == "Yes") button8.Background = Brushes.Green; else button8.Background = Brushes.Gray;
                        if (place[8] == "Yes") button9.Background = Brushes.Green; else button9.Background = Brushes.Gray;
                        if (place[9] == "Yes") button10.Background = Brushes.Green; else button10.Background = Brushes.Gray;
                        if (place[10] == "Yes") button11.Background = Brushes.Green; else button11.Background = Brushes.Gray;
                        if (place[11] == "Yes") button12.Background = Brushes.Green; else button12.Background = Brushes.Gray;
                        if (place[12] == "Yes") button13.Background = Brushes.Green; else button13.Background = Brushes.Gray;
                        if (place[13] == "Yes") button14.Background = Brushes.Green; else button14.Background = Brushes.Gray;
                        if (place[14] == "Yes") button15.Background = Brushes.Green; else button15.Background = Brushes.Gray;
                        if (place[15] == "Yes") button16.Background = Brushes.Green; else button16.Background = Brushes.Gray;
                        if (place[16] == "Yes") button17.Background = Brushes.Green; else button17.Background = Brushes.Gray;
                        if (place[17] == "Yes") button18.Background = Brushes.Green; else button18.Background = Brushes.Gray;
                        if (place[18] == "Yes") button19.Background = Brushes.Green; else button19.Background = Brushes.Gray;
                        if (place[19] == "Yes") button20.Background = Brushes.Green; else button20.Background = Brushes.Gray;
                        if (place[20] == "Yes") button21.Background = Brushes.Green; else button21.Background = Brushes.Gray;
                        if (place[21] == "Yes") button22.Background = Brushes.Green; else button22.Background = Brushes.Gray;
                        if (place[22] == "Yes") button23.Background = Brushes.Green; else button23.Background = Brushes.Gray;
                        if (place[23] == "Yes") button24.Background = Brushes.Green; else button24.Background = Brushes.Gray;
                        if (place[24] == "Yes") button25.Background = Brushes.Green; else button25.Background = Brushes.Gray;
                        if (place[25] == "Yes") button26.Background = Brushes.Green; else button26.Background = Brushes.Gray;
                        if (place[26] == "Yes") button27.Background = Brushes.Green; else button27.Background = Brushes.Gray;
                        if (place[27] == "Yes") button28.Background = Brushes.Green; else button28.Background = Brushes.Gray;
                        if (place[28] == "Yes") button29.Background = Brushes.Green; else button29.Background = Brushes.Gray;
                        if (place[29] == "Yes") button30.Background = Brushes.Green; else button30.Background = Brushes.Gray;
                    }
                    else
                    {
                        MessageBox.Show("\n\tНеможливо звязатися з сервером,\nспробуйте ще раз ");
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
            else
                if (index == 5)
                {
                    try
                    {
                        // створюємо TcpClient.
                        // Для е TcpListener 
                        // настроюємо на IP нашого сервера і той самий порт.
                        String responseData = String.Empty;

                        Int32 port = 9595;
                        TcpClient client = new TcpClient(server, port);

                        // Переводимо наше повідомлення в ЮТФ8, а потім в масив Byte.
                        Byte[] data = System.Text.Encoding.UTF8.GetBytes(index.ToString() + message);

                        // Получаемо потік для читання и запису даних.
                        NetworkStream stream = client.GetStream();

                        //відправяємо повідомлення  серверу. 

                        stream.Write(data, 0, data.Length);

                        data = new Byte[512];

                        // Масив для зберігання отриманих даних.

                        // Читаемо перший пакет відповіді сервера. 
     
                        String[] place = new String[54];
                        String price = String.Empty;
                        String pls = String.Empty;

                        stream.Read(data, 0, 512);
                        responseData = System.Text.Encoding.UTF8.GetString(data);

                        if (responseData[0] != '9')
                        {
                            MessageBox.Show("Квитки заброньовано :)");

                        }
                        else
                        {
                            MessageBox.Show("Не вдалося забронювати квитки :( ");
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

        private void Prices()
        {
            if (radioButton1.IsChecked == true)
            {
                textBox6.Text = prices;
            }
            else if (radioButton2.IsChecked == true)
            {
                textBox6.Text = Convert.ToString(Convert.ToDouble(prices) / 2);
            }
            else
            {
                MessageBox.Show("Виберіть тип квитка");
            }

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button1.Background == Brushes.Red)
            {
                button1.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("1, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button1.Background = Brushes.Red;
                textBox4.Text += 1 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            Prices();
            if (button2.Background == Brushes.Red)
            {
                button2.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("2, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button2.Background = Brushes.Red;
                textBox4.Text += 2 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button3.Background == Brushes.Red)
            {
                button3.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("3, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button3.Background = Brushes.Red;
                textBox4.Text += 3 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button4.Background == Brushes.Red)
            {
                button4.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("4, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button4.Background = Brushes.Red;
                textBox4.Text += 4 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button5.Background == Brushes.Red)
            {
                button5.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("5, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button5.Background = Brushes.Red;
                textBox4.Text += 5 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button6.Background == Brushes.Red)
            {
                button6.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("6, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button6.Background = Brushes.Red;
                textBox4.Text += 6 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button7.Background == Brushes.Red)
            {
                button7.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("7, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button7.Background = Brushes.Red;
                textBox4.Text += 7 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button8.Background == Brushes.Red)
            {
                button8.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("8, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button8.Background = Brushes.Red;
                textBox4.Text += 8 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button9.Background == Brushes.Red)
            {
                button9.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("9,", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button9.Background = Brushes.Red;
                textBox4.Text += 9 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button10.Background == Brushes.Red)
            {
                button10.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("10, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button10.Background = Brushes.Red;
                textBox4.Text += 10 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button11.Background == Brushes.Red)
            {
                button11.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("11, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button11.Background = Brushes.Red;
                textBox4.Text += 11 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button12.Background == Brushes.Red)
            {
                button12.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("12, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button12.Background = Brushes.Red;
                textBox4.Text += 12 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button13.Background == Brushes.Red)
            {
                button13.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("13, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button13.Background = Brushes.Red;
                textBox4.Text += 13 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button14.Background == Brushes.Red)
            {
                button14.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("14, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button14.Background = Brushes.Red;
                textBox4.Text += 14 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button15.Background == Brushes.Red)
            {
                button15.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("15, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button15.Background = Brushes.Red;
                textBox4.Text += 15 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button16_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button16.Background == Brushes.Red)
            {
                button16.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("16, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button16.Background = Brushes.Red;
                textBox4.Text += 16 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button17.Background == Brushes.Red)
            {
                button17.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("17, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button17.Background = Brushes.Red;
                textBox4.Text += 17 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button18.Background == Brushes.Red)
            {
                button18.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("18, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button18.Background = Brushes.Red;
                textBox4.Text += 18 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button19_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button19.Background == Brushes.Red)
            {
                button19.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("19, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button19.Background = Brushes.Red;
                textBox4.Text += 19 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button20.Background == Brushes.Red)
            {
                button20.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("20, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button20.Background = Brushes.Red;
                textBox4.Text += 20 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button21_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button21.Background == Brushes.Red)
            {
                button21.Background =  Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("21, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button21.Background = Brushes.Red;
                textBox4.Text += 21 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button22.Background == Brushes.Red)
            {
                button22.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("22, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button22.Background = Brushes.Red;
                textBox4.Text += 22 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button23_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button23.Background == Brushes.Red)
            {
                button23.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("23, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button23.Background = Brushes.Red;
                textBox4.Text += 23 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button24_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button24.Background == Brushes.Red)
            {
                button24.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("24, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button24.Background = Brushes.Red;
                textBox4.Text += 24 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button25_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button25.Background == Brushes.Red)
            {
                button25.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("25, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button25.Background = Brushes.Red;
                textBox4.Text += 25 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button26_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button26.Background == Brushes.Red)
            {
                button26.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("26, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button26.Background = Brushes.Red;
                textBox4.Text += 26 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button27_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button27.Background == Brushes.Red)
            {
                button27.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("27, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button27.Background = Brushes.Red;
                textBox4.Text += 27 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button28_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button28.Background == Brushes.Red)
            {
                button28.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("28, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button28.Background = Brushes.Red;
                textBox4.Text += 28 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button29_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button29.Background == Brushes.Red)
            {
                button29.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("29, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button29.Background = Brushes.Red;
                textBox4.Text += 29 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

        private void button30_Click(object sender, RoutedEventArgs e)
        {
            Prices();
            if (button30.Background == Brushes.Red)
            {
                button30.Background = Brushes.Green;
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) + 1);
                string k = textBox5.Text.Replace("30, ", "");
                textBox5.Text = k;
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) - Convert.ToDouble(textBox6.Text));
            }
            else
            {
                button30.Background = Brushes.Red;
                textBox4.Text += 30 + ", ";
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - 1);
                textBox5.Text = Convert.ToString(Convert.ToDouble(textBox5.Text) + Convert.ToDouble(textBox6.Text));

            }

        }

    }
}
