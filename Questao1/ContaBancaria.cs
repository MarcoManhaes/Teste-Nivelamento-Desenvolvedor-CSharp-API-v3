using System;
using System.Globalization;

namespace Questao1
{
    public class ContaBancaria {

        private double _saldo;

        public int Numero { get; } 
        public string Titular { get; private set; } 

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            Numero = numero;
            Titular = titular;
            Depositar(depositoInicial);
        }

        public ContaBancaria(int numero, string titular)
            : this(numero, titular, 0.0) { }


        public void Depositar(double valor)
        {
            if (valor < 0)
                throw new ArgumentException("O valor do depósito não pode ser negativo.");
            _saldo += valor;
        }

        public void Sacar(double valor)
        {
            const double taxaSaque = 3.50;

            if (valor <= 0)
                throw new ArgumentException("O valor do saque deve ser positivo.");

            _saldo -= valor + taxaSaque;
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {_saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }
    }
}
