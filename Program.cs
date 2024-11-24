using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;
using Microsoft.SqlServer.Server;

namespace Расчет_IV_опциона
{
    /*
    internal class Program
    {
        
        static void Main(string[] args)
        {
                     
            
            // Упрощённая формула Блэка-Шоулза для call-опциона без учёта стоимости денег
            /* Далее кусок кода на ПИТОНе
            def black_scholes_call_no_r(S, K, T, sigma):
            if sigma <= 0 or T <= 0:
            return 0
            d1 = (np.log(S / K) + 0.5 * sigma * *2 * T) / (sigma * np.sqrt(T))
            d2 = d1 - sigma * np.sqrt(T)
            call_price = S * si.norm.cdf(d1) - K * si.norm.cdf(d2)
            return call_price

            // Поиск implied volatility (IV)
            def implied_volatility_call_no_r(S, K, T, market_price, tol= 1e-6):
            func = lambda sigma: black_scholes_call_no_r(S, K, T, sigma) - market_price
            try:
            iv = brentq(func, 1e-6, 5.0, xtol = tol)
            return iv
            except ValueError as e:
            return np.nan

            // Расчёт IV
            iv = implied_volatility_call_no_r(S, K, T, market_price)
            iv
            
              
          
        }
    }
    */
}
    
    //
    // Код переписанный под C#
    //


    public class OptionCalculator
    {
         public static void Main()
        {
            // Входные данные

            Console.WriteLine("Ведите текущую цену БА: ");
            string str = Console.ReadLine();
            double S = double.Parse(str);

            Console.WriteLine("Ведите цену CALL опциона: ");
            str = Console.ReadLine();
            double marketPrice = double.Parse(str);

            Console.WriteLine("Ведите цену исполнения CALL опциона: ");
            str = Console.ReadLine();
            double K = double.Parse(str);

            Console.WriteLine("Ведите время до экспирации в днях: ");
            str = Console.ReadLine();
            double t = double.Parse(str);
            double T = t / 365;

            Console.WriteLine("Tекущая цена БА " + S);
            Console.WriteLine("Цена страйка " + K);
            Console.WriteLine("Время до экспирации в годах " + T);

           // Расчёт IV
            double iv = ImpliedVolatilityCallNoR(S, K, T, marketPrice);
            Console.WriteLine($"Implied Volatility: {iv}");
        }
    
        // Упрощённая формула Блэка-Шоулза для call-опциона без учёта стоимости денег
        public static double BlackScholesCallNoR(double S, double K, double T, double sigma)
        {
            if (sigma <= 0 || T <= 0)
                return 0;

            double d1 = (Math.Log(S / K) + 0.5 * Math.Pow(sigma, 2) * T) / (sigma * Math.Sqrt(T));
            double d2 = d1 - sigma * Math.Sqrt(T);

            var normal = new Normal();
            double callPrice = S * normal.CumulativeDistribution(d1) - K * normal.CumulativeDistribution(d2);

            return callPrice;
        }

        // Поиск implied volatility (IV)
        public static double ImpliedVolatilityCallNoR(double S, double K, double T, double marketPrice, double tol = 1e-6)
        {
            Func<double, double> func = sigma => BlackScholesCallNoR(S, K, T, sigma) - marketPrice;

            try
            {
                double iv = Brent.FindRoot(func, 1e-6, 5.0, tol, 100);
                return iv;
            }
            catch (Exception)
            {
                return double.NaN;
            }
        }

       
    }