using Microsoft.EntityFrameworkCore;
using System;

namespace Cidades.Dados
{
    public class CidadesContexto : DbContext
    {
        public CidadesContexto(DbContextOptions<CidadesContexto> options) : base(options)
        {
        }

        public DbSet<Domain.Cidades> Cidades { get; set; }
    }
}
