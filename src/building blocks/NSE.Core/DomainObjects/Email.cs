using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    public class Email
    {

        public const int EnderecoMaxLength = 254;
        public const int EnderecoMinLength = 5;
        public string Endereco { get; set; }

        protected Email() { }

        public Email(string endereco)
        {
            if (!Validar(endereco)) throw new DomainException("Email Inválido");
            Endereco = endereco;
        }

        public static bool Validar(string email)
        {
            var regexEmail = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            return regexEmail.IsMatch(email);

        }



    }
}
