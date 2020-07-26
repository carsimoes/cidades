using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace Cidades.DadosIniciais
{
    class Program
    {
        private string conectionString = "server=mysql669.umbler.com;port=41890;database=carsimoes;user=tonare;password=ramones01;";

        static void Main(string[] args)
        {
            var pathFile = AppDomain.CurrentDomain.BaseDirectory + "cidades_desafio_tecnico.csv";
            string filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

            using (StreamReader reader = new StreamReader(pathFile))
            {
                while (!reader.EndOfStream)
                {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        //listA.Add(values[0]);
                        //listB.Add(values[1]);

                }
            }
        }
    }

    
}
