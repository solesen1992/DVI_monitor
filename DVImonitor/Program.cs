using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace DVImonitor
{
    internal class Program
    {
        //DVI servicen:  http://dvimonitor.azurewebsites.net/monitor.asmx
        private static DVIService.monitorSoapClient ds = new DVIService.monitorSoapClient();

        //Tidszoner
        private static TimeZoneInfo Romance_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time"); //København
        private static TimeZoneInfo GMT_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"); //London
        private static TimeZoneInfo Singapore_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"); //Singapore


        static void Streg()
        {
            //Stregen i midten af consollen
            for (int i = 0; i < 28; i++)
            {
                Console.SetCursorPosition(60, i);
                Console.WriteLine("|");
            }
        }

        static void LagerStatus()
        {
            //Lagerstatus
            Console.SetCursorPosition(0, 1);
            WriteInBlue("Lagerstatus");
            Console.WriteLine();

            //Varer under minimum
            WriteInBlue("Varer under minimum:");
            WriteInBlue("------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string line in ds.StockItemsUnderMin())
                Console.WriteLine(line);
            Console.ResetColor();

            Console.WriteLine();

            //Varer der er for mange af på lageret (over maksimum)
            WriteInBlue("Varer over maksimum:");
            WriteInBlue("------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (string line in ds.StockItemsOverMax())
                Console.WriteLine(line);
            Console.ResetColor();

            Console.WriteLine();

            //Varer der er solgt mest til dags dato
            WriteInBlue("Mest solgte vare:");
            WriteInBlue("------------------------------------------------------------");
            foreach (string line in ds.StockItemsMostSold())
                Console.WriteLine(line);

            Console.WriteLine();
        }

        static void WriteInBlue(string text)
        {
            //Blå tekstfarve
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Temperatur()
        {
            //Lager og udendørs temperatur og fugtighed
            Console.SetCursorPosition(70, 1);
            WriteInBlue("Temperatur og fugtighed");

            //Lager
            Console.SetCursorPosition(70, 3);
            WriteInBlue("Lager:");

            //Lagerets temperatur
            Console.SetCursorPosition(70, 4);
            Console.WriteLine(ds.StockTemp().ToString("N2") + "°C     ");

            //Lagerets fugtighed
            Console.SetCursorPosition(70, 5);
            Console.WriteLine(ds.StockHumidity().ToString("N2") + "%");

            //Udendørs
            Console.SetCursorPosition(70, 7);
            WriteInBlue("Udenfor:");

            //Udendørs temperatur
            Console.SetCursorPosition(70, 8);
            Console.WriteLine(ds.OutdoorTemp().ToString("N2") + "°C     ");

            //Udendørs fugtighed
            Console.SetCursorPosition(70, 9);
            Console.WriteLine(ds.OutdoorHumidity().ToString("N2") + "%");
        }


        static void Time()
        {
            //Tid og dato
            DateTime dateTime_Romance = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Romance_Standard_Time); // København
            DateTime dateTime_GMT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, GMT_Standard_Time); // London
            DateTime dateTime_Singapore = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Singapore_Standard_Time); // Singapore

            //Sprog sættes til dansk
            var culture = new System.Globalization.CultureInfo("da-DK");

            //Tid og dato for København, London og Singapore
            Console.SetCursorPosition(15, 19);
            WriteInBlue("Dato / tid");
            Console.WriteLine();
            WriteInBlue("København:");
            Console.WriteLine($"{culture.DateTimeFormat.GetDayName(dateTime_Romance.DayOfWeek).ToString().ToUpper()} {dateTime_Romance}");
            WriteInBlue("London:");
            Console.WriteLine($"{culture.DateTimeFormat.GetDayName(dateTime_GMT.DayOfWeek).ToString().ToUpper()} {dateTime_GMT}");
            WriteInBlue("Singapore:");
            Console.WriteLine($"{culture.DateTimeFormat.GetDayName(dateTime_Singapore.DayOfWeek).ToString().ToUpper()} {dateTime_Singapore}");
        }

        static void NewsUpdates()
        {
            //Nyheder nederst i consollen
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("https://nordjyske.dk/rss/nyheder");

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");

            XmlNode itemNode = itemNodes[0];
            string title = itemNode.SelectSingleNode("title").InnerText;

            Console.SetCursorPosition(0, 27);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}", title);
            Console.ForegroundColor = ConsoleColor.White;
        }



        static void Main(string[] args)
        {
            //Indhold der vises i consollen
            Console.CursorVisible = false;

            Stopwatch time1Sek = Stopwatch.StartNew();
            Stopwatch time5Min = Stopwatch.StartNew();

            Streg();
            LagerStatus();
            WriteInBlue("------------------------------------------------------------");
            Temperatur();
            NewsUpdates();

            while (true)
            {
                //Opdaterer Lagerstatus og Temperatur hvert 5 minut
                if (time5Min.ElapsedMilliseconds >= 300000)
                {
                    time5Min.Restart();
                    LagerStatus();
                    Temperatur();
                    NewsUpdates();
                }

                //Opdaterer tid og dato hvert sekund
                if (time1Sek.ElapsedMilliseconds >= 1000)
                {
                    time1Sek.Restart();
                    Time();
                }
            }
        }
    }
}