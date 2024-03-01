using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;

namespace PS_01_03_2024.Models
{
    public class Violazione
    {
        public int IdViolazione { get; set; }
        [DisplayName("Violazione")]
        public string Descrizione { get; set; }
        public int IdVerbale { get; set; }

        [DisplayName("Contestabile")]
        public bool Contestabile { get; set; }

        public static List<Violazione> GetViolazioni()
        {
            List<Violazione> ListaViolazioni = new List<Violazione>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM TipoViolazione WHERE Contestabile = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Violazione viol = new Violazione();
                    viol.IdViolazione = reader.GetInt32(0);
                    viol.Descrizione = reader.GetString(1);
                    viol.IdVerbale = reader.GetInt32(2);
                    viol.Contestabile = reader.GetBoolean(3);
                    ListaViolazioni.Add(viol);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return ListaViolazioni;
        }
    }
}