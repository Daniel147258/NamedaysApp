using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uniza.Namedays;

namespace Uniza.NameDays.ViewerConsoleApp
{

    public record class ViewerCosnoleApp
    {
        private NameDayCalendar calendar = new NameDayCalendar();
        public void App()
        {

            FileInfo file = new FileInfo("C:\\Users\\danie\\source\\repos\\Namedays\\Uniza.Namedays\\namedays-sk.csv");
            calendar.Load(file);
            var names = calendar[DateTime.Now];
            string names2 = "";
            string startString = "KALENDÁR MIEN \nDnes " + DateTime.Now.Day.ToString() + "." + DateTime.Now.Month.ToString()
                        + "." + DateTime.Now.Year.ToString();
            if (file.Exists)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (i == names.Length - 1)
                        {
                            names2 += names[i] + ".";
                        }
                        else
                        {
                            names2 += names[i] + ", ";
                        }
                    }
                    if (k == 0)
                    {
                        if (names.Length == 0)
                        {
                            Console.WriteLine(startString + " nemá nikto meniny");
                        }
                        else if (names.Length == 1)
                        {
                            Console.WriteLine(startString + " Má meniny: " + names2 + "");
                        }
                        else
                        {
                            Console.WriteLine(startString + " Máju meniny: " + names2 + "");
                        }

                        names = calendar[DateTime.Now.AddDays(1)];
                        names2 = "";
                    }
                    else
                    {
                        if (names.Length == 0)
                        {
                            Console.WriteLine("Zajtra nemá nikto meniny\n");
                        }
                        else if (names.Length == 1)
                        {
                            Console.WriteLine("Zajtra má meniny: " + names2 + "\n");
                        }
                        else
                        {
                            Console.WriteLine("Zajtra  máju meniny: " + names2 + "\n");
                        }
                    }
                }

            }
            if (file.Exists)
            {
                bool ukonci = false;
                while (!ukonci)
                {
                    Console.WriteLine("Menu\n" + "1 - načítať kalendár\n" + "2 - zobraziť štatistiku\n" + "3 - vyhľadať mená\n" +
                        "4 - vyhľadať mená podľa dátumu\n" + "5 - zobraziť kalendár mien v mesiaci\n" + "6 | Escape - koniec\n");
                    Console.WriteLine("Vaša voľba: ");
                    string symbol = Console.ReadLine();
                    switch (symbol)
                    {
                        case "1":
                            Console.WriteLine("OTVORENIE\nZadajte cestu k súboru kalendára mien alebo stlačte Enter pre ukončenie.\n" +
                                "Zadajte cestu k CSV súboru:");
                            while (true)
                            {
                                string path = Console.ReadLine();
                                if (path == "")
                                    break;
                                FileInfo f = new FileInfo(path);
                                if (!f.Exists)
                                {
                                    Console.WriteLine("Zadaný súbor " + path + " neexistuje !\n");
                                    Console.WriteLine("Zadajte cestu k CSV súboru:");
                                }
                                else if (file.Extension.Equals(".csv"))
                                {
                                    Console.WriteLine("Zadaný súbor " + path + " nie je typu CSV !\n");
                                    Console.WriteLine("Zadajte cestu k CSV súboru:");
                                }
                                else
                                {
                                    Console.WriteLine("Súbor kalendára bol načítaný.\nPre pokračovanie stlačte Enter.\n");
                                    var key = Console.ReadKey();
                                    if (key.Key == ConsoleKey.Enter)
                                        break;

                                }
                            }
                            break;
                        case "2":
                            Statistic();
                            break;
                        case "3":
                            Search();
                            break;
                        case "4":
                            SearchByDate();
                            break;
                        case "5":
                            ShowActualMonth();
                            break;
                        case "6":
                            ukonci = true;
                            break;

                    }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyINfo = Console.ReadKey(true);
                        if (keyINfo.Key == ConsoleKey.Escape)
                            break;
                    }
                }
            }
            else { Console.WriteLine("Súbor sa nenašiel!!"); }

        }
        private void Statistic()
        {
            // uvod
            string retazec = "Štatistika\nCelkový počet mien v kalendári: " + calendar.NameCount + "\n"
                + "Celkový počet dní obsahujúci mena v kalendári: " + calendar.DayCount + "\n" +
                "Celkový počet mien v jednotlivých mesiacoch:\n";
            // mena v jedntolivych mesiacoch
            for (int i = 0; i < 12; i++)
            {
                DateTime date = new DateTime(2001, 1, 1).AddMonths(i);
                retazec += date.ToString("MMMM", new CultureInfo("sk-SK")) + ": " + calendar.GetNameDays(i + 1).Count() + "\n";
            }
            // Počet mien podľa začiatočných písmen
            retazec += "Počet mien podľa začiatočných písmen:\n";
            for (char c = 'A'; c <= 'Ž'; c++)
            {
                if (char.IsUpper(c))
                {
                    string chh = c.ToString();
                    int count = calendar.GetNameDays(chh).Count();
                    if (count > 0)
                        retazec += chh + ": " + count + "\n";
                }
            }
            // 
            retazec += "Počet mien podla dlžky znakov:\n";
            int[] array = new int[20];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = 0;
            }
            foreach (var item in calendar)
            {
                if (item.Name.Length > 2)
                    array[item.Name.Length]++;
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > 0)
                    retazec += i + ": " + array[i] + "\n";
            }
            retazec += "Pre ukončenie stlačte Enter\n";
            Console.WriteLine(retazec);
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                    break;
            }
        }

        private void Search()
        {
            string retazec = "VYHĽADAVANIE MIEN\nPre Ukončenie stlačte Enter\n";

            Console.WriteLine(retazec);
            while (true)
            {
                int i = 1;
                Console.WriteLine("Zadajte meno (regulárny vyraz):");
                string name = Console.ReadLine();
                if (name != null && name.Length > 0)
                {
                    var list = calendar.GetNameDays(name);
                    if (list.Count() == 0)
                    {
                        Console.WriteLine("Nič sa nenašlo");
                    }
                    else
                    {
                        foreach (var item in list)
                        {
                            Console.WriteLine(i + ". " + item.Name + " (" + item.DayMonth.Day + "." + item.DayMonth.Month + ")");
                            i++;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private void SearchByDate()
        {
            Console.WriteLine("VYHĽADAVANIE MIEN PODLA DÁTUMU\nPre ukončenie stlačte Enter.\n");
            while (true)
            {
                int i = 1;
                Console.WriteLine("Zadajte deň a mesiac :");
                string datum = Console.ReadLine();
                DateTime date;
                if (DateTime.TryParseExact(datum, "d.M", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {

                    var list = calendar[date.Month, date.Day];
                    if (list.Count() == 0) { Console.WriteLine("Nič sa nenašlo\n"); }
                    else
                    {
                        foreach (var item in list)
                        {
                            Console.WriteLine(i + ". " + item);
                            i++;
                        }
                    }
                }
                else if (datum.Length == 0)
                    break;
                else
                {
                    Console.WriteLine("Nepodarilo sa prejsť na dátum. Zadaj znova alebo stlač Enter pre ukončenie");
                    ConsoleKeyInfo keyINfo = Console.ReadKey();
                    if (keyINfo.Key == ConsoleKey.Enter)
                        break;
                }

            }
        }

        private void ShowActualMonth()
        {
            Console.WriteLine("Kalendár menin");
            DateTime date = DateTime.Now;
            string retazec = date.ToString("MMMM yyyy") + ":\n";
            var list = calendar.GetNameDays(date.Month);
            Dictionary<DayMonth, List<string>> zoznam = new Dictionary<DayMonth, List<string>>();
            foreach (var item in list)
            {
                if (zoznam.ContainsKey(item.DayMonth))
                {
                    zoznam[item.DayMonth].Add(item.Name);
                }
                else
                {
                    zoznam.Add(item.DayMonth, new List<string> { item.Name });
                }
            }
            foreach (var day in zoznam.Keys)
            {
                retazec = "";
                var hodnoty = zoznam[day];
                foreach (var i in hodnoty)
                {
                    if (day.ToDateTime().Year == date.Year && day.Month == date.Month)
                    {
                        if (day.ToDateTime().DayOfWeek == DayOfWeek.Saturday || day.ToDateTime().DayOfWeek == DayOfWeek.Sunday)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if (day.Day == date.Day)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        if (retazec.Equals(""))
                            retazec += day.Day + "." + date.Month + " " + day.ToDateTime().ToString("ddd") + " " + i;
                        else
                            retazec += ", " + i;
                    }
                }
                Console.WriteLine(retazec);
                Console.ResetColor();
            }
            {
            }
            Console.WriteLine();
        }
    }
}
