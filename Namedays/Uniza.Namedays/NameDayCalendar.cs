using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace Uniza.Namedays
{

    public record class NameDayCalendar : IEnumerable<Nameday>
    {

        //predstavuje zoznam menin(NameDay)
        private List<Nameday> Calendar = new List<Nameday>();

        /// <summary>
        /// vrati pocet menin v roku teda pocet vsetkych mien v roku 
        /// </summary>
        /// <returns> Pocet mien v kalendari(zozname) </returns>
        public int NameCount
        {
            get {
                int count = 0;
                foreach (var day in Calendar)
                {
                    if (day != null && !day.Name.Equals("") && !day.Name.Equals("-"))
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        /// <summary>
        /// vráti celkový počet všetkých dní v roku, v ktorých má niekto meniny
        /// </summary>
        /// <returns> Celkový počet všetkých dní v roku, v ktorých má niekto meniny </returns>
        public int DayCount
        {
            get
            {
                int count = 0;
                List<Nameday> list = new List<Nameday>();
                bool isThere = false;
                foreach (var day in Calendar)
                {
                    foreach (var item in list)
                    {
                    
                        if(item.DayMonth == day.DayMonth)
                            isThere = true;
                        else 
                            isThere = false;
                        
                    }
                    if (day.Name != null && day.Name.Length != 0 && !isThere && !day.Name.Equals("-"))
                    {
                        list.Add(day);
                        count++;
                    }
                }
                return count;
            }
        }


        /// <summary>
        /// vráti deň a mesiac oslavy zadaného mena (name), ak bolo meno nájdene.Ak nebolo meno v kalendári mien nájdené, vráti hodnotu null
        /// </summary>
        /// <param name="name"></param>
        /// <returns> deň a mesiac oslavy </returns>
        public DayMonth? this[string name]
        {
            get
            {
                var nameday = Calendar.FirstOrDefault(x => x.Name == name);
                return nameday != null ? nameday.DayMonth : null;
            }
        }


        /// <summary>
        /// viaceré verejné indexery s rôznymi parametrami.
        /// vratia pole reťazcov reprezentujúce mená podľa zadaného dňa a mesiaca v rôznych typoch

        /// <param name= "dayMonth"> </param>
        /// <returns> Mena </returns>
        public string[] this[DayMonth dayMonth] => GetNames(dayMonth).ToArray();

        /// <param name="date"> </param>
        /// <returns> Mena </returns>
        public string[] this[DateOnly date] => GetNames(new DayMonth(date.Day, date.Month)).ToArray();

        /// <param name="dateTime"> </param>
        /// <returns> Mena </returns>
        public string[] this[DateTime dateTime] => GetNames(new DayMonth(dateTime.Day, dateTime.Month)).ToArray();
       

        /// <param name="month"> Prvy parameter </param
        /// <param name="day"> Druhy parameter </param>
        /// <returns> Mena </returns>
        public string[] this[int month, int day] => GetNames(new DayMonth(day, month)).ToArray();

        public Nameday[] this[int month] => GetNameDays(month).ToArray();

        
        /// <summary>
        /// metóda vráti všetky meniny v kalendári(zozname)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Nameday> GetNameDays()
        {
            return Calendar;
        }

        /// <summary>
        /// metóda vráti všetky meniny v zadanom mesiaci
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public IEnumerable<Nameday> GetNameDays(int month)
        {
            return Calendar.Where(x => x.DayMonth.Month == month).Where(x => x.Name.Length >= 0);
        }

        /// <summary>
        ///metóda vráti všetky meniny, ktoré zodpovedajú zadanému reťazcu regulárneho výrazu (pattern), 
        ///ktorý sa bude aplikovať na mená v kalendári
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns>  meniny(NameDay) v kalendári </returns>
        public IEnumerable<Nameday> GetNameDays(string pattern)
        {
            return Calendar.Where(x => x.Name.Contains(pattern));
        }
        
        // pomocna metoda 
        private IEnumerable<string> GetNames(DayMonth dayMonth)
        {
            return Calendar.Where(x => x.DayMonth.Equals(dayMonth) && !x.Name.Equals("") && !x.Name.Equals("-") ).Select(x => x.Name);
        }

        /// <summary>
        /// implicitne implementovaná metóda z generického rozhrania IEnumerable<Nameday>
        /// </summary>
        /// <returns> Objekt implementujúci IEnumerator<Nameday> vracajúci všetky meniny v kalendári. </returns>
        public IEnumerator<Nameday> GetEnumerator()
        {
            return Calendar.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// metóda pridá meniny do kalendára(zoznamu)
        /// </summary>
        /// <param name="nameday"></param>
        public void Add(Nameday nameday)
        {
            if (!Contains(nameday.Name))
                   Calendar.Add(nameday);
            
        }

        /// <summary>
        /// metóda pridá meniny do kalendára(zoznamu)
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="names"></param>
        public void Add(int month, int day, params string[] names)
        {
            foreach (var name in names)
            {
                Add(new Nameday(name, new DayMonth(month, day)));
            }
        }

        /// <summary>
        /// metóda pridá meniny do kalendára
        /// </summary>
        /// <param name="dayMonth"></param>
        /// <param name="names"></param>
        public void Add(DayMonth dayMonth, params string[] names)
        {
            foreach (var name in names)
            {
                Add(new Nameday(name, dayMonth));
            }
        }

        /// <summary>
        /// metóda odstráni meno z kalendára mien. Ak ho nájde a odstráni, vráti hodnotu true. 
        /// Ak ho nenájde, nevyhodí žiadnu výnimku, ale vráti hodnotu false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Remove(string name)
        {
            int pocetVymazanych = Calendar.RemoveAll(x => x.Name.Equals(name));
            if(pocetVymazanych > 0)
                return true; 
            else 
                return false;
        }

        // vrati z zoznamu ci sa tam nachadza dane meno odovzdane parametrom. Pouzivam FirstOrDefault namiesto First
        // aby sa v pripade ze dane meno v zozname neexistuje nevyhodila vynimka ale iba vratil hodnota false.
        /// <summary>
        /// metóda vráti true, ak zadané meno v kalendári existuje. Ak neexistuje, vráti hodnotu false.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            //return zoznam.Contains(zoznam.First(x => x.Name.Equals(name)));
            return Calendar.Contains(Calendar.FirstOrDefault(x => x.Name.Equals(name)));
        }
        

        /// <summary>
        /// vymaze vsetky prvky z zoznamu
        /// </summary>
        public void Clear()
        {
            Calendar.Clear();
        }

        /// <summary>
        /// metóda načíta kalendár mien zo súboru s príponou CSV
        /// </summary>
        /// <param name="file"></param>
        public void Load(FileInfo file)
        {
            if (file.Exists)
            {
                using (var reader = new StreamReader(file.FullName))
                {
                    if (reader.BaseStream != null)
                    {
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            Delimiter = ";"
                        }))
                        {
                            csv.Read();
                            csv.ReadHeader();
                            string actualYear = DateTime.Now.Year.ToString();
                            while (csv.Read())
                            {
                                DateTime date;
                                if (DateTime.TryParseExact(csv.GetField<string>(0) + " " + actualYear, "d. M. yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                    for (int i = 1; i < csv.HeaderRecord.Length; i++)
                                    {
                                        var name = csv.GetField(i);
                                        if (!string.IsNullOrWhiteSpace(name))
                                        {
                                            Nameday nameday = new Nameday(name, new DayMonth(date.Day, date.Month));
                                            Add(nameday);

                                        }
                                        else if (name == "" || name == null)
                                        {
                                            Nameday nameday = new Nameday();
                                            Add(nameday);
                                        }
                                    }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Súbor sa nepodarilo otvoriť!!!");
                    }
                }
            }
            else 
            {
                Console.WriteLine("Súbor sa nepodarilo otvoriť!!!");
            }
        }

        /// <summary>
        /// metóda zapíše kalendár mien do súboru s príponou CSV
        /// </summary>
        /// <param name="file"></param>
        public void Save(FileInfo file)
        {
            using (var writer = new StreamWriter(file.FullName))
            {
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = false
                }))
                {
                    foreach (var item in Calendar)
                    {
                        string day = item.DayMonth.Day.ToString() + ". ";
                        string month = item.DayMonth.Month.ToString() + ".";
                        string name = item.Name.ToString();
                        string date = day + month;
                        csv.WriteField(date);
                        csv.WriteField(name);
                        csv.NextRecord();
                    };
                }
            }
        }
   
        static void Main(string[] args)
        {
           
        }
    }
}
