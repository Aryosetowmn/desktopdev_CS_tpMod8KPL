using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace tpmodul8_103022300114
{
    class CovidConfig
    {
        // lokasi file JSON-nya, disimpen di folder project yang lagi jalan
        public static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "covid_config.json");

        // ini buat nyimpen satuan suhu yang dipakai sekarang (celcius / fahrenheit)
        [JsonPropertyName("satuan_suhu")]
        public string SatuanSuhu { get; set; }

        // nyimpen batas maksimal hari demam biar bisa lolos masuk
        [JsonPropertyName("batas_hari_demam")]
        public int BatasDemam { get; set; }

        // pesan kalau user GAGAL lolos
        [JsonPropertyName("pesan_ditolak")]
        public string PesanDitolak { get; set; }

        // pesan kalau user LOLOS
        [JsonPropertyName("pesan_diterima")]
        public string PesanDiterima { get; set; }

        // nge-return pesan sesuai status yang dikirim ("ditolak" / "diterima")
        public string GetMessage(string status)
        {
            return status == "ditolak" ? PesanDitolak : PesanDiterima;
        }

        // constructor custom kalo mau bikin objek CovidConfig manual (jarang dipake di program utama sih)
        public CovidConfig(string satuan_suhu, int batas_hari_demam, string pesan_ditolak, string pesan_diterima)
        {
            this.SatuanSuhu = satuan_suhu;
            this.BatasDemam = batas_hari_demam;
            this.PesanDitolak = pesan_ditolak;
            this.PesanDiterima = pesan_diterima;
        }

        // ngisi nilai default kalo belum ada file config-nya
        private void setDefault()
        {
            this.SatuanSuhu = "celcius";
            this.BatasDemam = 14;
            this.PesanDitolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
            this.PesanDiterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
        }

        // constructor default, langsung manggil setDefault()
        public CovidConfig()
        {
            setDefault();
        }

        // ini buat ganti satuan suhu dari celcius <-> fahrenheit, terus langsung disave juga
        public void UbahSatuan()
        {
            SatuanSuhu = SatuanSuhu == "celcius" ? "fahrenheit" : "celcius";
            SaveNewConfig(); // abis ganti langsung save biar ga ilang
        }

        // method buat load config dari file JSON
        public void loadConfig()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    // baca isi file JSON-nya
                    string config_json = File.ReadAllText(filePath);

                    // ubah dari JSON jadi object
                    CovidConfig configFromFile = JsonSerializer.Deserialize<CovidConfig>(config_json);

                    // copy isi dari file ke objek sekarang
                    SatuanSuhu = configFromFile.SatuanSuhu;
                    BatasDemam = configFromFile.BatasDemam;
                    PesanDitolak = configFromFile.PesanDitolak;
                    PesanDiterima = configFromFile.PesanDiterima;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Terjadi kesalahan: " + ex.Message);
                }
            }
            else
            {
                // kalo file-nya gak ada, bikin file baru pake config default
                SaveNewConfig();
                Console.WriteLine("File tidak ditemukan, membuat file baru");
            }
        }

        // method buat nyimpen config yang sekarang ke file JSON
        public void SaveNewConfig()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true }; // biar JSON-nya rapi
                string jsonString = JsonSerializer.Serialize(this, options); // ubah objek ke JSON
                File.WriteAllText(filePath, jsonString); // tulis ke file
                Console.WriteLine("Berhasil menyimpan konfigurasi baru");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menyimpan konfigurasi baru: " + ex.Message);
            }
        }
    }
}