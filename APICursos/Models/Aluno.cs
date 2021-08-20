using System;
using System.Collections.Generic;

#nullable disable

namespace APICursos.Models
{
    public partial class Aluno
    {
        public int CodMatricula { get; set; }
        public string Nome { get; set; }
        public int CodTurma { get; set; }

        public virtual Turma CodTurmaNavigation { get; set; }
    }
}
