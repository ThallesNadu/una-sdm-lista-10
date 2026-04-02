using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OscarFilmeApi.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Diretor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        public bool Venceu { get; set; }
    }
}