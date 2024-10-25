using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backEnd.Entidades
{
    public class NutriScore
    {
        public int CalcularCalificacion(string informacionNutricionalJson)
        {
            dynamic infoNutricional = JsonConvert.DeserializeObject(informacionNutricionalJson);

            int puntaje = 0;
            int factoresConsiderados = 0;

            // Evaluar las calorías: 1 = Bueno, 5 = Malo
            if (infoNutricional["energy-kcal_value"] != null)
            {
                double calorias = infoNutricional["energy-kcal_value"];
                if (calorias < 100) puntaje += 1; // Bajo en calorías
                else if (calorias < 200) puntaje += 2;
                else if (calorias < 300) puntaje += 3;
                else if (calorias < 400) puntaje += 4;
                else puntaje += 5; // Alto en calorías
                factoresConsiderados++;
            }

            // Evaluar la grasa saturada
            if (infoNutricional["saturated-fat_value"] != null)
            {
                double grasaSaturada = infoNutricional["saturated-fat_value"];
                if (grasaSaturada < 1) puntaje += 1; // Bajo en grasa saturada
                else if (grasaSaturada < 5) puntaje += 2;
                else if (grasaSaturada < 10) puntaje += 3;
                else if (grasaSaturada < 15) puntaje += 4;
                else puntaje += 5; // Alto en grasa saturada
                factoresConsiderados++;
            }

            // Evaluar los azúcares
            if (infoNutricional["sugars_value"] != null)
            {
                double azucares = infoNutricional["sugars_value"];
                if (azucares < 5) puntaje += 1; // Bajo en azúcar
                else if (azucares < 10) puntaje += 2;
                else if (azucares < 20) puntaje += 3;
                else if (azucares < 30) puntaje += 4;
                else puntaje += 5; // Alto en azúcar
                factoresConsiderados++;
            }

            // Evaluar la sal
            if (infoNutricional["salt_value"] != null)
            {
                double sal = infoNutricional["salt_value"];
                if (sal < 0.3) puntaje += 1; // Bajo en sal
                else if (sal < 0.7) puntaje += 2;
                else if (sal < 1) puntaje += 3;
                else if (sal < 1.5) puntaje += 4;
                else puntaje += 5; // Alto en sal
                factoresConsiderados++;
            }

            // Evaluar las proteínas
            if (infoNutricional["proteins_value"] != null)
            {
                double proteinas = infoNutricional["proteins_value"];
                if (proteinas > 10) puntaje += 1; // Alto en proteínas
                else puntaje += 3; // Bajo en proteínas
                factoresConsiderados++;
            }

            // Evaluar la fibra
            if (infoNutricional["fiber_value"] != null)
            {
                double fibra = infoNutricional["fiber_value"];
                if (fibra > 5) puntaje += 1; // Alto en fibra
                else puntaje += 3; // Bajo en fibra
                factoresConsiderados++;
            }

            // Calcular el promedio de la puntuación
            if (factoresConsiderados > 0)
            {
                double promedio = (double)puntaje / factoresConsiderados;

                // Asegurar que el puntaje esté entre 1 y 5
                return Math.Max(1, Math.Min(5, (int)Math.Ceiling(promedio)));
            }

            // Si no se evaluaron factores, devolver un puntaje neutro de 3
            return 3;
        }
    }
}
