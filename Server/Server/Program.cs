using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            int counter = 0;
            // Console.BackgroundColor = ConsoleColor.DarkRed;
            TcpListener server = null;
            try
            {
                // Визначемо максимальну кільтьсть потоків
                // Нехай буде по 4 на кожен процесор
                int MaxThreadsCount = Environment.ProcessorCount * 4;
                //Console.WriteLine(MaxThreadsCount.ToString());
                // Встановимо максимальну кількість робочих потоків
                ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
                // Встановимо максимальну кількість робочих потоків
                ThreadPool.SetMinThreads(2, 2);


                // Встановлюємо порт для TcpListener = 9595.
                Int32 port = 9595;
                IPAddress localAddr = IPAddress.Parse("192.168.141.1");

                server = new TcpListener(localAddr, port);

                // Запускаемо TcpListener і очікуємо клієнтів.
                server.Start();

                // Приймаємо клієнтів в безкінечному циклі.
                Console.Write("--------------------------------------------------------------------------------\nОчiкування з'єднання... \n--------------------------------------------------------------------------------\n");

                while (true)
                {

                    // При появі клієнта добавляемо в чергу потоків його обробку.
                    ThreadPool.QueueUserWorkItem(ObrabotkaZaprosa, server.AcceptTcpClient());
                    // Виводимо інформацію про підключення.
                    counter++;

                    Console.Write("\nЗ'єднання №" + counter.ToString() + "!" + "\n");
                    Thread.Sleep(5000);
                    Console.Write("--------------------------------------------------------------------------------\nОчiкування з'єднання... \n--------------------------------------------------------------------------------\n");


                }
            }
            catch (SocketException e)
            {
                //У випадку помилки, виводимо, що це за помилка.
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Зупиняємо TcpListener.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        static void ObrabotkaZaprosa(object client_obj)
        {
            // Буфер для прийому даних.

            Byte []bytes= new Byte[256];
            String datar = null;
            String []data= new String[10];

           
           //Thread.Sleep(1000); 
           
            TcpClient client = client_obj as TcpClient;

            datar = null;

            // отримуємо інф. від клієнта
            NetworkStream stream = client.GetStream();

            
            // приймання даних в циклі поки не дійдем до кінця
            stream.Read(bytes, 0, 256);
            
            // перетворення даних в рядок
            datar = System.Text.Encoding.UTF8.GetString(bytes);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nЗ'єднання з клiєнтом успiшне");
            Console.Write("\nвхiдне повiдомлення :"+datar);

            if (datar[0] == '1')
            {
                byte[] msg = zaput1(datar);
                stream.Write(msg, 0, msg.Length);
            }
            else
                if (datar[0] == '2')
                {
                    byte[] msg = zaput2(datar);
                    stream.Write(msg, 0, msg.Length);
                }
                else
                    if (datar[0] == '3')
                    {
                        byte[] msg = zaput3(datar);
                        //відправлення відповіді
                        stream.Write(msg, 0, msg.Length);
                    }
                        else if (datar[0] == '4')
                        {
                            byte[] msg = zaput4(datar);
                            //відправлення відповіді
                            stream.Write(msg, 0, msg.Length);
                        }
                            else if (datar[0] == '5')
                            {
                                byte[] msg = zaput5(datar);
                                //відправлення відповіді
                                stream.Write(msg, 0, msg.Length);
                            }
                        else Console.Write("\nнекоректний запит клієнта \n");



            Console.Write("\nзакриття звязку client\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            // закриття з'єднання
            client.Close();
        }
        static byte[] zaput1(String datar)
        {

            String[] data = new String[10];
            String _from = String.Empty;
            String _to = String.Empty;
            bool eror = false;
            String user_name = String.Empty;
            String user_second_name = String.Empty;
            String user_password = String.Empty; ;
            String user_login = String.Empty;
            String email = String.Empty;
            String phone_number = String.Empty;
            String cod = String.Empty;

            int ind = 0;
            try
            {
                for (int y = 1, k = 0; y < datar.Length; y++)
                {
                    if (datar[y] == '#')
                    {
                        k++;
                        if (k == 2) break;
                    }
                    else
                        if ((datar[y] != '#') && (k == 0))
                            _from += datar[y];
                        else _to += datar[y];

                }
                Console.Write("\nПошук користувача    " + _from + "  -  " + _to + "\n");
                if ((_from == String.Empty) || (_to == String.Empty))
                    eror = true;

                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
                connect.Open();
                Console.Write("вiдкриття бази даних");

                OleDbCommand command = new OleDbCommand();
                command.Connection = connect;

                //command.CommandText = "SELECT departure_time FROM tickets";
                command.CommandText = "SELECT * FROM Users WHERE user_login LIKE '" + _from + "'and user_password LIKE'" + _to + "'";

                OleDbDataReader reader = command.ExecuteReader();
                bool found = false;
                while (reader.Read())
                {
                    found = true;
                    //  Console.Write("\n" + reader["from"].ToString() + " " + reader["to"].ToString() + " " + reader["intermediate_cities"].ToString() + " " + reader["departure_time"].ToString() + " " + reader["time_of_arrival"].ToString() + "\n");
                    user_name = reader["user_name"].ToString() + "#";
                    user_second_name = reader["user_second_name"].ToString() + "#";
                    user_login = reader["user_login"].ToString() + "#";
                    user_password = reader["user_password"].ToString() + "#";
                    phone_number = reader["phone_number"].ToString() + "#";
                    email = reader["email"].ToString() + "#";
                    //формування відповіді

                    data[ind] = user_name + user_second_name + user_login + user_password + phone_number + email ;
                    ind++;

                }

                reader.Close();
                cod = ind.ToString() + "#";

                if (!found)
                {
                    Console.Write("\nКористувача не знайдено");
                    eror = true;
                }
                else Console.Write("\nКористувач знайдений");

                Console.Write("\nЗакриття бази даних\n");

                connect.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            String message = String.Empty;
            byte[] msg;
            if (eror == true)
            {
                msg = System.Text.Encoding.UTF8.GetBytes("7");
                Console.Write("\nНадсилання iнформацiї про помилку \n");
               Console.Write("\n"+msg+"\n");
            }
            else
            {
                Console.Write("\nНадсилання вiдповiдi клiєнту\n");
                message += cod;
                for (int i = 0; i < ind; i++)
                    message += data[i];
                msg = System.Text.Encoding.UTF8.GetBytes(message);
            }
            //відправлення відповіді
            return msg;
        }
        static byte[] zaput2(String datar)
        {

            String[] data = new String[10];
            String _user_login = String.Empty;
            String _to = String.Empty;
            bool eror = false;
            String user_name = String.Empty;
            String user_second_name = String.Empty;
            String user_password = String.Empty; ;
            String user_login = String.Empty;
            String email = String.Empty;
            String phone_number = String.Empty;
            String cod = "1";
            int ind = 0;
            try
            {
                //login + password + name + secondname + phone + email          
                for (int y = 1, k = 0; y < datar.Length; y++)
                {
                    if (datar[y] == '#')
                    {
                        k++;
                        if (k == 6) break;
                    }
                    else
                        if ((datar[y] != '#') && (k == 0))
                            user_login += datar[y];
                        else
                            if ((datar[y] != '#') && (k == 1))
                                user_password += datar[y];
                            else
                               if ((datar[y] != '#') && (k == 2))
                                   user_name += datar[y];
                                else
                                    if ((datar[y] != '#') && (k == 3))
                                        user_second_name += datar[y]; 
                                    else
                                        if ((datar[y] != '#') && (k == 4))
                                            phone_number += datar[y]; 
                                          else
                                              if ((datar[y] != '#') && (k == 5))
                                                  email += datar[y]; 



                }
                Console.Write("\nПошук користувача    " + _user_login +  "\n");
               // if (_user_login == String.Empty) 
                  //  eror = true;

                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
                connect.Open();
                Console.Write("відкриття бази даних");

                OleDbCommand command = new OleDbCommand();
                command.Connection = connect;

                //command.CommandText = "SELECT departure_time FROM tickets";
                command.CommandText = "SELECT * FROM Users WHERE user_login LIKE '" + user_login + "'";

                OleDbDataReader reader = command.ExecuteReader();
                bool found = false;
                while (reader.Read())
                {
                    found = true;                  
                }



                if (!found)
                {
                    reader.Close();
                    cod = ind.ToString() + "#";

                    OleDbCommand myOleDbCommand = connect.CreateCommand();
                    myOleDbCommand.CommandText = "INSERT INTO Users" +
                        "(user_name, user_second_name, phone_number, user_login, user_password, email)" +
                "VALUES('" + user_name + "','" + user_second_name + "','" + phone_number + "','" + user_login + "','" + user_password + "','" + email + "')";
                    Console.Write("\nКористувача зареєстровано");
                    myOleDbCommand.ExecuteNonQuery();
                   
                }
                else
                {
                    Console.Write("\nКористувач уже iснує!!!");
                     eror = true;
                }

                Console.Write("\nЗакриття бази даних\n");

                connect.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            String message = String.Empty;
            byte[] msg;
            if (eror == true)
            {
                msg = System.Text.Encoding.UTF8.GetBytes("9");
                Console.Write("\nНадсилання iнформацiї про помилку \n");
                Console.Write("\n" + msg + "\n");
            }
            else
            {
                Console.Write("\nНадсилання вiдповiдi клiєнту\n");
                msg = System.Text.Encoding.UTF8.GetBytes("1");
            }
            //відправлення відповіді
            return msg;
        }



        static byte[] zaput3(String datar)
        {

            String[] data = new String[10];
            String _from = String.Empty;
            String _to = String.Empty;
            bool eror = false;
            String from = String.Empty;
            String to = String.Empty;
            String ic = String.Empty; ;
            String d_time = String.Empty;
            String time_o_a = String.Empty;
            String free_t = String.Empty;
            String cod = String.Empty;
            String Id_bus = String.Empty;
  
            int ind = 0;
            try
            {
                for (int y = 1, k = 0; y < datar.Length; y++)
                {
                    if (datar[y] == '#')
                    {
                        k++;
                        if (k == 2) break;
                    }
                    else
                        if ((datar[y] != '#') && (k == 0))
                            _from += datar[y];
                        else _to += datar[y];

                }
                Console.Write("\nПошук маршрута    " + _from + "  -  " + _to + "\n");
                if ((_from == String.Empty) || (_to == String.Empty))
                    eror = true;

                OleDbConnection connect = new OleDbConnection();
                connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
                connect.Open();
                Console.Write("вiдкриття бази даних");

                OleDbCommand command = new OleDbCommand();
                command.Connection = connect;

                //command.CommandText = "SELECT departure_time FROM tickets";
                command.CommandText = "SELECT * FROM tickets WHERE from LIKE '" + _from + "'and to LIKE'" + _to + "'";

                OleDbDataReader reader = command.ExecuteReader();
                bool found = false;
                while (reader.Read())
                {
                    found = true;
                    //  Console.Write("\n" + reader["from"].ToString() + " " + reader["to"].ToString() + " " + reader["intermediate_cities"].ToString() + " " + reader["departure_time"].ToString() + " " + reader["time_of_arrival"].ToString() + "\n");
                    from = reader["from"].ToString() + "#";
                    to = reader["to"].ToString() + "#";
                    ic = reader["intermediate_cities"].ToString() + "#";
                    d_time = reader["departure_time"].ToString() + "#";
                    time_o_a = reader["time_of_arrival"].ToString() + "#";
                    free_t = reader["free_t"] + "#";
                    Id_bus = reader["ID_bus"] + "#";
                    //формування відповіді

                    data[ind] = from + to + ic + d_time + time_o_a + free_t + Id_bus;
                    ind++;

                }

                reader.Close();
                cod = ind.ToString() + "#";

                if (!found)
                {
                    Console.Write("\nЗаданого маршруту не знайдено");
                    eror = true;
                }
                else Console.Write("\nМаршрут знайдений");

                Console.Write("\nЗакриття бази даних\n");

                connect.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            String message = String.Empty;
            byte[] msg;
            if (eror == true)
            {
                msg = System.Text.Encoding.UTF8.GetBytes("9");
                Console.Write("\nНадсилання iнформацiї про помилку \n");
            }
            else
            {
                Console.Write("\nНадсилання вiдповiдi клiєнту\n");
                message += cod;
                for (int i = 0; i < ind; i++)
                    message += data[i];
                msg = System.Text.Encoding.UTF8.GetBytes(message);
            }
            //відправлення відповіді
            return msg;
        }
        static byte[] zaput4(String datar)
        {
            String[] data = new String[10];

            bool eror = false;

            String cod = String.Empty;
            String Id_bus = String.Empty;

            for (int i = 1; i < datar.Length; i++)
            {
                Id_bus += datar[i];
            }

            bool found = false;
            int p = 0;
            
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
            connect.Open();
            Console.Write("вiдкриття бази даних");

            OleDbCommand command = new OleDbCommand();
            command.Connection = connect;
           
            command.CommandText = "SELECT * FROM places WHERE ID_bus LIKE " +Id_bus + "";

            OleDbDataReader reader2 = command.ExecuteReader();
            String message = String.Empty;
            while (reader2.Read())
            {                 //обрахунок кількості вільних місць  
                found = true;
                if (reader2["place1"].ToString() == "Yes") p++;
                if (reader2["place2"].ToString() == "Yes") p++;
                if (reader2["place3"].ToString() == "Yes") p++;
                if (reader2["place4"].ToString() == "Yes") p++;
                if (reader2["place5"].ToString() == "Yes") p++;
                if (reader2["place6"].ToString() == "Yes") p++;
                if (reader2["place7"].ToString() == "Yes") p++;
                if (reader2["place8"].ToString() == "Yes") p++;
                if (reader2["place9"].ToString() == "Yes") p++;
                if (reader2["place10"].ToString() == "Yes") p++;
                if (reader2["place11"].ToString() == "Yes") p++;
                if (reader2["place12"].ToString() == "Yes") p++;
                if (reader2["place13"].ToString() == "Yes") p++;
                if (reader2["place14"].ToString() == "Yes") p++;
                if (reader2["place15"].ToString() == "Yes") p++;
                if (reader2["place16"].ToString() == "Yes") p++;
                if (reader2["place17"].ToString() == "Yes") p++;
                if (reader2["place18"].ToString() == "Yes") p++;
                if (reader2["place19"].ToString() == "Yes") p++;
                if (reader2["place20"].ToString() == "Yes") p++;
                if (reader2["place21"].ToString() == "Yes") p++;
                if (reader2["place22"].ToString() == "Yes") p++;
                if (reader2["place23"].ToString() == "Yes") p++;
                if (reader2["place24"].ToString() == "Yes") p++;
                if (reader2["place25"].ToString() == "Yes") p++;
                if (reader2["place26"].ToString() == "Yes") p++;
                if (reader2["place27"].ToString() == "Yes") p++;
                if (reader2["place28"].ToString() == "Yes") p++;
                if (reader2["place29"].ToString() == "Yes") p++;
                if (reader2["place30"].ToString() == "Yes") p++;

                message = (p).ToString() + "#" + reader2["place1"].ToString() + "#" + reader2["place2"].ToString() + "#" +
                reader2["place3"].ToString() + "#" + reader2["place4"].ToString() + "#" + reader2["place5"].ToString() + "#" +
                reader2["place6"].ToString() + "#" + reader2["place7"].ToString() + "#" + reader2["place8"].ToString() + "#" +
                reader2["place9"].ToString() + "#" + reader2["place10"].ToString() + "#" + reader2["place11"].ToString() + "#" +
                reader2["place12"].ToString() + "#" + reader2["place13"].ToString() + "#" + reader2["place14"].ToString() + "#" +
                reader2["place15"].ToString() + "#" + reader2["place16"].ToString() + "#" + reader2["place17"].ToString() + "#" +
                reader2["place18"].ToString() + "#" + reader2["place19"].ToString() + "#" + reader2["place20"].ToString() + "#" +
                reader2["place21"].ToString() + "#" + reader2["place22"].ToString() + "#" + reader2["place23"].ToString() + "#" +
                reader2["place24"].ToString() + "#" + reader2["place25"].ToString() + "#" + reader2["place26"].ToString() + "#" +
                reader2["place27"].ToString() + "#" + reader2["place28"].ToString() + "#" + reader2["place29"].ToString() + "#" +
                reader2["place30"].ToString() + "#" + reader2["price"].ToString() + "#";
            }
            // message = cod.ToString();
            reader2.Close();
            Console.Write("\nЗакриття бази даних\n");


            if (!found)
            {
                Console.Write("\nНадсилання iнформацiї про помилку клiєнту\n");
                eror = true;
            }


            if (eror)
            {
                Console.Write("\nНадсилання iнформацiї про помилку\n");
                cod = "9";
                message = cod;
            }

            else Console.Write("\nНадсилання вiдповiдi \n");



            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
            return msg;
        }
        static byte[] zaput5(String datar)
        {
            bool eror = false;
 
            String free_t = String.Empty;
            String cod = String.Empty;
            String Id_bus = String.Empty;
            String id_bus = String.Empty;
            String wagon = String.Empty;

            String[] place = new String[54];
            int k = 0;
            for (int y = 1; y < datar.Length - 1; y++)
            {

                if (datar[y] == '#') k++;
                else
                {
                    if ((datar[y] != '#') && (k == 0)) id_bus += datar[y];
                    else
                        if ((datar[y] != '#') && (k == 1)) wagon += datar[y];
                        else if ((datar[y] != '#') && (k == 2)) free_t += datar[y];
                        else if ((datar[y] != '#') && (k == 3)) place[0] += datar[y];
                        else if ((datar[y] != '#') && (k == 4)) place[1] += datar[y];
                        else if ((datar[y] != '#') && (k == 5)) place[2] += datar[y];
                        else if ((datar[y] != '#') && (k == 6)) place[3] += datar[y];
                        else if ((datar[y] != '#') && (k == 7)) place[4] += datar[y];
                        else if ((datar[y] != '#') && (k == 8)) place[5] += datar[y];
                        else if ((datar[y] != '#') && (k == 9)) place[6] += datar[y];
                        else if ((datar[y] != '#') && (k == 10)) place[7] += datar[y];
                        else if ((datar[y] != '#') && (k == 11)) place[8] += datar[y];
                        else if ((datar[y] != '#') && (k == 12)) place[9] += datar[y];
                        else if ((datar[y] != '#') && (k == 13)) place[10] += datar[y];
                        else if ((datar[y] != '#') && (k == 14)) place[11] += datar[y];
                        else if ((datar[y] != '#') && (k == 15)) place[12] += datar[y];
                        else if ((datar[y] != '#') && (k == 16)) place[13] += datar[y];
                        else if ((datar[y] != '#') && (k == 17)) place[14] += datar[y];
                        else if ((datar[y] != '#') && (k == 18)) place[15] += datar[y];
                        else if ((datar[y] != '#') && (k == 19)) place[16] += datar[y];
                        else if ((datar[y] != '#') && (k == 20)) place[17] += datar[y];
                        else if ((datar[y] != '#') && (k == 21)) place[18] += datar[y];
                        else if ((datar[y] != '#') && (k == 22)) place[19] += datar[y];
                        else if ((datar[y] != '#') && (k == 23)) place[20] += datar[y];
                        else if ((datar[y] != '#') && (k == 24)) place[21] += datar[y];
                        else if ((datar[y] != '#') && (k == 25)) place[22] += datar[y];
                        else if ((datar[y] != '#') && (k == 26)) place[23] += datar[y];
                        else if ((datar[y] != '#') && (k == 27)) place[24] += datar[y];
                        else if ((datar[y] != '#') && (k == 28)) place[25] += datar[y];
                        else if ((datar[y] != '#') && (k == 29)) place[26] += datar[y];
                        else if ((datar[y] != '#') && (k == 30)) place[27] += datar[y];
                        else if ((datar[y] != '#') && (k == 31)) place[28] += datar[y];
                        else if ((datar[y] != '#') && (k == 32)) place[29] += datar[y];
                        else if ((datar[y] != '#') && (k == 33)) place[30] += datar[y];
                        else if (datar[y] != '$') break;
                }
            }
            // Console.Write(datar);
            OleDbConnection connect = new OleDbConnection();
            connect.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
            connect.Open();
            Console.Write("\nopen data base\n");

            OleDbCommand command = new OleDbCommand();
            command.Connection = connect;
            bool found = false;
            bool erorr1 = false;
            int l = 0;

            command.CommandText = "SELECT * FROM places WHERE ID_bus LIKE " + id_bus + "";
            Console.Write("\n" + id_bus +  "\n");
            OleDbDataReader reader3 = command.ExecuteReader();
            String comand = String.Empty;
            while (reader3.Read())
            {

                found = true;
                for (int y = 0; y < k - 3; y++)
                {
                    String g = "place" + place[y];

                    Console.Write("\n" + reader3[g].ToString() + "\n");

                    if (reader3[g].ToString() == "Yes") l++;

                    else
                    {
                        //erorr1 = true;
                        //comand += "place" + place[y] + "уже зайнятий\n";
                    }
                }

            }
            reader3.Close();
            connect.Close();


            if (!found) eror = true;
            found = false;
            Console.Write(comand);
            if (l == k - 3)
            {

                int pl = Convert.ToInt32(place[k - 4]);
                OleDbConnection conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\tickets.accdb");
                conn.Open();

                OleDbCommand comm = connect.CreateCommand();
                comm.CommandText = "UPDATE places SET place" + pl + "='Volodya' WHERE ID_bus LIKE '" + id_bus + "'";
               // comm.ExecuteNonQuery();

                Console.Write("\nМісця заброньовано");

                conn.Close();

                OleDbConnection connect3 = new OleDbConnection();
                connect3.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\tickets.accdb;Persist Security Info=False;";
                connect3.Open();

                OleDbCommand command3 = new OleDbCommand();
                command3.Connection = connect3;
                found = false;
                int p = 0;
                command3.CommandText = "SELECT * FROM places WHERE ID_bus LIKE '" + id_bus + "'";
                //int id = 0;
                OleDbDataReader reader2 = command3.ExecuteReader();
                String message = String.Empty;
                while (reader2.Read())
                {                 //обрахунок кількості вільних місць  
                    found = true;
                    if (reader2["place1"].ToString() == "Yes") { p++;}
                    if (reader2["place2"].ToString() == "Yes") { p++;}
                    if (reader2["place3"].ToString() == "Yes") { p++;}
                    if (reader2["place4"].ToString() == "Yes") { p++;}
                    if (reader2["place5"].ToString() == "Yes") { p++;}
                    if (reader2["place6"].ToString() == "Yes") { p++;}
                    if (reader2["place7"].ToString() == "Yes") { p++;}
                    if (reader2["place8"].ToString() == "Yes") { p++;}
                    if (reader2["place9"].ToString() == "Yes") { p++;}
                    if (reader2["place10"].ToString() == "Yes") { p++;}
                    if (reader2["place11"].ToString() == "Yes") { p++;}
                    if (reader2["place12"].ToString() == "Yes") { p++;}
                    if (reader2["place13"].ToString() == "Yes") { p++;}
                    if (reader2["place14"].ToString() == "Yes") { p++;}
                    if (reader2["place15"].ToString() == "Yes") { p++;}
                    if (reader2["place16"].ToString() == "Yes") { p++;}
                    if (reader2["place17"].ToString() == "Yes") { p++;}
                    if (reader2["place18"].ToString() == "Yes") { p++;}
                    if (reader2["place19"].ToString() == "Yes") { p++;}
                    if (reader2["place20"].ToString() == "Yes") { p++;}
                    if (reader2["place21"].ToString() == "Yes") { p++;}
                    if (reader2["place22"].ToString() == "Yes") { p++;}
                    if (reader2["place23"].ToString() == "Yes") { p++;}
                    if (reader2["place24"].ToString() == "Yes") { p++;}
                    if (reader2["place25"].ToString() == "Yes") { p++;}
                    if (reader2["place26"].ToString() == "Yes") { p++;}
                    if (reader2["place27"].ToString() == "Yes") { p++;}
                    if (reader2["place28"].ToString() == "Yes") { p++;}
                    if (reader2["place29"].ToString() == "Yes") { p++;}
                    if (reader2["place30"].ToString() == "Yes") { p++;}

                }
                if (!found) eror = true;
                // message = cod.ToString();
                reader2.Close();

                connect3.Close();

                String tickets = p.ToString();
                OleDbConnection conn2 = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\tickets.accdb");
                conn2.Open();

                OleDbCommand commd = connect.CreateCommand();

                commd.CommandText = "UPDATE tickets SET free_t=" + tickets + " WHERE ID_bus LIKE '" + id_bus + "'";
              //  commd.ExecuteNonQuery();
                conn2.Close();

                Console.Write("\nзакриття бази даних\n");

            }
            byte[] msg;
            if (erorr1 == true)
            {
                msg = System.Text.Encoding.UTF8.GetBytes("0");
                Console.Write("\nНеможливо прийняти замовлення , мiсця уже зайнятi \n");
                Console.Write("\nНадсилання iнформацiї про помилку \n");
            }
            else
                if (eror == true)
                {
                    msg = System.Text.Encoding.UTF8.GetBytes("0");
                    Console.Write("\nНадсилання iнформацiї про помилку \n");
                }
                else
                {

                    Console.Write("\nНадсилання вiдповiдi клiєнту\n");
                    msg = System.Text.Encoding.UTF8.GetBytes("1");
                }

            return msg;
        }
    }
}
