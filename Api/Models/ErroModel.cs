using System.Runtime.Serialization;

namespace Api.Models
{
    [DataContract]
    public class ErroModel
    {
        public string Codigo { get; private set; }
        public string Mensagem { get; private set; }

        public ErroModel(string codigo, string mensagem)
        {
            Codigo = codigo;
            Mensagem = mensagem;
        }
    }
}