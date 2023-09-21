using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {

        private double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular)
        {
            Numero = numero;
            Titular = titular;
        }

        public ContaBancaria(int numero, string titular, double deposito)
        {
            Numero = numero;
            Titular = titular;
            Saldo = deposito;
        }

        public void Deposito(double valor)
        {
            Saldo += valor;
        }

        public void Saque(double valor)
        {
            Saldo -= valor;
            Saldo -= TaxaSaque;
        }

        public int Numero { get; set; }

        public string Titular { get; set; }

        public double Saldo { get; set; }
    }
}
