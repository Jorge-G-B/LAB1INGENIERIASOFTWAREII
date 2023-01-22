using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace IngenieríaDeSoftwareIILAB1
{
    internal class Program
    {
        
        public interface ICamion
        {
            Guid Id { get; }

            ServiceLifetime Lifetime { get; }
            void showState();
        }

        #region Camion
        public class Camion: ICamionTransient, ICamionScoped, ICamionSingleton
        {
            public Guid Id => Guid.NewGuid();

            private List<Caja> contenido;

            public Camion()
            {
                contenido = new List<Caja>();
                Random random = new Random();
                for (int i = 0; i < random.Next(1, 4); i++)
                {
                    contenido.Add(new Caja());
                }
            }
            public void showState()
            {
                Console.WriteLine("ID Camion: " + Id + " y su contenido consta de: ");
                foreach (var caja in contenido)
                {
                    caja.showState();
                }
            }

        }

        public interface ICamionTransient : ICamion
        {

        }
        public interface ICamionSingleton : ICamion
        {

        }
        public interface ICamionScoped : ICamion
        {

        }
        #endregion

        #region Caja
        public interface ICaja
        {
            Guid ID { get; }
            void showState();
        }

        public class Caja : ICaja
        {
            public Guid ID => Guid.NewGuid();

            Random rnd = new Random();

            List<Fruta> contenido;

            public Caja()
            {
                contenido = new List<Fruta>();
                int tipo;
                DateTime fechaexp;
                Fruta fruta;
                int cantidad = rnd.Next(6);
                for (int i = 0; i < cantidad; i++)
                {
                    tipo = rnd.Next(4);
                    fechaexp = RandomDay();
                    switch (tipo)
                    {
                        case 0:
                            fruta = new Fruta("Manzana", fechaexp);
                            break;
                        case 1:
                            fruta = new Fruta("Banana", fechaexp);
                            break;
                        case 2:
                            fruta = new Fruta("Fresa", fechaexp);
                            break;
                        case 3:
                            fruta = new Fruta("Naranja", fechaexp);
                            break;
                        default:
                            fruta = new Fruta("Naranja", fechaexp);
                            break;
                    }
                    contenido.Add(fruta);
                }
            }
            public DateTime RandomDay()
            {
                DateTime start = new DateTime(1995, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(rnd.Next(range));
            }
            public void showState()
            {
                Console.WriteLine("Código de caja: " + ID + " y su contenido consta de: ");
                foreach (var fruta in contenido)
                {
                    fruta.showState();
                }
            }
        }

            
        
        #endregion

        #region Fruta
        public interface IFruta
        {
            string Tipo { get; }
            DateTime FechaExpiracion { get; }
            void showState();
        }

        public class Fruta : IFruta
        {
            private string _tipo;
            private DateTime _fecha;

            public Fruta(string tipo, DateTime fecha)
            {
                this._tipo = tipo;
                this._fecha = fecha;
            }

            public string Tipo => _tipo;

            public DateTime FechaExpiracion => _fecha;

            public void showState()
            {
                Console.WriteLine("La fruta es: " + Tipo + " expira en la fecha: " + FechaExpiracion);
            }
        }

        #endregion

        static void Escribir(ServiceProvider servicio)
        {
            Console.WriteLine("Ejemplo Scoped");
            var ScopedCamion = servicio.GetRequiredService<ICamionScoped>();
            ScopedCamion.showState();
            Console.WriteLine();
            Console.WriteLine("Ejemplo Transient");
            var TransientCamion = servicio.GetRequiredService<ICamionTransient>();
            TransientCamion.showState();
            Console.WriteLine();
            Console.WriteLine("Ejemplo Singleton");
            var SingletonCamion = servicio.GetRequiredService<ICamionSingleton>();
            SingletonCamion.showState();
            Console.WriteLine();
        }

        static void Main(string[] args)
        {

            ServiceCollection services = new ServiceCollection();
            services.AddScoped<ICamionScoped, Camion>();
            services.AddSingleton<ICamionSingleton, Camion>();
            services.AddTransient<ICamionTransient, Camion>();
            var serviceProvider = services.BuildServiceProvider();
            Console.WriteLine("Escribir 1");
            Escribir(serviceProvider);
            serviceProvider = services.BuildServiceProvider();
            Console.WriteLine("Escribir 2");    
            Escribir(serviceProvider);
            Console.ReadLine();


        }

    }
}
