using System.Collections.Generic;
using System.Linq;
using Ultetes.Dnn.Project_Ultetes_Dnn.Models; // Feltételezve, hogy a DTO-k és a ViewModel itt vannak

namespace Ultetes.Dnn.Project_Ultetes_Dnn.Components
{
    public class CalendarCalculator
    {
        /// <summary>
        /// Kiszámolja a vetési és aratási hónapokat a termékek és tulajdonságok alapján.
        /// </summary>
        public Dictionary<string, int[]> CalculateMonthColors(List<ProductTypeViewModel> products, IEnumerable<TypePropertyDTO> typeProperties)
        {
            var monthColors = new Dictionary<string, int[]>();

            foreach (var p in products)
            {
                if (!monthColors.ContainsKey(p.ProductTypeName))
                {
                    int[] months = new int[12];

                    // Kikeressük az adott terméktípushoz tartozó tulajdonságokat
                    var propsForThisType = typeProperties.Where(t => t.ProductTypeId == p.ProductTypeId).ToList();

                    foreach (var prop in propsForThisType)
                    {
                        var parts = prop.PropertyName.Split('_');

                        // Ha a formátum pl. "vetes_3" vagy "aratas_8"
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int mIdx))
                        {
                            mIdx -= 1; // Mert a tömb 0-tól indul, a hónapok meg 1-től

                            // Bitműveletek: 1 = vetés, 2 = aratás, 3 = mindkettő
                            if (parts[0] == "vetes")
                            {
                                months[mIdx] |= 1;
                            }
                            else if (parts[0] == "aratas")
                            {
                                months[mIdx] |= 2;
                            }
                        }
                    }

                    monthColors.Add(p.ProductTypeName, months);
                }
            }

            return monthColors;
        }
    }
}
