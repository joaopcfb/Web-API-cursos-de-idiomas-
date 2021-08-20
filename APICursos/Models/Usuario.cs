using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace APICursos.Models
{
    public partial class Usuario
    {
        [Required]
        [JsonPropertyName("username")]
        public string NomeUsuario { get; set; }
        [Required]
        [JsonPropertyName("password")]
        public string Senha { get; set; }
    }
}
