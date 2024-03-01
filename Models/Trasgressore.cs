using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;

namespace PS_01_03_2024.Models
{
    public class Trasgressore
    {
        // Istanziamo la classe con i propi valori, che combaciano con quelli del nostro DATABASE
        public int IdAnagrafica { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Cognome")]
        public string Cognome { get; set; }

        [DisplayName("Indirizzo")]
        public string Indirizzo { get; set; }

        [DisplayName("Città")]
        public string Citta { get; set; }

        [DisplayName("CAP")]
        public string CAP { get; set; }

        [DisplayName("CF")]
        public string CF { get; set; }

        // Ci connettiamo al DB e lanciamo la prima GET, che ci permetterà di visualizzare tutti i campi della tabella selezionata
        public static List<Trasgressore> VisualizzaTragressori()
        {
            List<Trasgressore> ListaTrasgressori = new List<Trasgressore>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
                string query = "SELECT * FROM Anagrafica";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Trasgressore trag = new Trasgressore();
                    trag.IdAnagrafica = reader.GetInt32(0);
                    trag.Nome = reader.GetString(1);
                    trag.Cognome = reader.GetString(2);
                    trag.Indirizzo = reader.GetString(3);
                    trag.Citta = reader.GetString(4);
                    trag.CAP = reader.GetString(5);
                    trag.CF = reader.GetString(6);
                    ListaTrasgressori.Add(trag);
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
            return ListaTrasgressori;
        }


        // Adesso il metodo per la CREATE, quindi per inserire nuovi dati in tabella. Nuovi trasgressori
        public static void CreaTrasgressore(Trasgressore trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString.ToString();
            // qui ho usato using perchè per qualche motivo avevo problemi di eccezioni
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Anagrafica (Nome, Cognome, Indirizzo, Citta, CAP, CF) VALUES (@Nome, @Cognome, @Indirizzo, @Citta, @CAP, @CF)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                        cmd.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                        cmd.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                        cmd.Parameters.AddWithValue("@Citta", trasgressore.Citta);
                        cmd.Parameters.AddWithValue("@CAP", trasgressore.CAP);
                        cmd.Parameters.AddWithValue("@CF", trasgressore.CF);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}