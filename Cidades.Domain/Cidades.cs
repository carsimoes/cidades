using System;
using System.ComponentModel.DataAnnotations;

namespace Cidades.Domain
{
    public class Cidades
    {
        public int Id { get; set; }

        [Required]
        public int Ibge { get; set; }

        [Required]
        [StringLength(2)]
        public string Uf { get; set; }

        [Required]
        [StringLength(120)]
        public string Nome { get; set; }

        [Required]
        [StringLength(200)]
        public string Longitude { get; set; }

        [Required]
        [StringLength(200)]
        public string Latitude { get; set; }

        [Required]
        [StringLength(250)]
        public string Regiao { get; set; }
    }
}
