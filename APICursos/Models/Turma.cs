using System;
using System.Collections.Generic;

#nullable disable

namespace APICursos.Models
{
    public partial class Turma
    {
        public Turma()
        {
            Alunos = new HashSet<Aluno>();
        }

        public int CodTurma { get; set; }
        public string IdiomaTurma { get; set; }
        public DateTime? DataInicio { get; set; }

        public virtual ICollection<Aluno> Alunos { get; set; }
    }
}
