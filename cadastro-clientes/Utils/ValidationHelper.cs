//CONTÉM CLASSES PARA TRABALHAR COM EXPRESSÕES REGULARES COMO, POR EXEMPLO, O "Regex" QUE VERIFICA PADRÕES DE TEXTOS
using System.Text.RegularExpressions;

namespace Utils
{
    public static class ValidationHelper
    {
        public static bool ValidarEmail(string email)
        {
            string padrao = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            //O MÉTODO "Regex.IsMatch()" VERIFICA SE O TEXTO CORRESPONDE AO PADRÃO DECLARADO NA VARIÁVEL "padrao". SE SIM, IRÁ RETORNAR "true". SE NÃO, "false"
            return Regex.IsMatch(email, padrao);
        }
    }
}
