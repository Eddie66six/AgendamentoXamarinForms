using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendamentoXamarinForms.Models
{
    public class Usuario
    {
        public bool auth { get; set; }
        public string messageResult { get; set; }
        public string token { get; set; }
        public int idPais { get; set; }
        public string idioma { get; set; }
        public Authorizations authorizations { get; set; }
        public string picture { get; set; }
        public string name { get; set; }
        public int IdCliente { get; set; }
        public int IdW12 { get; set; }
        public int IdFilial { get; set; }
        public string filialName { get; set; }
    }

    public class Authorizations
    {
        public bool CrossFit { get; set; }
        public bool Encomenda { get; set; }
        public bool Compra { get; set; }
        public bool TreinoAtivo { get; set; }
    }

    public class UsuarioError
    {
        public bool auth { get; set; }
        public string messageResult { get; set; }
        public string token { get; set; }
    }
}
