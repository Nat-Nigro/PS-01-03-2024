using PS_01_03_2024.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace PS_01_03_2024.Controllers
{
    public class OperazioniController : Controller
    {
        // GET: Operazioni
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportiMaggiorni400()
        {
            return View(Verbale.GetImportoMaggiore());
        }

        //Action per decurtamento maggiore di 10 punti
        public ActionResult PiuDi10()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> verbali = new List<Verbale>();
            try
            {
                conn.Open();
                string query = "SELECT v.*, tv.Descrizione, a.Nome, a.Cognome " +
                               "FROM Verbale v " +
                               "INNER JOIN TipoViolazione tv ON v.idViolazione = tv.idViolazione " +
                               "INNER JOIN Anagrafica a ON v.idAnagrafica = a.idAnagrafica " +
                               "WHERE v.DecurtamentoPunti > 10";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Verbale v = new Verbale();
                    v.IdVerbale = Convert.ToInt32(reader["idVerbale"]);
                    v.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                    v.IndirizzoViolazione = reader["IndirizzoViolazione"].ToString();
                    v.NominativoAgente = reader["NominativoAgente"].ToString();
                    v.DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                    v.Importo = Convert.ToDecimal(reader["Importo"]);
                    v.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    v.IdAnagrafica = Convert.ToInt32(reader["idAnagrafica"]);
                    v.IdViolazione = Convert.ToInt32(reader["idViolazione"]);
                    v.Descrizione = reader["descrizione"].ToString();
                    v.Nome = reader["Nome"].ToString();
                    v.Cognome = reader["Cognome"].ToString();
                    verbali.Add(v);
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
            return View(verbali);
        }

        // Metodo per stampare il totale dei punti decurtati per trasgressore
        public ActionResult TotPunti()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> verb = new List<Verbale>();

            conn.Open();
            string query = "SELECT IdAnagrafica, SUM(DecurtamentoPunti) AS TotalePuntiDecurtati FROM VERBALE GROUP BY IdAnagrafica ORDER BY SUM(DecurtamentoPunti) DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Verbale v = new Verbale();
                    v.IdAnagrafica = Convert.ToInt32(reader["IdAnagrafica"]);
                    //Richiamo i dati come IdAngrafica grazie al metodo istanziato nel model di nome VisualizzaTrasgressori
                    Trasgressore trasgressore = Trasgressore.VisualizzaTragressori().FirstOrDefault(t => t.IdAnagrafica == v.IdAnagrafica);
                    v.Trasgressore = trasgressore;
                    v.DecurtamentoPunti = Convert.ToInt32(reader["TotalePuntiDecurtati"]);

                    verb.Add(v);
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
            return View(verb);
        }

        //Metodo per stampare il totale dei verbali ragruppati per trasgressore
        public ActionResult TotaleVerbaliPerNome()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDB"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Verbale> riepilogo = new List<Verbale>();

            conn.Open();
            string query = @"
        SELECT 
            Anagrafica.Nome, 
            Anagrafica.Cognome,
            COUNT(Verbale.IdVerbale) AS ConteggioVerbali
        FROM 
            Anagrafica
        JOIN 
            Verbale ON Anagrafica.IdAnagrafica = Verbale.IdAnagrafica
        GROUP BY 
            Anagrafica.IdAnagrafica, Anagrafica.Nome, Anagrafica.Cognome
        ORDER BY 
            ConteggioVerbali DESC";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Verbale rv = new Verbale
                    {
                        Nome = reader["Nome"].ToString(),
                        Cognome = reader["Cognome"].ToString(),
                        ConteggioVerbali = Convert.ToInt32(reader["ConteggioVerbali"])
                    };

                    riepilogo.Add(rv);
                }
            }
            catch (Exception ex)
            {
                // Assicurati di gestire l'eccezione in modo appropriato
                Response.Write($"Errore durante il recupero dei dati: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return View(riepilogo);
        }
    }
}
