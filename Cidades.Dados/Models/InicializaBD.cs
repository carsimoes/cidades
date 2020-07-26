using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cidades.Dados.Models
{
    public static class InicializaBD
    {
        public static void Initialize(CidadesContexto context)
        {
            context.Database.EnsureCreated();

            if (context.Cidades.Any())
            {
                return;   
            }

           var cidades = LeituraCsv();

            foreach (Domain.Cidades c in cidades)
            {
                context.Cidades.Add(c);
            }

            context.SaveChanges();
        }

        private static List<Domain.Cidades> LeituraCsv()
        {
            var pathFile = AppDomain.CurrentDomain.BaseDirectory + "DadosIniciais\\cidades_desafio_tecnico.csv";

            var cidades = new List<Domain.Cidades>();
            
            StreamReader streamReader = new StreamReader(pathFile);
            using (StreamReader reader = streamReader)
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var cidade = new Domain.Cidades();
                    cidade.Ibge = Convert.ToInt32(values[0]);
                    cidade.Uf = values[1];
                    cidade.Nome = values[2];
                    cidade.Longitude = values[3];
                    cidade.Latitude = values[4];
                    cidade.Regiao = values[5];

                    cidades.Add(cidade);
                }
            }

            return cidades;
        }
    }  
}
