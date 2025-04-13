using System;
using tpmodul8_103022300114;
using tpmodul8_103022300114;

namespace tpmodul8_103022300114
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Bikin objek config dan coba ambil data dari file JSON-nya
            var config = new CovidConfig();
            config.loadConfig();

            Console.WriteLine("Selamat datang!!");

            // Kasih tahu satuan suhu yang lagi dipakai sekarang
            Console.WriteLine($"Satuan suhu yang digunakan saat ini: {config.SatuanSuhu}");

            // Nanya ke user, mau ubah satuan suhu gak?
            Console.Write("Apakah kamu mau mengubah satuan suhunya? (Y/N) : ");
            string ubahSatuan = Console.ReadLine();
            if (ubahSatuan.ToLower() == "y")
            {
                // Kalau jawab Y, langsung ubah celcius <-> fahrenheit dan simpen ke file
                config.UbahSatuan();
                Console.WriteLine("Berhasil mengubah satuan suhu!");
                Console.WriteLine($"Satuan suhu sekarang menjadi : {config.SatuanSuhu}");
            }

            Console.WriteLine();
            Console.WriteLine("Pertanyaan");

            // Minta input suhu badan user sesuai satuan yang lagi aktif
            Console.Write($"Berapa suhu badan anda saat ini? (dalam nilai {config.SatuanSuhu}): ");
            double suhu = Convert.ToDouble(Console.ReadLine());

            // Cek apakah suhu yang dimasukin masih aman atau udah di luar batas
            if (config.SatuanSuhu == "celcius")
            {
                if (suhu < 36.5 || suhu > 37.5)
                {
                    // Kalau di luar range, langsung tolak
                    Console.WriteLine();
                    Console.WriteLine(config.GetMessage("ditolak"));
                    return;
                }
            }
            else
            {
                if (suhu < 97.7 || suhu > 99.5)
                {
                    // Sama aja, kalau di luar range buat fahrenheit, tolak juga
                    Console.WriteLine();
                    Console.WriteLine(config.GetMessage("ditolak"));
                    return;
                }
            }

            // Kalau suhu aman, sekarang tanya kapan terakhir demam
            Console.Write("Berapa hari yang lalu anda terakhir memiliki gejala demam? : ");
            int hari = Convert.ToInt32(Console.ReadLine());

            // Cek apakah masih dalam batas hari demam yang diizinkan
            if (hari <= config.BatasDemam)
            {
                // Kalau masih terlalu deket (misal < 14 hari), tolak
                Console.WriteLine();
                Console.WriteLine(config.GetMessage("ditolak"));
            }
            else
            {
                // Kalau udah cukup lama dari terakhir demam, silakan masuk
                Console.WriteLine();
                Console.WriteLine(config.GetMessage("diterima"));
            }
        }
    }
}